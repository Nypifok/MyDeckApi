using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckAPI.Security;
using MyDeckAPI.Services;


namespace MyDeckAPI.Controllers
{
    [Authorize]
    [Route("mydeckapi/[controller]")]
    public class UserController : Controller
    {

        private readonly UserRepository<User> db;
        private readonly ILogger<UserController> logger;

        public UserController(ILogger<UserController> _logger, IGenericRepository<User> context)
        {

            db = (UserRepository<User>)context;
            logger = _logger;

        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult FindAll()
        {
            try
            {
                var content = db.FindAll();
                logger.LogInformation("------------> All users have been returned <------------");
                return Ok(Json(content));
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
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
                    logger.LogInformation("------------> User have been returned <------------");
                    return Ok(Json(content));
                }
                else
                {
                    logger.LogWarning("------------> User not found <------------");
                    return NotFound("User not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet("[action]/{id}")]
        public IActionResult UserProfile(Guid id)
        {
            try
            {
                var content = db.UserProfile(id);
                if (content != null)
                {
                    logger.LogInformation("------------> Profile have been returned <------------");
                    return Ok(content);
                }
                else
                {
                    logger.LogWarning("------------> User not found <------------");
                    return NotFound("User not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult SignInByGoogle([FromHeader(Name = "idtoken")] string idtoken)
        {
            try
            {
                var tmp = db.SignInByGoogle(idtoken);
                if (tmp != null)
                {
                    logger.LogWarning("------------> U are signed in <------------ \n");
                    return Ok(tmp);
                }
                logger.LogWarning("------------> U are not signed in <------------ \n");
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult RefreshTokens([FromBody]Tokens value)
        {
            try
            {
                var tmp = db.RefreshTokens(value);
                if (tmp != null)
                {
                    logger.LogWarning("------------> Token has been refreshed <------------ \n");
                    return Ok(tmp);
                }
                logger.LogWarning("------------> Token has not been refreshed <------------ \n");
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("[action]")]
        public IActionResult Update([FromBody]IEnumerable<User> value)
        {
            try
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
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles ="Owner, User")]
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
                    logger.LogInformation("------------> User have been deleted <------------");
                    return Ok();
                }
                else
                {
                    logger.LogWarning("------------> User not found <------------");
                    return NotFound("User not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{username}")]
        public IActionResult IsUserNameUnique(string username)
        {
            try
            {
                var content = db.IsUserNameUnique(username);
                if (content)
                {
                    logger.LogInformation("------------> User is unique <------------");
                    return Ok(Json(content));
                }
                else
                {
                    logger.LogWarning("------------> User is not unique <------------");
                    return BadRequest(Json(content));
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
