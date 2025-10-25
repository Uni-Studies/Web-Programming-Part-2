using System.Linq;
using API.Services;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateToken([FromForm]string username, [FromForm]string password)
        {
            UsersServices service = new UsersServices();
            User loggedUser = service.GetAll()
                                        .FirstOrDefault(u =>
                                            u.Username == username &&
                                            u.Password == password);

            TokenServices tokenService = new TokenServices();
            string token = tokenService.CreateToken(loggedUser);

            return Ok(new
            {
                token = token
            });
        }
    }
}
