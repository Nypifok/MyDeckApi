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
    public class SubscribeController : Controller
    {
        private readonly SubscribeRepository db;
        private readonly ILogger<SubscribeController> logger;
        private readonly SnakeCaseConverter snakeCaseConverter;

        public SubscribeController(ILogger<SubscribeController> _logger, IGenericRepository context, SnakeCaseConverter snakeCaseConverter)
        {
            db =(SubscribeRepository)context;
            logger = _logger;
            this.snakeCaseConverter = snakeCaseConverter;
        }


        [HttpGet("[action]")]
        public IActionResult FindAll()
        {
            try
            {
                var content = db.FindAll();
                logger.LogInformation("------------> All subscribes have been returned <------------");
                return Ok(snakeCaseConverter.ConvertToSnakeCase(content));
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ex.Message);
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
                    logger.LogInformation("------------> Subscribe have been returned <------------");
                    return Ok(snakeCaseConverter.ConvertToSnakeCase(content));
                }
                else
                {
                    logger.LogWarning("------------> Subscribe not found <------------");
                    return NotFound("Subscribe not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
                return BadRequest(ex.Message);
            }
        }

   
        [HttpPost("[action]")]
        public IActionResult Insert([FromBody] IEnumerable<Subscribe> value)
        {
            try
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
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
                return BadRequest(ex.Message);
            }
        }

  
        [HttpPut("[action]")]
        public IActionResult Update([FromBody]IEnumerable<Subscribe> value)
        {
            try
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
                    logger.LogInformation("------------> Subscribe have been deleted <------------");
                    return Ok();
                }
                else
                {
                    logger.LogWarning("------------> Subscribe not found <------------");
                    return NotFound("Subscribe not found");
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
