using StajYonetimBilgiSistemi.Models;
using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StajYonetimBilgiSistemi.Controllers
{

    [AllowAnonymous]
    public class HomeController : Controller
    {
        SBYSEntities db = new SBYSEntities();

        public ActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {

                return View();
            }
            return View("~/Views/Shared/Error.cshtml");
        }
        public ActionResult OpenPage()
        {

                var kullanicisayisi = db.Kullanicilar.Count();
                ViewBag.kullanicisayisi = kullanicisayisi;
                var stajyersayisi = db.STAJYER_TANIM.Count();
                ViewBag.stajyersayisi = stajyersayisi;
                var departmansayisi = db.KURUM_DEPARTMAN.Count();
                ViewBag.departmansayisi = departmansayisi;
                var kurumsayisi = db.KURUM_TANIM.Count();
                ViewBag.kurumsayisi = kurumsayisi;
                var model = db.Stajlar.Where(x => x.Aktiflik == true).ToList();
                return View(model);
         
        }



        public JsonResult BarChartDataEF()
        {
            var stajyerler = db.STAJYER_TANIM.ToList();
            Chart _chart = new Chart();
            _chart.labels = stajyerler.Select(x => x.Bolumler.BolumName).Distinct().ToArray();
            _chart.datasets = new List<Datasets>();


            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Tüm Sistemdeki Stajerlerin Bölüm Grafiği",
                //data = stajyerler.Select(x => x.BOLUMU.Value).ToArray(),
                data = stajyerler.GroupBy(a => a.BOLUMU).Select(a => a.Count()).ToArray(),
                backgroundColor = new string[] { "#4EF0EE" },
                borderColor = new string[] { "#000000" },
                borderWidth = "1"

            });
            _chart.datasets = _dataSet;
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BarChartDataEFUni()
        {
            var stajyerler = db.STAJYER_TANIM.ToList();
            Chart _chart = new Chart();
            _chart.labels = stajyerler.Select(x => x.Uni.UniName).Distinct().ToArray();
            _chart.datasets = new List<Datasets>();


            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Tüm Sistemdeki Stajerlerin Üniversite Grafiği",
                //data = stajyerler.Select(x => x.BOLUMU.Value).ToArray(),
                data = stajyerler.GroupBy(a => a.UNIVERSITE).Select(a => a.Count()).ToArray(),
                backgroundColor = new string[] { "#4EF0EE" },
                borderColor = new string[] { "#000000" },
                borderWidth = "1"

            });
            _chart.datasets = _dataSet;
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(Kullanicilar kullanicilar)
        {


            var userInDb = db.Kullanicilar.ToList();
            foreach (var user in userInDb)
            {
                if ((user.Email == kullanicilar.Email || user.KullaniciAdi == kullanicilar.Email) && Decrypt(user.Sifre) == kullanicilar.Sifre)
                {
                    if (userInDb != null)
                    {
                        FormsAuthentication.SetAuthCookie(kullanicilar.Email, false);



                        if (user.kodOnay == true)
                        {


                            var model = db.STAJYER_TANIM.ToList();
                            var model2 = db.KURUM_PERSONEL.ToList();
                            if (user.Rol == "Stajyer" || user.Rol =="")
                            {
                                foreach (var item2 in model)
                                {
                                    if (item2.EMAIL == user.Email )
                                    {
                                        var model5 = db.Kullanicilar.Where(x => x.Email == user.Email || x.KullaniciAdi == user.Email).FirstOrDefault();
                                        var model6 = db.STAJYER_TANIM.Where(x => x.EMAIL == user.Email).FirstOrDefault();
                                        model5.Rol = "Stajyer";
                                        model5.CalisanKurumTanim = item2.FK_STAJ_KURUM;
                                        model5.CalisanDepartman = item2.FK_DEPARTMAN;
                                        model5.CalisanPersonel = null;
                                        model5.Sifre = Encrypt(kullanicilar.Sifre);
                                        model6.kullaniciId = model5.Id;
                                        db.Entry(model6).State = System.Data.Entity.EntityState.Modified;

                                        db.Entry(model5).State = System.Data.Entity.EntityState.Modified;
                                        db.SaveChanges();

                                        return RedirectToAction("Index", "Home");

                                    }
                                }
                            }
                           else if (user.Rol == "Calisan")
                            {
                                foreach (var item3 in model2)
                                {
                                    if (item3.EMAIL == user.Email)
                                    {
                                        var model5 = db.Kullanicilar.Where(x => x.Email == user.Email ).FirstOrDefault();

                                        if (user.Rol == "Calisan")
                                        {

                                            model5.Rol = "Calisan";
                                            model5.CalisanKurumTanim = item3.FK_KURUM_TANIM;
                                            model5.CalisanDepartman = item3.FK_KURUM_DEPARTMAN;
                                            model5.CalisanPersonel = item3.PK_KURUM_PERSONEL;
                                            model5.Sifre = Encrypt(kullanicilar.Sifre);

                                        }
                                        if (user.Rol == "SirketYetkilisi")
                                        {

                                            model5.Rol = "SirketYetkilisi";
                                            model5.CalisanKurumTanim = item3.FK_KURUM_TANIM;
                                            model5.CalisanDepartman = item3.FK_KURUM_DEPARTMAN;
                                            model5.CalisanPersonel = item3.PK_KURUM_PERSONEL;
                                            model5.Sifre = Encrypt(kullanicilar.Sifre);

                                        }
                                        db.Entry(model5).State = System.Data.Entity.EntityState.Modified;
                                        db.SaveChanges();
                                        return RedirectToAction("Index", "Home");

                                    }

                                }
                            }
                          else  if (user.Rol == "Admin")
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else if (user.Rol == "SirketYetkilisi")
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                return RedirectToAction("AktifStajlar", "Home");
                            }

                        }
                        else
                        {
                            System.Threading.Thread.Sleep(5000);
                            return RedirectToAction("Dogrulama", "Home");

                        }


                    }
                    break;
                }

                else if ((user.Email == kullanicilar.Email || user.KullaniciAdi == kullanicilar.Email) && Decrypt(user.Sifre) != kullanicilar.Sifre)
                {
                    ViewBag.Hata = " şifrenizi yanlış girdiniz";
                    break;
                }
                else if ((user.Email != kullanicilar.Email && user.KullaniciAdi == kullanicilar.Email) && Decrypt(user.Sifre) == kullanicilar.Sifre)
                {
                    ViewBag.Hata = "Email veya kullanıcı adınızı yanlış girdiniz";
                    break;
                }
                else if ((user.Email == kullanicilar.Email && user.KullaniciAdi != kullanicilar.Email) && Decrypt(user.Sifre) == kullanicilar.Sifre)
                {
                    ViewBag.Hata = "Email veya kullanıcı adınızı yanlış girdiniz";
                    break;
                }
                else
                {
                    ViewBag.Hata = "Kullanıcı Bulunamadı";
                }
               
            }
            
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
        public ActionResult Register()
        {

            return View();
        }
  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Kullanicilar kullanicilar)
        {

        if(kullanicilar.AdiSoyadi.Length <4 || kullanicilar.AdiSoyadi.Length >50 || kullanicilar.Sifre.Length <8 || kullanicilar.Sifre.Length >50)
            {
                TempData["mesaj"] = "Ad Soyad 4-50 veya sifre 8-50 karakter arası olmalıdır!";
                return RedirectToAction("Register", TempData["mesaj"]);
            }
            Kullanicilar kullanicilar1 = new Kullanicilar();
            var model = db.STAJYER_TANIM.ToList();
            var model2 = db.KURUM_PERSONEL.ToList();
            foreach (var item in model)
            {
                if (item.EMAIL == kullanicilar.Email)
                {
                    kullanicilar1.Rol = "Stajyer";
                    kullanicilar1.CalisanKurumTanim = item.FK_STAJ_KURUM;
                    kullanicilar1.CalisanDepartman = item.FK_DEPARTMAN;
                    kullanicilar1.CalisanPersonel = null;
                    goto a2;
                }
            }
            foreach (var item in model2)
            {
                if (item.EMAIL == kullanicilar.Email)
                {
                    kullanicilar1.Rol = "Calisan";
                    kullanicilar1.CalisanKurumTanim = item.FK_KURUM_TANIM;
                    kullanicilar1.CalisanDepartman = item.FK_KURUM_DEPARTMAN;
                    kullanicilar1.CalisanPersonel = item.PK_KURUM_PERSONEL;
                    goto a2;
                }

            }
        a2: if (kullanicilar.Sifre == kullanicilar.SifreKontrol)
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
                else if (UserNameControl(kullanicilar.KullaniciAdi) && EMailControl(kullanicilar.Email) == false)
                {
                    ViewBag.ErrorMessage2 = "Bu email daha önce kullanılmış !";
                    return View();
                }
                else
                {

                    ViewBag.ErrorMessage = "Bu kullanıcı adı ve email daha önce alınmış !";
              
                    return View();
                    //return RedirectToAction("Register", "Home");
                }



                kullanicilar1.Sifre = Encrypt(kullanicilar.SifreKontrol);
                kullanicilar1.SifreKontrol = Encrypt(kullanicilar.SifreKontrol);

                kullanicilar1.KayitTarihi = DateTime.Now;
                db.Kullanicilar.Add(kullanicilar1);

            
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)

                    {

                        Console.WriteLine(e);
                    }
            

            }
         
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("silangul7kahramanoglu@gmail.com", "Email Onaylama");
                mail.To.Add(kullanicilar.Email);
                mail.IsBodyHtml = true;
                mail.Subject = "Doğrulama";
                Guid rastegele = Guid.NewGuid();
                kullanicilar1.Kod = rastegele.ToString().Substring(0, 6);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)

                {

                    Console.WriteLine(e);
                }
                string a = kullanicilar1.Kod;
                //var url = Url.Action( "Dogrulama", "Home",null, protocol: Request.Url.Scheme);
                mail.Body = "Merhaba :" + kullanicilar.AdiSoyadi + "</br> <a> Hesabınızı aktifleştirmek için lütfen kodu giriniz.</a>" + "</br> <a>Doğrulama Kodu :</a>" + a;

                NetworkCredential net = new NetworkCredential("silangul7kahramanoglu@gmail.com", "qffleeulsqavpylo");

                client.UseDefaultCredentials = false;
                client.Credentials = net;
                client.EnableSsl = true;
                try
                {
                    client.Send(mail);

                    ViewBag.hata = "Doğrulama kodu mail adresinize gönderilmiştir.";
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e);
                }

        
            return RedirectToAction("Dogrulama", "Home");
        }
        [HttpGet]
        public ActionResult AktifStajlar(string bilgi)
        {
            var model = db.Stajlar.Where(x => x.Aktiflik == true).ToList();

            ViewBag.bilgi = bilgi;
            return View(model);
        }
        [HttpGet]
        public ActionResult BasvuruList(int id)
        {
            var kullanici = db.Kullanicilar.Where(x=>x.KullaniciAdi == User.Identity.Name || x.Email ==User.Identity.Name).First();
            var model = db.Basvuru.Where(x => (x.Mail == kullanici.Email ) && x.Fk_Staj_Id == id).FirstOrDefault();
            if (model != null && model.BasvuruDurumu== "Red edildi")
            {
                ViewBag.Bilgi = "Red edilmişsiniz lütfen başka stajlara bakınız";
                return RedirectToAction("AktifStajlar", new { bilgi = ViewBag.Bilgi });

            }  
            if (model != null)
            {
                ViewBag.Bilgi = "Zaten staj başvurunuz  bulunmaktadır";
                return RedirectToAction("AktifStajlar", new { bilgi = ViewBag.Bilgi });

            }
            List<SelectListItem> degerler4 = (from i in db.Uni.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.UniName,
                                                  Value = i.UniId.ToString()

                                              }).ToList();
            ViewBag.dgr4 = degerler4;
            List<SelectListItem> degerler5 = (from i in db.Bolumler.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.BolumName,
                                                  Value = i.BolumId.ToString()

                                              }).ToList();
            ViewBag.dgr5 = degerler5;
            ViewBag.stajId = id;
            //var model2 = db.Basvuru.FirstOrDefault();
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult BasvuruList(int id,Basvuru b)
        {
      
            b.BasvuruTarihi = DateTime.Now;
            b.Fk_Staj_Id= id;
            b.BasvuruDurumu = "Onaylanmadı";
            db.Basvuru.Add(b);
         
            try
            {
                db.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }

            return RedirectToAction("Index");

        
        }
        private string Encrypt(string clearText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }

            return clearText;
        }

        private string Decrypt(string cipherText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return cipherText;
        }

        [HttpGet]
        public ActionResult Dogrulama()
        {
            ViewBag.hata5 = "Hesabınızı aktifleşirmeniz gerekmektedir.";

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dogrulama(string kod)
        {
            var model = db.Kullanicilar.ToList();
            foreach (var item in model)
            {
                if (item.Kod == kod)
                {
                    item.kodOnay = true;
                    db.SaveChanges();

                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    ViewBag.hata = "Yanlış kod girdiniz";

                }
            }
            return RedirectToAction("Dogrulama", "Home");
        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(Kullanicilar kullanicilar)
        {
            var model = db.Kullanicilar.Where(x => x.Email == kullanicilar.Email || x.KullaniciAdi == kullanicilar.Email).FirstOrDefault();
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
            if (User.Identity.IsAuthenticated)
            {
                var email = User.Identity.Name;
                var model = db.Kullanicilar.FirstOrDefault(x => x.Email == email || x.KullaniciAdi == email);
                if (model != null)
                {
                    ViewBag.data = Decrypt(model.Sifre);
                    return View(model);

                }
                else
                {
                    var model2 = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == email);
                    ViewBag.data = Decrypt(model2.Sifre);
                    return View(model2);

                }
            }
            return HttpNotFound();


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuncelleBireysel(Kullanicilar k)
        {
            var model = db.Kullanicilar.Find(k.Id);
     
            model.Sifre = Encrypt(k.Sifre);
            model.SifreKontrol = Encrypt(k.Sifre);
            model.AdiSoyadi = k.AdiSoyadi;
           
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
            return Json(!db.Kullanicilar.Any(x => x.KullaniciAdi == kullaniciAdi), JsonRequestBehavior.AllowGet);
        }
        public JsonResult EmailKontrol(string email)
        {
            return Json(!db.Kullanicilar.Any(x => x.Email == email), JsonRequestBehavior.AllowGet);
        }
    }
}