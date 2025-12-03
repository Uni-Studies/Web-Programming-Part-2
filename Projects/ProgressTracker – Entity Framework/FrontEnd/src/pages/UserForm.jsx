import React, { useState, useEffect } from 'react'

export default function UserForm({ initial = {}, onSave, onCancel }){
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [firstName, setFirstName] = useState('')
  const [lastName, setLastName] = useState('')

  useEffect(()=>{
    if(initial){
      setUsername(initial.username || '')
      setFirstName(initial.firstName || '')
      setLastName(initial.lastName || '')
    }
  }, [initial])

  async function submit(e){
    e.preventDefault()
    const payload = { username, password, firstName, lastName }
    await onSave(payload)
  }

  return (
    <div className="card mt-3 p-3">
      <form onSubmit={submit}>
        <div className="mb-2">
          <label className="form-label">Username</label>
          <input className="form-control" value={username} onChange={e=>setUsername(e.target.value)} />
        </div>
        <div className="mb-2">
          <label className="form-label">Password</label>
          <input type="password" className="form-control" value={password} onChange={e=>setPassword(e.target.value)} />
        </div>
        <div className="mb-2">
          <label className="form-label">First Name</label>
          <input className="form-control" value={firstName} onChange={e=>setFirstName(e.target.value)} />
        </div>
        <div className="mb-2">
          <label className="form-label">Last Name</label>
          <input className="form-control" value={lastName} onChange={e=>setLastName(e.target.value)} />
        </div>
        <div className="d-flex gap-2">
          <button className="btn btn-primary" type="submit">Save</button>
          <button type="button" className="btn btn-secondary" onClick={onCancel}>Cancel</button>
        </div>
      </form>
    </div>
  )
}
