import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../services/authService'

export default function Login() {
  const [username, setUsername] = useState('admin')
  const [password, setPassword] = useState('adminpass')
  const [error, setError] = useState(null)
  const { login } = useAuth()
  const navigate = useNavigate()

  async function submit(e) {
    e.preventDefault()
    setError(null)
    try {
      await login(username, password)
      navigate('/users')
    } catch (err) {
      setError(err.message || 'Login failed')
    }
  }

  return (
    <div className="row justify-content-center">
      <div className="col-md-6">
        <div className="card shadow">
          <div className="card-body">
            <h3 className="card-title">Login</h3>
            {error && <div className="alert alert-danger">{error}</div>}
            <form onSubmit={submit}>
              <div className="mb-3">
                <label>Username</label>
                <input className="form-control" value={username} onChange={e => setUsername(e.target.value)} />
              </div>
              <div className="mb-3">
                <label>Password</label>
                <input type="password" className="form-control" value={password} onChange={e => setPassword(e.target.value)} />
              </div>
              <button className="btn btn-primary">Login</button>
            </form>
          </div>
        </div>
      </div>
    </div>
  )
}
