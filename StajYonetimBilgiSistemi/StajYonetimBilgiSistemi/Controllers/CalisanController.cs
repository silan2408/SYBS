using ClosedXML.Excel;
//using DocumentFormat.OpenXml.EMMA;
//using DocumentFormat.OpenXml.Office.MetaAttributes;
//using DocumentFormat.OpenXml.Spreadsheet;
using Rotativa;
using StajYonetimBilgiSistemi.Models;
using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace StajYonetimBilgiSistemi.Controllers
{
    [Authorize(Roles = "Calisan")]
    public class CalisanController : Controller
    {
        SBYSEntities db = new SBYSEntities();
        // GET: Calisan
        public ActionResult Charts()
        {

            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return View("~/Views/Shared/Error.cshtml");
        }
        public JsonResult BarChartDataEF()
        {
            var b = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();

            var stajyerler = db.STAJYER_TANIM.Where(x => x.FK_STAJ_KURUM == b.CalisanKurumTanim).ToList();
            Chart _chart = new Chart();
            _chart.labels = stajyerler.Select(x => x.Bolumler.BolumName).Distinct().ToArray();
            _chart.datasets = new List<Datasets>();


            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "İş Yerindeki Stajerlerin Bölüm Grafiği",
                //data = stajyerler.Select(x => x.BOLUMU.Value).ToArray(),
                data = stajyerler.GroupBy(a => a.BOLUMU).Select(a => a.Count()).ToArray(),
                backgroundColor = new string[] { "#4EF0EE" },
                borderColor = new string[] { "#000000" },
                borderWidth = "1"

            });
            _chart.datasets = _dataSet;
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BarChartDataEFUni()
        {
            var b = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();

            var stajyerler = db.STAJYER_TANIM.Where(x => x.FK_STAJ_KURUM == b.CalisanKurumTanim).ToList();
            Chart _chart = new Chart();
            _chart.labels = stajyerler.Select(x => x.Uni.UniName).Distinct().ToArray();
            _chart.datasets = new List<Datasets>();


            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "İş Yerindeki Stajerlerin Üniversite Grafiği",
                //data = stajyerler.Select(x => x.BOLUMU.Value).ToArray(),
                data = stajyerler.GroupBy(a => a.UNIVERSITE).Select(a => a.Count()).ToArray(),
                backgroundColor = new string[] { "#4EF0EE" },
                borderColor = new string[] { "#000000" },
                borderWidth = "1"

            });
            _chart.datasets = _dataSet;
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FirmaBilgileriAl()
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();

            var model = db.KURUM_TANIM.First(x => x.PK_KURUM_TANIM == a.CalisanKurumTanim);


            return View(model);

        }

        public ActionResult PersonelBilgileriniAl(string ara)
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();

            var model = db.KURUM_PERSONEL.Where(x => x.FK_KURUM_TANIM == a.CalisanKurumTanim).ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                ara = ara.ToLower();
                model = model.Where(x => x.ADI.ToLower().Contains(ara) || x.SOYADI.ToLower().Contains(ara)).ToList();
            }


            return View(model);

        }

        public ActionResult StajerBilgileriniAl(string ara, string SelectOption, string SelectOption2)
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();

            var model = db.STAJYER_TANIM.Where(x => x.FK_STAJ_KURUM == a.CalisanKurumTanim).ToList();



            if (!string.IsNullOrEmpty(ara))
            {
                ara = ara.ToLower();
                model = model.Where(x => x.ADI.ToLower().Contains(ara) || x.SOYADI.ToLower().Contains(ara)).ToList();
            }
            if (SelectOption != "null" && SelectOption2 != "null" && SelectOption != null&& SelectOption2 != null)
            {
              
         SelectOption = SelectOption.ToLower();
                SelectOption2 = SelectOption2.ToLower();
                model = model.Where(x => x.Bolumler.BolumName.ToLower().ToString().Contains(SelectOption) && x.Uni.UniName.ToLower().ToString().Contains(SelectOption2)).ToList();
                return View(model);
          
             
    
            }

            else if (!String.IsNullOrEmpty(SelectOption2) && SelectOption == "null")
            {

                SelectOption = SelectOption2.ToLower();
                model = model.Where(x => x.Uni.UniName.ToLower().ToString().Contains(SelectOption)).ToList();
                return View(model);

            }
          else  if (!String.IsNullOrEmpty(SelectOption) && SelectOption2 == "null")
            {

                SelectOption2 = SelectOption.ToLower();
                model = model.Where(x => x.Bolumler.BolumName.ToLower().ToString().Contains(SelectOption2)).ToList();
                return View(model);
            }
        
         

            return View(model);


        }
        public ActionResult GetAll()
        {

            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
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

        [HttpPost]
        public FileResult Export()
        {

            DataTable dt = new DataTable("Stajyerler");
            dt.Columns.AddRange(new DataColumn[16] { new DataColumn("Id"),
                                            new DataColumn("Adı"),
                                            new DataColumn("Soyadı"),
                                            new DataColumn("Üniversite"),
                                            new DataColumn("Üniversite Numarası"),
                                            new DataColumn("Bölümü"),
                                            new DataColumn("Sınıfı"),
                                            new DataColumn("Email"),
                                            new DataColumn("Telefon"),
                                            new DataColumn("Staj Yılı"),
                                            new DataColumn("Staj Başlama Tarihi"),
                                            new DataColumn("Staj Bitiş Tarihi"),
                                            new DataColumn("Kurum Staj Sorumlusu"),
                                            new DataColumn("Kurum Onay Kişi"),
                                            new DataColumn("Staj Kurumu"),
                                            new DataColumn("Staj Departmanı") });

            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();

            var model = db.STAJYER_TANIM.Where(x => x.FK_STAJ_KURUM == a.CalisanKurumTanim).ToList();
            foreach (var stajyer in model)
            {
                dt.Rows.Add(stajyer.PK_STAJYER_TANIM, stajyer.ADI, stajyer.SOYADI, stajyer.Uni.UniName,
                    stajyer.UNIV_NO, stajyer.Bolumler.BolumName, stajyer.SINIFI, stajyer.EMAIL, stajyer.TELEFON, stajyer.STAJ_YILI
                    , stajyer.STAJ_BAS_TARIHI, stajyer.STAJ_BIT_TARIHI, stajyer.KURUM_PERSONEL.ADI, stajyer.KURUM_PERSONEL1.ADI, stajyer.KURUM_TANIM.FIRMA_ADI
                    , stajyer.KURUM_DEPARTMAN.ADI);
            }
            using (XLWorkbook kb = new XLWorkbook())
            {
                kb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    kb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Stajyerler.xlsx");
                }
            }
        }
        public ActionResult Stajlar()
        {
            var model = db.Stajlar.Where(x=>x.Aktiflik==true).ToList();

            return View(model);
        }
       
        public ActionResult SorumluOlunanStajyerler()
        {
            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();
            var b = (from c in db.KURUM_PERSONEL
                     where c.PK_KURUM_PERSONEL == a.CalisanPersonel
                     select c).First();
            var model = db.Stajlar.Where(x => x.Fk_Onaylayan_Personel == b.PK_KURUM_PERSONEL || x.Fk_Sorumlu_Personel == b.PK_KURUM_PERSONEL).ToList();
            if (model.Count() == 0)
            {
                ViewBag.data = "Sorumlu olduğunuz stajyer bulunmamaktadır";
            }
            else if (model != null)
            {
                return View(model);
            }
            return View();

        }
        public ActionResult StajyerleriAl(string ara, string SelectOption, string SelectOption2)
        {
      

            var a = (from c in db.Kullanicilar
                     where c.Email == User.Identity.Name || c.KullaniciAdi == User.Identity.Name
                     select c).SingleOrDefault();
            var b = (from c in db.KURUM_PERSONEL
                     where c.PK_KURUM_PERSONEL == a.CalisanPersonel
                     select c).First();
            var model = db.STAJYER_TANIM.Where(x => x.KURUM_ONAY_KISI == b.PK_KURUM_PERSONEL || x.KURUM_ST_SORUMLUSU == b.PK_KURUM_PERSONEL).ToList();
 if (!string.IsNullOrEmpty(ara))
            {
                ara = ara.ToLower();
                model = model.Where(x => x.ADI.ToLower().Contains(ara) || x.SOYADI.ToLower().Contains(ara)).ToList();

                return View(model);
            }


            if (!String.IsNullOrEmpty(SelectOption2) && SelectOption2 != "null")
            {

                SelectOption2 = SelectOption2.ToLower();
                model = model.Where(x => x.Uni.UniName.ToString().ToLower().Contains(SelectOption2)).ToList();

                return View(model);


            }
            else if (!String.IsNullOrEmpty(SelectOption) && SelectOption != "null")
            {

                SelectOption = SelectOption.ToLower();
                model = model.Where(x => x.Bolumler.BolumName.ToString().ToLower().Contains(SelectOption)).ToList();

                return View(model);
            }
            if (model.Count() == 0)
            {
                ViewBag.data = "Sorumlu olduğunuz stajyer bulunmamaktadır";
            }
            else if (model != null)
            {
                return View(model);
            }
           
            return View();

        }
    }
}