using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace StajYonetimBilgiSistemi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KullaniciController : Controller
    {
        SBYSEntities db = new SBYSEntities();
        // GET: Kullanici
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult KullaniciList(string ara)
        {
            var model = db.Kullanicilar.ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                ara = ara.ToLower();
                model = model.Where(x => x.AdiSoyadi.ToLower().Contains(ara) || x.KullaniciAdi.ToLower().Contains(ara)).ToList();

                return View(model);
            }

            return View(model);
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
                                                  Text = i.ADI +" "+i.SOYADI,
                                                  Value = i.PK_KURUM_PERSONEL.ToString()

                                              }).ToList();
            ViewBag.dgr3 = degerler3;

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guncelle(Kullanicilar k)
        {
            if (User.IsInRole("Stajyer"))
            {
                k.Rol = "Stajyer";
            }
            else if (User.IsInRole("Calisan"))

            {
                k.Rol = "Calisan";
            }
            else if (User.IsInRole("SirketYetkilisi"))

            {
                k.Rol = "SirketYetkilisi";
            }
            else if (User.IsInRole("Admin"))

            {
                k.Rol = "Admin";
            }
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

            return RedirectToAction("KullaniciList", "Kullanici");
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
        public ActionResult Stajlar()
        {
            var model = db.Stajlar.ToList();

            return View(model);
        }
        public ActionResult Basvurular()
        {
            var model = db.Basvuru.ToList();
          
            return View(model);
        }
    }
}
