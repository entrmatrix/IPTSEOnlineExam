﻿using IPTSEOnlineExam.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IPTSEOnlineExam.Controllers
{
    public class ResultController : Controller
    {
        [HttpGet]
        public ActionResult ResultPage()
        {
            List<Questions> lstQuestion = new List<Questions>();
            if (Session["Result"] != null)
            {
                int tot = 0;
                lstQuestion = (List<Questions>)Session["Result"];
                foreach (var item in lstQuestion)
                {
                    tot = tot + item.markScored;
                }
                ViewBag.Total = tot;
                return View(lstQuestion);
            }
            else
            {
                return View();
            }

        }
        public ActionResult PublishResult()
        {
            return RedirectToAction("ResultPage", "Result");
        }
        // GET: Result/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Result/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Result/Create
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

        // GET: Result/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Result/Edit/5
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

        // GET: Result/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Result/Delete/5
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