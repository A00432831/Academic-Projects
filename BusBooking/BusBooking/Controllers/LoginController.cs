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
        /// <summary>
        /// Authenticates the user by email and password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Authentication(BusBooking.user user)
        {
            using (BUSTICKETEntities db = new BUSTICKETEntities())
            {
                //Check wheteher the email and password provided by the user matches that in the database
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
                    Session["email"] = userDetails.email;
                    Session["role"] = userDetails.role;
                    if (userDetails.role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                        return RedirectToAction("Index", "Schedules");
                    return RedirectToAction("SearchBuses", "Schedules");                
                }
            }
        }
        /// <summary>
        /// User will be able to logout and will redirect to the home page
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}