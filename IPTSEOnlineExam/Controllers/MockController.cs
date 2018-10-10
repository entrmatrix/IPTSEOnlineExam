using IPTSEOnlineExam.BLL;
using IPTSEOnlineExam.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IPTSEOnlineExam.Controllers
{
    public class MockController : Controller
    {
        // GET: MockTest
        List<Questions> lstQuestions;
        Questions objQusetion;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MockTest()
        {
            MockTestBLL objMockTest = new MockTestBLL();
            if (Session["Next"] == null && Session["isTimeOut"] == null && Session["isSkip"] == null && Session["isPrev"] == null)
            {
                lstQuestions = new List<Questions>();
                objQusetion = new Questions();
                lstQuestions = objMockTest.GenerateQuestions();
                Session["Questions"] = lstQuestions.OrderBy(t2 => t2.QuestNo).ToList();
                objQusetion = lstQuestions.Select(t => t).FirstOrDefault();
                ViewBag.questionNo = objQusetion.QuestNo;
                Session["questNo"] = ViewBag.questionNo;
                ViewBag.remainingTime = 60;
                return View(objQusetion);
            }
            else
            {
                Questions a = (Questions)Session["qData"];
                ViewBag.questionNo = a.QuestNo;
                if (Session["remainTime"] != null)
                {
                    ViewBag.remainingTime = Session["remainTime"];
                    Session["remainTime"] = null;
                }
                return View(a);
            }
        }
        [HttpPost]
        public ActionResult NextQuestion(Questions aaa, string timeSpend)
        {
            int qId = 0;
            MockTestBLL objMock = new MockTestBLL();

            string aaa1 = ViewBag.Timedur;
            //aaa.SpendTime = 
            objMock.SaveAnswer(aaa, timeSpend);
            ViewBag.questionNo = Session["questNo"];
            lstQuestions = (List<Questions>)Session["Questions"];
            Session["Next"] = "true";
            var itemToRemove = lstQuestions.SingleOrDefault(r => r.Id == aaa.Id);
            lstQuestions.RemoveAll(t => t.Id == aaa.Id);
            Session["Questions"] = lstQuestions.OrderBy(t2 => t2.QuestNo).ToList();
            if (lstQuestions.Count <= 10 && lstQuestions.Count > 0 && ViewBag.questionNo <= 10)
            {
                lstQuestions = lstQuestions.Where(t => t.skipQuestions == false).Select(t1 => t1).ToList();
                if (lstQuestions.Count == 0)
                {
                    ViewData["success_msg"] = "Congratulation! you have successfully completed your Mock Test.";
                    return View("Successfull");
                }
                else
                {
                    objQusetion = new Questions();
                    objQusetion = lstQuestions.OrderBy(t2 => t2.QuestNo).FirstOrDefault();
                    Session["qData"] = objQusetion;
                    Session["questNo"] = objQusetion.QuestNo;
                    ViewBag.remainingTime = 60;
                    Session["remainTime"] = 60;
                    return RedirectToAction("MockTest", "Mock");
                }
            }
            else
            {
                ViewData["success_msg"] = "Congratulation! you have successfully completed your Mock Test.";
                return View("Successfull");
            }
        }

        [HttpPost]
        public ActionResult NextQuestionTimeOut(string QuestId, string isTimeOut, string totalTime)
        {
            MockTestBLL objMock = new MockTestBLL();
            objMock.SaveAnswerTimeOut(QuestId, isTimeOut, totalTime);
            ViewBag.questionNo = Session["questNo"];
            lstQuestions = (List<Questions>)Session["Questions"];
            Session["Next"] = "true";
            var itemToRemove = lstQuestions.SingleOrDefault(r => r.Id == Convert.ToInt32(QuestId));
            lstQuestions.RemoveAll(t => t.Id == Convert.ToInt32(QuestId));
            Session["Questions"] = lstQuestions.OrderBy(t2 => t2.QuestNo).ToList();
            if (lstQuestions.Count <= 10 && lstQuestions.Count > 0 && ViewBag.questionNo <= 10)
            {
                lstQuestions = lstQuestions.Where(t => t.skipQuestions == false).Select(t1 => t1).ToList();
                if (lstQuestions.Count == 0)
                {
                    ViewData["success_msg"] = "Congratulation! you have successfully completed your Mock Test.";
                    return View("Successfull");
                }
                else
                {
                    objQusetion = new Questions();
                    objQusetion = lstQuestions.OrderBy(t2 => t2.QuestNo).FirstOrDefault();
                    Session["qData"] = objQusetion;
                    Session["questNo"] = objQusetion.QuestNo;
                    ViewBag.remainingTime = 60;
                    Session["remainTime"] = 60;
                    return RedirectToAction("MockTest", "Mock");
                }
            }
            else
            {
                ViewData["success_msg"] = "Congratulation! you have successfully completed your Mock Test.";
                return View("Successfull");
            }

        }
        [HttpPost]
        public ActionResult SkipQuestion(string QuestId, string isSkip, string totalTime)
        {
            ViewBag.questionNo = Session["questNo"];
            Session["isSkip"] = isSkip;
            lstQuestions = (List<Questions>)Session["Questions"];
            objQusetion = new Questions();

            objQusetion = lstQuestions.Where(t => t.Id == Convert.ToInt32(QuestId)).Select(t1 => t1).FirstOrDefault();
            objQusetion.skipQuestions = true;
            objQusetion.skippedTime = Convert.ToInt32(totalTime.Substring(totalTime.Length - 2));
            Session["remainTime"] = 60;
            Session["questNo"] = objQusetion.QuestNo;
            lstQuestions = lstQuestions.Where(t => t.skipQuestions == false).Select(t1 => t1).OrderByDescending(t2 => t2.QuestNo).ToList();
            objQusetion = lstQuestions.OrderBy(t2 => t2.QuestNo).FirstOrDefault();
            Session["qData"] = objQusetion;
            //Session["remainTime"] = 60 - Convert.ToInt32(totalTime);
            return RedirectToAction("MockTest", "Mock");
        }
        [HttpPost]
        public ActionResult PrevQuestion(string QuestId, string isSkip, string totalTime)
        {
            Session["isPrev"] = isSkip;
            lstQuestions = (List<Questions>)Session["Questions"];
            objQusetion = new Questions();
            lstQuestions = lstQuestions.Where(t => t.skipQuestions == true).Select(t1 => t1).OrderBy(t2 => t2.QuestNo).ToList();
            if (lstQuestions.Count != 0)
            {
                objQusetion = lstQuestions.Where(t => t.skipQuestions == true).Select(t1 => t1).FirstOrDefault();
                objQusetion.skipQuestions = false;
                Session["remainTime"] = objQusetion.skippedTime;
                Session["questNo"] = objQusetion.QuestNo;
                Session["qData"] = objQusetion;
                return RedirectToAction("MockTest", "Mock");
            }
            else
            {
                return RedirectToAction("MockTest", "Mock");
            }
        }
        // GET: MockTest/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MockTest/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MockTest/Create
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

        // GET: MockTest/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MockTest/Edit/5
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
