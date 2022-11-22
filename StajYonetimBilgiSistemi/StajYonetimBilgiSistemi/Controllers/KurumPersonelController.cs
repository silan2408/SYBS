using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace StajYonetimBilgiSistemi.Controllers
{
    [Authorize(Roles = "Admin,Calisan")]
    public class KurumPersonelController : Controller
    {
        SBYSEntities14 db = new SBYSEntities14();
        public ActionResult Index()
        {
            return View(db.KURUM_PERSONEL.ToList());
        }
        public ActionResult PersonelEkle()
        {
            List<SelectListItem> degerler = (from i in db.KURUM_DEPARTMAN.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.ADI,
                                                 Value = i.PK_KURUM_DEPARTMAN.ToString()

                                             }).ToList();
            ViewBag.dgr = degerler;
            List<SelectListItem> degerler1 = (from i in db.KURUM_TANIM.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.FIRMA_ADI,
                                                 Value = i.PK_KURUM_TANIM.ToString()

                                             }).ToList();
            ViewBag.dgr1 = degerler1;
            return View();
        }
        public ActionResult PersonelEkle2(KURUM_PERSONEL p)
        {
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
        public ActionResult GuncelleBilgiGetir(int id)
        {

            var model = db.KURUM_PERSONEL.Find(id);
            List<SelectListItem> degerler = (from i in db.KURUM_DEPARTMAN.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.ADI,
                                                 Value = i.PK_KURUM_DEPARTMAN.ToString()

                                             }).ToList();
            ViewBag.dgr = degerler;

            return View(model);
        }
        public ActionResult Guncelle(KURUM_PERSONEL p)
        {
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