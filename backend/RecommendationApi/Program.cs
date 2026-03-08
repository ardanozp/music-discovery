using System.Net;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using RecommendationApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache(options => options.SizeLimit = 20_000);
builder.Services.AddHealthChecks()
    .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy(), tags: new[] { "live", "ready" });
builder.Services.AddRequestTimeouts(options =>
{
    options.DefaultPolicy = new()
    {
        Timeout = TimeSpan.FromSeconds(15)
    };
});

builder.WebHost.ConfigureKestrel(options =>
{
    // Keep JSON API payloads intentionally small.
    options.Limits.MaxRequestBodySize = 1 * 1024 * 1024;
    options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(15);
    options.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddHttpClient("Wikipedia", client =>
{
    client.Timeout = TimeSpan.FromSeconds(5);
    client.DefaultRequestHeaders.UserAgent.ParseAdd("MusicDiscoveryApp/1.0");
});

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>()
    ?.Where(x => !string.IsNullOrWhiteSpace(x))
    .Distinct(StringComparer.OrdinalIgnoreCase)
    .ToArray()
    ?? Array.Empty<string>();

var allowedMethods = builder.Configuration
    .GetSection("Cors:AllowedMethods")
    .Get<string[]>()
    ?.Where(x => !string.IsNullOrWhiteSpace(x))
    .Distinct(StringComparer.OrdinalIgnoreCase)
    .ToArray()
    ?? new[] { "GET", "POST" };

var allowedHeaders = builder.Configuration
    .GetSection("Cors:AllowedHeaders")
    .Get<string[]>()
    ?.Where(x => !string.IsNullOrWhiteSpace(x))
    .Distinct(StringComparer.OrdinalIgnoreCase)
    .ToArray()
    ?? new[] { "Content-Type", "X-Anonymous-Id" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.WithOrigins("http://localhost:4200")
                  .WithMethods("GET", "POST")
                  .WithHeaders("Content-Type", "X-Anonymous-Id");
            return;
        }

        if (allowedOrigins.Length == 0)
        {
            throw new InvalidOperationException("Production requires Cors:AllowedOrigins to be configured.");
        }

        policy.WithOrigins(allowedOrigins)
              .WithMethods(allowedMethods)
              .WithHeaders(allowedHeaders)
              .DisallowCredentials();
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // 4. İsteği reddettiğimizde dönülecek özel yanıt (Retry-After başlığı ve anlamlı JSON hatası ile)
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

        // Limitin ne zaman sıfırlanacağı bilgisini alıp başlığa (header) ekliyoruz
        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            context.HttpContext.Response.Headers.RetryAfter =
                ((int)retryAfter.TotalSeconds).ToString();
        }

        await context.HttpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status429TooManyRequests,
            Title = "Günlük istek limiti aşıldı.",
            Detail = "Aynı IP adresi üzerinden yapabileceğiniz istek sınırına ulaştınız. Lütfen daha sonra tekrar deneyin.",
            Type = "https://httpstatuses.com/429"
        }, cancellationToken: token);
    };

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        var partitionKey = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetSlidingWindowLimiter(partitionKey, _ => new SlidingWindowRateLimiterOptions
        {
            PermitLimit = 120,
            Window = TimeSpan.FromHours(24),
            SegmentsPerWindow = 6,
            QueueLimit = 0,
            AutoReplenishment = true
        });
    });

    // 2 & 3. Yalnızca IP bazlı çalışan özel FixedWindow policy'si
    options.AddPolicy("IpBasedSessionPolicy", context =>
    {
        var partitionKey = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(partitionKey, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 1, // X = 1 İstek
            Window = TimeSpan.FromHours(24),
            QueueLimit = 0,
            AutoReplenishment = true
        });
    });
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.ForwardLimit = 1;

    var knownProxyIps = builder.Configuration.GetSection("ReverseProxy:KnownProxies").Get<string[]>() ?? Array.Empty<string>();
    foreach (var ipText in knownProxyIps)
    {
        if (IPAddress.TryParse(ipText, out var ip))
        {
            options.KnownProxies.Add(ip);
        }
    }

    if (builder.Environment.IsDevelopment() && options.KnownProxies.Count == 0)
    {
        options.KnownProxies.Add(IPAddress.Loopback);
        options.KnownProxies.Add(IPAddress.IPv6Loopback);
    }
});

builder.Services.AddSingleton<AlbumDataService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<AlbumDataService>());
builder.Services.AddSingleton<RecommendationService>();
builder.Services.AddSingleton<IRecommendationService>(sp => sp.GetRequiredService<RecommendationService>());
builder.Services.AddSingleton<SessionStore>();
builder.Services.AddSingleton<AnonymousUserStore>();

var app = builder.Build();

app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler(exceptionApp =>
    {
        exceptionApp.Run(async context =>
        {
            var feature = context.Features.Get<IExceptionHandlerFeature>();
            var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("GlobalException");
            logger.LogError(feature?.Error, "Unhandled exception for {Path}", context.Request.Path);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Unexpected server error",
                Type = "https://httpstatuses.com/500"
            });
        });
    });

    app.UseHsts();
}

app.Use(async (context, next) =>
{
    context.Response.Headers.TryAdd("X-Content-Type-Options", "nosniff");
    context.Response.Headers.TryAdd("X-Frame-Options", "DENY");
    context.Response.Headers.TryAdd("Referrer-Policy", "no-referrer");
    context.Response.Headers.TryAdd("Permissions-Policy", "geolocation=(), microphone=(), camera=()");
    context.Response.Headers.TryAdd("Content-Security-Policy", "default-src 'none'; frame-ancestors 'none'; base-uri 'none'; form-action 'none'");
    await next();
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("FrontendPolicy");
app.UseRateLimiter();
app.UseRequestTimeouts();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health", new()
{
    Predicate = check => check.Tags.Contains("live")
});
app.MapHealthChecks("/ready", new()
{
    Predicate = check => check.Tags.Contains("ready")
});

app.Run();
