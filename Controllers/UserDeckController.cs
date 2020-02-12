using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyDeckAPI.Controllers
{
    [Route("mydeckapi/[controller]")]
    public class UserDeckController : Controller
    {
        private readonly IGenericRepository<UserDeck> db;
        private readonly ILogger<UserDeckController> logger;

        public UserDeckController(ILogger<UserDeckController> _logger, IGenericRepository<UserDeck> context)
        {
            db = context;
            logger = _logger;
        }


        [HttpGet("[action]")]
        public IActionResult FindAll()
        {
            var content = db.FindAll();
            logger.LogInformation("------------> All userdecks have been returned <------------");
            return Ok(Json(content));
        }

        // GET api/<controller>/5
        [HttpGet("[action]/{id}")]
        public IActionResult FindById(Guid id)
        {
            var content = db.FindById(id);
            if (content != null)
            {
                logger.LogInformation("------------> Userdeck have been returned <------------");
                return Ok(Json(content));
            }
            else
            {
                logger.LogWarning("------------> Userdeck not found <------------");
                return BadRequest();
            }
        }

        // POST api/<controller>
        [HttpPost("[action]")]
        public IActionResult Insert([FromBody] IEnumerable<UserDeck> value)
        {
            var content = value;
            foreach (UserDeck usrdck in content)
            {
                db.Insert(usrdck);
            }

            db.Save();
            logger.LogInformation("------------> Userdeck/s have been added <------------");
            return Ok();
        }

        // PUT api/<controller>/5
        [HttpPut("[action]")]
        public IActionResult Update([FromBody]IEnumerable<UserDeck> value)
        {
            var content = value;

            foreach (UserDeck usrdck in content)
            {
                db.Update(usrdck);
            }

            db.Save();
            logger.LogInformation("------------> Userdeck/s have been updated <------------");
            return Ok();
        }


        [HttpDelete("[action]/{id}")]
        public IActionResult DeleteById(Guid id)
        {
            var content = db.FindById(id);
            if (content != null)
            {
                db.Delete(id);
                db.Save();
                logger.LogInformation("------------> Userdeck have been deleted <------------");
                return Ok();
            }
            else
            {
                logger.LogWarning("------------> Userdeck not found <------------");
                return BadRequest();
            }
        }
    }
}

