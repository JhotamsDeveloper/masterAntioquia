using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GestionAntioquia.Controllers
{
    public class SocialEventsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}