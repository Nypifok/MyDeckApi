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
    public class SubscribeController : Controller
    {
        private readonly IGenericRepository<Subscribe> db;
        private readonly ILogger<SubscribeController> logger;

        public SubscribeController(ILogger<SubscribeController> _logger, IGenericRepository<Subscribe> context)
        {
            db = context;
            logger = _logger;
        }


        [HttpGet("[action]")]
        public IActionResult FindAll()
        {
            var content = db.FindAll();
            logger.LogInformation("------------> All subscribes have been returned <------------");
            return Ok(Json(content));
        }

        // GET api/<controller>/5
        [HttpGet("[action]/{id}")]
        public IActionResult FindById(Guid id)
        {
            var content = db.FindById(id);
            if (content != null)
            {
                logger.LogInformation("------------> Subscribe have been returned <------------");
                return Ok(Json(content));
            }
            else
            {
                logger.LogWarning("------------> Subscribe not found <------------");
                return BadRequest();
            }
        }

        // POST api/<controller>
        [HttpPost("[action]")]
        public IActionResult Insert([FromBody] IEnumerable<Subscribe> value)
        {
            var content = value;
            foreach (Subscribe sbs in content)
            {
                db.Insert(sbs);
            }

            db.Save();
            logger.LogInformation("------------> Subscribe/s have been added <------------");
            return Ok();
        }

        // PUT api/<controller>/5
        [HttpPut("[action]")]
        public IActionResult Update([FromBody]IEnumerable<Subscribe> value)
        {
            var content = value;

            foreach (Subscribe sbs in content)
            {
                db.Update(sbs);
            }

            db.Save();
            logger.LogInformation("------------> Subscribe/s have been updated <------------");
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
                logger.LogInformation("------------> Subscribe have been deleted <------------");
                return Ok();
            }
            else
            {
                logger.LogWarning("------------> Subscribe not found <------------");
                return BadRequest();
            }
        }
    }
}
