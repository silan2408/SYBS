using Rotativa;
using StajYonetimBilgiSistemi.Models.Entity;
using StajYonetimBilgiSistemi.Roller;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace StajYonetimBilgiSistemi.Controllers
{
    [Authorize(Roles = "SirketYetkilisi")]
    public class SirketYetkilisiController : Controller
    {
        SBYSEntities14 db = new SBYSEntities14();
        // GET: Calisan

        [HttpGet]
        public ActionResult FirmaBilgileriAl()
        {

            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name
                     select c).SingleOrDefault();

            var model = db.KURUM_TANIM.Where(x => x.PK_KURUM_TANIM == a.CalisanKurumTanim).ToList();


            return View(model);

        }

        [HttpGet]
        public ActionResult GuncelleBilgiGetirFirma(int id)
        {

            var model = db.KURUM_TANIM.Find(id);

            return View(model);
        }
        [HttpPost]
        public ActionResult GuncelleFirma(KURUM_TANIM p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("FirmaBilgileriAl");
        }

        [HttpGet]
        public ActionResult PersonelBilgileriniAl()
        {
            var a = (from c in db.Kullanicilar
                     where (c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name)
                     select c).SingleOrDefault();
            //var model2 = db.Kullanicilar.ToList();
            var model = db.KURUM_PERSONEL.Where(x => x.FK_KURUM_TANIM == a.CalisanKurumTanim).ToList();



            return View(model);

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

            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name
                     select c).SingleOrDefault();
            ViewBag.dgr2 = a.KURUM_TANIM.FIRMA_ADI;

            return View();
        }
        public ActionResult PersonelEkle2(KURUM_PERSONEL p)
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name
                     select c).SingleOrDefault();
            p.FK_KURUM_TANIM = a.CalisanKurumTanim;
            db.KURUM_PERSONEL.Add(p);
            try
            {
                db.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }
            return RedirectToAction("PersonelBilgileriniAl");
        }
        public ActionResult SilBilgiGetirPersonel(int id)
        {
            var model = db.KURUM_PERSONEL.Find(id);
            if (model == null) return HttpNotFound();
            return View(model);

        }
        public ActionResult SilPersonel(KURUM_PERSONEL p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("PersonelBilgileriniAl");
        }
        [HttpGet]
        public ActionResult GuncelleBilgiGetirPersonel(int id)
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

        [HttpPost]
        public ActionResult GuncellePersonel(KURUM_PERSONEL p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult StajerBilgileriniAl(string ara, string SelectOption, string SelectOption2)
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name
                     select c).SingleOrDefault();

            var model = db.STAJYER_TANIM.Where(x => x.FK_STAJ_KURUM == a.CalisanKurumTanim).ToList();


            if (!string.IsNullOrEmpty(ara))
            {
                ara = ara.ToLower();
                model = model.Where(x => x.ADI.ToLower().Contains(ara) || x.SOYADI.ToLower().Contains(ara)).ToList();
            }


            if (!String.IsNullOrEmpty(SelectOption) && SelectOption != "null")
            {

                SelectOption = SelectOption.ToLower();
                model = model.Where(x => x.UNIVERSITE.ToString().Contains(SelectOption)).ToList();
                return View(model);

            }
            else if (!String.IsNullOrEmpty(SelectOption2) && SelectOption2 != "null")
            {

                SelectOption2 = SelectOption2.ToLower();
                model = model.Where(x => x.BOLUMU.ToString().Contains(SelectOption2)).ToList();
                return View(model);
            }
            else
            {
                return View(model);
            }

        }
        public ActionResult StajyerEkle()
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name
                     select c).SingleOrDefault();
            ViewBag.dgr10 = a.KURUM_TANIM.FIRMA_ADI.ToString();

            List<SelectListItem> degerler = (from i in db.KURUM_PERSONEL.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.ADI,
                                                 Value = i.PK_KURUM_PERSONEL.ToString()

                                             }).ToList();
            ViewBag.dgr = degerler;

            List<SelectListItem> degerler1 = (from i in db.KURUM_PERSONEL.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.ADI,
                                                  Value = i.PK_KURUM_PERSONEL.ToString()

                                              }).ToList();
            ViewBag.dgr1 = degerler1;


            List<SelectListItem> degerler3 = (from i in db.KURUM_DEPARTMAN.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.ADI,
                                                  Value = i.PK_KURUM_DEPARTMAN.ToString()

                                              }).ToList();
            ViewBag.dgr3 = degerler3;
            List<SelectListItem> degerler4 = (from i in db.Uni.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.UniName,
                                                  Value = i.UniId.ToString()

                                              }).ToList();
            ViewBag.dgr4 = degerler4;
            List<SelectListItem> degerler5 = (from i in db.Bolumler.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.BolumName,
                                                  Value = i.BolumId.ToString()

                                              }).ToList();
            ViewBag.dgr5 = degerler5;
            return View();
        }
        public ActionResult StajyerEkle2(STAJYER_TANIM kp)
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name
                     select c).SingleOrDefault();
            kp.FK_STAJ_KURUM = (int)a.CalisanKurumTanim;
            
            db.STAJYER_TANIM.Add(kp);
            try
            {
                db.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }

            return RedirectToAction("StajerBilgileriniAl");
        }
        public ActionResult SilBilgiGetir(int id)
        {
            var model = db.STAJYER_TANIM.Find(id);
            if (model == null) return HttpNotFound();
            return View(model);

        }
        public ActionResult Sil(STAJYER_TANIM p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("StajerBilgileriniAl");
        }
        [HttpGet]
        public ActionResult GuncelleBilgiGetirStajyer(int id)
        {


            var model = db.STAJYER_TANIM.Find(id);


            List<SelectListItem> degerler = (from i in db.KURUM_PERSONEL.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.ADI,
                                                 Value = i.PK_KURUM_PERSONEL.ToString()

                                             }).ToList();
            ViewBag.dgr = degerler;

            List<SelectListItem> degerler1 = (from i in db.KURUM_PERSONEL.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.ADI,
                                                  Value = i.PK_KURUM_PERSONEL.ToString()

                                              }).ToList();
            ViewBag.dgr1 = degerler1;

            List<SelectListItem> degerler2 = (from i in db.KURUM_TANIM.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.FIRMA_ADI,
                                                  Value = i.PK_KURUM_TANIM.ToString()

                                              }).ToList();
            ViewBag.dgr2 = degerler2;

            List<SelectListItem> degerler3 = (from i in db.KURUM_DEPARTMAN.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.ADI,
                                                  Value = i.PK_KURUM_DEPARTMAN.ToString()

                                              }).ToList();
            ViewBag.dgr3 = degerler3;
            List<SelectListItem> degerler4 = (from i in db.Uni.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.UniName,
                                                  Value = i.UniId.ToString()

                                              }).ToList();
            ViewBag.dgr4 = degerler4;
            List<SelectListItem> degerler5 = (from i in db.Bolumler.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.BolumName,
                                                  Value = i.BolumId.ToString()

                                              }).ToList();
            ViewBag.dgr5 = degerler5;
            return View(model);
        }
        [HttpPost]
        public ActionResult GuncelleStajyer(STAJYER_TANIM p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("StajerBilgileriniAl");
        }
        public ActionResult GetAll()
        {

            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name
                     select c).SingleOrDefault();

            var model = db.STAJYER_TANIM.Where(x => x.FK_STAJ_KURUM == a.CalisanKurumTanim).ToList();



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

            return RedirectToAction("SirketYetkilisi", "StajerBilgileriniAl");
        }

    }
}