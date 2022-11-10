using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;

namespace StajYonetimBilgiSistemi.Controllers
{
    [Authorize(Roles ="Admin,Calisan")]
    public class StajyerTanimController : Controller
    {
        SBYSEntities12 db = new SBYSEntities12();
        // GET: StajyerTanim


        public ActionResult Index(string ara, string SelectOption,string SelectOption2)
        {
            var model = db.STAJYER_TANIM.ToList();
            if(!string.IsNullOrEmpty(ara))
            {
            ara = ara.ToLower();
                model = model.Where(x => x.ADI.ToLower().Contains(ara) || x.SOYADI.ToLower().Contains(ara) ).ToList();
            }


            if (!String.IsNullOrEmpty(SelectOption) && SelectOption != "null")
            {

                SelectOption = SelectOption.ToLower();
                model = model.Where(a => a.UNIVERSITE.ToString().Contains(SelectOption)).ToList();
                return View (model);

            }
           else if (!String.IsNullOrEmpty(SelectOption2) && SelectOption2 != "null")
            {

                SelectOption2 = SelectOption2.ToLower();
                model = model.Where(a => a.BOLUMU.ToString().Contains(SelectOption2)).ToList();
                return View(model);
            }
            else
            {
                return View(model);
            }
            
           
        }
    
        public ActionResult StajyerEkle()
        {

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
            return View();
        }
        public ActionResult StajyerEkle2(STAJYER_TANIM kp)
        {
            db.STAJYER_TANIM.Add(kp);
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
        public ActionResult Guncelle(STAJYER_TANIM p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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
            return RedirectToAction("Index");
        }
    }
}