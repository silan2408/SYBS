using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StajYonetimBilgiSistemi.Controllers
{
    [Authorize(Roles = "Admin,Calisan")]
    public class KurumDepartmanController : Controller
    {
        SBYSEntities12 db = new SBYSEntities12();
        // GET: KurumDepartman
        public ActionResult Index()
        {
            return View(db.KURUM_DEPARTMAN.ToList());
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
        public ActionResult DepartmanEkle2(KURUM_DEPARTMAN kp)
        {
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
        public ActionResult GuncelleBilgiGetir(int id)
        {
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
        public ActionResult Guncelle(KURUM_DEPARTMAN p)
        {
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