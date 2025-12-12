// Mirrors Common.Entities.User
export default class User {
  constructor(data = {}) {
    this.id = data.id
    this.username = data.username || ''
    this.password = data.password || ''
    this.firstName = data.firstName || ''
    this.lastName = data.lastName || ''
  }
}
