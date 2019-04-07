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
using System.Web.WebPages.Html;
using SelectListItem = System.Web.WebPages.Html.SelectListItem;

namespace BusBooking.Controllers
{
    public class schedulesController : Controller
    {
        private BUSTICKETEntities db = new BUSTICKETEntities();

        // GET: schedules
        public async Task<ActionResult> Index()
        {
            var schedules = db.schedules.ToList();
            return View(schedules);
        }
        // get : home page
        public async Task<ActionResult> SearchBuses()
        {
            var a = db.schedules.Select(arg => new { source = arg.source }).ToList().Distinct();
            var b = db.schedules.Select(arg => new { destination = arg.destination }).ToList().Distinct();
            var c = db.schedules.Select(arg => new { date = arg.date }).ToList().Distinct();
            ViewData["source"] = new SelectList(a, "source", "source");
            ViewData["destination"] = new SelectList(b, "destination", "destination");
            ViewData["date"] = new SelectList(c, "date", "date");


            return View();
        }
        // post: Home Page
        [HttpPost]
        public async Task<ActionResult> SearchBuses(schedule schedule)
        {
            var schedules = db.schedules.Where(s => s.source == schedule.source && s.destination== schedule.destination && s.date==schedule.date).Distinct();
            return View(await schedules.ToListAsync());

        }

        // GET: schedules/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            schedule schedule = await db.schedules.FindAsync(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // GET: schedules/Create
        public ActionResult Create()
        {
            ViewBag.bus_id = new SelectList(db.buses, "bus_id", "bus_name");
            return View();
        }

        // POST: schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "s_id,source,destination,date,cost,bus_id,description")] schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.schedules.Add(schedule);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.bus_id = new SelectList(db.buses, "bus_id", "bus_name", schedule.bus_id);
            return View(schedule);
        }

        // GET: schedules/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            schedule schedule = await db.schedules.FindAsync(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.bus_id = new SelectList(db.buses, "bus_id", "bus_name", schedule.bus_id);
            return View(schedule);
        }

        // POST: schedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "s_id,source,destination,date,cost,bus_id,description")] schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schedule).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.bus_id = new SelectList(db.buses, "bus_id", "bus_name", schedule.bus_id);
            return View(schedule);
        }

        // GET: schedules/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            schedule schedule = await db.schedules.FindAsync(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            schedule schedule = await db.schedules.FindAsync(id);
            db.schedules.Remove(schedule);
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
