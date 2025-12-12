import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import usersService from '../../services/usersService'

export default function UsersList() {
  const [users, setUsers] = useState([])
  const [loading, setLoading] = useState(false)

  // paging state
  const [page, setPage] = useState(1)
  const [itemsPerPage, setItemsPerPage] = useState(10)
  const [totalCount, setTotalCount] = useState(0)

  // filter state
  const [filter, setFilter] = useState({ username: '', firstName: '', lastName: '' })

  async function load(p = page, ps = itemsPerPage, f = filter) {
    setLoading(true)
    const data = await usersService.getUsers({ Pager: { Page: p, PageSize: ps }, OrderBy: 'Id', SortAsc: true, Filter: { Username: f.username, FirstName: f.firstName, LastName: f.lastName } })
    // API response shape: { isSuccess:true, data: { items: [...], pager: { count, page, pageSize } } }
    const payload = data?.data || {}
    setUsers(payload.items || [])
    const pager = payload.pager || { count: 0, page: p, pageSize: ps }
    setTotalCount(pager.count || 0)
    setPage(pager.page || p)
    setItemsPerPage(pager.pageSize || ps)
    setLoading(false)
  }

  useEffect(() => { load(page, itemsPerPage) }, [page, itemsPerPage])

  async function remove(id) {
    if (!confirm('Delete user?')) return
    await usersService.deleteUser(id)
    // reload current page; if last item on last page was deleted, adjust page
    const newTotal = Math.max(0, totalCount - 1)
    const totalPages = Math.max(1, Math.ceil(newTotal / itemsPerPage))
    if (page > totalPages) setPage(totalPages)
    else load(page, itemsPerPage)
  }

  const totalPages = Math.max(1, Math.ceil(totalCount / itemsPerPage))
  const pageNumbers = []
  for (let i = 1; i <= totalPages; i++) pageNumbers.push(i)

  function onItemsPerPageChange(e) {
    const ps = Number(e.target.value)
    setItemsPerPage(ps)
    setPage(1)
  }

  function applyFilter(e) {
    if (e) e.preventDefault()
    setPage(1)
    load(1, itemsPerPage, filter)
  }

  function clearFilter() {
    setFilter({ username: '', firstName: '', lastName: '' })
    setPage(1)
    load(1, itemsPerPage, { username: '', firstName: '', lastName: '' })
  }

  return (
    <div>
      <div className="d-flex justify-content-between mb-3">
        <h3>Users</h3>
        <Link to="/users/new" className="btn btn-primary">New User</Link>
      </div>

      {/* Filter form */}
      <form className="row g-2 mb-3 align-items-end" onSubmit={applyFilter}>
        <div className="col-md-3">
          <label className="form-label">Username</label>
          <input className="form-control" value={filter.username} onChange={e => setFilter({...filter, username: e.target.value})} />
        </div>
        <div className="col-md-3">
          <label className="form-label">First Name</label>
          <input className="form-control" value={filter.firstName} onChange={e => setFilter({...filter, firstName: e.target.value})} />
        </div>
        <div className="col-md-3">
          <label className="form-label">Last Name</label>
          <input className="form-control" value={filter.lastName} onChange={e => setFilter({...filter, lastName: e.target.value})} />
        </div>
        <div className="col-md-3 d-flex">
          <button className="btn btn-primary me-2" type="submit">Filter</button>
          <button className="btn btn-secondary" type="button" onClick={clearFilter}>Clear</button>
        </div>
      </form>

      {loading ? <div>Loading...</div> : (
        <>
          <table className="table table-striped">
            <thead>
              <tr>
                <th>Id</th>
                <th>Username</th>
                <th>FirstName</th>
                <th>LastName</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {users.map(u => (
                <tr key={u.id}>
                  <td>{u.id}</td>
                  <td>{u.username}</td>
                  <td>{u.firstName}</td>
                  <td>{u.lastName}</td>
                  <td>
                    <Link className="btn btn-sm btn-secondary me-2" to={`/users/${u.id}`}>Edit</Link>
                    <button className="btn btn-sm btn-danger" onClick={() => remove(u.id)}>Delete</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

          <div className="d-flex align-items-center justify-content-between">
            <div className="d-flex align-items-center">
              <label className="me-2">Items per page:</label>
              <select className="form-select" style={{ width: '100px' }} value={itemsPerPage} onChange={onItemsPerPageChange}>
                <option value={5}>5</option>
                <option value={10}>10</option>
                <option value={20}>20</option>
                <option value={50}>50</option>
              </select>
            </div>

            <nav>
              <ul className="pagination mb-0">
                <li className={`page-item ${page === 1 ? 'disabled' : ''}`}>
                  <button className="page-link" onClick={() => setPage(Math.max(1, page - 1))}>Previous</button>
                </li>

                {pageNumbers.map(n => (
                  <li key={n} className={`page-item ${n === page ? 'active' : ''}`}>
                    <button className="page-link" onClick={() => setPage(n)}>{n}</button>
                  </li>
                ))}

                <li className={`page-item ${page === totalPages ? 'disabled' : ''}`}>
                  <button className="page-link" onClick={() => setPage(Math.min(totalPages, page + 1))}>Next</button>
                </li>
              </ul>
            </nav>
          </div>
        </>
      )}
    </div>
  )
}
