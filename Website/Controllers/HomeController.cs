using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UrlStemming.Core;
using Website.Models;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new EqualityCheckerFormModel());
        }

        [HttpPost]
        public ActionResult Index(EqualityCheckerFormModel formModel)
        {
            if(!ModelState.IsValid)
            {
                return Redirect("/");
            }

            formModel.Submitted = true;

            var settings = formModel.GetSettings();
            formModel.OneStemmed = new StemmedUrl(formModel.One, settings);

            if (formModel.Two != null)
            {
                formModel.TwoStemmed = new StemmedUrl(formModel.Two, settings);
                formModel.AreEqual = formModel.OneStemmed.Equals(formModel.TwoStemmed);
            }
            
            return View(formModel);
        }

    }
}