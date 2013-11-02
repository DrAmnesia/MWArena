﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MWArenaWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string FilePath = Server.MapPath("~/closedbeta.html");
            // You can add other conditions also here
            if (System.IO.File.Exists(FilePath))
            {
                return File(FilePath, "text/html");
            }
            return View();
        }
    }
}
