using CiroHesaplama.Models;
using CiroHesaplama.Models.DIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CiroHesaplama.Controllers
{
    public class LoginController : Controller
    {


        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            string username = form["ka"].Trim();
            string password = form["pa"].Trim();

            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var MerkezRolNumara = db.Admin1s.Where(a => a.UserName.Equals(username) && a.PassWord.Equals(password) && a.Kategoriid.Equals("9")).FirstOrDefault();
                var FoodServiceGMRolNumara = db.Admin1s.Where(a => a.UserName.Equals(username) && a.PassWord.Equals(password) && a.Kategoriid.Equals("6")).FirstOrDefault();
                var BayramBalRolNumara = db.Admin1s.Where(a => a.UserName.Equals(username) && a.PassWord.Equals(password) && a.Kategoriid.Equals("7")).FirstOrDefault();
                var LokumStargateRolNumara = db.Admin1s.Where(a => a.UserName.Equals(username) && a.PassWord.Equals(password) && a.Kategoriid.Equals("8")).FirstOrDefault();
                var GMConcultantRolNumara = db.Admin1s.Where(a => a.UserName.Equals(username) && a.PassWord.Equals(password) && a.Kategoriid.Equals("5")).FirstOrDefault();
                if (MerkezRolNumara != null)
                {
                    Session["Adminid"] = MerkezRolNumara.Adminid.ToString();
                    Session["UserName"] = MerkezRolNumara.UserName.ToString();
                    return RedirectToAction("AnaSayfa", "Merkez");

                }
                else
                {
                    ViewData["Message"] = "kullanıcı adı yada şifre yanlış";
                
                }
                if (FoodServiceGMRolNumara != null)
                {
                    Session["Adminid"] = FoodServiceGMRolNumara.Adminid.ToString();
                    Session["UserName"] = FoodServiceGMRolNumara.UserName.ToString();
                    return RedirectToAction("Index", "GMFoodService");
                }
                else
                {
                    ViewData["Message"] = "kullanıcı adı yada şifre yanlış";

                }
                if (BayramBalRolNumara != null)
                {
                    Session["Adminid"] = BayramBalRolNumara.Adminid.ToString();
                    Session["UserName"] = BayramBalRolNumara.UserName.ToString();
                    return RedirectToAction("Index", "BayramBal");
                }
                else
                {
                    ViewData["Message"] = "kullanıcı adı yada şifre yanlış";

                }
                if (LokumStargateRolNumara != null)
                {
                    Session["Adminid"] = LokumStargateRolNumara.Adminid.ToString();
                    Session["UserName"] = LokumStargateRolNumara.UserName.ToString();
                    return RedirectToAction("Index", "Lokum");
                }
                else
                {
                    ViewData["Message"] = "kullanıcı adı yada şifre yanlış";

                }

                if (GMConcultantRolNumara != null)
                {
                    Session["Adminid"] = GMConcultantRolNumara.Adminid.ToString();
                    Session["UserName"] = GMConcultantRolNumara.UserName.ToString();
                    return RedirectToAction("Index", "GMConsultant");
                }
                else
                {
                    ViewData["Message"] = "kullanıcı adı yada şifre yanlış";

                }

            }

            return View("Index");
        }

    }
}
