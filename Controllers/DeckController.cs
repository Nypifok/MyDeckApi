using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyDeckAPI.Data.MediaContent;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckAPI.Services;



namespace MyDeckAPI.Controllers
{
   // [Authorize]
    [Route("mydeckapi/[controller]")]
    public class DeckController : Controller
    {
        
        private readonly DeckRepository db;
        private readonly ILogger<DeckController> logger;
        private readonly SnakeCaseConverter snakeCaseConverter;

        public DeckController(IGenericRepository context, ILogger<DeckController> _logger,SnakeCaseConverter snakeCaseConverter)
        {
            db = (DeckRepository)context;
            logger = _logger;
            this.snakeCaseConverter = snakeCaseConverter;
        }

        [HttpGet("[action]")]
        public IActionResult FindAll()
        {

            try
            {
                var content = db.FindAll();
                logger.LogInformation("------------> All decks have been returned <------------");
                return Ok(snakeCaseConverter.ConvertToSnakeCase(content));
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /*[HttpGet("[action]/{id}")]
        public IActionResult FindById(Guid id)
        {
            try
            {
                var content = db.WatchDeck(id);
                if (content != null)
                {
                    logger.LogInformation("------------> Deck have been returned <------------");
                    return Ok(content);
                }
                else
                {
                    logger.LogWarning("------------> Deck not found <------------");
                    return NotFound("Deck not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }*/


       /* [HttpPost("[action]")]
        public async Task<IActionResult> Insert([FromForm] FilledDeck value)
        {
            try
            {


                var content =await db.Insert(value);
                

               // db.Save();
                logger.LogInformation("------------> Deck/s have been added <------------");
                return Ok(content);
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }*/



        /*[HttpDelete("[action]/{id}")]
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
                    return NotFound("Deck / s not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }*/


       /* [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromForm] FilledDeck value)
        {

           
               var content= await db.Update(value);

            
            db.Save();
            logger.LogInformation("------------> Deck/s have been updated <------------");
            return Ok(content);

        }*/

        [HttpGet("[action]/{id}")]
        public IActionResult AllCurrentUserDecks(string id)
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
                    return NotFound("Deck/s not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet("[action]/{category}/{page}")]
        public IActionResult ChosenCategoryFeed(string category, int page)
        {
            try
            {
                var content = db.ChosenCategoryFeed(category, page);
                if (content != "[]")
                {
                    logger.LogInformation("------------> Deck/s have been returned <------------");
                    return Ok(content);
                }
                else
                {
                    logger.LogWarning("------------> Deck/s not found <------------");
                    return NotFound("Deck/s not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{id}")]
        public IActionResult AllUserDecks(Guid id)
        {
            try
            {
                var content = db.AllUserDecks(id);
                if (content != "[]")
                {
                    logger.LogInformation("------------> Deck/s have been returned <------------");
                    return Ok(content);
                }
                else
                {
                    logger.LogWarning("------------> Deck/s not found <------------");
                    return NotFound("Deck/s not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{id}")]
        public IActionResult AllCurrentUserDecksWithCards(string id)
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
                    return NotFound("Deck / s not found");
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
