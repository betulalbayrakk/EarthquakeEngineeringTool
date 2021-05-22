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
            Models.CapstoneProject1Entities db = new Models.CapstoneProject1Entities();
            Models.Kullanici girilenKullanici = new Models.Kullanici();
            try
            {
                girilenKullanici.Mail = form["inputEmail"].Trim();
                girilenKullanici.Password = form["inputPassword"].Trim();
                girilenKullanici.KullaniciTipi = null;
            }
            catch (Exception e)
            {
                ViewBag.Message = e.ToString();
            }

            var login = db.Kullanici.FirstOrDefault(a => a.Mail == girilenKullanici.Mail && a.Password == girilenKullanici.Password);
            if (login != null)
            {
                if (login.KullaniciTipi == "Ordinary User")
                {
                    Session["AdSoyad"] = login.AdSoyad;
                    Session["KullaniciTipi"] = "Ordinary User";
                    return Redirect("~/Upload/Index");
                }
                else if (login.KullaniciTipi == "Manager")
                {
                    Session["AdSoyad"] = login.AdSoyad;
                    Session["KullaniciTipi"] = "Manager";
                    return Redirect("~/Upload/Index");
                }
                else if (login.KullaniciTipi == "Other User")
                {
                    Session["AdSoyad"] = login.AdSoyad;
                    Session["KullaniciTipi"] = "Other User";
                    return Redirect("~/Upload/Index");
                }
            }
            else
            {
                ViewBag.Message = "Kullanıcı bilgilerinizi kontrol ediniz";
            }
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
            Models.CapstoneProject1Entities db = new Models.CapstoneProject1Entities();
            Models.Kullanici kullaniciEkle = new Models.Kullanici();
            kullaniciEkle.AdSoyad = form["inputName"].Trim();
            kullaniciEkle.Mail = form["inputEmail"].Trim();
            kullaniciEkle.Password = form["inputPassword"].Trim();
            kullaniciEkle.Country = form["inputCountry"].Trim();
            kullaniciEkle.Number = form["inputNumber"].Trim();
            kullaniciEkle.KullaniciTipi = "0";
            db.Kullanici.Add(kullaniciEkle);
            db.SaveChanges();
            //var CallCenter = db.Kullanici.FirstOrDefault(a => a.AdSoyad == kullaniciEkle.AdSoyad && a.Mail == kullaniciEkle.Mail && a.Password == kullaniciEkle.Password && a.Country == kullaniciEkle.Country && a.Number == kullaniciEkle.Number && a.KullaniciTipi == kullaniciEkle.KullaniciTipi);

            return Redirect("~/Login/Login");

        }
    }
}