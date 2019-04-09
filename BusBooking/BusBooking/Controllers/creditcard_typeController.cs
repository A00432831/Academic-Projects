using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BusBooking;

namespace BusBooking.Controllers
{
    public class creditcard_typeController : Controller
    {
        private BUSTICKETEntities db = new BUSTICKETEntities();

        // GET: creditcard_type
        public async Task<ActionResult> Index()
        {
            return View(await db.creditcard_type.ToListAsync());
        }

        // GET: creditcard_type/Details/5
        /// <summary>
        /// Fetching Credit card details based on Unique Creditcard ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            creditcard_type creditcard_type = await db.creditcard_type.FindAsync(id);
            if (creditcard_type == null)
            {
                return HttpNotFound();
            }
            return View(creditcard_type);
        }

        // GET: creditcard_type/Create
        /// <summary>
        /// Creation of Record for Different Credit card types by Admin
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: creditcard_type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "c_id,name,starts_with,length")] creditcard_type creditcard_type)
        {
            if (ModelState.IsValid)
            {
                db.creditcard_type.Add(creditcard_type);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(creditcard_type);
        }

        // GET: creditcard_type/Edit/5
        /// <summary>
        /// Editing of Existing Creditcard Details by Admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            creditcard_type creditcard_type = await db.creditcard_type.FindAsync(id);
            if (creditcard_type == null)
            {
                return HttpNotFound();
            }
            return View(creditcard_type);
        }

        // POST: creditcard_type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "c_id,name,starts_with,length")] creditcard_type creditcard_type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(creditcard_type).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(creditcard_type);
        }

        // GET: creditcard_type/Delete/5
        /// <summary>
        /// Deletion of CreditCard type based on Unique Id by Admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            creditcard_type creditcard_type = await db.creditcard_type.FindAsync(id);
            if (creditcard_type == null)
            {
                return HttpNotFound();
            }
            return View(creditcard_type);
        }

        // POST: creditcard_type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            creditcard_type creditcard_type = await db.creditcard_type.FindAsync(id);
            db.creditcard_type.Remove(creditcard_type);
            await db.SaveChangesAsync();
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
