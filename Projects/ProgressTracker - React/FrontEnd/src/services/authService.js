import React, { createContext, useContext, useState } from 'react'
import { baseURL } from '../api/config'

const AuthContext = createContext()

export function AuthProvider({ children }) {
  const [token, setToken] = useState(localStorage.getItem('token'))
  const [user, setUser] = useState(JSON.parse(localStorage.getItem('user') || 'null'))

  async function login(username, password) {
    const body = new URLSearchParams({ username, password })
    const res = await fetch(`${baseURL}/Auth`, { method: 'POST', headers: { 'Content-Type': 'application/x-www-form-urlencoded' }, body })
    if (!res.ok) throw new Error('Invalid credentials')
    const json = await res.json()
    // assuming API returns { token: '...' }
    setToken(json.token)
    localStorage.setItem('token', json.token)
    // extract username from token or set directly
    setUser({ username })
    localStorage.setItem('user', JSON.stringify({ username }))
  }

  function logout() {
    setToken(null)
    setUser(null)
    localStorage.removeItem('token')
    localStorage.removeItem('user')
  }

  // Avoid JSX in a .js file; use React.createElement so esbuild doesn't require JSX loader
  return React.createElement(AuthContext.Provider, { value: { token, user, login, logout } }, children)
}

export function useAuth() { return useContext(AuthContext) }

export default { AuthProvider, useAuth }
