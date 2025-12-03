import React, { useState } from 'react'
import { login, getToken } from '../services/authService'
import { useNavigate } from 'react-router-dom'

export default function Login(){
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState(null)
  const navigate = useNavigate()

  async function handleSubmit(e){
    e.preventDefault()
    setError(null)
    try{
      await login(username, password)
      navigate('/')
    }catch(err){
      setError(err.message || 'Login failed')
    }
  }

  return (
    <div className="d-flex justify-content-center align-items-center" style={{height:'100vh'}}>
      <div className="card p-4" style={{width:360}}>
        <h5 className="card-title">Login</h5>
        <form onSubmit={handleSubmit}>
          <div className="mb-3">
            <label className="form-label">Username</label>
            <input className="form-control" value={username} onChange={e=>setUsername(e.target.value)} />
          </div>
          <div className="mb-3">
            <label className="form-label">Password</label>
            <input type="password" className="form-control" value={password} onChange={e=>setPassword(e.target.value)} />
          </div>
          {error && <div className="alert alert-danger">{error}</div>}
          <button className="btn btn-primary w-100" type="submit">Login</button>
        </form>
      </div>
    </div>
  )
}
