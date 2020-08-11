using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckAPI.Security;
using MyDeckAPI.Services;
using Newtonsoft.Json;

namespace MyDeckAPI.Controllers
{
    [Authorize]
    [Route("mydeckapi/[controller]")]
    public class UserController : Controller
    {

        private readonly UserRepository db;
        private readonly ILogger<UserController> logger;
        private readonly SnakeCaseConverter snakeCaseConverter;

        public UserController(ILogger<UserController> _logger, UserRepository context, SnakeCaseConverter snakeCaseConverter)
        {
            db = (UserRepository)context;
            logger = _logger;
            this.snakeCaseConverter = snakeCaseConverter;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult FindAll()
        {
            try
            {
                var content = db.FindAll();
                logger.LogInformation("------------> All users have been returned <------------");
                return Ok(snakeCaseConverter.ConvertToSnakeCase(content));
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
                    return Ok(snakeCaseConverter.ConvertToSnakeCase(content));
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
        public async Task<IActionResult> SignInByGoogle([FromHeader(Name = "idtoken")] string idtoken)
        {
            try
            {
                string sessionId;
                Request.Cookies.TryGetValue("sessionId", out sessionId);
                if (sessionId == null) { throw new Exception("Empty SessionId");}
                var tmp = await db.SignInByGoogle(idtoken, Guid.Parse(sessionId));
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
        public async Task<IActionResult> RefreshTokens([FromBody]Tokens value)
        {
            try
            {
                string sessionId;
                Request.Cookies.TryGetValue("sessionId", out sessionId);
                if (sessionId == null) { throw new Exception("Empty SessionId"); }

                 var tmp = await db.RefreshTokens(value,Guid.Parse(sessionId));
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
                    return Ok(snakeCaseConverter.ConvertToSnakeCase(content));
                }
                else
                {
                    logger.LogWarning("------------> User is not unique <------------");
                    return BadRequest(snakeCaseConverter.ConvertToSnakeCase(content));
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
        public async Task<IActionResult> SignUpWithEmail([FromBody] User usr)
        {
            try
            {
                string sessionId;
                Request.Cookies.TryGetValue("sessionId", out sessionId);
                if (sessionId == null) { throw new Exception("Empty SessionId"); }
                var response=await db.SignUpWithEmail(usr,Guid.Parse(sessionId));
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost("[action]/{token}")]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            try
            {
                var obj = new { Username = "asdasd", Email = "asdasda", Password = Encoding.ASCII.GetBytes("sadsadsadsad") };
                return Ok(Json(obj));
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
