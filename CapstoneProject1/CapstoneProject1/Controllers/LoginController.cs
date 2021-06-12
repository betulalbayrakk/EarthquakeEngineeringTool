using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapstoneProject1.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            //Models.Capstone1Entities1 db = new Models.Capstone1Entities1();
            Models.Kullanici girilenKullanici = new Models.Kullanici();
            try
            {
                girilenKullanici.mail = form["inputEmail"].Trim();
                //girilenKullanici.Parola = form["inputPassword"].Trim();
                //girilenKullanici.KullaniciTipi = null;
            }
            catch (Exception e)
            {
                ViewBag.Message = e.ToString();
            }

            //var login = db.Kullanici.FirstOrDefault(a => a.Mail == girilenKullanici.Mail && a.Parola == girilenKullanici.Parola);
            //if (login != null)
            //{
            //    if (login.KullaniciTipi == "Ordinary User")
            //    {
            //        Session["AdSoyad"] = login.AdSoyad;
            //        Session["KullaniciTipi"] = "Ordinary User";
            //        return Redirect("~/Upload/Index");
            //    }
            //    else if (login.KullaniciTipi == "Manager")
            //    {
            //        Session["AdSoyad"] = login.AdSoyad;
            //        Session["KullaniciTipi"] = "Manager";
            //        return Redirect("~/Upload/Index");
            //    }
            //    else if (login.KullaniciTipi == "Other User")
            //    {
            //        Session["AdSoyad"] = login.AdSoyad;
            //        Session["KullaniciTipi"] = "Other User";
            //        return Redirect("~/Upload/Index");
            //    }
            //}
            //else
            //{
            //    ViewBag.Message = "Kullanıcı bilgilerinizi kontrol ediniz";
            //}
            return View();
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(FormCollection form)
        {
            return View();
        }
    }
}