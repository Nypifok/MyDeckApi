using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckAPI.Services;

namespace MyDeckAPI.Controllers
{
    [Route("mydeckapi/[controller]")]
    public class CardController : Controller
    {
        private readonly CardRepository<Card> db;
        private readonly ILogger<CardController> logger;

        public CardController(ILogger<CardController> _logger, IGenericRepository<Card> context)
        {
            db = (CardRepository<Card>)context;
            logger = _logger;
        }


        [HttpGet("[action]")]
        public IActionResult FindAll()
        {
            try
            {
                var content = db.FindAll();
                logger.LogInformation("------------> All cards have been returned <------------");
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
                    logger.LogInformation("------------> Card have been returned <------------");
                    return Ok(Json(content));
                }
                else
                {
                    logger.LogWarning("------------> Card not found <------------");
                    return BadRequest("Card not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("[action]")]
        public IActionResult Insert([FromBody] IEnumerable<Card> value)
        {
            try
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
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("[action]")]
        public IActionResult Update([FromBody]IEnumerable<Card> value)
        {
            try
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
                    logger.LogInformation("------------> Card have been deleted <------------");
                    return Ok();
                }
                else
                {
                    logger.LogWarning("------------> Card not found <------------");
                    return BadRequest("Card not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
