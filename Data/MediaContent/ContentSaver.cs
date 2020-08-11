using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MyDeckAPI.Data.MediaContent
{
    public class ContentSaver
    {
        public virtual async Task<string> Save(IFormFile file)
        {
                var currentDirectory = Directory.GetCurrentDirectory();
                var dataPath = Path.GetDirectoryName(currentDirectory);
                Directory.CreateDirectory(dataPath + @"/UsersData/");
                var randomName = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(file.FileName);

                using (var stream = File.Create(dataPath + @"/UsersData/" + randomName + extension))
                {
                    await file.CopyToAsync(stream);
                }

                return randomName;           
        }
        public virtual async Task<string> Save(IFormFile file, string path)
        {
            Directory.CreateDirectory(path);
            var randomName = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(file.FileName);
            if (extension == ".jpeg" || extension == ".jpg" || extension == ".png")
            {
                using (var stream = new FileStream(path + randomName + extension, FileMode.OpenOrCreate))
                {
                    await file.CopyToAsync(stream);
                }

                return randomName;
            }
            return null;
        }
        public async Task DownloadPicture(string url)
        {
            using (var client = new WebClient())
            {
                var content = client.DownloadData(url);
                var stream = new MemoryStream(content);

            }
        }
    }
}
