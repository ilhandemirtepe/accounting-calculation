using CiroHesaplama.Models;
using CiroHesaplama.Models.DIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using CiroHesaplama.Utils;
namespace CiroHesaplama.Controllers
{
    public class AdminCRUDController : BaseController
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
                    Value = item.Kategoriid.ToString(),
                 
                });
            }
            ViewBag.rollerim = rolList;
        }
        public ActionResult Index(int? page)
        {
            var kullaniciSorgu = (from k in db.KategoriTablosus 
                                  join a in db.Admin1s 
                                  on k.Kategoriid equals a.Kategoriid
                                  select new
                                      {
                                          k.KategoriAdi,
                                          a.Adminid,
                                          a.UserName,
                                          a.PassWord
                                    
                                      });
            List<AdminHelperim> kullaniciListesi = new List<AdminHelperim>();
            foreach (var item in kullaniciSorgu)
            {
                AdminHelperim kh = new AdminHelperim();
                kh.AdminidNumberViewModel = item.Adminid;
                kh.AdminPassWordViewModel = item.PassWord;
                kh.AdminKategoriNameViewModel = item.KategoriAdi;
                kh.AdminUserNameViewModel = item.UserName;
                kullaniciListesi.Add(kh);
            }
            return View(kullaniciListesi.ToPagedList(page ?? 1, 10));
        }
        public ActionResult Ekle()
        {
            KategorileriDroplaraAt();
            return View();
        }
        [HttpPost]
        public ActionResult Ekle(FormCollection frm)
        {
          
            string KullaniciAdimiz = frm.Get("txtKullaniciAdimiz");
            string Sifre = frm.Get("txtSifre");
            int RolNumara = Convert.ToInt32(frm.Get("rollerim"));
            Admin1 t = new Admin1
            {
               Kategoriid=RolNumara,
               UserName=KullaniciAdimiz,
               PassWord=Sifre      
            };
            db.Admin1s.InsertOnSubmit(t);
            db.SubmitChanges();
            KategorileriDroplaraAt();
            ViewBag.sonuc = "Kayıt başarılı";
            return RedirectToAction("Index");
         
        }
        public ActionResult Sil(int id)
        {
            if (id != 0)
            {
                Admin1 k = db.Admin1s.First(x => x.Adminid == id);
                db.Admin1s.DeleteOnSubmit(k);
                db.SubmitChanges();
                ViewBag.silindi = "silme işlemi başarılı 2 sn sonra yönlendirileceksiniz";
                Response.AppendHeader("Refresh", "2;url=../Index");
                return View();
            }
            return View();
        }
        public ActionResult LogOut()
        {
            return RedirectToAction("Index", "Login");
        }
    }
}
