import React from 'react'
import { logout } from '../services/authService'

export default function Topbar(){
  return (
    <nav className="navbar navbar-expand navbar-light bg-white topbar mb-4 static-top shadow">
      <div className="container-fluid">
        <h5>Dashboard</h5>
        <div className="ms-auto">
          <button className="btn btn-sm btn-outline-secondary" onClick={() => { logout(); window.location.href = '/login' }}>
            Logout
          </button>
        </div>
      </div>
    </nav>
  )
}
