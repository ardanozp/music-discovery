<div align="center">
  <h1>🎧 moodalbum</h1>
  <p><strong>An anonymous music discovery platform focused on full-album experiences.</strong></p>
  <p>
    Answer 3 simple questions—Energy, Familiarity, and Time—and discover 3 albums tailored to your mood.
    No login, no tracking. Just music.
  </p>
  <br>
  👉 https://moodalbum.com
</div>

<br />

<img width="1909" height="875" alt="1" src="https://github.com/user-attachments/assets/6a978e10-7c41-4c92-889a-aa3e2318d724" />
<img width="1917" height="871" alt="2" src="https://github.com/user-attachments/assets/eb202258-19d1-4ae5-a46a-05a513f3c878" />
<img width="1909" height="870" alt="3" src="https://github.com/user-attachments/assets/3fb9050f-af68-4b4f-859c-c7ee0efd30c6" />


---

## ✨ Overview

Most music platforms push users toward familiar artists.  
moodalbum is built to do the opposite—help users discover full albums, not just tracks.

The system combines user mood inputs with a lightweight recommendation engine to surface diverse and relevant albums from a curated dataset of 31,000+ entries.

---

## 🧠 How it Works

- Users select **Energy**, **Familiarity**, and **Time**
- Albums are pre-scored across these dimensions  
- Similarity is calculated using **weighted Euclidean distance**  
- A small randomness factor ensures fresh results each time  
- A diversity rule prevents the same artist from dominating results  

---

## ⚙️ Key Highlights

- **Anonymous by design** → No authentication, uses `X-Anonymous-Id`
- **Fast & lightweight** → In-memory processing, no external DB dependency  
- **Balanced recommendations** → Combines relevance + randomness  
- **Era-aware discovery** → Supports temporal exploration  
- **Clean UX** → Simple, distraction-free interface  

---

## 🛠 Tech Stack

**Frontend**
- Angular (TypeScript, RxJS)

**Backend**
- .NET 8 (ASP.NET Core Web API)

**Infrastructure**
- Nginx, Linux server
- GitHub Actions (CI/CD)

---

## 🎯 Purpose

This project was built to explore:
- Recommendation system design (without ML)
- Anonymous system architecture
- Performance-focused backend design
- Clean, minimal user experience

---

## 📌 Notes

- No user data is stored  
- No third-party authentication  
- Fully stateless interaction model  

---

## © License
MIT License
