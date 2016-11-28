using CiroHesaplama.Models;
using CiroHesaplama.Models.DIO;
using CiroHesaplama.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
namespace CiroHesaplama.Controllers
{
    public class LokumController : BaseController
    {
        CultureInfo dil = new CultureInfo("tr-TR");
        DataClasses1DataContext db = new DataClasses1DataContext();
        public ActionResult Index(int? page)
        {
            var kullaniciSorgu = (from k in db.KategoriTablosus
                                  join s in db.Subes on k.Kategoriid equals s.Kategoriid
                                  orderby s.Tarih
                                  where k.KategoriAdi.Equals("Lokum Stargate")
                                  select new
                                  {
                                      k.KategoriAdi,
                                      s.Ciroid,
                                      s.Tarih,
                                      s.FaturaSayisi,
                                      s.FaturaTutar,
                                      s.Havale,
                                      s.Nakit,
                                      s.KrediKarti,
                                      s.Cek,
                                      s.TahsilTutar,
                                      s.HarcamaAdet,
                                      s.HarcamaTutar,
                                      s.GunlukTutulanNot
                                  }).OrderByDescending(s => s.Tarih).Take(150);
            List<SubeHelperim> kullaniciListesi = new List<SubeHelperim>();
            foreach (var item in kullaniciSorgu)
            {
                SubeHelperim kh = new SubeHelperim();
                kh.CiroidHelper = item.Ciroid;
                kh.FaturaSayisiHelper = Convert.ToInt32(item.FaturaSayisi);
                kh.FaturaTutarHelper = item.FaturaTutar.ToString("#,###,###.#0");
                kh.NakitHelper = item.Nakit.ToString("#,###,###.#0");
                kh.KrediKartiHelper = item.KrediKarti.ToString("#,###,###.#0");
                kh.CekHelper = item.Cek.ToString("#,###,###.#0");
                kh.HavaleHelper = item.Havale.ToString("#,###,###.#0");
                kh.HarcamaAdetHelper = Convert.ToInt32(item.HarcamaAdet);
                kh.HarcamaTutarHelper = item.HarcamaTutar.ToString("#,###,###.#0");
                kh.GunlukTahsilatTutarHelper = item.TahsilTutar.ToString("#,###,###.#0");
                kh.GunlukNotHelper = item.GunlukTutulanNot;
                kh.KategoriNameHelper = item.KategoriAdi;
                kh.TarihHelper = item.Tarih;
                kullaniciListesi.Add(kh);
            }
            return View(kullaniciListesi.ToPagedList(page ?? 1, 10));
        }

        public ActionResult NotListele(int? page)
        {
            var kullaniciSorgu = (from k in db.KategoriTablosus
                                  join s in db.Subes on k.Kategoriid equals s.Kategoriid
                                  orderby s.Tarih
                                  where k.KategoriAdi.Equals("Lokum Stargate")
                                  select new
                                  {
                                      k.KategoriAdi,
                                      s.Ciroid,
                                      s.Tarih,
                                      s.GunlukTutulanNot
                                  }).OrderByDescending(s => s.Tarih).Take(150);
            List<SubeHelperim> kullaniciListesi = new List<SubeHelperim>();
            foreach (var item in kullaniciSorgu)
            {
                SubeHelperim kh = new SubeHelperim();
                kh.CiroidHelper = item.Ciroid;
                kh.GunlukNotHelper = item.GunlukTutulanNot;
                kh.KategoriNameHelper = item.KategoriAdi;
                kh.TarihHelper = item.Tarih;
                kullaniciListesi.Add(kh);
            }
            return View(kullaniciListesi.ToPagedList(page ?? 1, 10));

        }
        public void KategorileriDroplaraAt()
        {
            List<SelectListItem> rolList = new List<SelectListItem>();
            var rollerSorgu = from k in db.KategoriTablosus select k;
            foreach (var item in rollerSorgu)
            {
                rolList.Add(new SelectListItem
                {
                    Text = item.KategoriAdi,
                    Value = item.Kategoriid.ToString(),
                    Selected = item.Kategoriid == 8 ? true : false
                });
            }
            ViewBag.rollerim = rolList;
        }
        public ActionResult Ekle()
        {
            KategorileriDroplaraAt();
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Ekle(FormCollection frm)
        {
            int FaturaSayimiz = Convert.ToInt32(frm.Get("txtFaturaSayimiz"));
            decimal FaturaTutarimiz = Convert.ToDecimal(frm.Get("txtFaturaTutarimiz"));
            string FormatFaturaTutarimiz = FaturaTutarimiz.ToString("#,###,###.#0");
            decimal Nakitimiz = Convert.ToDecimal(frm.Get("txtNakitimiz"));
            string FormatNakitimiz = Nakitimiz.ToString("#,###,###.#0");
            decimal Cekimiz = Convert.ToDecimal(frm.Get("txtCekimize"));
            string FormatCekimiz = Cekimiz.ToString("#,###,###.#0");
            decimal KrediKartimiz = Convert.ToDecimal(frm.Get("txtKrediKartimiz"));
            string FormatKrediKartimiz = KrediKartimiz.ToString("#,###,###.#0");
            decimal Havalemiz = Convert.ToDecimal(frm.Get("txtHavalemiz"));
            string FormatHavalemiz = Havalemiz.ToString("#,###,###.#0");
            int HarcamaAdetimiz = Convert.ToInt32(frm.Get("txtHarcamaAdetimiz"));
            decimal HarcamaTutarimiz = Convert.ToDecimal(frm.Get("txtHarcamaTutarimiz"));
            string FormatHarcamaTutarimiz = HarcamaTutarimiz.ToString("#,###,###.#0");
            DateTime FTarih = Convert.ToDateTime(frm.Get("textBoxDogum"));
            string Notumuz = frm.Get("txtNotumuz");
            int RolNumara = Convert.ToInt32(frm.Get("rollerim"));
            Sube t = new Sube
            {
                FaturaSayisi = FaturaSayimiz,
                FaturaTutar = Convert.ToDouble(FormatFaturaTutarimiz),
                Nakit = Convert.ToDouble(FormatNakitimiz),
                Cek = Convert.ToDouble(FormatCekimiz),
                KrediKarti = Convert.ToDouble(FormatKrediKartimiz),
                Havale = Convert.ToDouble(FormatHavalemiz),
                Tarih = FTarih,
                HarcamaAdet = HarcamaAdetimiz,
                HarcamaTutar = Convert.ToDouble(FormatHarcamaTutarimiz),
                TahsilTutar = Convert.ToDouble(FormatNakitimiz) + Convert.ToDouble(FormatCekimiz) + Convert.ToDouble(FormatKrediKartimiz) + Convert.ToDouble(FormatHavalemiz),
                Kategoriid = RolNumara,
                GunlukTutulanNot = Notumuz
            };
            db.Subes.InsertOnSubmit(t);
            db.SubmitChanges();
            KategorileriDroplaraAt();
            ViewBag.sonuc = "Kayıt başarılı";
            return RedirectToAction("Index");

        }
        public ActionResult Sil(int id)
        {
            if (id != 0)
            {
                Sube k = db.Subes.First(x => x.Ciroid == id);
                db.Subes.DeleteOnSubmit(k);
                db.SubmitChanges();
                ViewBag.silindi = "silme işlemi başarılı 2 sn sonra yönlendirileceksiniz";
                Response.AppendHeader("Refresh", "2;url=../Index");
                return View();
            }
            return View();
        }
        public void KategorilerUpdate(int id)
        {
            List<SelectListItem> rolList = new List<SelectListItem>();
            var rolSorgu = from r in db.KategoriTablosus select r;
            foreach (var item in rolSorgu)
            {

                if (id == item.Kategoriid)
                {
                    rolList.Add(new SelectListItem
                    {
                        Text = item.KategoriAdi,
                        Value = item.Kategoriid.ToString(),
                        Selected = true
                    });
                }
                else
                {
                    rolList.Add(new SelectListItem
                    {
                        Text = item.KategoriAdi,
                        Value = item.Kategoriid.ToString(),
                    });
                }

            }
            ViewBag.rollerim = rolList;
        }

        public ActionResult Duzenle(int id)
        {
            ViewBag.duzenlendi = "";
            var kullaniciDuzenle = db.Subes.First(x => x.Ciroid == id);
            KategorilerUpdate(kullaniciDuzenle.Kategoriid);
            return View(kullaniciDuzenle);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Duzenle(Sube k, FormCollection frm)
        {
            int rolid = Convert.ToInt32(frm.Get("rollerim"));
            if (ModelState.IsValid)
            {
                var m = db.Subes.FirstOrDefault(x => x.Ciroid == k.Ciroid);
                m.Ciroid = k.Ciroid;
                m.FaturaSayisi = k.FaturaSayisi;
                m.FaturaTutar = k.FaturaTutar;
                m.Cek = k.Cek;
                m.Nakit = k.Nakit;
                m.KrediKarti = k.KrediKarti;
                m.Havale = k.Havale;
                m.TahsilTutar = m.Nakit + m.Cek + m.KrediKarti + m.Havale;
                m.HarcamaAdet = k.HarcamaAdet;
                m.HarcamaTutar = k.HarcamaTutar;
                m.GunlukTutulanNot = k.GunlukTutulanNot;
                m.Tarih = DateTime.Now;
                m.Kategoriid = rolid;
                db.SubmitChanges();
                KategorilerUpdate(rolid);

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public ActionResult LogOut()
        {

            return RedirectToAction("Index", "Login");

        }


    }
}
