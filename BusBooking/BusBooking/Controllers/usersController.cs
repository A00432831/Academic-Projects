using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BusBooking.Controllers
{
    public class usersController : Controller
    {
        private BUSTICKETEntities db = new BUSTICKETEntities();

        // GET: users
        public async Task<ActionResult> Index()
        {

            return View(await db.users.ToListAsync());
        }

        // GET: users/Details/5
        /// <summary>
        /// Fetching User Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = await db.users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }



        // GET: users/Create

        public ActionResult Create()
        {
            return View();
        }

        // POST: users/Create
        /// <summary>
        ///Creating New Users and their  Validation from DB to prevent duplicate User Creation based on Unique Email ID 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "user_id,firstname,lastname,email,contact,apt_number,street_number,city,state,country,postal_Code,password,role,confirmPassword")] user user)
        {
            List<user> us = new List<user>();
            us = db.users.ToList();
            // LINQ Query for Validating Uniqueness of Email Id
            string users = us.Where(x => x.email == user.email).Select(x => x.email).FirstOrDefault();

            if (ModelState.IsValid && users == null)
            {

                db.users.Add(user);
                int userId = await db.SaveChangesAsync();
                if (Session["user_id"] != null)
                {
                    return RedirectToAction("Details", new { Id = userId });
                }
                else
                {
                    return RedirectToAction("LoginPage", "login");
                }
            }
            else if(users != null && users.Any())
            {

                ModelState.AddModelError("", "User Already exists");
            }

            return View(user);
        }
        // GET: users/Edit/5
        /// <summary>
        /// Editing of Existing Users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = await db.users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_id,firstname,lastname,email,contact,apt_number,street_number,city,state,country,postal_Code,role,password,confirmPassword")] user user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: users/Delete/5
        /// <summary>
        /// Deleting of Users and their Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = await db.users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            user user = await db.users.FindAsync(id);
            db.users.Remove(user);
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
