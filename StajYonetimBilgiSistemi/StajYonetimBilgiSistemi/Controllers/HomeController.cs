using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StajYonetimBilgiSistemi.Controllers
{
    
    [AllowAnonymous]
    public class HomeController : Controller
    {
        SBYSEntities12 db = new SBYSEntities12();


        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(Kullanicilar kullanicilar)
        {
            var userInDb = db.Kullanicilar.FirstOrDefault(x => x.Email == kullanicilar.Email && x.Sifre == kullanicilar.Sifre);
            if (userInDb != null)
            {
                FormsAuthentication.SetAuthCookie(kullanicilar.Email , false);
                //return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.hata = "Kullanıcı adı veya şifrenizi yanlış girdiniz !";
            }

            
            if(ModelState.IsValid== true)
            {
                HttpCookie cookie = new HttpCookie("vs");

                if (kullanicilar.rememberme == true)
                {
                    cookie["KullaniciAdi"] = kullanicilar.Email;
                    cookie["Sifre"] = kullanicilar.Sifre;
                    cookie.Expires = DateTime.Now.AddDays(4);
                    HttpContext.Response.Cookies.Add(cookie);


                }
                else
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Response.Cookies.Add(cookie);

                }
                var row = db.Kullanicilar.Where(model => model.Email == kullanicilar.Email && model.Sifre == kullanicilar.Sifre).FirstOrDefault();
                if(row!= null)
                {
                    Session["KullaniciAdi"] = kullanicilar.Email;
                   
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Register(Kullanicilar kullanicilar)
        {
            Kullanicilar kullanicilar1 = new Kullanicilar();

            if (kullanicilar.Sifre == kullanicilar.SifreKontrol)
            {
                kullanicilar1.AdiSoyadi = kullanicilar.AdiSoyadi;

                if (UserNameControl(kullanicilar.KullaniciAdi) && EMailControl(kullanicilar.Email))
                {
                    kullanicilar1.KullaniciAdi = kullanicilar.KullaniciAdi;
                    kullanicilar1.Email = kullanicilar.Email;

                }
                else if (UserNameControl(kullanicilar.KullaniciAdi) == false && EMailControl(kullanicilar.Email))
                {
                    ViewBag.ErrorMessage = "Bu kullanıcı adı daha önce alınmış !";
                    return View();
                }
                else if(UserNameControl(kullanicilar.KullaniciAdi)  && EMailControl(kullanicilar.Email) == false)
                {
                    ViewBag.ErrorMessage2 = "Bu email daha önce kullanılmış !";
                    return View();
                }
                else
                {

                    ViewBag.ErrorMessage = "Bu kullanıcı adı daha önce alınmış !";
                    ViewBag.ErrorMessage2 = "Bu email daha önce kullanılmış !";
                    return View();
                    //return RedirectToAction("Register", "Home");
                }
                
                

                kullanicilar1.Sifre = kullanicilar.Sifre;
                kullanicilar1.SifreKontrol = kullanicilar.SifreKontrol;
                kullanicilar1.Rol = "Stajyer";
                kullanicilar1.KayitTarihi = DateTime.Now;
                db.Kullanicilar.Add(kullanicilar1);
                db.SaveChanges();
            }

            return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(Kullanicilar kullanicilar)
        {
            var model = db.Kullanicilar.Where(x => x.Email == kullanicilar.Email).FirstOrDefault();
            if (model != null)
            {
                Guid rastegele = Guid.NewGuid();
                model.Sifre = rastegele.ToString().Substring(0, 8);
                model.SifreKontrol = model.Sifre;
                db.SaveChanges();
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("silangul7kahramanoglu@gmail.com", "şifre sıfırlama");
                mail.To.Add(model.Email);
                mail.IsBodyHtml = true;
                mail.Subject = "şifre değiştirme isteği";
                mail.Body = "Merhaba :" + model.AdiSoyadi + "<br/> Kullanıcı adınız :" + model.KullaniciAdi + "<br/> Şifreniz :" + model.Sifre;
                NetworkCredential net = new NetworkCredential("silangul7kahramanoglu@gmail.com", "qffleeulsqavpylo");

                client.UseDefaultCredentials = false;
                client.Credentials = net;
                client.EnableSsl = true;
                try
                {
                    client.Send(mail);

                    Response.Redirect("/Home/Login/");
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e);
                }

            }
            ViewBag.hata = "böyle bir email adresi bulunamadı";
            return View();
        }

        [HttpGet]
        public ActionResult GuncelleBireysel()
        {
            if(User.Identity.IsAuthenticated)
            {
                var email = User.Identity.Name;
                var model = db.Kullanicilar.FirstOrDefault(x => x.Email == email );
                if(model!=null)
                {
                    return View(model);
                }
                else
                {
                    return View(new Kullanicilar());

                }
            }
            return HttpNotFound();


        }
        [HttpPost]
        public ActionResult GuncelleBireysel(Kullanicilar k)
        {
            
          if(User.IsInRole("Admin"))
            {
                k.Rol = "Admin";
            }

            if (User.IsInRole("Stajyer"))
            {
                k.Rol = "Stajyer";
            }

            if (User.IsInRole("Calisan"))
            {
                k.Rol = "Calisan";
            }

            k.KayitTarihi = DateTime.Now;
            db.Entry(k).State = System.Data.Entity.EntityState.Modified;
            try
            {
                db.SaveChanges();
            } 
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }
            FormsAuthentication.SignOut();
            FormsAuthentication.SetAuthCookie(k.KullaniciAdi, false);
            return RedirectToAction("Login", "Home");
        }
        public Boolean UserNameControl(string kullanıcıAdı)
        {
            if (!db.Kullanicilar.Any(x => x.KullaniciAdi == kullanıcıAdı))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean EMailControl(string email)
        {
            if (!db.Kullanicilar.Any(x => x.Email == email))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public JsonResult KullaniciAdiKontrol(string kullaniciAdi)
        {
            return Json(!db.Kullanicilar.Any(x => x.KullaniciAdi == kullaniciAdi),JsonRequestBehavior.AllowGet);
        }
        public JsonResult EmailKontrol(string email)
        {
            return Json(!db.Kullanicilar.Any(x => x.Email == email),JsonRequestBehavior.AllowGet);
        }
    }
}