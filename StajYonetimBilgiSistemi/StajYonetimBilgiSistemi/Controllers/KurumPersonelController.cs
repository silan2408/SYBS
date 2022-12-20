//using DocumentFormat.OpenXml.EMMA;
using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace StajYonetimBilgiSistemi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KurumPersonelController : Controller
    {
        SBYSEntities db = new SBYSEntities();
        public ActionResult Index(string ara)
        {
            var model = db.KURUM_PERSONEL.ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                ara = ara.ToLower();
                model = model.Where(x => x.ADI.ToLower().Contains(ara) || x.SOYADI.ToLower().Contains(ara)).ToList();
            }
            return View(model);
        }
        public ActionResult PersonelEkle()
        {
            List<KURUM_TANIM> degerler1 = db.KURUM_TANIM.ToList();
            ViewBag.dgr1 = new SelectList(degerler1, "PK_KURUM_TANIM", "FIRMA_ADI");
      
            return View();
        }
        public JsonResult GetDepartman(int FK_KURUM_TANIM)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<KURUM_DEPARTMAN> degerler = db.KURUM_DEPARTMAN.Where(x => x.FK_KURUM_TANIM == FK_KURUM_TANIM).ToList();
         
            return Json(degerler,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersonelEkle2(KURUM_PERSONEL p)
        {
            if (p.ADI == null || p.SOYADI == null || p.UNVAN == null ||p.FK_KURUM_DEPARTMAN.Equals(null) || p.FK_KURUM_TANIM== null||p.GSM==null|| p.EMAIL==null)
            {
                TempData["mesaj"] = "Alanlar boş olamaz!";
                return RedirectToAction("PersonelEkle", TempData["mesaj"]);
            }
            else if (p.ADI.Length > 20 || p.ADI.Length < 3|| p.SOYADI.Length > 30 || p.SOYADI.Length < 3|| p.UNVAN.Length > 30 || p.GSM.Length != 13||  p.EMAIL.Length > 40 || p.EMAIL.Length < 12)
            {
                TempData["mesaj"] = "Alanlar istenen uzunlukta olmalı!";
                return RedirectToAction("PersonelEkle", TempData["mesaj"]);
            }
            var a = db.KURUM_PERSONEL.Where(x => x.ADI == p.ADI && x.SOYADI == p.SOYADI && x.UNVAN == p.UNVAN && x.FK_KURUM_DEPARTMAN == p.FK_KURUM_DEPARTMAN &&
            x.FK_KURUM_TANIM == p.FK_KURUM_TANIM && x.GSM == p.GSM && x.EMAIL == p.EMAIL).FirstOrDefault();
            if(a != null)
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
            db.KURUM_PERSONEL.Add(p);
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
            var model = db.KURUM_PERSONEL.Find(id);

            List<KURUM_TANIM> degerler1 = db.KURUM_TANIM.ToList();
            ViewBag.dgr1 = new SelectList(degerler1, "PK_KURUM_TANIM", "FIRMA_ADI");

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guncelle(KURUM_PERSONEL p)
        {
            if (p.ADI == null || p.SOYADI == null || p.UNVAN == null || p.FK_KURUM_DEPARTMAN.Equals(null) || p.GSM == null || p.EMAIL == null)
            {
                TempData["mesaj"] = "Alanlar boş olamaz!";
                return RedirectToAction("GuncelleBilgiGetir", new { v = TempData["mesaj"], id = p.PK_KURUM_PERSONEL });
            }
            else if (p.ADI.Length > 20 || p.ADI.Length < 3 || p.SOYADI.Length > 30 || p.SOYADI.Length < 3 || p.UNVAN.Length > 50 || p.GSM.Length != 13|| p.EMAIL.Length > 40 || p.EMAIL.Length < 12)
            {
                TempData["mesaj"] = "Alanlar istenen uzunlukta olmalı!";
                return RedirectToAction("GuncelleBilgiGetir", new { v = TempData["mesaj"], id = p.PK_KURUM_PERSONEL });
            }
           
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult SilBilgiGetir(int id)
        {
            var model = db.KURUM_PERSONEL.Find(id);
            if (model == null) return HttpNotFound();
            return View(model);

        }
        public ActionResult Sil(KURUM_PERSONEL p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}