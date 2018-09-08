using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IPTSEOnlineExam.Controllers
{
    public class MockController : Controller
    {
        // GET: MockTest
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MockTest()
        {
            return View();
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
