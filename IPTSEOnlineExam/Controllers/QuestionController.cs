using IPTSEOnlineExam.BLL;
using IPTSEOnlineExam.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace IPTSEOnlineExam.Controllers
{
    public class QuestionController : Controller
    {
        // GET: MockTest
        public ActionResult Index()
        {
            AdminBLL adminBLL = new AdminBLL();
            return View(adminBLL.GetQuestions());
        }

        // GET: MockTest/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AdminBLL adminBLL = new AdminBLL();
            Questions question = adminBLL.GetQuestionDetails(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // GET: MockTest/Create
        public ActionResult Create()
        {
            AdminBLL adminBLL = new AdminBLL();
            ViewBag.exam_id = new SelectList(adminBLL.GetExam(), "TestId", "Name");
            ViewBag.category_id = new SelectList(adminBLL.GetQuestionCategory(), "Id", "Category");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Questions questions_tbl)
        {
            AdminBLL adminBLL = new AdminBLL();
            if (ModelState.IsValid)
            {
                var count = questions_tbl.questionsChoice.RemoveAll(m => string.IsNullOrEmpty(m.ChoiceText));
                questions_tbl.questionsChoice.ForEach(m =>
                {
                    m.IsActive = true;
                    m.CreatedBy = "Admin";
                    m.CreatedDate = DateTime.Now;
                });

                questions_tbl.IsActive = true;

                if (adminBLL.ValidAnswer(questions_tbl))
                {
                    try
                    {
                        adminBLL.AddQuestion(questions_tbl);
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = ex.InnerException;
                        throw ex;
                    }
                }
                else
                {
                    ViewBag.Error_Message = "Select atleast one answer";
                }
            }

            ViewBag.exam_id = new SelectList(adminBLL.GetExam(), "TestId", "Name", questions_tbl.TestId);
            ViewBag.category_id = new SelectList(adminBLL.GetQuestionCategory(), "Id", "Category", questions_tbl.QuestionCategoryId);
            return View(questions_tbl);
        }

        // GET: MockTest/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminBLL adminBLL = new AdminBLL();
            Questions question = adminBLL.GetQuestionDetails(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.exam_id = new SelectList(adminBLL.GetExam(), "TestId", "Name", question.TestId);
            ViewBag.category_id = new SelectList(adminBLL.GetQuestionCategory(), "Id", "Category", question.QuestionCategoryId);

            return View(question);

        }

        // POST: MockTest/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Questions ques)
        {
            AdminBLL adminBLL = new AdminBLL();
            try
            {

                if (ModelState.IsValid)
                {
                    adminBLL.SaveChanges(ques);
                    return RedirectToAction("Index");
                }
                ViewBag.exam_id = new SelectList(adminBLL.GetExam(), "TestId", "Name", ques.TestId);
                ViewBag.category_id = new SelectList(adminBLL.GetQuestionCategory(), "Id", "Category", ques.QuestionCategoryId);
                return View(ques);
            }
            catch (Exception ex)
            {
                return View();
            }

        }

        // GET: MockTest/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MockTest/Delete/5
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
