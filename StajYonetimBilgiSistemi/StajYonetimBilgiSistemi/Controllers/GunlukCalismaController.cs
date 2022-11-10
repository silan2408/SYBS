using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StajYonetimBilgiSistemi.Controllers
{
    [Authorize(Roles = "Stajyer")]
    public class GunlukCalismaController : Controller
    {
        SBYSEntities12 db = new SBYSEntities12();
        // GET: GunlukCalisma
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
              var model =    db.GUNLUK_CALISMA.Where(x => x.Kullanicilar.Email == User.Identity.Name).ToList();
              
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
          
            List<SelectListItem> degerler  = (from i in db.STAJYER_TANIM.ToList()
                                             select new SelectListItem
                                             
                                             
                                             {
                                                 Text = i.ADI,
                                                 Value = i.PK_STAJYER_TANIM.ToString()

                                             }).ToList();
            ViewBag.dgr = degerler;
            return View();
        }
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult GunlukCalismaEkle2 (GUNLUK_CALISMA kp)
        {

            
              
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
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
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
        public JsonResult GetEvents()
        {
            using (SBYSEntities12 dc = new SBYSEntities12())
            {
                var events = dc.Event.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            var status = false;
            using (SBYSEntities12 dc = new SBYSEntities12())
            {
                if (e.EventID > 0)
                {
                    //Update the event
                    var v = dc.Event.Where(a => a.EventID == e.EventID).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.Son = e.Son;
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
            using (SBYSEntities12 dc = new SBYSEntities12())
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
    }
}   