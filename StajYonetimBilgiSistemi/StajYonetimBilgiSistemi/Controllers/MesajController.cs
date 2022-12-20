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
        SBYSEntities db = new SBYSEntities();

        // GET: Mesaj

        public ActionResult Index( string ara, int PageNumber = 1)
        {
            var model = db.MesajBilgileri.Where(x => (x.AlıcıMail == User.Identity.Name) && (x.SilindiMi == false || x.SilindiMi == null)).OrderByDescending(x=>x.MesajBilgileriId).ToList();
  ViewBag.totalPages = Math.Ceiling(model.Count / 5.0);
            ViewBag.PageNumber = PageNumber;
           
            model = model.Skip((PageNumber - 1) * 5).Take(5).ToList();
             if (!string.IsNullOrEmpty(ara))
            {
                ara = ara.ToLower();
                 model = db.MesajBilgileri.Where(x => (x.AlıcıMail == User.Identity.Name) && (x.SilindiMi == false || x.SilindiMi == null) && (x.GönderenMail.ToLower().Contains(ara) || x.Konu.ToLower().Contains(ara))).OrderByDescending(x => x.MesajBilgileriId).ToList();

                ViewBag.totalPages = Math.Ceiling(model.Count / 5.0);
                ViewBag.PageNumber = PageNumber;

                model = model.Skip((PageNumber - 1) * 5).Take(5).ToList();
                return View(model);
            }
            return View(model);
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
            var model = db.MesajBilgileri.Where(x => x.MesajBilgileriId == id).ToList();

   
            return View(model);
        }
        public ActionResult MesajDetaylari2(int id)
        {
            var model = db.MesajBilgileri.Where(x => x.MesajBilgileriId == id).ToList();


            return View(model);
        }
        public FileResult Download(string file)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(file));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file);
        }
        [HttpGet]
        public ActionResult CopKutusu(int PageNumber = 1)
        {
            var model = db.MesajBilgileri.Where(x => (x.SilindiMi == true) && (x.AlıcıMail == User.Identity.Name || x.GönderenMail == User.Identity.Name)).OrderByDescending(x => x.MesajBilgileriId).ToList();
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
            ViewBag.totalPages = Math.Ceiling(model.Count / 5.0);
            ViewBag.PageNumber = PageNumber;
            model = model.Skip((PageNumber - 1) * 5).Take(5).ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult MesajYaz(string alıcı)
        {
            if(alıcı != null)
            {
              Console.WriteLine(alıcı);
                ViewBag.Alıcı = alıcı;
             
            }

            return View();
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        [ValidateInput(false)] //bu tanım
     
        public ActionResult MesajYaz(MesajBilgileri mesajBilgileri)
        {
            if(mesajBilgileri.Konu ==null || mesajBilgileri.MesajIcerigi == null || mesajBilgileri.AlıcıMail==null)
            {
                TempData["mesaj"] = "Alanlar boş olamaz!";
                return RedirectToAction("MesajYaz", TempData["mesaj"]);
            }
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
                 
                    string yol = "~/image/" + dosyaadi;
                    if (yol == "~/image/")
                    {
                        goto a1;

                    }
                    Console.WriteLine(Request.Files[0].FileName);
                    Request.Files[0].SaveAs(Server.MapPath(yol));

                    mesajBilgileri1.Dosyalar = "~/image/" + dosyaadi;
                }
            a1: if (Request.Files[1] != null)
                {
                    string dosyaadi = Path.GetFileName(Request.Files[1].FileName);
            
                    string yol = "~/image/" + dosyaadi;
                    if (yol == "~/image/")
                    {
                        goto a2;

                    }
                    Console.WriteLine(Request.Files[1].FileName);
                    Request.Files[1].SaveAs(Server.MapPath(yol));

                    mesajBilgileri1.Dosyalar2 = "~/image/" + dosyaadi ;
                }
            a2: if (Request.Files[2] != null)
                {
                    string dosyaadi = Path.GetFileName(Request.Files[2].FileName);
                 
                    string yol = "~/image/" + dosyaadi ;
                    if (yol == "~/image/")
                    {
                        goto a3;

                    }
                    Console.WriteLine(Request.Files[2].FileName);
                    Request.Files[2].SaveAs(Server.MapPath(yol));

                    mesajBilgileri1.Dosyalar3 = "~/image/" + dosyaadi ;
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
            ViewBag.Gidenveri1 = a;


            var model2 = db.MesajBilgileri.Where(x => (x.GönderenMail == User.Identity.Name) && (x.SilindiMi == false || x.SilindiMi == null)).ToList();
            int b = model2.Count;
            ViewBag.Gidenveri2 = b;

            var model3 = db.MesajBilgileri.Where(x => (x.SilindiMi == true) && (x.AlıcıMail == User.Identity.Name || x.GönderenMail == User.Identity.Name)).ToList();
            int c = model3.Count;
            ViewBag.Gidenveri3 = c;

            return PartialView();

        }


        public ActionResult GonderilenKutusu(string ara ,int PageNumber =1)
        {
            var model = db.MesajBilgileri.Where(x => (x.GönderenMail == User.Identity.Name) && (x.SilindiMi == false || x.SilindiMi == null)).OrderByDescending(x => x.MesajBilgileriId).ToList();
        
            ViewBag.totalPages = Math.Ceiling(model.Count / 5.0);
            ViewBag.PageNumber = PageNumber;
            model = model.Skip((PageNumber - 1) * 5).Take(5).ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                ara = ara.ToLower();
              model = db.MesajBilgileri.Where(x => (x.GönderenMail == User.Identity.Name) && (x.SilindiMi == false || x.SilindiMi == null) && (x.AlıcıMail.ToLower().Contains(ara) || x.Konu.ToLower().Contains(ara))).OrderByDescending(x => x.MesajBilgileriId).ToList();
                ViewBag.totalPages = Math.Ceiling(model.Count / 5.0);
                ViewBag.PageNumber = PageNumber;

                model = model.Skip((PageNumber - 1) * 5).Take(5).ToList();
                return View(model);
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