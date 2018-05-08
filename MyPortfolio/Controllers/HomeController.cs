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
using System.Security.Cryptography;
using System.Text;

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
                    WebMail.Send(to: "renzellerodrigueza01@gmail.com", subject: "Feedback and Comments", body: "Name: " + model.Name + " Comments: " + model.Comments + "  Phone: " + model.Phone + "  Email Address: " + model.Email, isBodyHtml: true);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return View("Contact", model);
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Admin()
        {
            if (Session["AccountId"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {

            }
            return View();
        }
        public ActionResult LogoutAction()
        {
            try
            {
                Session.Abandon();
                return RedirectToAction("Index","Home");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpPost]
        public ActionResult GetLoginInformation(AccountPOCO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Account user = null;
                    SHA512 sha512 = SHA512Managed.Create();
                    byte[] bytes = Encoding.UTF8.GetBytes(model.Password);
                    byte[] hash = sha512.ComputeHash(bytes);
                    using (var context = new MyPortfolioDatabaseEntities())
                    {
                        user = (from x in context.Accounts
                                where x.UserName.Equals(model.UserName) && x.PasswordHash.Equals(hash)
                                select x).FirstOrDefault();
                    }
                    if (user != null)
                    {
                        Session["AccountID"] = user.AccountId.ToString();
                        return RedirectToAction("Admin");
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return View("Login", model);
        }
    }
}