import React, { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import usersService from '../../services/usersService'

export default function UserForm() {
  const { id } = useParams()
  const navigate = useNavigate()
  const [user, setUser] = useState({ username: '', password: '', firstName: '', lastName: '' })

  useEffect(() => {
    if (id) {
      usersService.getUser(id).then(r => setUser(r.data))
    }
  }, [id])

  async function submit(e) {
    e.preventDefault()
    if (id) await usersService.updateUser(id, user)
    else await usersService.createUser(user)
    navigate('/users')
  }

  return (
    <div className="col-md-6">
      <div className="card shadow">
        <div className="card-body">
          <h3 className="card-title">{id ? 'Edit' : 'New'} User</h3>
          <form onSubmit={submit}>
            <div className="mb-3">
              <label>Username</label>
              <input className="form-control" value={user.username} onChange={e => setUser({...user, username: e.target.value})} />
            </div>
            <div className="mb-3">
              <label>Password</label>
              <input type="password" className="form-control" value={user.password} onChange={e => setUser({...user, password: e.target.value})} />
            </div>
            <div className="mb-3">
              <label>First Name</label>
              <input className="form-control" value={user.firstName} onChange={e => setUser({...user, firstName: e.target.value})} />
            </div>
            <div className="mb-3">
              <label>Last Name</label>
              <input className="form-control" value={user.lastName} onChange={e => setUser({...user, lastName: e.target.value})} />
            </div>
            <button className="btn btn-primary">Save</button>
          </form>
        </div>
      </div>
    </div>
  )
}
