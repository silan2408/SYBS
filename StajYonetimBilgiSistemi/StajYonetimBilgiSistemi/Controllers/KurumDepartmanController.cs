using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace StajYonetimBilgiSistemi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KurumDepartmanController : Controller
    {
        SBYSEntities db = new SBYSEntities();
        public ActionResult Index(string ara)
        {
            var model = db.KURUM_DEPARTMAN.ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                ara = ara.ToLower();
                model = model.Where(x => x.ADI.ToLower().Contains(ara)).ToList();
            }

            return View(model);
        }
        public ActionResult DepartmanEkle()
        {
            List<SelectListItem> degerler = (from i in db.KURUM_TANIM.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.FIRMA_ADI,
                                                 Value = i.PK_KURUM_TANIM.ToString()

                                             }).ToList();
            ViewBag.dgr = degerler;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DepartmanEkle2(KURUM_DEPARTMAN kp)
        {
            var model2 = db.KURUM_DEPARTMAN.Where(x => x.FK_KURUM_TANIM == kp.FK_KURUM_TANIM  && x.ADI == kp.ADI).FirstOrDefault();
            if(model2 != null)
            {
                TempData["mesaj"] = "Bu Departman daha önce eklenmiş!";
                return RedirectToAction("DepartmanEkle", TempData["mesaj"]);
            }
            if (kp.FK_KURUM_TANIM == null || kp.AKTIF == null || kp.ADI == null)
            {
                TempData["mesaj"] = "Alanlar boş olamaz!";
                return RedirectToAction("DepartmanEkle", TempData["mesaj"]);
            }
            else if (kp.ADI.Length > 50 || kp.ADI.Length < 4 )
            {
                TempData["mesaj"] = "Alanlar istenen uzunlukta olmalı!";
                return RedirectToAction("DepartmanEkle", TempData["mesaj"]);
            }
            db.KURUM_DEPARTMAN.Add(kp);
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
        public ActionResult GuncelleBilgiGetir(int id ,string v)
        {
            TempData["V"] = v;
            var model = db.KURUM_DEPARTMAN.Find(id);

            List<SelectListItem> degerler = (from i in db.KURUM_TANIM.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.FIRMA_ADI,
                                                 Value = i.PK_KURUM_TANIM.ToString()

                                             }).ToList();
            ViewBag.dgr = degerler;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guncelle(KURUM_DEPARTMAN p)
        {
            if (p.KURUM_TANIM.FIRMA_ADI == null || p.AKTIF == null || p.ADI == null)
            {
                TempData["mesaj"] = "Alanlar boş olamaz!";
                return RedirectToAction("GuncelleBilgiGetir", new { v = TempData["mesaj"], id = p.PK_KURUM_DEPARTMAN });
            }
            else if (p.KURUM_TANIM.FIRMA_ADI.Length > 50 || p.KURUM_TANIM.FIRMA_ADI.Length < 4)
            { 
                TempData["mesaj"] = "Alanlar istenen uzunlukta olmalı!";
                return RedirectToAction("GuncelleBilgiGetir", new { v = TempData["mesaj"], id = p.PK_KURUM_DEPARTMAN });
            }
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult SilBilgiGetir(int id)
        {
            var model = db.KURUM_DEPARTMAN.Find(id);
            if (model == null) return HttpNotFound();
            return View(model);

        }
        public ActionResult Sil(KURUM_DEPARTMAN p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}