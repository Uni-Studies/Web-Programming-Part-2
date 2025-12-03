import React, { useEffect, useState } from 'react'
import { getUsers, createUser, updateUser, deleteUser } from '../services/userService'
import UserForm from './UserForm'

export default function Users(){
  const [items, setItems] = useState([])
  const [loading, setLoading] = useState(true)
  const [editing, setEditing] = useState(null)
  const [error, setError] = useState(null)
  const [page, setPage] = useState(1)
  const [pageSize, setPageSize] = useState(10)
  const [totalCount, setTotalCount] = useState(0)

  async function load(p = page, ps = pageSize){
    setLoading(true)
    setError(null)
    try{
      const data = await getUsers({ OrderBy: 'Id', SortAsc: true, Pager: { Page: p, PageSize: ps } })
      setItems(data || [])
      // Assume API returns total count or we estimate from results
      setTotalCount(data?.length || 0)
    }catch(err){ setError(err.message) }
    setLoading(false)
  }

  useEffect(()=>{ load(page, pageSize) }, [page, pageSize])

  async function handleCreate(payload){
    await createUser(payload)
    setPage(1)
    load(1, pageSize)
  }
  async function handleUpdate(id, payload){
    await updateUser(id, payload)
    setEditing(null)
    load(page, pageSize)
  }
  async function handleDelete(id){
    if(!confirm('Delete user?')) return
    await deleteUser(id)
    load(page, pageSize)
  }

  const totalPages = Math.ceil(totalCount / pageSize)

  return (
    <div>
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h3>Users</h3>
        <button className="btn btn-primary" onClick={()=>setEditing({})}>New User</button>
      </div>

      {loading ? <div>Loading...</div> : (
        <>
          {error && <div className="alert alert-danger">{error}</div>}
          <table className="table table-striped">
            <thead>
              <tr><th>Id</th><th>Username</th><th>FirstName</th><th>LastName</th><th>Actions</th></tr>
            </thead>
            <tbody>
              {items.map(u=> (
                <tr key={u.id}>
                  <td>{u.id}</td>
                  <td>{u.username}</td>
                  <td>{u.firstName}</td>
                  <td>{u.lastName}</td>
                  <td>
                    <button className="btn btn-sm btn-secondary me-2" onClick={()=>setEditing(u)}>Edit</button>
                    <button className="btn btn-sm btn-danger" onClick={()=>handleDelete(u.id)}>Delete</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

          {/* Pagination Controls */}
          <div className="d-flex justify-content-between align-items-center mt-3">
            <div>
              <label className="me-2">Items per page:</label>
              <select className="form-select d-inline-block" style={{width:'auto'}} value={pageSize} onChange={e => setPageSize(parseInt(e.target.value))}>
                <option value={5}>5</option>
                <option value={10}>10</option>
                <option value={20}>20</option>
                <option value={50}>50</option>
              </select>
            </div>
            <div>
              <nav>
                <ul className="pagination mb-0">
                  <li className={`page-item ${page === 1 ? 'disabled' : ''}`}>
                    <button className="page-link" onClick={() => setPage(p => Math.max(1, p - 1))} disabled={page === 1}>Previous</button>
                  </li>
                  {Array.from({ length: totalPages }, (_, i) => i + 1).slice(Math.max(0, page - 2), page + 1).map(p => (
                    <li key={p} className={`page-item ${p === page ? 'active' : ''}`}>
                      <button className="page-link" onClick={() => setPage(p)}>{p}</button>
                    </li>
                  ))}
                  <li className={`page-item ${page === totalPages ? 'disabled' : ''}`}>
                    <button className="page-link" onClick={() => setPage(p => Math.min(totalPages, p + 1))} disabled={page === totalPages}>Next</button>
                  </li>
                </ul>
              </nav>
            </div>
          </div>
        </>
      )}

      {editing !== null && <UserForm initial={editing} onCancel={()=>setEditing(null)} onSave={async (payload)=>{
        if(editing.id) await handleUpdate(editing.id, payload)
        else await handleCreate(payload)
      }} />}
    </div>
  )
}

