# FrontEnd (Vite + React)

This is a minimal Vite + React frontend scaffold for the ProgressTracker API.

Features implemented:
- Vite React app
- SBAdmin2 CSS included via CDN
- Login page using JWT (POST to `https://localhost:5000/api/Auth` with `application/x-www-form-urlencoded`)
- Users management (list, create, update, delete) using fetch and simple components
- Services use a global `BASE_URL` in `src/services/config.js`

Quick start:

1. cd into `FrontEnd`
2. install dependencies:

```powershell
npm install
```

3. run dev server:

```powershell
npm run dev
```

Notes:
- This scaffold uses the Fetch API (no Axios) and React hooks (no Redux).
- The backend must be running at `https://localhost:5000` with proper SSL configured.
- Token is stored in `localStorage` as `token`.
