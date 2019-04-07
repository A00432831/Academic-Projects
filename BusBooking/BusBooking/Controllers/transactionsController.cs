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
    public class transactionsController : Controller
    {
        private BUSTICKETEntities db = new BUSTICKETEntities();

        // GET: transactions
        public async Task<ActionResult> Index()
        {
            var transactions = db.transactions.Include(t => t.creditcard_type).Include(t => t.schedule).Include(t => t.user);
            return View(await transactions.ToListAsync());
        }

        // GET: transactions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            transaction transaction = await db.transactions.FindAsync(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: transactions/Create
        public ActionResult Create()
        {
            ViewBag.c_id = new SelectList(db.creditcard_type, "c_id", "name");
            ViewBag.s_id = new SelectList(db.schedules, "s_id", "source");
            ViewBag.user_id = new SelectList(db.users, "user_id", "name");
            return View();
        }

        // POST: transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "t_id,nameOnCard,cardNumber,unit_price,quantity,total_price,exp_Date,createdOn,createdBy,c_id,s_id,user_id")] transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.transactions.Add(transaction);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.c_id = new SelectList(db.creditcard_type, "c_id", "name", transaction.c_id);
            ViewBag.s_id = new SelectList(db.schedules, "s_id", "source", transaction.s_id);
            ViewBag.user_id = new SelectList(db.users, "user_id", "name", transaction.user_id);
            return View(transaction);
        }

        // GET: transactions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            transaction transaction = await db.transactions.FindAsync(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.c_id = new SelectList(db.creditcard_type, "c_id", "name", transaction.c_id);
            ViewBag.s_id = new SelectList(db.schedules, "s_id", "source", transaction.s_id);
            ViewBag.user_id = new SelectList(db.users, "user_id", "name", transaction.user_id);
            return View(transaction);
        }

        // POST: transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "t_id,nameOnCard,cardNumber,unit_price,quantity,total_price,exp_Date,createdOn,createdBy,c_id,s_id,user_id")] transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.c_id = new SelectList(db.creditcard_type, "c_id", "name", transaction.c_id);
            ViewBag.s_id = new SelectList(db.schedules, "s_id", "source", transaction.s_id);
            ViewBag.user_id = new SelectList(db.users, "user_id", "name", transaction.user_id);
            return View(transaction);
        }

        // GET: transactions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            transaction transaction = await db.transactions.FindAsync(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            transaction transaction = await db.transactions.FindAsync(id);
            db.transactions.Remove(transaction);
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
