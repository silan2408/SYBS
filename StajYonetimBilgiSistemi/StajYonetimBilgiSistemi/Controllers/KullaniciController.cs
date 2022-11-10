using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StajYonetimBilgiSistemi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KullaniciController : Controller
    {
        SBYSEntities12 db = new SBYSEntities12();
        // GET: Kullanici
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult KullaniciList()
        {
            return View(db.Kullanicilar.ToList());
        }

        public ActionResult GuncelleBilgiGetir(int id)
        { 
            var model = db.Kullanicilar.Find(id);
            List<SelectListItem> degerler = (from i in db.KURUM_TANIM.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.FIRMA_ADI,
                                                 Value = i.PK_KURUM_TANIM.ToString()

                                             }).ToList();
            ViewBag.dgr = degerler;
            List<SelectListItem> degerler2 = (from i in db.KURUM_DEPARTMAN.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.ADI,
                                                 Value = i.PK_KURUM_DEPARTMAN.ToString()

                                             }).ToList();
            ViewBag.dgr2 = degerler2;
            List<SelectListItem> degerler3 = (from i in db.KURUM_PERSONEL.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.ADI,
                                                 Value = i.PK_KURUM_PERSONEL.ToString()

                                             }).ToList();
            ViewBag.dgr3 = degerler3;
          
            return View(model);
        }
        public ActionResult Guncelle(Kullanicilar k)
        {
            k.Rol = "Stajyer";
            k.KayitTarihi = DateTime.Now;
            db.Entry(k).State = System.Data.Entity.EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }

            return RedirectToAction("KullaniciList", "Home");
        }
      
        public ActionResult SilBilgiGetir(int id)
        {
            var model = db.Kullanicilar.Find(id);
            if (model == null) return HttpNotFound();
            return View(model);
        }
        public ActionResult Sil(Kullanicilar k)
        {

            db.Entry(k).State = System.Data.Entity.EntityState.Deleted;

            db.SaveChanges();


            return RedirectToAction("KullaniciList", "Kullanici");
        }
    }
}