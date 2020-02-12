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
    public class DeckController : Controller
    {
        private readonly IGenericRepository<Deck> db;
        private readonly ILogger<DeckController> logger;

        public DeckController(IGenericRepository<Deck> context, ILogger<DeckController> _logger)
        {
            db = context;
            logger = _logger;
        }
        [HttpGet("[action]")]
        public IActionResult FindAll()
        {
            var content = db.FindAll();
            logger.LogInformation("------------> All decks have been returned <------------");
            return Ok(Json(content));
        }

        [HttpGet("[action]/{id}")]
        public IActionResult FindById(Guid id)
        {
            var content = db.FindById(id);
            if (content != null)
            {
                logger.LogInformation("------------> Deck have been returned <------------");
                return Ok(Json(content));
            }
            else
            {
                logger.LogWarning("------------> Deck not found <------------");
                return BadRequest();
            }
        }


        [HttpPost("[action]")]
        public IActionResult Insert([FromBody] IEnumerable<Deck> value)
        {
            var content = value;
            foreach (Deck deck in content)
            {
                db.Insert(deck);
            }

            db.Save();
            logger.LogInformation("------------> Deck/s have been added <------------");
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
                logger.LogInformation("------------> Deck have been deleted <------------");
                return Ok();
            }
            else
            {
                logger.LogWarning("------------> Deck not found <------------");
                return BadRequest();
            }
        }


        [HttpPut("[action]")]
        public IActionResult Update([FromBody]IEnumerable<Deck> value)
        {
            var content = value;

            foreach (Deck deck in content)
            {
                db.Update(deck);
                
            }

            db.Save();
            logger.LogInformation("------------> Deck/s have been updated <------------");
            return Ok();
        }
    }
}
