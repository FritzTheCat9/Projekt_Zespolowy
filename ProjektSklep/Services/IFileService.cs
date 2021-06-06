using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektSklep.Services
{
    public interface IFileService
    {
        string UploadImage(IFormFile file);
        bool IsImageValid(IFormFile image);
    }
}
