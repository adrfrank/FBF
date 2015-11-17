using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrankCompiler.Core;

namespace FormulasBienFormadas.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Validate(string line) {
            IParser parser = new FormulasBienFormadasParser();
            var result= parser.Parse(line);
            return Json(result, JsonRequestBehavior.AllowGet);           
        }

        public ActionResult Gramatica() {
            return File("~/Content/gramatica.pdf", "application/pdf");
        }
    }
}