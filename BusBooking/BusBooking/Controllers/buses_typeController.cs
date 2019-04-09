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
    public class buses_typeController : Controller
    {
        private BUSTICKETEntities db = new BUSTICKETEntities();

        // GET: buses_type
        public async Task<ActionResult> Index()
        {
            return View(await db.buses_type.ToListAsync());
        }

        // GET: buses_type/Details/5
        /// <summary>
        /// Fectching Details of Bus types based on Unique ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            buses_type buses_type = await db.buses_type.FindAsync(id);
            //Validating for Null buses_type
            if (buses_type == null)
            {
                return HttpNotFound();
            }
            return View(buses_type);
        }

        // GET: buses_type/Create
        /// <summary>
        /// Creation of New Buses_types
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: buses_type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "bus_type_id,name")] buses_type buses_type)
        {
            if (ModelState.IsValid)
            {
                db.buses_type.Add(buses_type);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(buses_type);
        }

        // GET: buses_type/Edit/5
        /// <summary>
        /// Editing Details of Bus types based on Unique ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            buses_type buses_type = await db.buses_type.FindAsync(id);
            if (buses_type == null)
            {
                return HttpNotFound();
            }
            return View(buses_type);
        }

        // POST: buses_type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "bus_type_id,name")] buses_type buses_type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(buses_type).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(buses_type);
        }

        // GET: buses_type/Delete/5
        /// <summary>
        /// Deleting Details of Bus types based on Unique ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            buses_type buses_type = await db.buses_type.FindAsync(id);
            if (buses_type == null)
            {
                return HttpNotFound();
            }
            return View(buses_type);
        }

        // POST: buses_type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            buses_type buses_type = await db.buses_type.FindAsync(id);
            db.buses_type.Remove(buses_type);
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
