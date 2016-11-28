using CiroHesaplama.Models;
using CiroHesaplama.Models.DIO;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CiroHesaplama.Controllers
{
    public class ExportController : Controller
    {
        //
        // GET: /Export/
        DataClasses1DataContext db = new DataClasses1DataContext();
        public void KategorileriDroplaraAt()
        {
            List<SelectListItem> rolList = new List<SelectListItem>();
            var rollerSorgu = from k in db.KategoriTablosus select k;
            foreach (var item in rollerSorgu)
            {
                rolList.Add(new SelectListItem
                {
                    Text = item.KategoriAdi,
                    Value = item.Kategoriid.ToString()
                });
            }
            ViewBag.rollerim = rolList;
        }
        public ActionResult ExportData(string rollerim)
        {
            var kullaniciSorgu = (from k in db.KategoriTablosus
                                  join s in db.Subes on k.Kategoriid equals s.Kategoriid
                                  orderby s.Tarih
                                  select new ExportDataViewModel
                                  {
                                      KategoriNameHelper = k.KategoriAdi,
                                      CiroidHelper = s.Ciroid,
                                      TarihHelper = s.Tarih,
                                      FaturaSayisiHelper = s.FaturaSayisi,
                                      FaturaTutarHelper = s.FaturaTutar,
                                      HavaleHelper = s.Havale,
                                      NakitHelper = s.Nakit,
                                      KrediKartiHelper = s.KrediKarti,
                                      CekHelper = s.Cek,
                                      GunlukTahsilatTutarHelper = s.TahsilTutar,
                                      HarcamaAdetHelper = s.HarcamaAdet,
                                      HarcamaTutarHelper = s.HarcamaTutar,
                                      GunlukNotHelper = s.GunlukTutulanNot
                                  }).ToList();
           


           
            ViewBag.deneme = kullaniciSorgu;
            return View();
        }
        public ActionResult ExportPDF()
        {
            return new  ActionAsPdf("ExportData")
            {
                FileName=Server.MapPath("~/Content/Invoice.pdf")

            };
         
        }

    }
}
