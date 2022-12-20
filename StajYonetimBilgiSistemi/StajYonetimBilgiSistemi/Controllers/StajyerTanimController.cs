using ClosedXML.Excel;
using Rotativa;
using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace StajYonetimBilgiSistemi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StajyerTanimController : Controller
    {
        SBYSEntities db = new SBYSEntities();
        // GET: StajyerTanim

       
        public ActionResult Index(string ara, string SelectOption, string SelectOption2)
        {
            var model = db.STAJYER_TANIM.ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                ara = ara.ToLower();
                model = model.Where(x => x.ADI.ToLower().Contains(ara) || x.SOYADI.ToLower().Contains(ara)).ToList();
         
                return View(model);
            }


            if (!String.IsNullOrEmpty(SelectOption2) && SelectOption2 != "null")
            {

                SelectOption2 = SelectOption2.ToLower();
                model = model.Where(a => a.Uni.UniName.ToString().ToLower().Contains(SelectOption2)).ToList();
            
                return View(model);
          

            }
            else if (!String.IsNullOrEmpty(SelectOption) && SelectOption != "null")
            {

                SelectOption = SelectOption.ToLower();
                model = model.Where(a => a.Bolumler.BolumName.ToString().ToLower().Contains(SelectOption)).ToList();
   
                return View(model);
            }
      
            return View(model);
           


        }
   

        public ActionResult StajyerEkle()
        {

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
            else if (  kp.STAJ_BAS_TARIHI>kp.STAJ_BIT_TARIHI || kp.ADI.Length > 20 || kp.ADI.Length < 3 || kp.SOYADI.Length > 30 || kp.SOYADI.Length < 3 
                || kp.UNIV_NO.Length > 20 || kp.TELEFON.Length != 13 || kp.EMAIL.Length > 40 || kp.EMAIL.Length < 12 || kp.SINIFI.Length>10 ||kp.STAJ_YILI>2020 ||
                kp.STAJ_YILI<2000 )
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
           
            

            db.STAJYER_TANIM.Add(kp);
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
     
        public ActionResult GuncelleBilgiGetir(int id,string v)
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
                                                  Text = i.ADI + " " + i.SOYADI,
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
        public ActionResult Guncelle(STAJYER_TANIM p)
        {
            if (p.ADI == null ||p.SOYADI == null || p.UNIVERSITE == null || p.UNIV_NO == null || p.BOLUMU == null || p.SINIFI == null || p.EMAIL == null
                || p.TELEFON == null || p.STAJ_YILI.Equals(null) || p.STAJ_BAS_TARIHI == null || p.STAJ_BIT_TARIHI == null || p.KURUM_ST_SORUMLUSU.Equals(null)
                || p.KURUM_ONAY_KISI.Equals(null)
                || p.FK_STAJ_KURUM.Equals(null) || p.FK_DEPARTMAN.Equals(null))
            {
                TempData["mesaj"] = "Alanlar boş olamaz!";
                return RedirectToAction("GuncelleBilgiGetir", new { v = TempData["mesaj"], id = p.PK_STAJYER_TANIM });
            }
            else if (p.STAJ_BAS_TARIHI > p.STAJ_BIT_TARIHI || p.ADI.Length > 20 || p.ADI.Length < 3 || p.SOYADI.Length > 30 || p.SOYADI.Length < 3
                || p.UNIV_NO.Length > 20 || p.TELEFON.Length != 13 || p.EMAIL.Length > 40 || p.EMAIL.Length < 12 || p.SINIFI.Length > 10 ||p.STAJ_YILI > 2020 ||
                p.STAJ_YILI < 2000)
            {
                TempData["mesaj"] = "Alanlar istenen uzunlukta olmalı!";
                return RedirectToAction("GuncelleBilgiGetir", new { v = TempData["mesaj"], id = p.PK_STAJYER_TANIM });
            }
          

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
            var model = db.STAJYER_TANIM.Find(id);
            if (model == null) return HttpNotFound();
            return View(model);

        }


        public ActionResult Sil(STAJYER_TANIM p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index","StajyerTanim");
        }
     
        public ActionResult GetAll()
        {


            var model = db.STAJYER_TANIM.ToList();


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

         
            var model = db.STAJYER_TANIM.ToList();
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
    }
}