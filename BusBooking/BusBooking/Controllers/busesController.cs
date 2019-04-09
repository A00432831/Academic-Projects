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
    public class busesController : Controller
    {
        private BUSTICKETEntities db = new BUSTICKETEntities();

        // GET: buses
        public async Task<ActionResult> Index()
        {
            var buses = db.buses.Include(b => b.buses_type);
            return View(await buses.ToListAsync());
        }

        // GET: buses/Details/5
        /// <summary>
        /// Fectching Details of Buses based on Unique ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bus bus = await db.buses.FindAsync(id);
            if (bus == null)
            {
                return HttpNotFound();
            }
            return View(bus);
        }

        // GET: buses/Create
        /// <summary>
        /// Creation of Buses based on exisiting bustypes
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {   // fetching all existing bustypes with their respective Id's
            ViewBag.bus_type_id = new SelectList(db.buses_type, "bus_type_id", "name");
            return View();
        }

        // POST: buses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "bus_id,bus_name,bus_type_id,total_seats")] bus bus)
        {
            if (ModelState.IsValid)
            {
                db.buses.Add(bus);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.bus_type_id = new SelectList(db.buses_type, "bus_type_id", "name", bus.bus_type_id);
            return View(bus);
        }

        // GET: buses/Edit/5
        /// <summary>
        /// Editing of Bus detailes based on Unique ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bus bus = await db.buses.FindAsync(id);
            if (bus == null)
            {
                return HttpNotFound();
            }
            ViewBag.bus_type_id = new SelectList(db.buses_type, "bus_type_id", "name", bus.bus_type_id);
            return View(bus);
        }

        // POST: buses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "bus_id,bus_name,bus_type_id,total_seats")] bus bus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bus).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.bus_type_id = new SelectList(db.buses_type, "bus_type_id", "name", bus.bus_type_id);
            return View(bus);
        }

        // GET: buses/Delete/5
        /// <summary>
        /// Deleting of Bus details based on Unique ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bus bus = await db.buses.FindAsync(id);
            if (bus == null)
            {
                return HttpNotFound();
            }
            return View(bus);
        }

        // POST: buses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            bus bus = await db.buses.FindAsync(id);
            db.buses.Remove(bus);
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
