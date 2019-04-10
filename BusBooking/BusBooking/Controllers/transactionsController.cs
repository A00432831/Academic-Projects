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
        /// <summary>
        /// Get all transaction
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Details of transaction
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Details of transaction by user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Creates the transactions and stores it in the database after checking the validations
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
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
            transaction.total_price = transaction.quantity * transaction.unit_price;
            transaction.createdOn = DateTime.Now;
            transaction.createdBy = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            transaction.user_id = Convert.ToInt32(Session["user_id"].ToString());

            if (ModelState.IsValid)
            {
                db.transactions.Add(transaction);
                int transactionId = await db.SaveChangesAsync();
                return RedirectToAction("Details",new { id = transaction.t_id });
            }

            return View(transaction);
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
