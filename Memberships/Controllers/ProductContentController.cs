﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Memberships.Controllers
{
    public class ProductContentController : Controller
    {
        [Authorize]
        public async Task<ActionResult> Index(int id)
        {
            return View();
        }
    }
}