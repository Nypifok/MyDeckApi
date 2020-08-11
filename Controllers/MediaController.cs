using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyDeckAPI.Data.MediaContent;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyDeckAPI.Controllers
{
    [Route("mydeckapi/[controller]")]
    public class MediaController : Controller
    {

        private readonly ILogger<DeckController> logger;

        public MediaController( ILogger<DeckController> _logger)
        {
            logger = _logger;
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Media(Guid id)
        {
            try
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                var dataPath = Path.GetDirectoryName(currentDirectory);
                var imageFileStream = System.IO.File.OpenRead(dataPath + @"/UsersData/" + id.ToString() + ".jpg");
                return File(imageFileStream, "image/jpeg");
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
