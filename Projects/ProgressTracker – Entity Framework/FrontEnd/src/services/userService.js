import { apiFetch, buildQueryString } from './api'

export async function getUsers(params){
  // params is an object, build query string like ?OrderBy=Id&SortAsc=true&Pager.Page=1&Pager.PageSize=10
  const qs = buildQueryString(params)
  return apiFetch(`/Users${qs}`)
}

export async function createUser(payload){
  return apiFetch(`/Users`, { method: 'POST', headers: {'Content-Type':'application/json'}, body: JSON.stringify(payload) })
}

export async function updateUser(id, payload){
  return apiFetch(`/Users/${id}`, { method: 'PUT', headers: {'Content-Type':'application/json'}, body: JSON.stringify(payload) })
}

export async function deleteUser(id){
  return apiFetch(`/Users/${id}`, { method: 'DELETE' })
}
