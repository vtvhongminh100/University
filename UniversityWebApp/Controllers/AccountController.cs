﻿using ModelPr.CommonClass;
using ModelPr.ModelViews;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UniversityDao.Dao;
using UniversityDao.EF;
using System.Configuration;

namespace UniversityWebApp.Controllers {
    public class AccountController : Controller {
        // GET: Login
        private UniversityDbContext db = new UniversityDbContext();

        public AccountController() {

        }
        public ActionResult Index() {
            return View();
        }
        public ActionResult MyProfile() {
            var session = (UserLogin)Session[CommonCls.User_session];
            if (session == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountDao dao = new AccountDao();
            var user = dao.GetUserByUserName(session.Username);
            ProfileModel profile = new ProfileModel() {
                Address = user.Address, Email = user.Email, FullName = user.FullName, Phone = user.Phone, Image = user.Image
            };
            return View("Profile", profile);
        }

        [HttpPost]
        public ActionResult UploadAvatar(HttpPostedFileBase file) {
            try {
                var guid = Guid.NewGuid().ToString();
                string today = DateTime.Now.ToShortDateString().Replace("/", "");
                string extension = Path.GetExtension(file.FileName);
                string fName = today + "-" + guid + extension;
                var allowedExtensions = new[] { ".png", ".jpg", ".gif" };
                if (allowedExtensions.Contains(extension.ToLower()) && file.ContentLength <= (1 * 1024000) && file.ContentLength > 0) {

                    var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\", Server.MapPath(@"\")));

                    string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "Uploaded");

                    var fileName1 = Path.GetFileName(fName);

                    bool isExists = System.IO.Directory.Exists(pathString);

                    if (!isExists)
                        System.IO.Directory.CreateDirectory(pathString);

                    var path = string.Format("{0}\\{1}", pathString, fName);
                    file.SaveAs(path);
                    //
                    var session = (UserLogin)Session[CommonCls.User_session];
                    if (session == null) {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    AccountDao dao = new AccountDao();
                    dao.UploadImage(session.UserID, fName);
                    //
                    TempData["Message"] = "Upload Successfully!";
                    return RedirectToAction("MyProfile", "Account");
                } else {
                    TempData["Message"] = "Please upload image file within 1 MB!";
                    return RedirectToAction("MyProfile", "Account");
                }
            } catch (Exception ex) {

            }
            return RedirectToAction("MyProfile", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyProfile([Bind(Include = "Password,ConfirmPassword,FullName,Address,Email,Phone")] ProfileModel profile) {
            if (ModelState.IsValid) {
                var session = (UserLogin)Session[CommonCls.User_session];
                if (session == null) {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                AccountDao dao = new AccountDao();
                var account = dao.GetUserByUserName(session.Username);
                account.FullName = profile.FullName;
                account.Address = profile.Address;
                account.Email = profile.Email;
                account.Phone = profile.Phone;
                if (profile.Password != null) {
                    account.Password = profile.Password;
                    db.Entry(account).State = EntityState.Modified;
                    db.SaveChanges();
                    // Destroy session

                    //
                    return RedirectToAction("Index", "Account");
                } else {
                    db.Entry(account).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("MyProfile", "Account");
                }
            }
            return View("Profile", profile);
        }

        public ActionResult Logout() {
            Session.Clear();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model) {
            Session.Clear();
            if (ModelState.IsValid) {
                AccountDao dao = new AccountDao();
                int x = dao.LoginCheck(model);
                if (x == 0) {
                    ModelState.AddModelError("", "This account is not exist !!!");
                } else if (x == 2) {
                    ModelState.AddModelError("", "The account is locked !!!");
                } else if (x == 1) {
                    var user = dao.GetUserByUserName(model.Username);
                    var usersession = new UserLogin();
                    usersession.Username = user.Username;
                    usersession.UserID = user.UserID;
                    Session.Add(CommonCls.User_session, usersession);
                    return RedirectToAction("Index", "Home", new { area = "" });
                } else if (x == -1) {
                    ModelState.AddModelError("", "Password is not correct !!!");
                }
            }
            return View("Index");
        }
        public ActionResult Registry()
        {
            return View("Registry");
        }
        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(AccountModel model)
        {
            if (ModelState.IsValid)
            {
                AccountDao dao = new AccountDao();
                Account account = dao.GetUserByUserName(model.Username);
                if (account != null)
                {
                    ModelState.AddModelError("", "This account is existing !!!");
                }
                else
                {
                    int accountId = dao.Registry(model);
                    if (accountId != 0)
                    {
                        MailMessage m = new MailMessage(
                        new MailAddress(ConfigurationManager.AppSettings["mailAccount"], "Web Registration"),
                        new MailAddress(model.Email));
                        m.Subject = "Email confirmation";
                        m.Body = string.Format("Dear {0}<BR/>Thank you for your registration, please click on the below link to complete your registration:<BR/> <a href=\"{1}\" title=\"User Email Confirm\">Verify now</a>", model.Username, Url.Action("ConfirmEmail", "Account", new { AccountId = accountId }, Request.Url.Scheme));
                        m.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                        smtp.Credentials = new NetworkCredential(
                            ConfigurationManager.AppSettings["mailAccount"],
                            ConfigurationManager.AppSettings["mailPassword"]
                            );
                        smtp.EnableSsl = true;
                        smtp.Send(m);
                        return RedirectToAction("Confirm", "Account", new { Email = model.Email });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The account did not create");
                    }
                }

            }
            return View("Registry");
        }
        [AllowAnonymous]
        public ActionResult Confirm(string Email)
        {
            ViewBag.Email = Email;
            return View();
        }
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public ActionResult ConfirmEmail(int AccountId)
        {
            AccountDao dao = new AccountDao();
            bool x = dao.UpdateEmailConfirmed(AccountId);
            if (x == true)
            {
                return View("Thanks");
            }
            else
            {
                return View("Error");
            }
        }

    }
}