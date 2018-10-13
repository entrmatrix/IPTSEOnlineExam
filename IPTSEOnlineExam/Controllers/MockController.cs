using IPTSEOnlineExam.BLL;
using IPTSEOnlineExam.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace IPTSEOnlineExam.Controllers
{
    public class MockController : Controller
    {
        // GET: MockTest
        List<Questions> lstQuestions;
        List<Questions> lstQuestionsForResult = new List<Questions>();
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
                objQusetion.remainingTime = 60;
                objQusetion.TotalTime = "9:59";
                ViewBag.TotalTime = "9:59";
                ViewBag.isSkip = false;
                return View(objQusetion);
            }
            else
            {
                Questions a = (Questions)Session["qData"];
                ViewBag.questionNo = a.QuestNo;
                if(Session["isBack"]!=null)
                {
                    ViewBag.isSkip = Session["isBack"];
                }
                if (Session["remainTime"] != null)
                {
                    ViewBag.remainingTime = Session["remainTime"];
                    Session["remainTime"] = null;
                }
                return View(a);
            }
        }
        [HttpPost]
        public ActionResult NextQuestion(Questions aaa)
        {
            MockTestBLL objMock = new MockTestBLL();
            if (Session["Result"] != null)
            {
                lstQuestionsForResult = (List<Questions>)Session["Result"];
            }
            Boolean isAnswer = objMock.isAnswer(aaa.selectedvalue);
            if(isAnswer==true)
            { aaa.markScored = 1; }
            else
            { aaa.markScored = 0; }
            string aaa1 = ViewBag.Timedur;
            ViewBag.questionNo = Session["questNo"];
            lstQuestions = (List<Questions>)Session["Questions"];
            Session["Next"] = "true";
            var itemToRemove = lstQuestions.SingleOrDefault(r => r.Id == aaa.Id && r.skipQuestions == false);
            itemToRemove.skippedTime = Convert.ToString(60 - Convert.ToInt32(aaa.skippedTime.Substring(aaa.skippedTime.Length - 2))) + " sec";
            itemToRemove.SelectedText = objMock.selectedAnswer(aaa.selectedvalue);
            Boolean isAnswer1 = objMock.isAnswer(aaa.selectedvalue);
            if (isAnswer1 == true)
            { itemToRemove.markScored = 1; }
            else
            { itemToRemove.markScored = 0; }
            lstQuestionsForResult.Add(itemToRemove);
            Session["Result"] = lstQuestionsForResult;
            lstQuestions.RemoveAll(t => t.Id == aaa.Id && t.skipQuestions == false);
            Session["Questions"] = lstQuestions.OrderBy(t2 => t2.QuestNo).ToList();
            if (lstQuestions.Count <= 10 && lstQuestions.Count > 0 && ViewBag.questionNo <= 10)
            {
                lstQuestions = lstQuestions.Where(t => t.skipQuestions == false).Select(t1 => t1).ToList();
                if (lstQuestions.Count == 0)
                {
                    var lstQuestionsSkipped = lstQuestions.Where(t => t.skipQuestions == true).Select(t1 => t1).ToList();
                    if (lstQuestionsSkipped.Count > 0)
                    {
                        Session["isBack"] = true;
                        foreach(var item in lstQuestionsSkipped)
                        {
                            lstQuestionsForResult.Add(item);
                            Session["Result"] = lstQuestionsForResult;
                            var spendTime1 = item.TotalTime.Substring(item.TotalTime.Length - 5);
                            ViewBag.TotalTime = spendTime1;
                            var spendTimeArr1 = spendTime1.Split(':');
                            int spendSecond1 = 60 - Convert.ToInt32(spendTimeArr1[1]);
                            item.SpendTime = spendSecond1.ToString();
                            objMock.SaveAnswer(item);
                        }
                    }
                    else
                    { Session["isBack"] = false; }

                    
                    ViewData["success_msg"] = "Congratulation! you have successfully completed your Mock Test.";
                    return Json(new { success = true, responseText = "Exam Complete!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //ViewBag.isSkip = true;
                    var lstQuestionsNew = lstQuestions.Where(t => t.skipQuestions == true).Select(t1 => t1).ToList();
                    if (lstQuestionsNew.Count > 0)
                    { Session["isBack"] = true; }
                    else
                    { Session["isBack"] = false; }
                    objQusetion = new Questions();
                    objQusetion = lstQuestions.OrderBy(t2 => t2.QuestNo).FirstOrDefault();
                    var spendTime = aaa.TotalTime.Substring(aaa.TotalTime.Length - 5);
                    objQusetion.TotalTime = spendTime;
                    objQusetion.remainingTime = 60;
                    Session["qData"] = objQusetion;
                    Session["questNo"] = objQusetion.QuestNo;
                    ViewBag.remainingTime = 60;
                    Session["remainTime"] = 60;
                    
                    var spendTimeArr = spendTime.Split(':');
                    int spendSecond = 60-Convert.ToInt32(spendTimeArr[1]);
                    aaa.SpendTime= spendSecond.ToString();
                    objMock.SaveAnswer(aaa);
                    return RedirectToAction("MockTest", "Mock");
                }
            }
            else
            {
                ViewData["success_msg"] = "Congratulation! you have successfully completed your Mock Test.";
                return Json(new { success = true, responseText = "Exam Complete!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult NextQuestionTimeOut(Questions questionsTimeOut)
        {
            MockTestBLL objMock = new MockTestBLL();
           // lstQuestionsForResult = new List<Questions>();
            lstQuestionsForResult = (List < Questions > )Session["Result"];
            if (Session["Result"] != null)
            {
                lstQuestionsForResult = (List<Questions>)Session["Result"];
            }
            //Boolean isAnswer = objMock.isAnswer(aaa.selectedvalue);
            //if (isAnswer == true)
            //{ aaa.markScored = 1; }
            //else
            //{ aaa.markScored = 0; }

            ViewBag.questionNo = Session["questNo"];
            lstQuestions = (List<Questions>)Session["Questions"];
            Session["Next"] = "true";
            var itemToRemove = lstQuestions.SingleOrDefault(r => r.Id == Convert.ToInt32(questionsTimeOut.Id) && r.skipQuestions==false);
            itemToRemove.skippedTime = Convert.ToString(60 - Convert.ToInt32(questionsTimeOut.skippedTime.Substring(questionsTimeOut.skippedTime.Length - 2)));
            itemToRemove.SelectedText = objMock.selectedAnswer(questionsTimeOut.selectedvalue);
            Boolean isAnswer1 = objMock.isAnswer(questionsTimeOut.selectedvalue);
            if (isAnswer1 == true)
            { itemToRemove.markScored = 1; }
            else
            { itemToRemove.markScored = 0; }
            lstQuestionsForResult.Add(itemToRemove);
            Session["Result"] = lstQuestionsForResult;
            lstQuestions.RemoveAll(t => t.Id == Convert.ToInt32(questionsTimeOut.Id) && t.skipQuestions == false);
            Session["Questions"] = lstQuestions.OrderBy(t2 => t2.QuestNo).ToList();
            if (lstQuestions.Count <= 10 && lstQuestions.Count > 0 && ViewBag.questionNo <= 10)
            {
                lstQuestions = lstQuestions.Where(t => t.skipQuestions == false).Select(t1 => t1).ToList();
                if (lstQuestions.Count == 0)
                {
                    var lstQuestionsSkipped = lstQuestions.Where(t => t.skipQuestions == true).Select(t1 => t1).ToList();
                    if (lstQuestionsSkipped.Count > 0)
                    {
                        Session["isBack"] = true;
                        foreach (var item in lstQuestionsSkipped)
                        {
                            lstQuestionsForResult.Add(item);
                            Session["Result"] = lstQuestionsForResult;
                            var spendTime1 = item.TotalTime.Substring(item.TotalTime.Length - 4);
                            var spendTimeArr1 = spendTime1.Split(':');
                            int spendSecond1 = 60 - Convert.ToInt32(spendTimeArr1[1]);
                            item.SpendTime = spendSecond1.ToString();
                            objMock.SaveAnswer(item);
                        }
                    }
                    else
                    { Session["isBack"] = false; }


                    ViewData["success_msg"] = "Congratulation! you have successfully completed your Mock Test.";
                    return Json(new { success = true, responseText = "Exam Complete!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var lstQuestNew = lstQuestions.Where(t => t.skipQuestions == true).Select(t1 => t1).ToList();
                    if(lstQuestNew.Count>0)
                    { Session["isBack"] = true; }
                    else
                    { Session["isBack"] = false; }
                    objQusetion = new Questions();
                    objQusetion = lstQuestions.OrderBy(t2 => t2.QuestNo).FirstOrDefault();
                    objQusetion.remainingTime = 60;
                    var spendTime = questionsTimeOut.TotalTime.Substring(questionsTimeOut.TotalTime.Length - 5);
                    objQusetion.TotalTime = spendTime;
                    Session["qData"] = objQusetion;
                    Session["questNo"] = objQusetion.QuestNo;
                    ViewBag.remainingTime = 60;
                    Session["remainTime"] = 60;
                    var spendTimeArr = spendTime.Split(':');
                    int spendSecond = 60 - Convert.ToInt32(spendTimeArr[1]);
                    questionsTimeOut.SpendTime = spendSecond.ToString();
                    objMock.SaveAnswerTimeOut(questionsTimeOut);
                    return RedirectToAction("MockTest", "Mock");
                }
            }
            else
            {
                ViewData["success_msg"] = "Congratulation! you have successfully completed your Mock Test.";
                return Json(new { success = true, responseText = "Exam Complete!" }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult SkipQuestion(Questions skipQuestions)
        {
            var spendTime = skipQuestions.TotalTime.Substring(skipQuestions.TotalTime.Length - 5);
            ViewBag.TotalTime = spendTime;
            ViewBag.questionNo = Session["questNo"];
            Session["isSkip"] = true;
            lstQuestions = (List<Questions>)Session["Questions"];
            objQusetion = new Questions();

            objQusetion = lstQuestions.Where(t => t.Id == Convert.ToInt32(skipQuestions.Id)).Select(t1 => t1).FirstOrDefault();
            objQusetion.skipQuestions = true;
            Session["isBack"] = true;
            objQusetion.skippedTime = skipQuestions.skippedTime.Substring(skipQuestions.skippedTime.Length - 2);
            Session["remainTime"] = 60;
            Session["questNo"] = objQusetion.QuestNo;
            lstQuestions = lstQuestions.Where(t => t.skipQuestions == false).Select(t1 => t1).OrderByDescending(t2 => t2.QuestNo).ToList();
            objQusetion = lstQuestions.OrderBy(t2 => t2.QuestNo).FirstOrDefault();
            objQusetion.remainingTime = 60;
            objQusetion.TotalTime = spendTime;
            Session["qData"] = objQusetion;

            return RedirectToAction("MockTest", "Mock");
        }
        [HttpPost]
        public ActionResult PrevQuestion(Questions prevQuestions)
        {
            Session["isPrev"] = true;
            var spendTime = prevQuestions.TotalTime.Substring(prevQuestions.TotalTime.Length - 5);
            ViewBag.TotalTime = spendTime;
            lstQuestions = (List<Questions>)Session["Questions"];
            objQusetion = new Questions();
            lstQuestions = lstQuestions.Where(t => t.skipQuestions == true).Select(t1 => t1).OrderBy(t2 => t2.QuestNo).ToList();
            if (lstQuestions.Count != 0)
            {
                if(lstQuestions.Count>1)
                { Session["isBack"] = true; }
                else
                { Session["isBack"] = false; }
                objQusetion = lstQuestions.Where(t => t.skipQuestions == true).Select(t1 => t1).FirstOrDefault();
                objQusetion.skipQuestions = false;
                objQusetion.remainingTime = Convert.ToInt32(objQusetion.skippedTime);
                objQusetion.TotalTime = spendTime;
                Session["remainTime"] = objQusetion.skippedTime;
                Session["questNo"] = objQusetion.QuestNo;
                Session["qData"] = objQusetion;
                return RedirectToAction("MockTest", "Mock");
            }
            else
            {
                Session["isSkip"] = false;
                Session["isBack"] = false;
                objQusetion.TotalTime = spendTime;
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
