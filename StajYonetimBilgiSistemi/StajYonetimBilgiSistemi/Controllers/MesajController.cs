using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StajYonetimBilgiSistemi.Controllers
{
    public class MesajController : Controller
    {
        SBYSEntities12 db = new SBYSEntities12();

        // GET: Mesaj
        [HttpGet]
        public ActionResult Index()
        {
            var model = db.MesajBilgileri.Where(x => (x.AlıcıMail == User.Identity.Name) && (x.SilindiMi == false || x.SilindiMi == null)).ToList();
           
            return View(model);
        }

        [HttpPost]
        public ActionResult Index( string ara)
        {
            var model = db.MesajBilgileri.Where(x => (x.AlıcıMail == User.Identity.Name) && (x.SilindiMi == false || x.SilindiMi == null)).ToList();

            if (!string.IsNullOrEmpty(ara))
            {
                ara = ara.ToLower();
                model = model.Where(x => x.GönderenMail.ToLower().Contains(ara) ).ToList();
            }
           
            return RedirectToAction("Index", "Mesaj");
        }
        [HttpGet]
        public ActionResult Index2(int id)
        {
        
            Console.WriteLine(id);
            var model = db.MesajBilgileri.Where(x => x.MesajBilgileriId == id).FirstOrDefault(); 
            model.SilindiMi = true;
            db.SaveChanges();
            return RedirectToAction("Index", "Mesaj");
        }
        [HttpGet]
        public ActionResult Index3(int id)
        {

            Console.WriteLine(id);
            var model = db.MesajBilgileri.Where(x => x.MesajBilgileriId == id).FirstOrDefault();
            model.SilindiMi = true;
            db.SaveChanges();
            return RedirectToAction("GonderilenKutusu", "Mesaj");
        }
        [HttpGet]
        public ActionResult MesajDetaylari(int id)
        {
            var model = db.MesajBilgileri.Where(x=>x.MesajBilgileriId== id ).ToList();
      
            return View(model);
        }
        [HttpGet]
        public ActionResult CopKutusu()
        {
            var model = db.MesajBilgileri.Where(x => (x.SilindiMi == true ) && (x.AlıcıMail == User.Identity.Name || x.GönderenMail == User.Identity.Name)).ToList();
            int a = model.Count;
            ViewBag.Gidenveri = a;
        
            return View(model);
        }
        [HttpGet]
        public ActionResult MesajYaz()
        {
  

            return View();
        }   
        [HttpPost]
        public ActionResult MesajYaz2(MesajBilgileri mesajBilgileri)
        {
          MesajBilgileri   mesajBilgileri1 = new MesajBilgileri();
            mesajBilgileri1.MesajTarihi = DateTime.Now;
            mesajBilgileri1.GönderenMail = User.Identity.Name; //null
            mesajBilgileri1.AlıcıMail = mesajBilgileri.AlıcıMail;
            mesajBilgileri1.Konu = mesajBilgileri.Konu;
            mesajBilgileri1.MesajIcerigi = mesajBilgileri.MesajIcerigi;
 
                //null
           

            try
            {
              db.MesajBilgileri.Add(mesajBilgileri1);
 db.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
                return View();
            }
           
            return RedirectToAction("GonderilenKutusu","Mesaj");
        }
        public PartialViewResult MesajListesiMenusu()
        {
            var model = db.MesajBilgileri.Where(x => (x.AlıcıMail == User.Identity.Name) && (x.SilindiMi == false || x.SilindiMi == null)).ToList();
            int a = model.Count;
            ViewBag.Gidenveri = a;


            var model2 = db.MesajBilgileri.Where(x => (x.GönderenMail == User.Identity.Name) && (x.SilindiMi == false || x.SilindiMi == null)).ToList();
            int b = model2.Count;
            ViewBag.Gidenveri2 = b;

            var model3 = db.MesajBilgileri.Where(x => (x.SilindiMi == true) && (x.AlıcıMail == User.Identity.Name || x.GönderenMail == User.Identity.Name)).ToList();
            int c = model3.Count;
            ViewBag.Gidenveri3 = c;

            return PartialView();
            
        }
        public ActionResult GonderilenKutusu(string ara)
        {
            var model = db.MesajBilgileri.Where(x =>( x.GönderenMail == User.Identity.Name) && (x.SilindiMi == false || x.SilindiMi == null)).ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                ara = ara.ToLower();
                model = model.Where(x => x.AlıcıMail.ToLower().Contains(ara)).ToList();
            }
            return View(model);
        }
      

    }
}