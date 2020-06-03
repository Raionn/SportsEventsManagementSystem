using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportBook.Models;

namespace SportBook.Controllers
{
    public class ImageController : Controller
    {
        private readonly AzureStorageConfig storageConfig = null;
        public IActionResult Index()
        {
            return View();
        }
    }
}