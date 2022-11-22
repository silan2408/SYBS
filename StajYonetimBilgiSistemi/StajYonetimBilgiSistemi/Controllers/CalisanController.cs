using Rotativa;
using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Web.Mvc;

namespace StajYonetimBilgiSistemi.Controllers
{
    [Authorize(Roles = "Calisan")]
    public class CalisanController : Controller
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
        public ActionResult PersonelBilgileriniAl()
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name
                     select c).SingleOrDefault();

            var model = db.KURUM_PERSONEL.Where(x => x.FK_KURUM_TANIM == a.CalisanKurumTanim).ToList();


            return View(model);

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

            return RedirectToAction("Calisan", "StajerBilgileriniAl");
        }

    }
}