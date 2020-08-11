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
    [Authorize]
    [Route("mydeckapi/[controller]")]
    public class UserDeckController : Controller
    {
        private readonly UserDeckRepository db;
        private readonly ILogger<UserDeckController> logger;
        private readonly SnakeCaseConverter snakeCaseConverter;

        public UserDeckController(ILogger<UserDeckController> _logger, IGenericRepository context, SnakeCaseConverter snakeCaseConverter)
        {
            db = (UserDeckRepository)context;
            logger = _logger;
            this.snakeCaseConverter = snakeCaseConverter;
        }


        [HttpGet("[action]")]
        public IActionResult FindAll()
        {
            try
            {
                var content = db.FindAll();
                logger.LogInformation("------------> All userdecks have been returned <------------");
                return Ok(snakeCaseConverter.ConvertToSnakeCase(content));
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
                    logger.LogInformation("------------> Userdeck have been returned <------------");
                    return Ok(snakeCaseConverter.ConvertToSnakeCase(content));
                }
                else
                {
                    logger.LogWarning("------------> Userdeck not found <------------");
                    return NotFound("Userdeck not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("[action]")]
        public IActionResult Insert([FromBody] IEnumerable<UserDeck> value)
        {
            try
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
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
                return BadRequest(ex.Message);
            }
        }        

        [HttpPut("[action]")]
        public IActionResult Update([FromBody]IEnumerable<UserDeck> value)
        {
            try
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
                    logger.LogInformation("------------> Userdeck have been deleted <------------");
                    return Ok();
                }
                else
                {
                    logger.LogWarning("------------> Userdeck not found <------------");
                    return NotFound("Userdeck not found");
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

