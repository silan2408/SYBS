using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace StajYonetimBilgiSistemi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KurumTanimController : Controller
    {
        SBYSEntities db = new SBYSEntities();
        public ActionResult Index(string ara)
        {
            var model = db.KURUM_TANIM.ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                ara = ara.ToLower();
                model = model.Where(x => x.FIRMA_ADI.ToLower().Contains(ara)).ToList();
            }
            return View(model);
        }
        public ActionResult KurumEkle()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KurumEkle2(KURUM_TANIM kp)
        {
            if (kp.FIRMA_ADI == null || kp.FIRMA_YEKTILISI == null || kp.FIRMA_ADRESİ == null)
            {
                TempData["mesaj"] = "Alanlar boş olamaz!";
                return RedirectToAction("KurumEkle", TempData["mesaj"]);
            }
            else if (kp.FIRMA_ADI.Length > 50 || kp.FIRMA_ADI.Length < 4 || kp.FIRMA_YEKTILISI.Length > 50 || kp.FIRMA_YEKTILISI.Length < 4 || kp.FIRMA_ADRESİ.Length > 100 || kp.FIRMA_ADRESİ.Length < 4)
            {
                TempData["mesaj"] = "Alanlar istenen uzunlukta olmalı!";
                return RedirectToAction("KurumEkle", TempData["mesaj"]);
            }

            db.KURUM_TANIM.Add(kp);
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
        public ActionResult GuncelleBilgiGetir(int id, string v)
        {
            TempData["V"] = v;

            var model = db.KURUM_TANIM.Find(id);

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guncelle(KURUM_TANIM p)
        {
            if (p.FIRMA_ADI == null || p.FIRMA_YEKTILISI == null || p.FIRMA_ADI == null)
            {
                TempData["mesaj"] = "Alanlar boş olamaz!";
                return RedirectToAction("KurumEkle", TempData["mesaj"]);
            }
            else if (p.FIRMA_ADI.Length > 50 || p.FIRMA_ADI.Length < 4 || p.FIRMA_YEKTILISI.Length > 50 || p.TELEFON.Length >14 || p.TELEFON.Length < 13 || p.FIRMA_YEKTILISI.Length < 4 || p.FIRMA_ADI.Length > 100 || p.FIRMA_ADI.Length < 4)
            {
                TempData["mesaj"] = "Alanlar istenen uzunlukta olmalı!";
                return RedirectToAction("GuncelleBilgiGetir",new { v = TempData["mesaj"], id = p.PK_KURUM_TANIM });
            }
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult SilBilgiGetir(int id)
        {
            var model = db.KURUM_TANIM.Find(id);
            if (model == null) return HttpNotFound();
            return View(model);

        }
        public ActionResult Sil(KURUM_TANIM p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}