//using DocumentFormat.OpenXml.EMMA;
//using DocumentFormat.OpenXml.Vml;
using Rotativa;
using StajYonetimBilgiSistemi.Models;
using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace StajYonetimBilgiSistemi.Controllers
{
    [Authorize(Roles = "Stajyer")]
    public class GunlukCalismaController : Controller
    {
        SBYSEntities db = new SBYSEntities();
        // GET: GunlukCalisma
        public ActionResult Charts()
        {

            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return View("~/Views/Shared/Error.cshtml");
        }
 
        public ActionResult Index(string ara)
        {
       
                var model = db.GUNLUK_CALISMA.Where(x => x.Kullanicilar.Email == User.Identity.Name || x.Kullanicilar.KullaniciAdi == User.Identity.Name).OrderByDescending(x => x.PK_GUNLUK_CALISMA).ToList();
                 if (!string.IsNullOrEmpty(ara))
            {
                ara = ara.ToLower();
                model = model.Where(x => x.Baslik.ToLower().Contains(ara) ).ToList();

                return View(model);
            }
           
                return View(model);
       

        }

        public ActionResult StajBilgileri()
        {
            var bul = db.STAJYER_TANIM.Where(x => (x.Kullanicilar.Email == User.Identity.Name || x.Kullanicilar.KullaniciAdi == User.Identity.Name)).FirstOrDefault();
     
            var model = db.Stajlar.Where(x =>( x.Pk_Staj_Id == bul.StajId )).FirstOrDefault();
        

            return View(model);


        }
        [HttpGet]
        public ActionResult FirmaBilgileriAl()
        {
            var b = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();

            var model = db.KURUM_TANIM.First(x => x.PK_KURUM_TANIM == b.CalisanKurumTanim);
            

            return View(model);

        }
        public JsonResult BarChartDataEF()
        {
            var b = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();

            var stajyerler = db.STAJYER_TANIM.Where(x=>x.FK_STAJ_KURUM == b.CalisanKurumTanim).ToList();
            Chart _chart = new Chart();
            _chart.labels = stajyerler.Select(x => x.Bolumler.BolumName).Distinct().ToArray();
            _chart.datasets = new List<Datasets>();


            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "İş Yerindeki Stajerlerin Bölüm Grafiği",
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
            var b = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();

            var stajyerler = db.STAJYER_TANIM.Where(x => x.FK_STAJ_KURUM == b.CalisanKurumTanim).ToList();
            Chart _chart = new Chart();
            _chart.labels = stajyerler.Select(x => x.Uni.UniName).Distinct().ToArray();
            _chart.datasets = new List<Datasets>();


            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "İş Yerindeki Stajerlerin Üniversite Grafiği",
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
        public ActionResult GunlukCalismaEkle()
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();

            var b = (from c in db.STAJYER_TANIM
                     where c.kullaniciId == a.Id
                     select c).SingleOrDefault();
            ViewBag.stajyer = b.ADI + " " + b.SOYADI;
            List<SelectListItem> degerler = (from i in db.STAJYER_TANIM.ToList()
                                             select new SelectListItem


                                             {
                                                 Text = i.ADI +" " + i.SOYADI,
                                                 Value = i.PK_STAJYER_TANIM.ToString()

                                             }).ToList();
            ViewBag.dgr = degerler;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GunlukCalismaEkle2(GUNLUK_CALISMA kp )
        {
            if (kp.ACIKLAMA == null)
            {
                TempData["mesaj"] = "Açıklama boş olamaz!";
                return RedirectToAction("GunlukCalismaEkle", TempData["mesaj"]);
            }
            if (kp.Baslik.Length>100 || kp.Baslik.Length<3 || kp.ACIKLAMA.Length<9 || kp.ACIKLAMA.Length>2000)
            {
                TempData["mesaj"] = "Alanlar istenen ölçüde değil!";
                return RedirectToAction("GunlukCalismaEkle", TempData["mesaj"]);
            }
         
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();

            var b = (from c in db.STAJYER_TANIM
                     where c.kullaniciId == a.Id
                     select c).SingleOrDefault();
            kp.FK_STAJYER_TANIM = b.PK_STAJYER_TANIM;
            kp.kullaniciadi = a.Id;
            db.GUNLUK_CALISMA.Add(kp);

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
        public ActionResult GuncelleBilgiGetir(int id,string V)
        {

            var model = db.GUNLUK_CALISMA.Find(id);
            List<SelectListItem> degerler = (from i in db.STAJYER_TANIM.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.ADI ,
                                                 Value = i.PK_STAJYER_TANIM.ToString()

                                             }).ToList();
            ViewBag.dgr = degerler;
            TempData["V"] = V;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guncelle(GUNLUK_CALISMA p)
        {
    
            if (p.ACIKLAMA == null)
            {
                TempData["mesaj"] = "Açıklama boş olamaz!";
                return RedirectToAction("GunlukCalismaEkle", TempData["mesaj"]);
            }
            if (p.Baslik.Length > 100 || p.Baslik.Length < 3 || p.ACIKLAMA.Length < 9 || p.ACIKLAMA.Length > 2000)
            {
                TempData["mesaj"] = "Alanlar istenen ölçüde değil!";
                return RedirectToAction("GunlukCalismaEkle", TempData["mesaj"]);
            }
                var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();
            p.kullaniciadi = a.Id;

            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
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
        public ActionResult SilBilgiGetir(int id)
        {
            var model = db.GUNLUK_CALISMA.Find(id);
            if (model == null) return HttpNotFound();
            return View(model);

        }
        public ActionResult Sil(GUNLUK_CALISMA p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Calendar()
        {

            return View();


        }
        [HttpGet]
        public JsonResult GetEvents()
        {
            using (SBYSEntities dc = new SBYSEntities())
            {
                var events = dc.Event.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            var status = false;
            using (SBYSEntities dc = new SBYSEntities())
            {
                if (e.EventID > 0)
                {
                    //Update the event
                    var v = dc.Event.Where(a => a.EventID == e.EventID).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;


                    }
                }
                else
                {
                    dc.Event.Add(e);
                }

                dc.SaveChanges();
                status = true;

            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (SBYSEntities dc = new SBYSEntities())
            {
                var v = dc.Event.Where(a => a.EventID == eventID).FirstOrDefault();
                if (v != null)
                {
                    dc.Event.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        public ActionResult GetAll()
        {


            var model = db.GUNLUK_CALISMA.Where(x => x.Kullanicilar.Email == User.Identity.Name || x.Kullanicilar.KullaniciAdi == User.Identity.Name).ToList();


            return View(model);

        }
    

        public ActionResult ExportToPdf()
        {
            bool k = true;
            if (k)
            {
                var q = new ActionAsPdf("GetAll");
                {
                    //q.PageWidth = 560;
                    //q.PageHeight = 360;
                    q.PageSize = Rotativa.Options.Size.A4;
                    //q.PageMargins.Left = 25;
                    //q.PageMargins.Right= 25;
                    q.IsJavaScriptDisabled = true;


                }
                return q;

            }

            return RedirectToAction("StajyerTanim", "Index");
        }


        public ActionResult GetAll2(int id)
        {

            var model = db.GUNLUK_CALISMA.Where(x => x.PK_GUNLUK_CALISMA == id).Take(1).ToList();


            return View(model);

        }
        public ActionResult ExportToPdf2(int id)
        {

            bool k = true;
            if (k)
            {
                var mymodel =  new ActionAsPdf("GetAll2", new {id= id});
                return mymodel;
            }

            return RedirectToAction("StajyerTanim", "Index");
        }
        [HttpGet]
        public ActionResult Basvurular()
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();
            var b = (from c in db.STAJYER_TANIM
                     where c.kullaniciId == a.Id
                     select c).SingleOrDefault();

            var model = db.Basvuru.Where(x => x.Fk_Stajyer_Id == b.PK_STAJYER_TANIM).ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult BasvurDetayları(int id)
        {
            var model = db.Basvuru.Find(id);
            return View(model);
        }
        [HttpGet]
        public ActionResult BasvuruList()
        {
            var model = db.STAJYER_TANIM.Where(x => (x.Kullanicilar.Email == User.Identity.Name || x.Kullanicilar.KullaniciAdi == User.Identity.Name) && x.StajId != null).FirstOrDefault();
            if (model != null)
            {
                ViewBag.Bilgi = "Zaten devam eden stajınız bulunmaktadır";
              return  RedirectToAction("AktifStajlar", new { bilgi = ViewBag.Bilgi });
                
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
            var model2 = db.Basvuru.FirstOrDefault();
            return View(model2);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BasvuruList(Basvuru  b)
        {
            db.Basvuru.Add(b);
            try
            {
                db.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }

            return RedirectToAction("BasvuruList");

        }
        [HttpGet]
        public ActionResult AktifStajlar(string Bilgi)
        {
            ViewBag.Bilgi = Bilgi;
            var model = db.Stajlar.Where(x=>x.Aktiflik==true).ToList();
            return View(model);
        }
    
    }
}