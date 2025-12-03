import { BASE_URL } from './config'

export async function login(username, password){
  const res = await fetch(`${BASE_URL}/Auth`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ username, password })
  })

  if(!res.ok){
    const txt = await res.text()
    throw new Error(txt || 'Login failed')
  }

  const data = await res.json()
  // assume token is returned as { token: '...' } or plain string
  const token = data.token || data || ''
  localStorage.setItem('token', token)
  return token
}

export function logout(){
  localStorage.removeItem('token')
}

export function getToken(){
  return localStorage.getItem('token')
}
