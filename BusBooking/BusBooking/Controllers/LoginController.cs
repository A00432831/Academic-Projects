using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusBooking.Controllers
{
    public class LoginController : Controller
    {

        // GET: Login
        public ActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authentication(BusBooking.user user)
        {
            using (BUSTICKETEntities db = new BUSTICKETEntities())
            {
                var userDetails = db.users.Where(x => x.email == user.email && x.password == user.password).FirstOrDefault();
                if (userDetails == null)
                {
                    userDetails = new user();
                    userDetails.loginErrorMessage = " Wrong Email or Password. Try again !!";
                    return View("LoginPage", userDetails);
                }
                else
                {
                    Session["user_id"] = userDetails.user_id;
                    if (userDetails.role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                        return RedirectToAction("Index", "Schedules");
                    return RedirectToAction("SearchBuses", "Schedules");                
                }
            }
        }
    }
}