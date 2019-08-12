using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ClassLibrary2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HW59_Instigram_April30.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment _environment;
        private string _connectionString;

        public HomeController(IHostingEnvironment environment,
            IConfiguration configuration)
        {
            _environment = environment;
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        public IActionResult Index()
        {
            ImageManager mgr = new ImageManager(_connectionString);
           IEnumerable<Image> images =  mgr.GetImage();

            return View(images);
        }
        public IActionResult ViewImage(int id)
        {
            ImageManager mgr = new ImageManager(_connectionString);
            Image image = mgr.GetById(id);
            return View(image);
        }
        public IActionResult AddImage()
        {
            return View();
        }
        public IActionResult AddImage(string title, IFormFile file)
        {
            ImageManager mgr = new ImageManager(_connectionString);
            Image image = new Image();
            image.Title = title;
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string fullPath = Path.Combine(_environment.WebRootPath, "uploadedfiles", fileName);
            using (FileStream stream = new FileStream(fullPath, FileMode.CreateNew))
            {
                file.CopyTo(stream);
            }
            image.FileName = fileName;
            image.Like = 0;
            mgr.Add(image);
            return Redirect("/");
        }
    }
}
