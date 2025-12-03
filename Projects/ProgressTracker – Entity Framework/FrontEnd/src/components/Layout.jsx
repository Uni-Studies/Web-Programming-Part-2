import React from 'react'
import { Outlet } from 'react-router-dom'
import Sidebar from './Sidebar'
import Topbar from './Topbar'

export default function Layout(){
  return (
    <div id="wrapper" className="d-flex">
      <Sidebar />
      <div id="content-wrapper" className="flex-grow-1 d-flex flex-column">
        <Topbar />
        <div className="container-fluid">
          <Outlet />
        </div>
      </div>
    </div>
  )
}
