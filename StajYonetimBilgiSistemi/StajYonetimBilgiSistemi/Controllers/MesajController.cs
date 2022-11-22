using Rotativa;
using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace StajYonetimBilgiSistemi.Controllers
{
    public class MesajController : Controller
    {
        SBYSEntities14 db = new SBYSEntities14();

        // GET: Mesaj
        [HttpGet]
        public ActionResult Index()
        {
            var model = db.MesajBilgileri.Where(x => (x.AlıcıMail == User.Identity.Name) && (x.SilindiMi == false || x.SilindiMi == null)).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string ara)
        {
            var model = db.MesajBilgileri.Where(x => (x.AlıcıMail == User.Identity.Name) && (x.SilindiMi == false || x.SilindiMi == null)).ToList();

            if (!string.IsNullOrEmpty(ara))
            {
                ara = ara.ToLower();
                model = model.Where(x => x.GönderenMail.ToLower().Contains(ara)).ToList();
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
        //[HttpGet]
        public ActionResult MesajDetaylari(int id)
        {
            var model = db.MesajBilgileri.Where(x => x.MesajBilgileriId == id).FirstOrDefault();
            ViewBag.uzanti = Path.GetExtension(model.Dosyalar);
            ViewBag.uzanti2 = Path.GetExtension(model.Dosyalar2);
            ViewBag.uzanti3 = Path.GetExtension(model.Dosyalar3);
            var mode2l = db.MesajBilgileri.Where(x => x.MesajBilgileriId == id).ToList();
            //var nextID = db.MesajBilgileri.OrderBy(i => i.MesajBilgileriId)
            //       .SkipWhile(i => i.MesajBilgileriId != id)
            //       .Skip(1)
            //       .Select(i => i.MesajBilgileriId);

            //ViewBag.NextID = nextID;
            return View(mode2l);
        }
        [HttpGet]
        public ActionResult CopKutusu()
        {
            var model = db.MesajBilgileri.Where(x => (x.SilindiMi == true) && (x.AlıcıMail == User.Identity.Name || x.GönderenMail == User.Identity.Name)).ToList();
            int a = model.Count;
            ViewBag.Gidenveri = a;
            foreach (var item in model)
            {
                var g = DateTime.Today.Subtract((DateTime)item.MesajTarihi).Days;
                if (g > 30)
                {
                    db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }

            }
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
            MesajBilgileri mesajBilgileri1 = new MesajBilgileri();
            mesajBilgileri1.MesajTarihi = DateTime.Now;
            mesajBilgileri1.GönderenMail = User.Identity.Name; //null
            mesajBilgileri1.AlıcıMail = mesajBilgileri.AlıcıMail;
            mesajBilgileri1.Konu = mesajBilgileri.Konu;
            mesajBilgileri1.MesajIcerigi = mesajBilgileri.MesajIcerigi;
            if (Request.Files.Count > 0)
            {
                if (Request.Files[0] != null)
                {
                    string dosyaadi = Path.GetFileName(Request.Files[0].FileName);
                    string uzanti = Path.GetExtension(Request.Files[0].FileName);
                    string yol = "~/image/" + dosyaadi + uzanti;
                    if (yol == "~/image/")
                    {
                        goto a1;

                    }
                    Console.WriteLine(Request.Files[0].FileName);
                    Request.Files[0].SaveAs(Server.MapPath(yol));

                    mesajBilgileri1.Dosyalar = "~/image/" + dosyaadi + uzanti;
                }
            a1: if (Request.Files[1] != null)
                {
                    string dosyaadi = Path.GetFileName(Request.Files[1].FileName);
                    string uzanti = Path.GetExtension(Request.Files[1].FileName);
                    string yol = "~/image/" + dosyaadi + uzanti;
                    if (yol == "~/image/")
                    {
                        goto a2;

                    }
                    Console.WriteLine(Request.Files[1].FileName);
                    Request.Files[1].SaveAs(Server.MapPath(yol));

                    mesajBilgileri1.Dosyalar2 = "~/image/" + dosyaadi + uzanti;
                }
            a2: if (Request.Files[2] != null)
                {
                    string dosyaadi = Path.GetFileName(Request.Files[2].FileName);
                    string uzanti = Path.GetExtension(Request.Files[2].FileName);
                    string yol = "~/image/" + dosyaadi + uzanti;
                    if (yol == "~/image/")
                    {
                        goto a3;

                    }
                    Console.WriteLine(Request.Files[2].FileName);
                    Request.Files[2].SaveAs(Server.MapPath(yol));

                    mesajBilgileri1.Dosyalar3 = "~/image/" + dosyaadi + uzanti;
                }

            }

        a3: try
            {
                db.MesajBilgileri.Add(mesajBilgileri1);
                db.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
                return View();
            }

            return RedirectToAction("GonderilenKutusu", "Mesaj");
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
            var model = db.MesajBilgileri.Where(x => (x.GönderenMail == User.Identity.Name) && (x.SilindiMi == false || x.SilindiMi == null)).ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                ara = ara.ToLower();
                model = model.Where(x => x.AlıcıMail.ToLower().Contains(ara)).ToList();
            }
          
            return View(model);
        }

        public ActionResult GetAll()
        {


            var model = db.Kullanicilar.ToList();


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

    }
}