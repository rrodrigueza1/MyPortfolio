using MyPortfolio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Web.Helpers;
using System.IO;

namespace MyPortfolio.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            PhotoModel photoModel = new PhotoModel()
            {
                ImageFilePath = Directory.EnumerateFiles(Server.MapPath("~/Images/FSOSSImages")).Select(image => "~/Images/FSOSSImages/" + Path.GetFileName(image))
            };
            return View(photoModel);
        }        

        public ActionResult Contact()
        {
            ViewBag.Message = "Looking for Software Developer - Entry Level Position";

            return View();
        }

        [HttpPost]
        public ActionResult GetContactInformation(ContactInformationModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    WebMail.Send(to: "renzellerodrigueza01@gmail.com", subject: "Feedback and Comments", body: "Name: " + model.Name + " Comments: "+ model.Comments + "  Phone: " + model.Phone + "  Email Address: " + model.Email, isBodyHtml: true);            
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return View("Contact", model);
        }
    }
}