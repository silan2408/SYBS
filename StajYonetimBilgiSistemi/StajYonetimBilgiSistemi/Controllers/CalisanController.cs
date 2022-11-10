using Microsoft.Ajax.Utilities;
using Rotativa;
using StajYonetimBilgiSistemi.Models.Entity;
using StajYonetimBilgiSistemi.Roller;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StajYonetimBilgiSistemi.Controllers
{
    [Authorize(Roles = "Stajyer,Calisan")]
    public class CalisanController : Controller
    {
        SBYSEntities12 db = new SBYSEntities12();
        // GET: Calisan
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult FirmaBilgileriAl()
        {
            //string a = User.Identity.Name;
            FirmaBilgileri firma = new FirmaBilgileri();
            firma.kurum_tanım = db.KURUM_TANIM.ToList();
            firma.kurum_personel = db.KURUM_PERSONEL.ToList();
            ////var model = db.KURUM_PERSONEL.Where(x => x.EMAIL == User.Identity.Name);
            //var sorgu = from d1 in db.KURUM_TANIM
            //            join d2 in db.KURUM_PERSONEL
            //            on d1.PK_KURUM_TANIM equals d2.FK_KURUM_TANIM
            //            select d1;



            return View(firma);

        }
        [HttpGet]
        public ActionResult PersonelBilgileriniAl(int id )
        {
            var model = db.KURUM_PERSONEL.Where(x => x.FK_KURUM_TANIM == id).ToList();



            return View(model);

        }

        public ActionResult StajerBilgileriniAl(int id, string ara, string SelectOption, string SelectOption2)
        {
            var model = db.STAJYER_TANIM.Where(x => x.FK_STAJ_KURUM == id).ToList();



            if (!string.IsNullOrEmpty(ara))
            {
                ara = ara.ToLower();
                model = model.Where(x => x.ADI.ToLower().Contains(ara) || x.SOYADI.ToLower().Contains(ara)).ToList();
            }


            if (!String.IsNullOrEmpty(SelectOption) && SelectOption != "null")
            {

                SelectOption = SelectOption.ToLower();
                model = model.Where(a => a.UNIVERSITE.ToString().Contains(SelectOption)).ToList();
                return View(model);

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
        public ActionResult ExportToPdf()
        {
            bool k = true;
            if (k)
            {
                var q = new ActionAsPdf("StajerBilgileriniAl");
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