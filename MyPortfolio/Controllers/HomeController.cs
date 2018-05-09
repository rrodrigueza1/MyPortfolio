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
using System.Data.Entity.Validation;
using System.Diagnostics;

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

        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }
            }
            GetProjectModelList();
            return View("Admin");
        }

        [HttpPost]
        public ActionResult SaveProjectForm(SingleProjectModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var context = new MyPortfolioDatabaseEntities())
                    {
                        Project project = new Project()
                        {
                            ProjectName = model.ProjectName,
                            DateCreated = model.DateCreated,
                            ProjectDescription = model.ProjectDescription,
                            DateFinished = model.DateFinished,
                            ProjectList = model.ProjectLink
                        };
                        context.Projects.Add(project);
                        context.SaveChanges();
                    }
                }
                catch (DbEntityValidationException e)
                {

                    foreach (var eve in e.EntityValidationErrors)
                    {
                        foreach (var validationErrors in e.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Trace.TraceInformation("Property: {0} Error: {1}",
                                                        validationError.PropertyName,
                                                        validationError.ErrorMessage);
                            }
                        }
                    }
                }
            }
            GetProjectModelList();
            return View("Admin");
        }

        public ActionResult Login()
        {
            if (Session["AccountId"] != null)
            {
                return RedirectToAction("Admin", "Home");
            }
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
                GetProjectModelList();
            }

            return View();
        }

        public void GetProjectModelList()
        {
            ProjectModel projectModelEntity = new ProjectModel();
            List<Project> projectList = new List<Project>();
            using (var context = new MyPortfolioDatabaseEntities())
            {
                var projectModel = (from x in context.Projects
                                    select new ProjectModel
                                    {
                                        ProjectName = x.ProjectName,
                                        ProjectId = x.ProjectId
                                    }).ToList();
                projectModelEntity.ProjectList = new SelectList(projectModel, "ProjectId", "ProjectName");
            }
            ViewBag.VBProjectList = projectModelEntity.ProjectList;

        }
        public ActionResult LogoutAction()
        {
            try
            {
                Session.Abandon();
                return RedirectToAction("Index", "Home");
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
            return View("Login");
        }
    }
}