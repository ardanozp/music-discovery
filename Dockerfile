# ==========================================
# Stage 1: Build Angular Frontend
# ==========================================
FROM node:20-alpine AS frontend-build
WORKDIR /app/frontend

# Install dependencies using package lock for reproducible builds
COPY frontend/music-ui/package*.json ./
RUN npm ci

# Copy frontend source files
COPY frontend/music-ui/ ./

# Build Angular application for production (outputs to dist/music-ui/browser)
RUN npm run build -- --configuration production

# ==========================================
# Stage 2: Build ASP.NET Core Backend
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS backend-build
WORKDIR /src

# Copy csproj and restore dependencies (layer cache)
COPY backend/RecommendationApi/RecommendationApi.csproj ./backend/RecommendationApi/
RUN dotnet restore backend/RecommendationApi/RecommendationApi.csproj

# Copy the full backend source
COPY backend/ ./backend/

# Publish backend in Release mode
WORKDIR /src/backend/RecommendationApi
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ==========================================
# Stage 3: Final Runtime Image
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

# Set container environment variables
ENV ASPNETCORE_URLS=http://+:80 \
    DOTNET_RUNNING_IN_CONTAINER=true

# Expose internal HTTP port
EXPOSE 80

# Copy published backend artifacts
COPY --from=backend-build /app/publish .

# Copy compiled Angular static files into wwwroot for SPA hosting
COPY --from=frontend-build /app/frontend/dist/music-ui/browser ./wwwroot

ENTRYPOINT ["dotnet", "RecommendationApi.dll"]
