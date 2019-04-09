using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BusBooking.Controllers
{
    public class transactionsController : Controller
    {
        private BUSTICKETEntities db = new BUSTICKETEntities();

        // GET: transactions
        public async Task<ActionResult> Index()
        {
            IQueryable<transaction> transactions = db.transactions.Include(t => t.creditcard_type).Include(t => t.schedule).Include(t => t.user);
            return View(await transactions.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> Index(string schedule_id)
        {
            var scheduleId = schedule_id;
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


        // GET: transactions/DetailsByUserId/5
        public async Task<ActionResult> DetailsByUserId(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user users = await db.users.FindAsync(id);
            ICollection<transaction> transaction = users.transactions;
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }


        // GET: transactions/Create
        public ActionResult Create() { 
            ViewBag.c_id = new SelectList(db.creditcard_type, "c_id", "name");
            ViewBag.s_id = new SelectList(db.schedules, "s_id", "source");
            ViewBag.user_id = new SelectList(db.users, "user_id", "name");
            return View();
        }

        // GET: transactions/Create?id=1&prize=10
        /// <summary>
        /// Schedule ID and Schedule Prize.
        /// </summary>
        /// <param name="id">Schedule ID</param>
        /// <param name="prize">Schedule Prize.</param>
        /// <returns></returns>
        public ActionResult CreateFromSchedule(int id,int prize)
        {
            transaction trans = new transaction();
            trans.s_id= id;
            trans.unit_price = prize;
            ViewBag.c_id = new SelectList(db.creditcard_type, "c_id", "name");
            ViewBag.s_id = new SelectList(db.schedules, "s_id", "source");
            ViewBag.user_id = new SelectList(db.users, "user_id", "name");
            return View("Create",trans);
        }

        // POST: transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "t_id,nameOnCard,cardNumber,unit_price,quantity,total_price,exp_Date,createdOn,createdBy,c_id,s_id,user_id")] transaction transaction)
        {
            string cardStart = transaction.cardNumber.Substring(0, 2);
            creditcard_type cctype = db.creditcard_type.Where(x => x.starts_with == cardStart && x.length == transaction.cardNumber.Length).FirstOrDefault();
            if (cctype == null)
            {
                ViewBag.errormessage = "Invalid Card Number";
                return View("Create",transaction);
            }
            transaction.c_id = cctype.c_id;
            transaction.s_id = 1;
            transaction.total_price = transaction.quantity * transaction.unit_price;
            transaction.createdOn = DateTime.Now;
            transaction.createdBy = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            transaction.user_id = Convert.ToInt32(Session["user_id"].ToString());

            if (ModelState.IsValid)
            {
                db.transactions.Add(transaction);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

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
