using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using MvcApiSample.Entities;
using MvcApiSample.Repositories;
using System.Reflection;

namespace MvcApiSample.Controllers
{
    public class UsersController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Json(UserRepository.Items); // create JSONResult instances and returns it such as View and ViewResult
            // before the instruction UserRepository.Items the static constructor for the list will be called
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            return View();
        }

        [HttpPut]
        public IActionResult Create([FromBody] User model)
        {
            int id = 1;
            foreach(User item in UserRepository.Items)
            {
                if(id <= item.Id)
                {
                    id = item.Id + 1;
                }
            }
            model.Id = id;
            UserRepository.Items.Add(model);
            
            return Created(model.Id.ToString(), model); // controller method; returns CreatedResult - successor of IActionResult 
        }

        public IActionResult Update(User model)
        {
            return View();
        }

        [HttpDelete]
        public IActionResult Delete(int id) // this id is expected as a HTTP parameter and not from the body
        {
            User user = null;
            foreach (User item in UserRepository.Items)
            {
                if (id == item.Id)
                {
                    user = item;
                    break;
                }
            }
            if(user != null)
            {
                UserRepository.Items.Remove(user);
                return Ok(user);
            }

            return NotFound(user);
        }
    }
}
