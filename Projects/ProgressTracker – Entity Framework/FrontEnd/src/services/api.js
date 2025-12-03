import { BASE_URL } from './config'

function buildQuery(obj, prefix = ''){
  const parts = [];
  for(const k in obj){
    const val = obj[k]
    if(val === undefined || val === null) continue
    if(typeof val === 'object'){
      if(Array.isArray(val)) continue
      for(const sub in val){
        parts.push(`${encodeURIComponent(prefix?`${prefix}.${k}`:k)}.${encodeURIComponent(sub)}=${encodeURIComponent(val[sub])}`)
      }
    } else {
      parts.push(`${encodeURIComponent(prefix?`${prefix}.${k}`:k)}=${encodeURIComponent(val)}`)
    }
  }
  return parts.join('&')
}

export async function apiFetch(path, opts = {}){
  const url = `${BASE_URL}${path}`
  const token = localStorage.getItem('token')
  const headers = opts.headers || {}
  if(token) headers['Authorization'] = `Bearer ${token}`
  const res = await fetch(url, {...opts, headers})
  if(!res.ok){
    const txt = await res.text()
    throw new Error(txt || res.statusText)
  }
  const contentType = res.headers.get('content-type')
  if(contentType && contentType.includes('application/json')) return res.json()
  return res.text()
}

export function buildQueryString(params){
  if(!params) return ''
  const q = buildQuery(params)
  return q ? `?${q}` : ''
}
