using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektSklep.Services
{
    public class FileService : IFileService
    {
        public FileService()
        {

        }

        public bool IsImageValid(IFormFile file)
        {
            using var image = Image.Load(file.OpenReadStream());
            if (image.Width <= 1000 && image.Height <= 1000)
            {
                return true;
            }
            return false;
        }

        public string UploadImage(IFormFile file)
        {
            var uploadPath = "~/Content/Images/Products/";
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using var image = Image.Load(file.OpenReadStream());
            image.Mutate(x => x.Resize(256, 256));
            image.Save(filePath);

            return filePath;
        }
    }
}
