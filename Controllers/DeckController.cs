using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckAPI.Services;



namespace MyDeckAPI.Controllers
{   
    [Route("mydeckapi/[controller]")]
    public class DeckController : Controller
    {
        private readonly DeckRepository<Deck> db;
        private readonly ILogger<DeckController> logger;

        public DeckController(IGenericRepository<Deck> context, ILogger<DeckController> _logger)
        {
            db = (DeckRepository<Deck>)context;
            logger = _logger;
        }
        [HttpGet("[action]")]
        public IActionResult FindAll()
        {
            
            try
            {
                var content = db.FindAll();
                logger.LogInformation("------------> All decks have been returned <------------");
                return Ok(Json(content));
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]/{id}")]
        public IActionResult FindById(Guid id)
        {
            try
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
                    return BadRequest("Deck not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("[action]")]
        public IActionResult Insert([FromBody] IEnumerable<Deck> value)
        {
            try
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
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
                return BadRequest(ex.Message);
            }
        }



        [HttpDelete("[action]/{id}")]
        public IActionResult DeleteById(Guid id)
        {
            try
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
                    return BadRequest("Deck / s not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
                return BadRequest(ex.Message);
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

        [HttpGet("[action]/{login}")]
        public IActionResult AllCurrentUserDecks(string login)
        {
            try
            {
                var content = db.AllCurrentUserDecks(login);
                if (content !="[]")
                {
                    logger.LogInformation("------------> Deck/s have been returned <------------");
                    return Ok(content);
                }
                else
                {
                    logger.LogWarning("------------> Deck/s not found <------------");
                    return BadRequest("Deck/s not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{id}")]
        public IActionResult AllCurrentUserDecksById(Guid id)
        {
            try
            {
                var content = db.AllCurrentUserDecks(id);
                if (content != "[]")
                {
                    logger.LogInformation("------------> Deck/s have been returned <------------");
                    return Ok(content);
                }
                else
                {
                    logger.LogWarning("------------> Deck/s not found <------------");
                    return BadRequest("Deck/s not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{login}")]
        public IActionResult AllCurrentUserDecksWithCards(string login)
        {
            try
            {
                var content = db.AllCurrentUserDecksWithCards(login);
                if (content != "[]")
                {
                    logger.LogInformation("------------> Deck/s have been returned <------------");
                    return Ok(content);
                }
                else
                {
                    logger.LogWarning("------------> Deck/s not found <------------");
                    return BadRequest("Deck / s not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]/{id}")]
        public IActionResult AllCurrentUserDecksWithCardsById(Guid id)
        {
            try
            {
                var content = db.AllCurrentUserDecksWithCards(id);
                if (content != "[]")
                {
                    logger.LogInformation("------------> Deck/s have been returned <------------");
                    return Ok(content);
                }
                else
                {
                    logger.LogWarning("------------> Deck/s not found <------------");
                    return BadRequest("Deck / s not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }

    }

}
