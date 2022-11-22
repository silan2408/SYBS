using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StajYonetimBilgiSistemi.Controllers
{

    [AllowAnonymous]
    public class HomeController : Controller
    {
        SBYSEntities14 db = new SBYSEntities14();

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
            var a = 1;
        
            var userInDb = db.Kullanicilar.ToList();
            foreach (var user in userInDb)
            {
                if ((user.Email == kullanicilar.Email || user.KullaniciAdi == kullanicilar.Email) && Decrypt(user.Sifre)== kullanicilar.Sifre)
                {
                    a = 2;
                    if (userInDb != null)
                    {
                        FormsAuthentication.SetAuthCookie(kullanicilar.Email, false);

                        foreach (var item in userInDb)
                        {
                            if (item.Email == kullanicilar.Email || item.KullaniciAdi == kullanicilar.Email)
                            {

                                if (item.kodOnay == true)
                                {


                                    var model = db.STAJYER_TANIM.ToList();
                                    var model2 = db.KURUM_PERSONEL.ToList();
                                    foreach (var item2 in model)
                                    {
                                        if (item2.EMAIL  == kullanicilar.Email)
                                        {
                                            var model5 = db.Kullanicilar.Where(x => x.Email == kullanicilar.Email).FirstOrDefault();
                                            model5.Rol = "Stajyer";
                                            model5.CalisanKurumTanim = item2.FK_STAJ_KURUM;
                                            model5.CalisanDepartman = item2.FK_DEPARTMAN;
                                            model5.CalisanPersonel = null;
                                            UpdateModel(model5);
                                            db.SaveChanges();
                                           


                                        }
                                    }
                                    foreach (var item3 in model2)
                                    {
                                        if (item3.EMAIL == kullanicilar.Email)
                                        {

                                            var model5 = db.Kullanicilar.Where(x => x.Email == kullanicilar.Email).FirstOrDefault();
                                            model5.Rol = "Calisan";
                                            model5.CalisanKurumTanim = item3.FK_KURUM_TANIM;
                                            model5.CalisanDepartman = item3.FK_KURUM_DEPARTMAN;
                                            model5.CalisanPersonel = item3.PK_KURUM_PERSONEL;
                                            UpdateModel(model5);
                                            db.SaveChanges();

                                        }

                                    }


                                    return RedirectToAction("Index", "Home");


                                }
                                else
                                {
                                    System.Threading.Thread.Sleep(5000);
                                    return RedirectToAction("Dogrulama", "Home");


                                }
                            }
                        }
                    }
                }


            }
            if (a == 1)
            {
                ViewBag.Hata = "Email veya şifrenizi yanlış girdiniz";
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
        int a = 1;
        [HttpPost]
        public ActionResult Register(Kullanicilar kullanicilar)
        {
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
                }
                else
                {
                    kullanicilar1.Rol = "";
                }
            }
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
                else if (UserNameControl(kullanicilar.KullaniciAdi) && EMailControl(kullanicilar.Email) == false)
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



                kullanicilar1.Sifre = Encrypt(kullanicilar.SifreKontrol);
                kullanicilar1.SifreKontrol = Encrypt(kullanicilar.SifreKontrol) ;

                kullanicilar1.KayitTarihi = DateTime.Now;
                db.Kullanicilar.Add(kullanicilar1);

                if (a == 1)
                {
                    db.SaveChanges();
                    a = 0;
                }

            }
            if (a == 0)
            {

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("silangul7kahramanoglu@gmail.com", "Email Onaylama");
                mail.To.Add(kullanicilar.Email);
                mail.IsBodyHtml = true;
                mail.Subject = "Doğrulama";
                Guid rastegele = Guid.NewGuid();
                kullanicilar1.Kod = rastegele.ToString().Substring(0, 6);
                db.SaveChanges();
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

            }
            return RedirectToAction("Dogrulama", "Home");
        }
        [HttpGet]
        public ActionResult SirketYetkilileri()
        {



            return View();
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
            if (User.Identity.IsAuthenticated)
            {
                var email = User.Identity.Name;
                var model = db.Kullanicilar.FirstOrDefault(x => x.Email == email);
                if (model != null)
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

            if (User.IsInRole("Admin"))
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
            return Json(!db.Kullanicilar.Any(x => x.KullaniciAdi == kullaniciAdi), JsonRequestBehavior.AllowGet);
        }
        public JsonResult EmailKontrol(string email)
        {
            return Json(!db.Kullanicilar.Any(x => x.Email == email), JsonRequestBehavior.AllowGet);
        }
    }
}