using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UsersRestApi.Entities;
using UsersRestApi.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UsersRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public IActionResult Get() //IEnumerable<string> --> IActionResult or List<User>; if the list is used, then the framework will determine the response type
        {
            return Ok(UsersRepository.Items);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            User item = UsersRepository.Items.Find(u => u.Id == id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT api/<UsersController>/5
        [HttpPut]
        public IActionResult Put([FromBody] User model)
        {
            int id = 1;
            foreach (User item in UsersRepository.Items)
            {
                if (id <= item.Id)
                {
                    id = item.Id + 1;
                }
            }
            model.Id = id;
            UsersRepository.Items.Add(model);

            return Created(model.Id.ToString(), model);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete] // id can come as HTTP parameter or route parameter
        public IActionResult Delete(int id)
        {
            User user = null;
            foreach (User item in UsersRepository.Items)
            {
                if (id == item.Id)
                {
                    user = item;
                    break;
                }
            }
            if (user != null)
            {
                UsersRepository.Items.Remove(user);
                return Ok(user);
            }

            return NotFound(user);
        }
    }
}
