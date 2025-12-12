import React from 'react'
import { NavLink } from 'react-router-dom'

export default function Sidebar() {
  return (
    <ul className="navbar-nav bg-gradient-primary sidebar sidebar-dark accordion" id="accordionSidebar">
      <a className="sidebar-brand d-flex align-items-center justify-content-center" href="/">
        <div className="sidebar-brand-icon rotate-n-15">
          <i className="fas fa-laugh-wink"></i>
        </div>
        <div className="sidebar-brand-text mx-3">ProgressTracker</div>
      </a>

      <hr className="sidebar-divider my-0" />

      <li className="nav-item">
        <NavLink className="nav-link" to="/users">
          <i className="fas fa-users"></i>
          <span>Users</span>
        </NavLink>
      </li>

      <hr className="sidebar-divider d-none d-md-block" />
    </ul>
  )
}
