using ClosedXML.Excel;
//using DocumentFormat.OpenXml.EMMA;
//using DocumentFormat.OpenXml.Vml;
using Rotativa;
using StajYonetimBilgiSistemi.Models;
using StajYonetimBilgiSistemi.Models.Entity;
using StajYonetimBilgiSistemi.Roller;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace StajYonetimBilgiSistemi.Controllers
{
    [Authorize(Roles = "SirketYetkilisi")]
    public class SirketYetkilisiController : Controller
    {
        SBYSEntities db = new SBYSEntities();
        // GET: Calisan
        public ActionResult Charts()
        {

            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return View("~/Views/Shared/Error.cshtml");
        }
        public JsonResult BarChartDataEF()
        {
            var b = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();

            var stajyerler = db.STAJYER_TANIM.Where(x => x.FK_STAJ_KURUM == b.CalisanKurumTanim).ToList();
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
        public ActionResult FirmaBilgileriAl()
        {

            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();

            var model = db.KURUM_TANIM.First(x => x.PK_KURUM_TANIM == a.CalisanKurumTanim);


            return View(model);

        }

        [HttpGet]
        public ActionResult GuncelleBilgiGetirFirma(int id)
        {

            var model = db.KURUM_TANIM.Find(id);

            return View(model);
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult GuncelleFirma(KURUM_TANIM p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("FirmaBilgileriAl");
        }

  
        public ActionResult PersonelBilgileriniAl(string ara)
        {
            var a = (from c in db.Kullanicilar
                     where (c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name)
                     select c).SingleOrDefault();
            //var model2 = db.Kullanicilar.ToList();
            var model = db.KURUM_PERSONEL.Where(x => x.FK_KURUM_TANIM == a.CalisanKurumTanim).ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                ara = ara.ToLower();
                model = model.Where(x => x.ADI.ToLower().Contains(ara) || x.SOYADI.ToLower().Contains(ara)).ToList();
            }
            return View(model);
        }

        public ActionResult PersonelEkle()
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();
            List<SelectListItem> degerler = (from i in db.KURUM_DEPARTMAN.Where(x=>x.FK_KURUM_TANIM==a.CalisanKurumTanim && x.AKTIF ==1).ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.ADI,
                                                 Value = i.PK_KURUM_DEPARTMAN.ToString()

                                             }).ToList();
            ViewBag.dgr = degerler;

           
            ViewBag.dgr2 = a.KURUM_TANIM.FIRMA_ADI;

            return View();
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult PersonelEkle2(KURUM_PERSONEL p)
        {
            if (p.ADI == null || p.SOYADI == null || p.UNVAN == null || p.FK_KURUM_DEPARTMAN.Equals(null) || p.GSM == null || p.EMAIL == null)
            {
                TempData["mesaj"] = "Alanlar boş olamaz!";
                return RedirectToAction("PersonelEkle", TempData["mesaj"]);
            }
            else if (p.ADI.Length > 20 || p.ADI.Length < 3 || p.SOYADI.Length > 30 || p.SOYADI.Length < 3 || p.UNVAN.Length > 30 || p.GSM.Length != 13 || p.EMAIL.Length > 40 || p.EMAIL.Length < 12)
            {
                TempData["mesaj"] = "Alanlar istenen uzunlukta olmalı!";
                return RedirectToAction("PersonelEkle", TempData["mesaj"]);
            }
            var c = db.KURUM_PERSONEL.Where(x => x.ADI == p.ADI && x.SOYADI == p.SOYADI && x.UNVAN == p.UNVAN && x.FK_KURUM_DEPARTMAN == p.FK_KURUM_DEPARTMAN &&
            x.FK_KURUM_TANIM == p.FK_KURUM_TANIM && x.GSM == p.GSM && x.EMAIL == p.EMAIL).FirstOrDefault();
            if (c != null)
            {
                TempData["mesaj"] = "Bu personel zaten kayıtlı !";
                return RedirectToAction("PersonelEkle", TempData["mesaj"]);
            }
            var b = db.KURUM_PERSONEL.Where(x => x.GSM == p.GSM || x.EMAIL == p.EMAIL).FirstOrDefault();
            if (b != null)
            {
                TempData["mesaj"] = "Bu telefon veya email adresi kullanılıyor!";
                return RedirectToAction("PersonelEkle", TempData["mesaj"]);
            }
            var a = (from d in db.Kullanicilar
                     where d.Email == User.Identity.Name
                     select d).SingleOrDefault();
            p.FK_KURUM_TANIM = a.CalisanKurumTanim;
            db.KURUM_PERSONEL.Add(p);
            try
            {
                db.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }
            return RedirectToAction("PersonelBilgileriniAl");
        }
        public ActionResult SilBilgiGetirPersonel(int id)
        {
            var model = db.KURUM_PERSONEL.Find(id);
            if (model == null) return HttpNotFound();
            return View(model);

        }
        public ActionResult SilPersonel(KURUM_PERSONEL p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("PersonelBilgileriniAl");
        }
        [HttpGet]
        public ActionResult GuncelleBilgiGetirPersonel(int id,string v)
        {

            TempData["V"] = v;
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();
            var model = db.KURUM_PERSONEL.Find(id);
            List<SelectListItem> degerler = (from i in db.KURUM_DEPARTMAN.Where(x => x.FK_KURUM_TANIM == a.CalisanKurumTanim && x.AKTIF == 1).ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.ADI,
                                                 Value = i.PK_KURUM_DEPARTMAN.ToString()

                                             }).ToList();
            ViewBag.dgr = degerler;

            return View(model);
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult GuncellePersonel(KURUM_PERSONEL p)
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();
            if (p.ADI == null || p.SOYADI == null || p.UNVAN == null || p.FK_KURUM_DEPARTMAN.Equals(null) || p.GSM == null || p.EMAIL == null)
            {
                TempData["mesaj"] = "Alanlar boş olamaz!";
                return RedirectToAction("GuncelleBilgiGetirPersonel", new { v = TempData["mesaj"], id = p.PK_KURUM_PERSONEL });
            }
            else if (p.ADI.Length > 20 || p.ADI.Length < 3 || p.SOYADI.Length > 30 || p.SOYADI.Length < 3 || p.UNVAN.Length > 50 || p.GSM.Length != 13 || p.EMAIL.Length > 40 || p.EMAIL.Length < 12)
            {
                TempData["mesaj"] = "Alanlar istenen uzunlukta olmalı!";
                return RedirectToAction("GuncelleBilgiGetirPersonel", new { v = TempData["mesaj"], id = p.PK_KURUM_PERSONEL });
            }
            p.FK_KURUM_TANIM = a.CalisanKurumTanim;
            try
            {  
                db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
                return View();
            }

            return RedirectToAction("PersonelBilgileriniAl");
        }

        public ActionResult StajerBilgileriniAl(string ara, string SelectOption, string SelectOption2)
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi== User.Identity.Name
                     select c).SingleOrDefault();

            var model = db.STAJYER_TANIM.Where(x => x.FK_STAJ_KURUM == a.CalisanKurumTanim).ToList();


            if (!string.IsNullOrEmpty(ara))
            {
                ara = ara.ToLower();
                model = model.Where(x => x.ADI.ToLower().Contains(ara) || x.SOYADI.ToLower().Contains(ara)).ToList();
            }


            if (SelectOption != "null" && SelectOption2 != "null" && SelectOption != null && SelectOption2 != null)
            {

                SelectOption = SelectOption.ToLower();
                SelectOption2 = SelectOption2.ToLower();
                model = model.Where(x => x.Bolumler.BolumName.ToLower().ToString().Contains(SelectOption) && x.Uni.UniName.ToLower().ToString().Contains(SelectOption2)).ToList();
                return View(model);



            }

            else if (!String.IsNullOrEmpty(SelectOption2) && SelectOption == "null")
            {

                SelectOption = SelectOption2.ToLower();
                model = model.Where(x => x.Uni.UniName.ToLower().ToString().Contains(SelectOption)).ToList();
                return View(model);

            }
            else if (!String.IsNullOrEmpty(SelectOption) && SelectOption2 == "null")
            {

                SelectOption2 = SelectOption.ToLower();
                model = model.Where(x => x.Bolumler.BolumName.ToLower().ToString().Contains(SelectOption2)).ToList();
                return View(model);
            }
         
            return View(model);
        }
        public ActionResult StajyerEkle()
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();
            ViewBag.dgr10 = a.KURUM_TANIM.FIRMA_ADI.ToString();

            List<SelectListItem> degerler = (from i in db.KURUM_PERSONEL.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.ADI +" "+ i.SOYADI,
                                                 Value = i.PK_KURUM_PERSONEL.ToString()

                                             }).ToList();
            ViewBag.dgr = degerler;

            List<SelectListItem> degerler1 = (from i in db.KURUM_PERSONEL.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.ADI + " " + i.SOYADI,
                                                  Value = i.PK_KURUM_PERSONEL.ToString()

                                              }).ToList();
            ViewBag.dgr1 = degerler1;


            List<SelectListItem> degerler3 = (from i in db.KURUM_DEPARTMAN.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.ADI ,
                                                  Value = i.PK_KURUM_DEPARTMAN.ToString()

                                              }).ToList();
            ViewBag.dgr3 = degerler3;
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
            return View();
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult StajyerEkle2(STAJYER_TANIM kp)
        {
            if (kp.ADI == null || kp.SOYADI == null || kp.UNIVERSITE == null || kp.UNIV_NO == null || kp.BOLUMU == null || kp.SINIFI == null || kp.EMAIL == null
               || kp.TELEFON == null || kp.STAJ_YILI.Equals(null) || kp.STAJ_BAS_TARIHI == null || kp.STAJ_BIT_TARIHI == null || kp.KURUM_ST_SORUMLUSU.Equals(null)
               || kp.KURUM_ONAY_KISI.Equals(null)
               || kp.FK_STAJ_KURUM.Equals(null) || kp.FK_DEPARTMAN.Equals(null))
            {
                TempData["mesaj"] = "Alanlar boş olamaz!";
                return RedirectToAction("StajyerEkle", TempData["mesaj"]);
            }
            else if (kp.STAJ_BAS_TARIHI > kp.STAJ_BIT_TARIHI || kp.ADI.Length > 20 || kp.ADI.Length < 3 || kp.SOYADI.Length > 30 || kp.SOYADI.Length < 3
                || kp.UNIV_NO.Length > 20 || kp.TELEFON.Length != 13 || kp.EMAIL.Length > 40 || kp.EMAIL.Length < 12 || kp.SINIFI.Length > 10 || kp.STAJ_YILI > 2020 ||
                kp.STAJ_YILI < 2000)
            {
                TempData["mesaj"] = "Alanlar istenen uzunlukta olmalı!";
                return RedirectToAction("StajyerEkle", TempData["mesaj"]);
            }
            var a = db.STAJYER_TANIM.Where(x => x.ADI == kp.ADI && x.SOYADI == kp.SOYADI && x.UNIVERSITE == kp.UNIVERSITE && x.UNIV_NO == kp.UNIV_NO &&
            x.BOLUMU == kp.BOLUMU && x.SINIFI == kp.SINIFI && x.EMAIL == kp.EMAIL && x.TELEFON == kp.TELEFON && x.STAJ_YILI == kp.STAJ_YILI
             && x.STAJ_BAS_TARIHI == kp.STAJ_BAS_TARIHI && x.STAJ_BIT_TARIHI == kp.STAJ_BIT_TARIHI && x.KURUM_ST_SORUMLUSU == kp.KURUM_ST_SORUMLUSU
              && x.KURUM_ONAY_KISI == kp.KURUM_ONAY_KISI && x.FK_STAJ_KURUM == kp.FK_STAJ_KURUM && x.FK_DEPARTMAN == kp.FK_DEPARTMAN).FirstOrDefault();
            if (a != null)
            {
                TempData["mesaj"] = "Bu personel zaten kayıtlı !";
                return RedirectToAction("StajyerEkle", TempData["mesaj"]);
            }
            var b = db.STAJYER_TANIM.Where(x => x.TELEFON == kp.TELEFON || x.EMAIL == kp.EMAIL).FirstOrDefault();
            if (b != null)
            {
                TempData["mesaj"] = "Bu telefon veya email adresi kullanılıyor!";
                return RedirectToAction("StajyerEkle", TempData["mesaj"]);
            }


            var d = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name
                     select c).SingleOrDefault();
            kp.FK_STAJ_KURUM = (int)d.CalisanKurumTanim;
            
            db.STAJYER_TANIM.Add(kp);
            try
            {
                db.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }

            return RedirectToAction("StajerBilgileriniAl");
        }
        public ActionResult SilBilgiGetir(int id)
        {
            var model = db.STAJYER_TANIM.Find(id);
            if (model == null) return HttpNotFound();
            return View(model);

        }
        public ActionResult Sil(STAJYER_TANIM p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("StajerBilgileriniAl");
        }
        [HttpGet]
        public ActionResult GuncelleBilgiGetirStajyer(int id,string v)
        {


            TempData["V"] = v;
            var model = db.STAJYER_TANIM.Find(id);


            List<SelectListItem> degerler = (from i in db.KURUM_PERSONEL.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.ADI + " " + i.SOYADI ,
                                                 Value = i.PK_KURUM_PERSONEL.ToString()

                                             }).ToList();
            ViewBag.dgr = degerler;

            List<SelectListItem> degerler1 = (from i in db.KURUM_PERSONEL.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.ADI + " " + i.SOYADI ,
                                                  Value = i.PK_KURUM_PERSONEL.ToString()

                                              }).ToList();
            ViewBag.dgr1 = degerler1;

            List<SelectListItem> degerler2 = (from i in db.KURUM_TANIM.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.FIRMA_ADI,
                                                  Value = i.PK_KURUM_TANIM.ToString()

                                              }).ToList();
            ViewBag.dgr2 = degerler2;

            List<SelectListItem> degerler3 = (from i in db.KURUM_DEPARTMAN.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.ADI,
                                                  Value = i.PK_KURUM_DEPARTMAN.ToString()

                                              }).ToList();
            ViewBag.dgr3 = degerler3;
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
            return View(model);
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult GuncelleStajyer(STAJYER_TANIM p)
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();

            if (p.ADI == null || p.SOYADI == null || p.UNIVERSITE == null || p.UNIV_NO == null || p.BOLUMU == null || p.SINIFI == null || p.EMAIL == null
            || p.TELEFON == null || p.STAJ_YILI.Equals(null) || p.STAJ_BAS_TARIHI == null || p.STAJ_BIT_TARIHI == null || p.KURUM_ST_SORUMLUSU.Equals(null)
            || p.KURUM_ONAY_KISI.Equals(null)
            || p.FK_STAJ_KURUM.Equals(null)    || p.FK_DEPARTMAN.Equals(null))
            {
                TempData["mesaj"] = "Alanlar boş olamaz!";
                return RedirectToAction("GuncelleBilgiGetirStajyer", new { v = TempData["mesaj"], id = p.PK_STAJYER_TANIM });
            }
            else if (p.STAJ_BAS_TARIHI >= p.STAJ_BIT_TARIHI || p.ADI.Length > 20 || p.ADI.Length < 3 || p.SOYADI.Length > 30 || p.SOYADI.Length < 3
                || p.UNIV_NO.Length > 20 || p.TELEFON.Length != 13 || p.EMAIL.Length > 40 || p.EMAIL.Length < 12 || p.SINIFI.Length > 20 || p.STAJ_YILI > 2030 ||
                p.STAJ_YILI < 2000)
            {
                TempData["mesaj"] = "Alanlar istenen uzunlukta olmalı!";
                return RedirectToAction("GuncelleBilgiGetirStajyer", new { v = TempData["mesaj"], id = p.PK_STAJYER_TANIM });
            }

            p.FK_STAJ_KURUM = (int)a.CalisanKurumTanim;
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("StajerBilgileriniAl");
        }
        public ActionResult GetAll()
        {

            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();

            var model = db.STAJYER_TANIM.Where(x => x.FK_STAJ_KURUM == a.CalisanKurumTanim).ToList();



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

            return RedirectToAction("SirketYetkilisi", "StajerBilgileriniAl");
        }

        [HttpPost]
        public FileResult Export()
        {

            DataTable dt = new DataTable("Stajyerler");
            dt.Columns.AddRange(new DataColumn[16] { new DataColumn("Id"),
                                            new DataColumn("Adı"),
                                            new DataColumn("Soyadı"),
                                            new DataColumn("Üniversite"),
                                            new DataColumn("Üniversite Numarası"),
                                            new DataColumn("Bölümü"),
                                            new DataColumn("Sınıfı"),
                                            new DataColumn("Email"),
                                            new DataColumn("Telefon"),
                                            new DataColumn("Staj Yılı"),
                                            new DataColumn("Staj Başlama Tarihi"),
                                            new DataColumn("Staj Bitiş Tarihi"),
                                            new DataColumn("Kurum Staj Sorumlusu"),
                                            new DataColumn("Kurum Onay Kişi"),
                                            new DataColumn("Staj Kurumu"),
                                            new DataColumn("Staj Departmanı") });

            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();

            var model = db.STAJYER_TANIM.Where(x => x.FK_STAJ_KURUM == a.CalisanKurumTanim).ToList();
            foreach (var stajyer in model)
            {
                dt.Rows.Add(stajyer.PK_STAJYER_TANIM, stajyer.ADI, stajyer.SOYADI, stajyer.Uni.UniName,
                    stajyer.UNIV_NO, stajyer.Bolumler.BolumName, stajyer.SINIFI, stajyer.EMAIL, stajyer.TELEFON, stajyer.STAJ_YILI
                    , stajyer.STAJ_BAS_TARIHI, stajyer.STAJ_BIT_TARIHI, stajyer.KURUM_PERSONEL.ADI, stajyer.KURUM_PERSONEL1.ADI, stajyer.KURUM_TANIM.FIRMA_ADI
                    , stajyer.KURUM_DEPARTMAN.ADI);
            }
            using (XLWorkbook kb = new XLWorkbook())
            {
                kb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    kb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Stajyerler.xlsx");
                }
            }
        }
        public ActionResult Stajlar()
        {
            var model = db.Stajlar.ToList();
           
            return View(model);
        }
        [HttpGet]
        public ActionResult StajEkle()
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();
            ViewBag.dgr10 = a.KURUM_TANIM.FIRMA_ADI.ToString();
   

            List<SelectListItem> degerler3 = (from i in db.KURUM_DEPARTMAN.Where(x=>x.FK_KURUM_TANIM==a.CalisanKurumTanim && x.AKTIF == 1).ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.ADI,
                                                  Value = i.PK_KURUM_DEPARTMAN.ToString()

                                              }).ToList();
            ViewBag.dgr3 = degerler3;
            List<SelectListItem> degerler4 = (from i in db.KURUM_PERSONEL.Where(x => x.FK_KURUM_TANIM == a.CalisanKurumTanim ).ToList()
                                              select new SelectListItem
                                              {
                                                  Text =i.UNVAN +":  "+ i.ADI + "  "+ i.SOYADI +" / "+i.KURUM_DEPARTMAN.ADI ,
                                                  Value = i.PK_KURUM_PERSONEL.ToString()

                                              }).ToList();
            ViewBag.dgr4 = degerler4;
            List<SelectListItem> degerler5 = (from i in db.KURUM_PERSONEL.Where(x => x.FK_KURUM_TANIM == a.CalisanKurumTanim).ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.UNVAN + ":  " + i.ADI + "  " + i.SOYADI + " / " + i.KURUM_DEPARTMAN.ADI,
                                                  Value = i.PK_KURUM_PERSONEL.ToString()

                                              }).ToList();
            ViewBag.dgr5 = degerler5;
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StajEkle(Stajlar s)
        {

            var d = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name
                     select c).SingleOrDefault();
            s.Fk_Kurum_Id = (int)d.CalisanKurumTanim;
          
            db.Stajlar.Add(s);
            try
            {
                db.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }

            return RedirectToAction("StajEkle");
        }
        public ActionResult Degistir(int id)
        {
            var model = db.Stajlar.Find(id);
          if(model.Aktiflik==true)
            {
               model.Aktiflik = false;
                goto a;
            }
            if (model.Aktiflik == false)
            {
                model.Aktiflik = true;
                goto a;
            }
          a:  db.Entry(model).State = System.Data.Entity.EntityState.Modified;
  
            try
            {
                db.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }

            return RedirectToAction("Stajlar");
        }
        public ActionResult StajSil(int id)
        {
            var model = db.Stajlar.Find(id);
            db.Entry(model).State = System.Data.Entity.EntityState.Deleted;

            try
            {
                db.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }

            return RedirectToAction("Stajlar");
        }
        [HttpGet]
        public ActionResult StajGüncelle(int id)
        {
            var a= (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                    select c).SingleOrDefault();
            ViewBag.dgr10 = a.KURUM_TANIM.FIRMA_ADI.ToString();
            var model = db.Stajlar.Find(id);
            List<SelectListItem> degerler3 = (from i in db.KURUM_DEPARTMAN.Where(x => x.FK_KURUM_TANIM == a.CalisanKurumTanim && x.AKTIF == 1).ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.ADI,
                                                  Value = i.PK_KURUM_DEPARTMAN.ToString()

                                              }).ToList();
            ViewBag.dgr3 = degerler3;
            List<SelectListItem> degerler4 = (from i in db.KURUM_PERSONEL.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.UNVAN + ":  " + i.ADI + "  " + i.SOYADI + " / " + i.KURUM_DEPARTMAN.ADI,
                                                  Value = i.PK_KURUM_PERSONEL.ToString()

                                              }).ToList();
            ViewBag.dgr4 = degerler4;
            List<SelectListItem> degerler5 = (from i in db.KURUM_PERSONEL.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.UNVAN + ":  " + i.ADI + "  " + i.SOYADI + " / " + i.KURUM_DEPARTMAN.ADI,
                                                  Value = i.PK_KURUM_PERSONEL.ToString()

                                              }).ToList();
            ViewBag.dgr5 = degerler5;
            return View(model);
        }
        [HttpPost]
            public ActionResult StajGüncelle(Stajlar s)
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name
                     select c).SingleOrDefault();
            s.Fk_Kurum_Id = (int)a.CalisanKurumTanim;

           
            db.Entry(s).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }

     

            return RedirectToAction("Stajlar");
        }
        [HttpGet]
        public ActionResult SorumluOlunanStajyerler()
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();
            var b = (from c in db.KURUM_PERSONEL
                     where c.PK_KURUM_PERSONEL == a.CalisanPersonel
                     select c).First();
            var model = db.Stajlar.Where(x=>x.Fk_Onaylayan_Personel == b.PK_KURUM_PERSONEL || x.Fk_Sorumlu_Personel == b.PK_KURUM_PERSONEL).ToList();
            if(model.Count() == 0)
            {
                ViewBag.data = "Sorumlu olduğunuz stajyer bulunmamaktadır";
            }
            else if(model != null)
            {
                return View(model);
            }
            return View();

        }
        [HttpGet]
        public ActionResult Basvurular()
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();
            var b = (from c in db.Stajlar
                     where c.Fk_Kurum_Id == a.CalisanKurumTanim
                     select c).First();

            var model = db.Basvuru.Where(x => x.Fk_Staj_Id == b.Pk_Staj_Id).ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult BasvurDetayları(int id)
        {
            var model = db.Basvuru.Find(id);
            return View(model);
        }

        public ActionResult OnayDegis(int id)
        {
          
            var model = db.Basvuru.Find(id);
            var kontrol = db.STAJYER_TANIM.Where(x => x.EMAIL == model.Mail).FirstOrDefault();
            if (kontrol == null)
            {  
                if(model.BasvuruDurumu == "Onaylandı")
              {
                model.BasvuruDurumu = "Onaylanmadı";
                    goto a;
              }
               if (model.BasvuruDurumu == "Onaylanmadı" || model.BasvuruDurumu =="Red edildi")
               {  
                    STAJYER_TANIM st = new STAJYER_TANIM();
                    model.BasvuruDurumu = "Onaylandı";
               
                st.ADI = model.Ad;
                st.SOYADI = model.Soyad;
                st.UNIVERSITE = model.Uni_Id;
                st.UNIV_NO = model.UniNo.ToString();
                st.BOLUMU = model.Bolum_Id;
                    st.SINIFI = model.Sınıf;
                         st.TELEFON = model.Tel;
                    st.EMAIL = model.Mail;
                    st.STAJ_YILI = DateTime.Now.Year;
                    st.STAJ_BAS_TARIHI = model.Stajlar.Staj_Bas_Tarihi;
                    st.STAJ_BIT_TARIHI = model.Stajlar.Staj_Bit_Tarihi;
                    st.KURUM_ST_SORUMLUSU = model.Stajlar.Fk_Sorumlu_Personel;
                    st.KURUM_ONAY_KISI = (int)model.Stajlar.Fk_Onaylayan_Personel;
                    st.FK_STAJ_KURUM = model.Stajlar.Fk_Kurum_Id;
                    st.FK_DEPARTMAN = model.Stajlar.Fk_Kurum_Departman;
                    st.StajId = model.Fk_Staj_Id;

                    db.STAJYER_TANIM.Add(st);
                    db.SaveChanges();
                    var ıd = db.STAJYER_TANIM.Where(x=>x.EMAIL == model.Mail).First();
                    model.Fk_Stajyer_Id = ıd.PK_STAJYER_TANIM;
               }

            }
            else
            {
                if (model.BasvuruDurumu == "Onaylandı")
                {
                    model.BasvuruDurumu = "Onaylanmadı";
                    goto a;
                }
                if (model.BasvuruDurumu == "Onaylanmadı")
                {
                    model.BasvuruDurumu = "Onaylandı";
                    goto a;
             
                }

            }

     a:     db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("BasvurDetayları", new {id = model.PK_Basvuru_Id});
        }
        public ActionResult Redet(int id)
        {
            var model = db.Basvuru.Find(id);
            model.BasvuruDurumu = "Red edildi";
            db.SaveChanges();
            return RedirectToAction("Basvurular");
        }
    }

}