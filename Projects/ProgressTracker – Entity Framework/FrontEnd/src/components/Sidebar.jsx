import React from 'react'
import { NavLink } from 'react-router-dom'

export default function Sidebar(){
  return (
    <nav className="sidebar bg-gradient-primary">
      <div className="sidebar-heading text-white p-3">Progress Tracker</div>
      <ul className="nav flex-column">
        <li className="nav-item">
          <NavLink to="/users" className="nav-link text-white">Users</NavLink>
        </li>
      </ul>
    </nav>
  )
}
