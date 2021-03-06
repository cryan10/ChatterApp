﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chatter.Models;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;

namespace Chatter.Controllers
{
    [Authorize]
    [RequireHttps]
    public class ChatterUserInfoController : Controller
    {
        private ChatterEntities3 db = new ChatterEntities3();

        // GET: ChatterUserInfo
        public ActionResult Index()
        {
            var chatterUserInfoes = db.ChatterUserInfoes.Include(c => c.AspNetUser);
            return View(chatterUserInfoes.ToList());
        }
        public JsonResult TestJson()
        {
            //string jsonTest = "{ \"firstName\": \"Melanie\",\"lastName\": \"McGee\", \"children\": [{\"firstName\": \"Mira\", \"age\": 13 },{\"firstName\": \"Ethan\", \"age\": null }] }";

            //SELECT AspNetUsers.UserName, ChatterUserInfo.Message, ChatterUserInfo.Timestamp
            //FROM ChatterUserInfo
            //INNER JOIN AspNetUsers on ChatterUserInfo.UserID = AspNetUsers.Id


            var chats = from ChatterUserInfoes in db.ChatterUserInfoes
                        select new
                        {
                            ChatterUserInfoes.AspNetUser.UserName,
                            ChatterUserInfoes.Message,
                            ChatterUserInfoes.Timestamp
                        };

            var output = JsonConvert.SerializeObject(chats.ToList());

            return Json(output, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PostChats([Bind(Include = "Message")] ChatterUserInfo chat)
        {

            chat.Timestamp = DateTime.Now;
            string currentUserId = User.Identity.GetUserId();
            chat.AspNetUser = db.AspNetUsers.FirstOrDefault(x => x.Id == currentUserId);


            if (ModelState.IsValid)
            {
                db.ChatterUserInfoes.Add(chat);
                db.SaveChanges();
            }
            return new JsonResult() { Data = JsonConvert.SerializeObject(chat.ID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        // GET: ChatterUserInfo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChatterUserInfo chatterUserInfo = db.ChatterUserInfoes.Find(id);
            if (chatterUserInfo == null)
            {
                return HttpNotFound();
            }
            return View(chatterUserInfo);
        }

        // GET: ChatterUserInfo/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: ChatterUserInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserID,Message,Timestamp")] ChatterUserInfo chatterUserInfo)
        {
            if (ModelState.IsValid)
            {
                db.ChatterUserInfoes.Add(chatterUserInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", chatterUserInfo.UserID);
            return View(chatterUserInfo);
        }

        // GET: ChatterUserInfo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChatterUserInfo chatterUserInfo = db.ChatterUserInfoes.Find(id);
            if (chatterUserInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", chatterUserInfo.UserID);
            return View(chatterUserInfo);
        }

        // POST: ChatterUserInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserID,Message,Timestamp")] ChatterUserInfo chatterUserInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chatterUserInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", chatterUserInfo.UserID);
            return View(chatterUserInfo);
        }

        // GET: ChatterUserInfo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChatterUserInfo chatterUserInfo = db.ChatterUserInfoes.Find(id);
            if (chatterUserInfo == null)
            {
                return HttpNotFound();
            }
            return View(chatterUserInfo);
        }

        // POST: ChatterUserInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChatterUserInfo chatterUserInfo = db.ChatterUserInfoes.Find(id);
            db.ChatterUserInfoes.Remove(chatterUserInfo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
