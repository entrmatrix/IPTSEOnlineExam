using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IPTSEOnlineExam.Models;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using IPTSEOnlineExam.BLL.Models;

namespace IPTSE_portal.Controllers
{
    public class IPTSELoginController : Controller
    {
        private LoginDataModel db = new LoginDataModel();
        private RegistrationDataModel rDB = new RegistrationDataModel();
        private PaymentDataModel pDB = new PaymentDataModel();
        //// GET: IPTSELogin
        //public ActionResult Index()
        //{
        //    return View(db.login_table.ToList());
        //}

        //// GET: IPTSELogin/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    login_table login_table = db.login_table.Find(id);
        //    if (login_table == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(login_table);
        //}

        [HttpGet]
        public ActionResult Login()
        {
            //Session.Clear();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(login_table login_table)
        {
            if (ModelState.IsValid)
            {
                               
                using (db)
                {
                    Session["TestId"] = "";
                    byte[] encode = new byte[login_table.password.Length];
                    encode = System.Text.Encoding.UTF8.GetBytes(login_table.password);
                    login_table.password = Convert.ToBase64String(encode);
                    bool allDigits = login_table.email.All(char.IsDigit);
                    if(allDigits)
                    {
                        var loginId = Int32.Parse(login_table.email);
                        var obj = db.login_table.Where(a => a.Id.Equals(loginId) && a.password.Equals(login_table.password)).FirstOrDefault();
                        if (obj != null)
                        {
                            Session["id"] = obj.Id.ToString();
                            Session["UserProfile"] = obj;
                            ViewBag.UID = HttpUtility.UrlEncode(Encrypt(obj.Id.ToString()));
                            Session["Type"] = obj.Login_type;
                            payment_details payment_Details = new payment_details();
                            payment_Details.Id = Decimal.Parse(Session["id"].ToString());
                            var objPayment = pDB.payment_details.Where(a => a.Id.Equals(payment_Details.Id)).FirstOrDefault();
                            if (objPayment == null)
                            {

                                ViewBag.PaymentMessage = "Payment";
                                return View();
                            }
                            else
                            {
                                var insType = rDB.IPTSE_Reg_table.Where(t => t.Id.Equals(obj.Id)).Select(t1 => t1.InstitutionType).FirstOrDefault();
                                if (insType == "School")
                                {
                                    Session["TestId"] = 2;
                                    return RedirectToAction("Index", "Full"); }
                                else if (insType == "College")
                                { Session["TestId"] = 3; return RedirectToAction("Index", "College"); }
                                else
                                { Session["TestId"] = 1; return RedirectToAction("Index", "Mock"); }
                            }
                            
                        }
                        else
                        {

                            if (loginId == Decimal.Parse("91620195"))
                            {
                                //if (login_table.password == "YWJjQDEyMw==") //"SVBUU0VfQURNSU5fTE9HSU4 =")
                                //{
                                    Session["admin_login"] = "91620195";
                                    return RedirectToAction("Index", "Admin");
                                //}
                            }
                            ViewBag.ErrorMessage = "Invalid Credentials....";
                            return View();
                        }

                     }
                    else
                    {
                        var obj1 = db.login_table.Where(a => a.email.Equals(login_table.email) && a.password.Equals(login_table.password)).FirstOrDefault();
                        if (obj1 != null)
                        {
                            Session["id"] = obj1.Id.ToString();
                            ViewBag.UID = HttpUtility.UrlEncode(Encrypt(obj1.Id.ToString()));
                            Session["UserProfile"] = obj1;
                            Session["Type"] = obj1.Login_type;
                            obj1.LastLoginDateTime = DateTime.Now;
                            payment_details payment_Details = new payment_details();
                            payment_Details.Id = Decimal.Parse(Session["id"].ToString());
                            var objPayment = pDB.payment_details.Where(a => a.Id.Equals(payment_Details.Id)).FirstOrDefault();
                            if (objPayment == null)
                            {

                                ViewBag.PaymentMessage = "Payment";
                                return View();
                            }
                            else
                            {
                                var insType = rDB.IPTSE_Reg_table.Where(t => t.Id.Equals(obj1.Id)).Select(t1 => t1.InstitutionType).FirstOrDefault();
                                if (insType == "School")
                                { Session["TestId"] = 2; return RedirectToAction("Index", "Full"); }
                                else if (insType == "College")
                                { Session["TestId"] = 3; return RedirectToAction("Index", "College"); }
                                else
                                { Session["TestId"] = 1; return RedirectToAction("Index", "Mock"); }
                            }
                        }
                        else
                        {
                            if (login_table.Id == Decimal.Parse("91620195"))
                            {
                                if (login_table.password == "SVBUU0VfQURNSU5fTE9HSU4=")
                                {
                                    Session["admin_login"] = "91620195";
                                    return RedirectToAction("Index", "Admin");
                                }
                            }
                            ViewBag.ErrorMessage = "Invalid Credentials....";
                            return View();
                        }
                    }
                  
                   
                }
            }
            return View(login_table);
        }



        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }


        //// GET: IPTSELogin/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    login_table login_table = db.login_table.Find(id);
        //    if (login_table == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(login_table);
        //}

        //// POST: IPTSELogin/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,password")] login_table login_table)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(login_table).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(login_table);
        //}

        //// GET: IPTSELogin/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    login_table login_table = db.login_table.Find(id);
        //    if (login_table == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(login_table);
        //}

        //// POST: IPTSELogin/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    login_table login_table = db.login_table.Find(id);
        //    db.login_table.Remove(login_table);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
