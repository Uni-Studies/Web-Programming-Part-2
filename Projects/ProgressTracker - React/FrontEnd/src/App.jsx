import React from 'react'
import { Routes, Route, Navigate } from 'react-router-dom'
import Login from './pages/Login'
import UsersList from './pages/Users/UsersList'
import UserForm from './pages/Users/UserForm'
import Sidebar from './components/Sidebar'
import Topbar from './components/Topbar'
import { AuthProvider, useAuth } from './services/authService'

function PrivateRoute({ children }) {
  const { token } = useAuth()
  return token ? children : <Navigate to="/login" />
}

export default function App() {
  return (
    <AuthProvider>
      <div id="wrapper">
        <Sidebar />
        <div id="content-wrapper" className="d-flex flex-column">
          <div id="content">
            <Topbar />
            <div className="container-fluid">
              <Routes>
                <Route path="/login" element={<Login />} />
                <Route path="/users" element={<PrivateRoute><UsersList /></PrivateRoute>} />
                <Route path="/users/new" element={<PrivateRoute><UserForm /></PrivateRoute>} />
                <Route path="/users/:id" element={<PrivateRoute><UserForm /></PrivateRoute>} />
                <Route path="/" element={<Navigate to="/users" />} />
              </Routes>
            </div>
          </div>
        </div>
      </div>
    </AuthProvider>
  )
}
