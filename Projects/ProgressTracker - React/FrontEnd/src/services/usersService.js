import { baseURL } from '../api/config'
import { useAuth } from './authService'

function getAuthHeader() {
  const token = localStorage.getItem('token')
  return token ? { Authorization: `Bearer ${token}` } : {}
}

async function request(path, options = {}) {
  const headers = { 'Content-Type': 'application/json', ...getAuthHeader(), ...(options.headers || {}) }
  const res = await fetch(baseURL + path, { ...options, headers })
  if (!res.ok) {
    const text = await res.text()
    throw new Error(text || 'Request failed')
  }
  if (res.status === 204) return null
  return await res.json()
}

export default {
  async getUsers(query = {}) {
    const qs = new URLSearchParams()
    if (query.OrderBy) qs.set('OrderBy', query.OrderBy)
    if (query.SortAsc !== undefined) qs.set('SortAsc', query.SortAsc)
    if (query.Pager) {
      qs.set('Pager.Page', query.Pager.Page)
      qs.set('Pager.PageSize', query.Pager.PageSize)
    }
    if (query.Filter) {
      for (const k in query.Filter) if (query.Filter[k]) qs.set(`Filter.${k}`, query.Filter[k])
    }
    return await request('/Users?' + qs.toString())
  },
  async getUser(id) { return await request('/Users/' + id) },
  async createUser(u) { return await request('/Users', { method: 'POST', body: JSON.stringify(u) }) },
  async updateUser(id, u) { return await request('/Users/' + id, { method: 'PUT', body: JSON.stringify(u) }) },
  async deleteUser(id) { return await request('/Users/' + id, { method: 'DELETE' }) }
}
