export async function fetchWithAuth(url, opts = {}) {
  const token = localStorage.getItem('token')
  const headers = { ...(opts.headers || {}), ...(token ? { Authorization: `Bearer ${token}` } : {}) }
  const res = await fetch(url, { ...opts, headers })
  if (!res.ok) {
    const text = await res.text()
    throw new Error(text || 'Request failed')
  }
  if (res.status === 204) return null
  return res.json()
}
