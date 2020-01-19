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
    public class CardController : Controller
    {
        private readonly IGenericRepository<Card> db;
        private readonly ILogger<CardController> logger;

        public CardController(ILogger<CardController> _logger, IGenericRepository<Card> context)
        {
            db = context;
            logger = _logger;
        }


        [HttpGet("[action]")]
        public IActionResult FindAll()
        {
            var content = db.FindAll();
            logger.LogInformation("------------> All cards have been returned <------------");
            return Ok(Json(content));
        }

       
        [HttpGet("[action]/{id}")]
        public IActionResult FindById(int id)
        {
            var content = db.FindById(id);
            if (content != null)
            {
                logger.LogInformation("------------> Card have been returned <------------");
                return Ok(Json(content));
            }
            else
            {
                logger.LogWarning("------------> Card not found <------------");
                return BadRequest();
            }
        }

        
        [HttpPost("[action]")]
        public IActionResult Insert([FromBody] IEnumerable<Card> value)
        {
            var content = value;
            foreach (Card crd in content)
            {
                db.Insert(crd);
            }

            db.Save();
            logger.LogInformation("------------> Card/s have been added <------------");
            return Ok();
        }

       
        [HttpPut("[action]")]
        public IActionResult Update([FromBody]IEnumerable<Card> value)
        {
            var content = value;

            foreach (Card crd in content)
            {
                db.Update(crd);
            }

            db.Save();
            logger.LogInformation("------------> Card/s have been updated <------------");
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
                logger.LogInformation("------------> Card have been deleted <------------");
                return Ok();
            }
            else
            {
                logger.LogWarning("------------> Card not found <------------");
                return BadRequest();
            }
        }
    }
}
