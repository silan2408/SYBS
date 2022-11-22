using Rotativa;
using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;

namespace StajYonetimBilgiSistemi.Controllers
{
    [Authorize(Roles = "Stajyer")]
    public class GunlukCalismaController : Controller
    {
        SBYSEntities14 db = new SBYSEntities14();
        // GET: GunlukCalisma
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                var model = db.GUNLUK_CALISMA.Where(x => x.Kullanicilar.Email == User.Identity.Name).ToList();

                return View(model);
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
                return View();
            }

        }



        [HttpGet]
        public ActionResult GunlukCalismaEkle()
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name
                     select c).SingleOrDefault();

            var b = (from c in db.STAJYER_TANIM
                     where c.PK_STAJYER_TANIM == a.Id
                     select c).SingleOrDefault();
            ViewBag.stajyer = b.ADI;
            List<SelectListItem> degerler = (from i in db.STAJYER_TANIM.ToList()
                                             select new SelectListItem


                                             {
                                                 Text = i.ADI,
                                                 Value = i.PK_STAJYER_TANIM.ToString()

                                             }).ToList();
            ViewBag.dgr = degerler;

            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GunlukCalismaEkle2(GUNLUK_CALISMA kp)
        {

            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name
                     select c).SingleOrDefault();

            var b = (from c in db.STAJYER_TANIM
                     where c.PK_STAJYER_TANIM == a.Id
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
        public ActionResult GuncelleBilgiGetir(int id)
        {

            var model = db.GUNLUK_CALISMA.Find(id);
            List<SelectListItem> degerler = (from i in db.STAJYER_TANIM.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.ADI,
                                                 Value = i.PK_STAJYER_TANIM.ToString()

                                             }).ToList();
            ViewBag.dgr = degerler;
            return View(model);
        }
        public ActionResult Guncelle(GUNLUK_CALISMA p)
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name
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
            using (SBYSEntities14 dc = new SBYSEntities14())
            {
                var events = dc.Event.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            var status = false;
            using (SBYSEntities14 dc = new SBYSEntities14())
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
            using (SBYSEntities14 dc = new SBYSEntities14())
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


            var model = db.GUNLUK_CALISMA.Where(x => x.Kullanicilar.Email == User.Identity.Name).ToList();


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
    }
}