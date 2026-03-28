<div align="center">
  <h1>🎧 moodalbum</h1>
  <p><strong>A beautifully crafted, anonymous music discovery platform.</strong></p>
  <p>
    Answer 3 simple questions—Energy, Familiarity, and Time—and algorithmically discover 3 perfect albums tailored entirely to your mood. No login, no tracking, just pure music discovery from a curated pool of +31,000 albums.
  </p>
  <br>
  <a href="https://moodalbum.com">Live Demo (moodalbum.com)</a>
</div>

<br />

## ✨ Key Features

- **No Strings Attached:** 100% anonymous usage using `X-Anonymous-Id`. No registration, no passwords.
- **Advanced Recommendation Engine:** Calculates pure Euclidean distance across 3 dynamically weighted dimensions (Energy, Familiarity, Time) against an in-memory database of scored metadata.
- **Era-Based Temporal Logic:** Ensures temporal diversity (e.g., retrieving classics from pre-1985, golden era from 1985–2000, and modern from 2000–2010 when "Past" is requested) completely ignoring distance penalties inside the era bucket for maximum randomness.
- **Sleek UI/UX:** Built on Angular 19 with custom layout transitions, CSS keyframe animations, and intuitive card-discovery flows. Mobile-first responsive design.
- **Direct Streaming Integration:** Auto-resolves album and artist inputs into direct Spotify and Wikipedia links using front-end routing techniques.

---

## 🔒 Security & Architecture under the Hood

The backend is engineered for high stability and security, specifically tailored to operate flawlessly on a public perimeter:

- **Enterprise Rate Limiting:** 
  - `IpBasedSessionPolicy` enforcing **1 request per 24 hours per IP**.
  - `GlobalLimiter` implementing a sliding window of max 120 API requests/24h.
  - Development environments feature a structured `BypassDailyLimit` flag to safely bypass during iteration.
- **Fortified Headers (Anti-Clickjacking & XSS):** 
  - `X-Frame-Options: DENY`
  - Strict `Content-Security-Policy: frame-ancestors 'none'`
  - `X-Content-Type-Options: nosniff`
- **Zero Secrets:** The codebase relies purely on public datasets and runtime calculations, meaning no API Keys, database strings, or AWS secrets are hardcoded in the codebase.
- **In-Memory Concurrency:** Safely handles state using ASP.NET Core `MemoryCache` and ConcurrentDictionaries for blisteringly fast JSON queries without relying on an external DB overlay.

---

## 🛠 Tech Stack

**Frontend:**
* Angular 19 (TypeScript)
* Reactive Forms & RxJS
* Pure Custom CSS layout & Micro-Animations

**Backend:**
* .NET 8 (C#)
* ASP.NET Core Web API
* Microsoft.AspNetCore.RateLimiting

**Deployment:**
* Nginx Reverse Proxy
* Ubuntu Server (Hetzner)
* GitHub Actions (CI/CD)

---

## 🚀 Getting Started

If you want to run this project locally, simply follow these steps.

### Prerequisites
- Node.js (v20+) -> `npm install -g @angular/cli`
- .NET 8 SDK

### 1. Run the API (Backend)
Navigate to the Web API project.
```bash
cd backend/RecommendationApi
dotnet run
```
The API will spin up on `http://localhost:5240`. During development, `BypassDailyLimit` within `appsettings.Development.json` is set to `true`, meaning rate limiting won't stop your testing.

### 2. Run the UI (Frontend)
Navigate to the Angular project.
```bash
cd frontend/music-ui
npm install
ng serve
```
The application will be accessible at `http://localhost:4200`.

---

## 📦 Deployment pipeline (CI/CD)

This project contains a fully automated GitHub Actions workflow inside `.github/workflows/deploy.yml`. 
By pushing directly to `main`, the CI pipeline automatically:
1. Builds the Angular bundle and scp's it to `/var/www/music-frontend/`
2. Compiles `.NET 8 Release` binaries and overrides the `/var/www/music-backend/` output.
3. Restarts the Systemd Linux daemon (`music-backend.service`).

### Required GitHub Secrets:
To deploy to your own server, you must provide the following standard Secrets in GitHub Actions:
- `SERVER_IP`
- `SERVER_USER`
- `SERVER_SSH_KEY`

---

## ©️ License
Open-sourced under the MIT License. Feel free to fork, customize, and deploy your own instance of the music discovery engine.
