using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace IPTSEOnlineExam.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            if(Session["ErrorMessage"]!=null)
            {
                Exception ex =(Exception)Session["ErrorMessage"];
                sendMail(ex);
            }
            return View();
        }
        private void sendMail(Exception ex)
        {
            string smsg = ex.InnerException.ToString();
            smsg += "<br/><br/>" + ex.StackTrace.ToString();
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add(new MailAddress("mpathak@igentdigital.com"));
                message.From = new MailAddress("info@iptse.in");
                message.Subject = "IPTSE Examination Error.";
                message.Body = smsg;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Port = 80;
                client.Host = "smtpout.asia.secureserver.net";
                NetworkCredential nc = new NetworkCredential("info@iptse.in", "Iptse@2018");
                client.EnableSsl = false;
                client.UseDefaultCredentials = true;
                client.Credentials = nc;
                client.Send(message);
            }
            catch (Exception ex1)
            {
                ViewBag.ErrorMessage = "Unable to send mail. Please contact support team for regisration confirmation";
            }
        }
        // GET: Error/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Error/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Error/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Error/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Error/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Error/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Error/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
