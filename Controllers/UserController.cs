using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;

namespace MyDeckAPI.Controllers
{
    [Route("mydeckapi/[controller]")]
    public class UserController : Controller
    {
        private readonly IGenericRepository<User> db;
        private readonly ILogger<UserController> logger;

        public UserController(ILogger<UserController> _logger, IGenericRepository<User> context)
        {
            db = context;
            logger = _logger;
        }


        [HttpGet("[action]")]
        public IActionResult FindAll()
        {
            var content = db.FindAll();
            logger.LogInformation("------------> All users have been returned <------------");
            return Ok(Json(content));
        }

        // GET api/<controller>/5
        [HttpGet("[action]/{id}")]
        public IActionResult FindById(int id)
        {
            var content = db.FindById(id);
            if (content != null)
            {
                logger.LogInformation("------------> User have been returned <------------");
                return Ok(Json(content));
            }
            else
            {
                logger.LogWarning("------------> User not found <------------");
                return BadRequest();
            }
        }

        // POST api/<controller>
        [HttpPost("[action]")]
        public IActionResult Insert([FromBody] IEnumerable<User> value)
        {
            var content = value;
            foreach (User usr in content)
            {
                db.Insert(usr);
            }

            db.Save();
            logger.LogInformation("------------> User/s have been added <------------");
            return Ok();
        }

        // PUT api/<controller>/5
        [HttpPut("[action]")]
        public IActionResult Update([FromBody]IEnumerable<User> value)
        {
            var content = value;
            
            foreach (User usr in content)
            {
                db.Update(usr);
            }

            db.Save();
            logger.LogInformation("------------> User/s have been updated <------------");
            return Ok();
        }


        [HttpDelete("[action]/{id}")]
        public IActionResult DeleteById(int id)
        {
            var content = db.FindById(id);
            if (content != null)
            {
                db.Delete(id);
                db.Save();
                logger.LogInformation("------------> User have been deleted <------------");
                return Ok();
            }
            else
            {
                logger.LogWarning("------------> User not found <------------");
                return BadRequest();
            }
        }
    }
}
