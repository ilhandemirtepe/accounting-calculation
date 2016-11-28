using CiroHesaplama.Models;
using CiroHesaplama.Models.DIO;
using CiroHesaplama.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace CiroHesaplama.Controllers
{
    public class MerkezController : BaseController
    {
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
        public ActionResult YasamaDongusuListele()
        {
            var YasamDongusuSorgu = from s in db.Subes
                                    join k in db.KategoriTablosus on s.Kategoriid equals k.Kategoriid
                                    select new
                                    {
                                        s.Ciroid,
                                        s.FaturaSayisi,
                                        s.FaturaTutar,
                                        s.Nakit,
                                        s.KrediKarti,
                                        s.Cek,
                                        s.Havale,
                                        s.HarcamaAdet,
                                        s.HarcamaTutar,
                                        k.KategoriAdi,
                                        s.Tarih,
                                        s.GunlukTutulanNot,
                                        s.TahsilTutar,

                                    };
            List<MerkezHelper> YasamDongusuListesi = new List<MerkezHelper>();

            foreach (var item in YasamDongusuSorgu)
            {
                MerkezHelper kh = new MerkezHelper();
                kh.CiroNo = item.Ciroid;
                kh.FaturaSayisi = item.FaturaSayisi;
                kh.FaturaTutari = item.FaturaTutar.ToString("#,###,###.#0");
                kh.Nakit = item.Nakit.ToString("#,###,###.#0");
                kh.KrediKarti = item.KrediKarti.ToString("#,###,###.#0");
                kh.Cek = item.Cek.ToString("#,###,###.#0");
                kh.Havale = item.Havale.ToString("#,###,###.#0");
                kh.HarcamaAdet = item.HarcamaAdet;
                kh.HarcamaTutar = item.HarcamaTutar.ToString("#,###,###.#0");
                kh.SubeAdi = item.KategoriAdi;
                kh.Tarih = item.Tarih;
                kh.GunlukNot = item.GunlukTutulanNot;
                kh.GunlukTahsilTutar = item.TahsilTutar.ToString("#,###,###.#0");
                YasamDongusuListesi.Add(kh);
            }
            KategorileriDroplaraAt();
            MerkezListeModel newModel = new MerkezListeModel()
            {
                Filtreleme = new Filtreleme(),
                MerkezListeModelimiz = YasamDongusuListesi
            };

            return View(newModel);
        }
        [HttpPost]
        public ActionResult YasamaDongusuListele(MerkezListeModel filt, string rollerim)
        {
            if (filt.Filtreleme.DonguTarih1Filtre.Year < 1900)
                filt.Filtreleme.DonguTarih1Filtre = DateTime.Now.AddMonths(-1);
            if (filt.Filtreleme.DonguTarih2Filtre.Year < 1900)
                filt.Filtreleme.DonguTarih2Filtre = DateTime.Now;
            if (filt.Filtreleme.DonguTarih1Filtre > filt.Filtreleme.DonguTarih2Filtre)
                filt.Filtreleme.DonguTarih1Filtre = filt.Filtreleme.DonguTarih2Filtre;

            var YasamDongusuSorgu = from s in db.Subes
                                    join k in db.KategoriTablosus on s.Kategoriid equals k.Kategoriid
                                    where (s.Tarih >= filt.Filtreleme.DonguTarih1Filtre && s.Tarih <= filt.Filtreleme.DonguTarih2Filtre)
                                    select new
                                    {
                                        s.Ciroid,
                                        k.KategoriAdi,
                                        k.Kategoriid,
                                        s.Cek,
                                        s.Tarih,
                                        s.FaturaSayisi,
                                        s.FaturaTutar,
                                        s.Nakit,
                                        s.KrediKarti,
                                        s.Havale,
                                        s.HarcamaAdet,
                                        s.HarcamaTutar,
                                        s.TahsilTutar,

                                    };
            if (!String.IsNullOrEmpty(rollerim))
            {
                YasamDongusuSorgu = YasamDongusuSorgu.Where(m => m.Kategoriid == int.Parse(rollerim));


                string toplamcek = YasamDongusuSorgu.Where(m => m.Kategoriid == int.Parse(rollerim)).Sum(m => m.Cek).ToString("#,###,###.#0");
                ViewBag.tc = toplamcek;


                string toplamnakit = YasamDongusuSorgu.Where(m => m.Kategoriid == int.Parse(rollerim)).Sum(m => m.Nakit).ToString("#,###,###.#0");
                ViewBag.tn = toplamnakit;



                string toplamhavale = YasamDongusuSorgu.Where(m => m.Kategoriid == int.Parse(rollerim)).Sum(m => m.Havale).ToString("#,###,###.#0");
                ViewBag.th = toplamhavale;



                string toplamkredikarti = YasamDongusuSorgu.Where(m => m.Kategoriid == int.Parse(rollerim)).Sum(m => m.KrediKarti).ToString("#,###,###.#0");
                ViewBag.tkk = toplamkredikarti;


                string toplamgunluktahsiltutar = YasamDongusuSorgu.Where(m => m.Kategoriid == int.Parse(rollerim)).Sum(m => m.TahsilTutar).ToString("#,###,###.#0");
                ViewBag.tgt = toplamgunluktahsiltutar;




                string toplamFaturaSayisi = YasamDongusuSorgu.Where(m => m.Kategoriid == int.Parse(rollerim)).Sum(m => m.FaturaSayisi).ToString();
                ViewBag.tfs = toplamFaturaSayisi;


                string toplamFaturaTutar = YasamDongusuSorgu.Where(m => m.Kategoriid == int.Parse(rollerim)).Sum(m => m.FaturaTutar).ToString("#,###,###.#0");
                ViewBag.tft = toplamFaturaTutar;


                string toplamHarcamaAdet = YasamDongusuSorgu.Where(m => m.Kategoriid == int.Parse(rollerim)).Sum(m => m.HarcamaAdet).ToString();
                ViewBag.tha = toplamHarcamaAdet;


                string toplamHarcamaTutar = YasamDongusuSorgu.Where(m => m.Kategoriid == int.Parse(rollerim)).Sum(m => m.HarcamaTutar).ToString("#,###,###.#0");
                ViewBag.tht = toplamHarcamaTutar;

            }
            List<MerkezHelper> YasamDongusuListesi = new List<MerkezHelper>();
            foreach (var item in YasamDongusuSorgu)
            {
                MerkezHelper kh = new MerkezHelper();
                kh.CiroNo = item.Ciroid;
                kh.SubeAdi = item.KategoriAdi;
                kh.Cek = item.Cek.ToString("#,###,###.#0");
                kh.Tarih = item.Tarih;
                kh.FaturaSayisi = item.FaturaSayisi;
                kh.FaturaTutari = item.FaturaTutar.ToString("#,###,###.#0");
                kh.Nakit = item.Nakit.ToString("#,###,###.#0");
                kh.KrediKarti = item.KrediKarti.ToString("#,###,###.#0");
                kh.Havale = item.Havale.ToString("#,###,###.#0");
                kh.HarcamaAdet = item.HarcamaAdet;
                kh.HarcamaTutar = item.HarcamaTutar.ToString("#,###,###.#0");
                kh.GunlukTahsilTutar = item.TahsilTutar.ToString("#,###,###.#0");

                YasamDongusuListesi.Add(kh);
            }
            KategorileriDroplaraAt();
            MerkezListeModel newModel = new MerkezListeModel()
            {
                Filtreleme = filt.Filtreleme,
                MerkezListeModelimiz = YasamDongusuListesi
            };


            return View(newModel);
        }
        public ActionResult NotListele()
        {
            var YasamDongusuSorgu = from s in db.Subes
                                    join k in db.KategoriTablosus on s.Kategoriid equals k.Kategoriid
                                    select new
                                    {
                                        s.Ciroid,
                                        k.KategoriAdi,
                                        s.Tarih,
                                        s.GunlukTutulanNot,
                                    };
            List<MerkezHelper> YasamDongusuListesi = new List<MerkezHelper>();

            foreach (var item in YasamDongusuSorgu)
            {
                MerkezHelper kh = new MerkezHelper();
                kh.CiroNo = item.Ciroid;
                kh.SubeAdi = item.KategoriAdi;
                kh.Tarih = item.Tarih;
                kh.GunlukNot = item.GunlukTutulanNot;
                YasamDongusuListesi.Add(kh);
            }
            KategorileriDroplaraAt();
            MerkezListeModel newModel = new MerkezListeModel()
            {
                Filtreleme = new Filtreleme(),
                MerkezListeModelimiz = YasamDongusuListesi
            };

            return View(newModel);
        }
        [HttpPost]
        public ActionResult NotListele(MerkezListeModel filt, string rollerim)
        {
            if (filt.Filtreleme.DonguTarih1Filtre.Year < 1900)
                filt.Filtreleme.DonguTarih1Filtre = DateTime.Now.AddMonths(-1);
            if (filt.Filtreleme.DonguTarih2Filtre.Year < 1900)
                filt.Filtreleme.DonguTarih2Filtre = DateTime.Now;
            if (filt.Filtreleme.DonguTarih1Filtre > filt.Filtreleme.DonguTarih2Filtre)
                filt.Filtreleme.DonguTarih1Filtre = filt.Filtreleme.DonguTarih2Filtre;

            var YasamDongusuSorgu = from s in db.Subes
                                    join k in db.KategoriTablosus on s.Kategoriid equals k.Kategoriid
                                    where (s.Tarih >= filt.Filtreleme.DonguTarih1Filtre && s.Tarih <= filt.Filtreleme.DonguTarih2Filtre)
                                    select new
                                    {
                                        s.Ciroid,
                                        k.KategoriAdi,
                                        k.Kategoriid,
                                        s.Tarih,
                                        s.GunlukTutulanNot

                                    };
            if (!String.IsNullOrEmpty(rollerim))
            {
                YasamDongusuSorgu = YasamDongusuSorgu.Where(m => m.Kategoriid == int.Parse(rollerim));
            }
            List<MerkezHelper> YasamDongusuListesi = new List<MerkezHelper>();

            foreach (var item in YasamDongusuSorgu)
            {
                MerkezHelper kh = new MerkezHelper();
                kh.CiroNo = item.Ciroid;
                kh.SubeAdi = item.KategoriAdi;
                kh.Tarih = item.Tarih;
                YasamDongusuListesi.Add(kh);
            }
            KategorileriDroplaraAt();
            MerkezListeModel newModel = new MerkezListeModel()
            {
                Filtreleme = filt.Filtreleme,
                MerkezListeModelimiz = YasamDongusuListesi
            };
            return View(newModel);
        }
        public ActionResult LogOut()
        {

            return RedirectToAction("Index", "Login");

        }

        public ActionResult YasamaDongusuListele1()
        {
            var YasamDongusuSorgu1 = from s in db.Subes
                                     join k in db.KategoriTablosus on s.Kategoriid equals k.Kategoriid
                                     select new
                                     {
                                         s.Ciroid,
                                         s.FaturaSayisi,
                                         s.FaturaTutar,
                                         s.Nakit,
                                         s.KrediKarti,
                                         s.Cek,
                                         s.Havale,
                                         s.HarcamaAdet,
                                         s.HarcamaTutar,
                                         k.KategoriAdi,
                                         s.Tarih,
                                         s.GunlukTutulanNot,
                                         s.TahsilTutar,

                                     };
            List<MerkezHelper> YasamDongusuListesi1 = new List<MerkezHelper>();

            foreach (var item in YasamDongusuSorgu1)
            {
                MerkezHelper kh1 = new MerkezHelper();
                kh1.CiroNo = item.Ciroid;
                kh1.FaturaSayisi = item.FaturaSayisi;
                kh1.FaturaTutari = item.FaturaTutar.ToString("#,###,###.#0");
                kh1.Nakit = item.Nakit.ToString("#,###,###.#0");
                kh1.KrediKarti = item.KrediKarti.ToString("#,###,###.#0");
                kh1.Cek = item.Cek.ToString("#,###,###.#0");
                kh1.Havale = item.Havale.ToString("#,###,###.#0");
                kh1.HarcamaAdet = item.HarcamaAdet;
                kh1.HarcamaTutar = item.HarcamaTutar.ToString("#,###,###.#0");
                kh1.SubeAdi = item.KategoriAdi;
                kh1.Tarih = item.Tarih;
                kh1.GunlukNot = item.GunlukTutulanNot;
                kh1.GunlukTahsilTutar = item.TahsilTutar.ToString("#,###,###.#0");
                YasamDongusuListesi1.Add(kh1);
            }
            KategorileriDroplaraAt();
            MerkezListeModel newModel = new MerkezListeModel()
            {
                Filtreleme = new Filtreleme(),
                MerkezListeModelimiz = YasamDongusuListesi1
            };

            return View(newModel);
        }
        [HttpPost]
        public ActionResult YasamaDongusuListele1(MerkezListeModel filt1, string rollerim)
        {
            if (filt1.Filtreleme.DonguTarih1Filtre.Year < 1900)
                filt1.Filtreleme.DonguTarih1Filtre = DateTime.Now.AddMonths(-1);
            if (filt1.Filtreleme.DonguTarih2Filtre.Year < 1900)
                filt1.Filtreleme.DonguTarih2Filtre = DateTime.Now;
            if (filt1.Filtreleme.DonguTarih1Filtre > filt1.Filtreleme.DonguTarih2Filtre)
                filt1.Filtreleme.DonguTarih1Filtre = filt1.Filtreleme.DonguTarih2Filtre;

            var YasamDongusuSorgu1 = from s in db.Subes
                                     join k in db.KategoriTablosus on s.Kategoriid equals k.Kategoriid
                                     where (s.Tarih >= filt1.Filtreleme.DonguTarih1Filtre && s.Tarih <= filt1.Filtreleme.DonguTarih2Filtre)
                                     select new
                                     {
                                         s.Ciroid,
                                         k.KategoriAdi,
                                         k.Kategoriid,
                                         s.Cek,
                                         s.Tarih,
                                         s.FaturaSayisi,
                                         s.FaturaTutar,
                                         s.Nakit,
                                         s.KrediKarti,
                                         s.Havale,
                                         s.HarcamaAdet,
                                         s.HarcamaTutar,
                                         s.TahsilTutar,

                                     };
            if (!String.IsNullOrEmpty(rollerim))
            {
                YasamDongusuSorgu1 = YasamDongusuSorgu1.Where(m => m.Kategoriid == int.Parse(rollerim));


                string toplamcek = YasamDongusuSorgu1.Where(m => m.Kategoriid == int.Parse(rollerim)).Sum(m => m.Cek).ToString("#,###,###.#0");
                ViewBag.tc = toplamcek;


                string toplamnakit = YasamDongusuSorgu1.Where(m => m.Kategoriid == int.Parse(rollerim)).Sum(m => m.Nakit).ToString("#,###,###.#0");
                ViewBag.tn = toplamnakit;



                string toplamhavale = YasamDongusuSorgu1.Where(m => m.Kategoriid == int.Parse(rollerim)).Sum(m => m.Havale).ToString("#,###,###.#0");
                ViewBag.th = toplamhavale;



                string toplamkredikarti = YasamDongusuSorgu1.Where(m => m.Kategoriid == int.Parse(rollerim)).Sum(m => m.KrediKarti).ToString("#,###,###.#0");
                ViewBag.tkk = toplamkredikarti;


                string toplamgunluktahsiltutar = YasamDongusuSorgu1.Where(m => m.Kategoriid == int.Parse(rollerim)).Sum(m => m.TahsilTutar).ToString("#,###,###.#0");
                ViewBag.tgt = toplamgunluktahsiltutar;




                string toplamFaturaSayisi = YasamDongusuSorgu1.Where(m => m.Kategoriid == int.Parse(rollerim)).Sum(m => m.FaturaSayisi).ToString();
                ViewBag.tfs = toplamFaturaSayisi;


                string toplamFaturaTutar = YasamDongusuSorgu1.Where(m => m.Kategoriid == int.Parse(rollerim)).Sum(m => m.FaturaTutar).ToString("#,###,###.#0");
                ViewBag.tft = toplamFaturaTutar;


                string toplamHarcamaAdet = YasamDongusuSorgu1.Where(m => m.Kategoriid == int.Parse(rollerim)).Sum(m => m.HarcamaAdet).ToString();
                ViewBag.tha = toplamHarcamaAdet;


                string toplamHarcamaTutar = YasamDongusuSorgu1.Where(m => m.Kategoriid == int.Parse(rollerim)).Sum(m => m.HarcamaTutar).ToString("#,###,###.#0");
                ViewBag.tht = toplamHarcamaTutar;

            }
            List<MerkezHelper> YasamDongusuListesi1 = new List<MerkezHelper>();
            foreach (var item in YasamDongusuSorgu1)
            {
                MerkezHelper kh = new MerkezHelper();
                kh.CiroNo = item.Ciroid;
                kh.SubeAdi = item.KategoriAdi;
                kh.Cek = item.Cek.ToString("#,###,###.#0");
                kh.Tarih = item.Tarih;
                kh.FaturaSayisi = item.FaturaSayisi;
                kh.FaturaTutari = item.FaturaTutar.ToString("#,###,###.#0");
                kh.Nakit = item.Nakit.ToString("#,###,###.#0");
                kh.KrediKarti = item.KrediKarti.ToString("#,###,###.#0");
                kh.Havale = item.Havale.ToString("#,###,###.#0");
                kh.HarcamaAdet = item.HarcamaAdet;
                kh.HarcamaTutar = item.HarcamaTutar.ToString("#,###,###.#0");
                kh.GunlukTahsilTutar = item.TahsilTutar.ToString("#,###,###.#0");


                kh.ToplamCek = ViewBag.tc;
                kh.ToplamNakit = ViewBag.tn;
                kh.ToplamHavale = ViewBag.th;
                kh.ToplamKrediKarti = ViewBag.tkk;
                kh.ToplamGunlukTahsilTutar = ViewBag.tgt;
                kh.ToplamFaturaSayisi = ViewBag.tfs;
                kh.ToplamFaturaTutari = ViewBag.tft;
                kh.ToplamHarcamaAdet = ViewBag.tha;
                kh.ToplamHarcamaTutar = ViewBag.tht;

                YasamDongusuListesi1.Add(kh);
            }
            KategorileriDroplaraAt();
            MerkezListeModel newModel = new MerkezListeModel()
            {
                Filtreleme = filt1.Filtreleme,
                MerkezListeModelimiz = YasamDongusuListesi1
            };

            //sil başı
            GridView gv = new GridView();
            gv.DataSource = YasamDongusuListesi1;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=inceoglu.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            //sil sonu
            return View(newModel);
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ListProduct = this.db.Subes.OrderByDescending(x=>x.Tarih).ToList().Take(15);
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection frm)
        {

            string [] ids = frm["productid"].Split(new char[]{','});
            foreach (var item in ids)
            {
                var product =this.db.Subes.First(x => x.Ciroid ==int.Parse(item));
                this.db.Subes.DeleteOnSubmit(product);
                this.db.SubmitChanges();
            }
           

            return RedirectToAction("Index");
        }

        public ActionResult YasamaDongusuListele2()
        {
            var YasamDongusuSorgu1 = from s in db.Subes  select new
                                     {
                                         s.Ciroid,
                                         s.FaturaSayisi,
                                         s.FaturaTutar,
                                         s.Nakit,
                                         s.KrediKarti,
                                         s.Cek,
                                         s.Havale,
                                         s.HarcamaAdet,
                                         s.HarcamaTutar,
                                         s.Tarih,
                                         s.GunlukTutulanNot,
                                         s.TahsilTutar,

                                     };
            List<MerkezHelper> YasamDongusuListesi1 = new List<MerkezHelper>();

            foreach (var item in YasamDongusuSorgu1)
            {
                MerkezHelper kh1 = new MerkezHelper();
                kh1.CiroNo = item.Ciroid;
                kh1.FaturaSayisi = item.FaturaSayisi;
                kh1.FaturaTutari = item.FaturaTutar.ToString("#,###,###.#0");
                kh1.Nakit = item.Nakit.ToString("#,###,###.#0");
                kh1.KrediKarti = item.KrediKarti.ToString("#,###,###.#0");
                kh1.Cek = item.Cek.ToString("#,###,###.#0");
                kh1.Havale = item.Havale.ToString("#,###,###.#0");
                kh1.HarcamaAdet = item.HarcamaAdet;
                kh1.HarcamaTutar = item.HarcamaTutar.ToString("#,###,###.#0");
                kh1.Tarih = item.Tarih;
                kh1.GunlukNot = item.GunlukTutulanNot;
                kh1.GunlukTahsilTutar = item.TahsilTutar.ToString("#,###,###.#0");
                YasamDongusuListesi1.Add(kh1);
            }
            MerkezListeModel newModel = new MerkezListeModel()
            {
                Filtreleme = new Filtreleme(),
                MerkezListeModelimiz = YasamDongusuListesi1
            };

            return View(newModel);
        }
        [HttpPost]
        public ActionResult YasamaDongusuListele2(MerkezListeModel filt1)
        {
            if (filt1.Filtreleme.DonguTarih1Filtre.Year < 1900)
                filt1.Filtreleme.DonguTarih1Filtre = DateTime.Now.AddMonths(-1);
            if (filt1.Filtreleme.DonguTarih2Filtre.Year < 1900)
                filt1.Filtreleme.DonguTarih2Filtre = DateTime.Now;
            if (filt1.Filtreleme.DonguTarih1Filtre > filt1.Filtreleme.DonguTarih2Filtre)
                filt1.Filtreleme.DonguTarih1Filtre = filt1.Filtreleme.DonguTarih2Filtre;

            var YasamDongusuSorgu1 = from s in db.Subes 
                                     where (s.Tarih >= filt1.Filtreleme.DonguTarih1Filtre && s.Tarih <=             filt1.Filtreleme.DonguTarih2Filtre)
                                     select new
                                     {
                                         s.Ciroid,
                                         s.Cek,
                                         s.Tarih,
                                         s.FaturaSayisi,
                                         s.FaturaTutar,
                                         s.Nakit,
                                         s.KrediKarti,
                                         s.Havale,
                                         s.HarcamaAdet,
                                         s.HarcamaTutar,
                                         s.TahsilTutar,

                                     };
 
              


                string toplamcek = YasamDongusuSorgu1.Sum(m => m.Cek).ToString("#,###,###.#0");
                ViewBag.tc = toplamcek;


                string toplamnakit = YasamDongusuSorgu1.Sum(m => m.Nakit).ToString("#,###,###.#0");
                ViewBag.tn = toplamnakit;



                string toplamhavale = YasamDongusuSorgu1.Sum(m => m.Havale).ToString("#,###,###.#0");
                ViewBag.th = toplamhavale;



                string toplamkredikarti = YasamDongusuSorgu1.Sum(m => m.KrediKarti).ToString("#,###,###.#0");
                ViewBag.tkk = toplamkredikarti;


                string toplamgunluktahsiltutar = YasamDongusuSorgu1.Sum(m => m.TahsilTutar).ToString("#,###,###.#0");
                ViewBag.tgt = toplamgunluktahsiltutar;




                string toplamFaturaSayisi = YasamDongusuSorgu1.Sum(m => m.FaturaSayisi).ToString();
                ViewBag.tfs = toplamFaturaSayisi;


                string toplamFaturaTutar = YasamDongusuSorgu1.Sum(m => m.FaturaTutar).ToString("#,###,###.#0");
                ViewBag.tft = toplamFaturaTutar;


                string toplamHarcamaAdet = YasamDongusuSorgu1.Sum(m => m.HarcamaAdet).ToString();
                ViewBag.tha = toplamHarcamaAdet;


                string toplamHarcamaTutar = YasamDongusuSorgu1.Sum(m => m.HarcamaTutar).ToString("#,###,###.#0");
                ViewBag.tht = toplamHarcamaTutar;

           
            List<MerkezHelper> YasamDongusuListesi1 = new List<MerkezHelper>();
            foreach (var item in YasamDongusuSorgu1)
            {
                MerkezHelper kh = new MerkezHelper();
                kh.CiroNo = item.Ciroid;
                kh.Cek = item.Cek.ToString("#,###,###.#0");
                kh.Tarih = item.Tarih;
                kh.FaturaSayisi = item.FaturaSayisi;
                kh.FaturaTutari = item.FaturaTutar.ToString("#,###,###.#0");
                kh.Nakit = item.Nakit.ToString("#,###,###.#0");
                kh.KrediKarti = item.KrediKarti.ToString("#,###,###.#0");
                kh.Havale = item.Havale.ToString("#,###,###.#0");
                kh.HarcamaAdet = item.HarcamaAdet;
                kh.HarcamaTutar = item.HarcamaTutar.ToString("#,###,###.#0");
                kh.GunlukTahsilTutar = item.TahsilTutar.ToString("#,###,###.#0");


                kh.ToplamCek = ViewBag.tc;
                kh.ToplamNakit = ViewBag.tn;
                kh.ToplamHavale = ViewBag.th;
                kh.ToplamKrediKarti = ViewBag.tkk;
                kh.ToplamGunlukTahsilTutar = ViewBag.tgt;
                kh.ToplamFaturaSayisi = ViewBag.tfs;
                kh.ToplamFaturaTutari = ViewBag.tft;
                kh.ToplamHarcamaAdet = ViewBag.tha;
                kh.ToplamHarcamaTutar = ViewBag.tht;

                YasamDongusuListesi1.Add(kh);
            }
            KategorileriDroplaraAt();
            MerkezListeModel newModel = new MerkezListeModel()
            {
                Filtreleme = filt1.Filtreleme,
                MerkezListeModelimiz = YasamDongusuListesi1
            };
            return View(newModel);
        }
        public ActionResult AnaSayfa()
        {
            return View("AnaSayfa");
        }

        public ActionResult YasamaDongusuListele3()
        {
            var YasamDongusuSorgu1 = from s in db.Subes
                                     join k in db.KategoriTablosus on s.Kategoriid equals k.Kategoriid
                                     select new
                                     {
                                         s.Ciroid,
                                         s.FaturaSayisi,
                                         s.FaturaTutar,
                                         s.Nakit,
                                         s.KrediKarti,
                                         s.Cek,
                                         s.Havale,
                                         s.HarcamaAdet,
                                         s.HarcamaTutar,
                                         k.KategoriAdi,
                                         s.Tarih,
                                         s.GunlukTutulanNot,
                                         s.TahsilTutar,

                                     };
            List<MerkezHelper> YasamDongusuListesi1 = new List<MerkezHelper>();

            foreach (var item in YasamDongusuSorgu1)
            {
                MerkezHelper kh1 = new MerkezHelper();
                kh1.CiroNo = item.Ciroid;
                kh1.FaturaSayisi = item.FaturaSayisi;
                kh1.FaturaTutari = item.FaturaTutar.ToString("#,###,###.#0");
                kh1.Nakit = item.Nakit.ToString("#,###,###.#0");
                kh1.KrediKarti = item.KrediKarti.ToString("#,###,###.#0");
                kh1.Cek = item.Cek.ToString("#,###,###.#0");
                kh1.Havale = item.Havale.ToString("#,###,###.#0");
                kh1.HarcamaAdet = item.HarcamaAdet;
                kh1.HarcamaTutar = item.HarcamaTutar.ToString("#,###,###.#0");
                kh1.SubeAdi = item.KategoriAdi;
                kh1.Tarih = item.Tarih;
                kh1.GunlukNot = item.GunlukTutulanNot;
                kh1.GunlukTahsilTutar = item.TahsilTutar.ToString("#,###,###.#0");
                YasamDongusuListesi1.Add(kh1);
            }
           
            MerkezListeModel newModel = new MerkezListeModel()
            {
                Filtreleme = new Filtreleme(),
                MerkezListeModelimiz = YasamDongusuListesi1
            };

            return View(newModel);
        }
        [HttpPost]
        public ActionResult YasamaDongusuListele3(MerkezListeModel filt1)
        {
            if (filt1.Filtreleme.DonguTarih1Filtre.Year < 1900)
                filt1.Filtreleme.DonguTarih1Filtre = DateTime.Now.AddMonths(-1);
            if (filt1.Filtreleme.DonguTarih2Filtre.Year < 1900)
                filt1.Filtreleme.DonguTarih2Filtre = DateTime.Now;
            if (filt1.Filtreleme.DonguTarih1Filtre > filt1.Filtreleme.DonguTarih2Filtre)
                filt1.Filtreleme.DonguTarih1Filtre = filt1.Filtreleme.DonguTarih2Filtre;

            var YasamDongusuSorgu1 = from s in db.Subes
                                     join k in db.KategoriTablosus on s.Kategoriid equals k.Kategoriid
                                     where (s.Tarih >= filt1.Filtreleme.DonguTarih1Filtre && s.Tarih <= filt1.Filtreleme.DonguTarih2Filtre)
                                     select new
                                     {
                                         s.Ciroid,
                                         k.KategoriAdi,
                                         k.Kategoriid,
                                         s.Cek,
                                         s.Tarih,
                                         s.FaturaSayisi,
                                         s.FaturaTutar,
                                         s.Nakit,
                                         s.KrediKarti,
                                         s.Havale,
                                         s.HarcamaAdet,
                                         s.HarcamaTutar,
                                         s.TahsilTutar,

                                     };
           


                string toplamcek = YasamDongusuSorgu1.Sum(m => m.Cek).ToString("#,###,###.#0");
                ViewBag.tc = toplamcek;


                string toplamnakit = YasamDongusuSorgu1.Sum(m => m.Nakit).ToString("#,###,###.#0");
                ViewBag.tn = toplamnakit;



                string toplamhavale = YasamDongusuSorgu1.Sum(m => m.Havale).ToString("#,###,###.#0");
                ViewBag.th = toplamhavale;



                string toplamkredikarti = YasamDongusuSorgu1.Sum(m => m.KrediKarti).ToString("#,###,###.#0");
                ViewBag.tkk = toplamkredikarti;


                string toplamgunluktahsiltutar = YasamDongusuSorgu1.Sum(m => m.TahsilTutar).ToString("#,###,###.#0");
                ViewBag.tgt = toplamgunluktahsiltutar;




                string toplamFaturaSayisi = YasamDongusuSorgu1.Sum(m => m.FaturaSayisi).ToString();
                ViewBag.tfs = toplamFaturaSayisi;


                string toplamFaturaTutar = YasamDongusuSorgu1.Sum(m => m.FaturaTutar).ToString("#,###,###.#0");
                ViewBag.tft = toplamFaturaTutar;


                string toplamHarcamaAdet = YasamDongusuSorgu1.Sum(m => m.HarcamaAdet).ToString();
                ViewBag.tha = toplamHarcamaAdet;


                string toplamHarcamaTutar = YasamDongusuSorgu1.Sum(m => m.HarcamaTutar).ToString("#,###,###.#0");
                ViewBag.tht = toplamHarcamaTutar;

            
            List<MerkezHelper> YasamDongusuListesi1 = new List<MerkezHelper>();
            foreach (var item in YasamDongusuSorgu1)
            {
                MerkezHelper kh = new MerkezHelper();
                kh.CiroNo = item.Ciroid;
                kh.SubeAdi = item.KategoriAdi;
                kh.Cek = item.Cek.ToString("#,###,###.#0");
                kh.Tarih = item.Tarih;
                kh.FaturaSayisi = item.FaturaSayisi;
                kh.FaturaTutari = item.FaturaTutar.ToString("#,###,###.#0");
                kh.Nakit = item.Nakit.ToString("#,###,###.#0");
                kh.KrediKarti = item.KrediKarti.ToString("#,###,###.#0");
                kh.Havale = item.Havale.ToString("#,###,###.#0");
                kh.HarcamaAdet = item.HarcamaAdet;
                kh.HarcamaTutar = item.HarcamaTutar.ToString("#,###,###.#0");
                kh.GunlukTahsilTutar = item.TahsilTutar.ToString("#,###,###.#0");


                kh.ToplamCek = ViewBag.tc;
                kh.ToplamNakit = ViewBag.tn;
                kh.ToplamHavale = ViewBag.th;
                kh.ToplamKrediKarti = ViewBag.tkk;
                kh.ToplamGunlukTahsilTutar = ViewBag.tgt;
                kh.ToplamFaturaSayisi = ViewBag.tfs;
                kh.ToplamFaturaTutari = ViewBag.tft;
                kh.ToplamHarcamaAdet = ViewBag.tha;
                kh.ToplamHarcamaTutar = ViewBag.tht;

                YasamDongusuListesi1.Add(kh);
            }
            KategorileriDroplaraAt();
            MerkezListeModel newModel = new MerkezListeModel()
            {
                Filtreleme = filt1.Filtreleme,
                MerkezListeModelimiz = YasamDongusuListesi1
            };

            //sil başı
            GridView gv = new GridView();
            gv.DataSource = YasamDongusuListesi1;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=inceoglu.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            //sil sonu
            return View(newModel);
        }

       
        
    }
}
