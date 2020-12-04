using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Http;
using System.IO;

/* https://www.youtube.com/watch?v=pLuHhQBBPe8 - poradnik odnosnie kodu */

namespace ProjektSklep.Controllers
{
    public class FileUploadController : Controller
    {
        //lista dozwolonych formatow
        private List<string> allowedExtensions = new List<string>() { "jpg", "pdf" };

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("FileUpload")]
        public async Task<IActionResult> Index(List<IFormFile> files)
        {
            //laczny rozmiar plikow
            var size = files.Sum(f => f.Length);

            //lista sciezek do zalaczonych plikow
            var filePaths = new List<string>();
            //dla kazdego pliku, ktory zostal wybrany...
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    //sprawdzenie, czy zuploadowano akceptowalny format pliku
                    int len = formFile.FileName.Length;
                    string ext = formFile.FileName.Substring(len-3);
                    if (allowedExtensions.Contains(ext) == false)
                    {
                        /* NIE MA INFORMACJI ZWROTNEJ */ 
                        ViewData["ExtensionWarning"] = "Niedozwolony format pliku - ." + ext;
                        //return View("~/Views/Home/Index.cshtml");
                        return RedirectToAction("Index", "Home");
                    }

                    //...tworzona jest sciezka wg wzorca OBECNY_KATALOG + ATTACHMENTS + NAZWA_PLIKU
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Attachments", formFile.FileName);
                    filePaths.Add(filePath);

                    //jesli dobrze zrozumialem, to tutaj tworzony jest strumien, do ktorego
                    //kopiowany jest aktualny plik
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            //na koniec zwracane sa szczegolowe dane na temat uploadowania - przydatne przy sprawdzaniu
            //dokladnych informacji zuploadowanych plikow
            return Ok(new { files.Count, size, filePaths });
        }
    }
}
