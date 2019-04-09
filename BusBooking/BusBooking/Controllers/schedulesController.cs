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
using System.Web.Script.Serialization;

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
            var d = new List<string>();
            foreach (var item in c)
            {
                //System.Diagnostics.Debug.WriteLine("Before Date is {0} and After date is {1}",item.date,item.date.ToString("MM/dd/yyyy"));
                d.Add(item.date);
            }
            d = d.Distinct().ToList();
            ViewData["source"] = new SelectList(a, "source", "source");
            ViewData["destination"] = new SelectList(b, "destination", "destination");
            //ViewData["dateTime"] = new SelectList(c, "date", "date");
            ViewData["date"] = new SelectList(c, "date", "date");


            return View();
        }
        // post: Home Page
        [HttpPost]
        public ActionResult SearchBuses(string sources, string destinations, DateTime dates)
        {
            db.Configuration.ProxyCreationEnabled = false;
            string date = dates.Date.ToString("yyyy-MM-dd");
            var schedules = db.schedules.Where(s => s.source == sources && s.destination == destinations && s.date == date).ToList();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string json = serializer.Serialize(schedules);
            return Json(schedules.Select( s => new
            {
                source = s.source,
                destination = s.destination,
                cost = s.cost,
                description = s.description,
                date = s.date,
                time = s.time.ToString(),
                s_id=s.s_id
            })
                , JsonRequestBehavior.AllowGet);

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
