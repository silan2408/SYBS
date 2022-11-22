using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace StajYonetimBilgiSistemi.Controllers
{
    [Authorize(Roles = "Admin,Calisan")]
    public class KurumTanimController : Controller
    {
        SBYSEntities14 db = new SBYSEntities14();
        public ActionResult Index()
        {

            return View(db.KURUM_TANIM.ToList());
        }
        public ActionResult KurumEkle()
        {

            return View();
        }
        public ActionResult KurumEkle2(KURUM_TANIM kp)
        {
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
        public ActionResult GuncelleBilgiGetir(int id)
        {

            var model = db.KURUM_TANIM.Find(id);

            return View(model);
        }
        public ActionResult Guncelle(KURUM_TANIM p)
        {
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