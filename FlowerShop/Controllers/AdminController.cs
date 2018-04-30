using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlowerShop.Models;
using PagedList;
using System.IO;

namespace FlowerShop.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        dbFlowerShopDataContext db = new dbFlowerShopDataContext();
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
            {
                return RedirectToAction("AdminDangnhap", "Admin");
            }
            return View();
        }

        [HttpGet]
        public ActionResult AdminDangnhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminDangnhap(FormCollection collection)
        {
            // Gán các giá tr? ngu?i dùng nh?p li?u cho các bi?n 
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                //Gán giá tr? cho d?i tu?ng du?c t?o m?i (ad)        

                ADMIN ad = db.ADMINs.SingleOrDefault(n => n.TenTK == tendn && n.MatKhau == matkhau);
                if (ad != null)
                {
                   // ViewBag.Thongbao = "Chúc m?ng dang nh?p thành công";
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        //===================Admin đăng xuất
        public ActionResult LogOff()
        {
            Session["Taikhoanadmin"] = null;
            Session["Taikhoanadmin"] = "";
            return RedirectToAction("Index", "FlowerShop");
        }
        //===================HOA
        public ActionResult Hoa(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.SACHes.ToList());
            return View(db.HOAs.ToList().OrderBy(n => n.Mahoa).ToPagedList(pageNumber, pageSize));
        }
        


        //them moi hoa
        [HttpGet]       
        public ActionResult Themmoihoa()
        {
            //Dua du lieu vao dropdownList
            //Lay ds tu tabke chu de, s?p xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaTK = new SelectList(db.THIETKEs.ToList().OrderBy(n => n.KieuThietKe), "MaTK", "KieuThietKe");
            return View();
        }


        //upload hinh anh
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Themmoihoa(HOA hoa, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaTK = new SelectList(db.THIETKEs.ToList().OrderBy(n => n.KieuThietKe), "MaTK", "KieuThietKe");
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    hoa.Anhbia = fileName;
                    //Luu vao CSDL
                    db.HOAs.InsertOnSubmit(hoa);
                    db.SubmitChanges();
                    return RedirectToAction("Hoa");
                }
                return View();
            }

        }

        //Hi?n th? s?n ph?m
        public ActionResult Chitiethoa(int id)
        {
            //Lay ra doi tuong sach theo ma
            HOA hoa = db.HOAs.SingleOrDefault(n => n.Mahoa == id);
            ViewBag.Masach = hoa.Mahoa;
            if (hoa == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(hoa);
        }

        //Xóa s?n ph?m
        [HttpGet]
        public ActionResult Xoahoa(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            HOA hoa = db.HOAs.SingleOrDefault(n => n.Mahoa == id);
            ViewBag.Masach = hoa.Mahoa;
            if (hoa == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(hoa);
        }

        [HttpPost, ActionName("Xoahoa")]
        public ActionResult Xacnhanxoa(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            HOA hoa = db.HOAs.SingleOrDefault(n => n.Mahoa == id);
            ViewBag.Mahoa = hoa.Mahoa;
            if (hoa == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.HOAs.DeleteOnSubmit(hoa);
            db.SubmitChanges();
            return RedirectToAction("Hoa");
        }

        //[HttpGet]
        //public ActionResult Suahoa(int id)
        //{
        //    //Lay ra doi tuong sach theo ma
        //    HOA hoa = db.HOAs.SingleOrDefault(n => n.Mahoa == id);
        //    //ViewBag.Mahoa = hoa.Mahoa;
        //    if (hoa == null)
        //    {
        //        Response.StatusCode = 404;
        //        return null;
        //    }
        //    //Dua du lieu vao dropdownList
        //    //Lay ds tu tabke chu de, s?p xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude
        //    ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", hoa.MaCD);
        //    ViewBag.MaTK = new SelectList(db.THIETKEs.ToList().OrderBy(n => n.KieuThietKe), "MaTK", "KieuThietKe",hoa.MaTK);
        //    return View(hoa);
        //}

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Suahoa(HOA hoa , HttpPostedFileBase fileUpload)
        //{
        //    //Dua du lieu vao dropdownload
        //    ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
        //    ViewBag.MaTK = new SelectList(db.THIETKEs.ToList().OrderBy(n => n.KieuThietKe), "MaTK", "KieuThietKe");
        //    //Kiem tra duong dan file
        //    if (fileUpload == null)
        //    {
        //        ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
        //        return View();
        //    }
        //    //Them vao CSDL
        //    else
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            //Luu ten fie, luu y bo sung thu vien using System.IO;
        //            var fileName = Path.GetFileName(fileUpload.FileName);
        //            //Luu duong dan cua file
        //            var path = Path.Combine(Server.MapPath("~/images"), fileName);
        //            //Kiem tra hình anh ton tai chua?
        //            if (System.IO.File.Exists(path))
        //                ViewBag.Thongbao = "Hình ảnh đã tồn tại";
        //            else
        //            {
        //                //Luu hinh anh vao duong dan
        //                fileUpload.SaveAs(path);
        //            }
        //            hoa.Anhbia = fileName;
        //            //Luu vao CSDL   
        //            UpdateModel(hoa);
        //            db.SubmitChanges();
        //        }
        //        return RedirectToAction("Hoa");
        //    }
        //}
        //===============================================================================================================
        //====================CHU DE
        public ActionResult Chude(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.SACHes.ToList());
            return View(db.CHUDEs.ToList().OrderBy(n => n.MaCD).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Themmoichude()
        {            
            return View();
        }        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Themmoichude(CHUDE cd)
        {
            if (ModelState.IsValid)
            {
                //chen du lieu vao bang khach hang
                db.CHUDEs.InsertOnSubmit(cd);
                //luu vao co so du lieu
                db.SubmitChanges();

                return RedirectToAction("Chude", "Admin");
            }

            return View();
        }

        //Xóa s?n ph?m
        [HttpGet]
        public ActionResult Xoachude(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            CHUDE cd = db.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            ViewBag.Masach = cd.MaCD;
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(cd);
        }

        [HttpPost, ActionName("Xoachude")]
        public ActionResult XNxoachude(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            CHUDE cd = db.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            ViewBag.Mahoa = cd.MaCD;
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.CHUDEs.DeleteOnSubmit(cd);
            db.SubmitChanges();
            return RedirectToAction("Hoa");
        }
        [HttpGet]
        public ActionResult Suachude(int id)
        {
            //Lay ra doi tuong sach theo ma
            CHUDE cd = db.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            ViewBag.Machude = cd.MaCD;
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }            
            return View(cd);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Suachude(CHUDE cd)
        {
            if (ModelState.IsValid)
            {
                //chen du lieu vao bang khach hang
                UpdateModel(cd);
                //luu vao co so du lieu
                db.SubmitChanges();

                return RedirectToAction("Chude", "Admin");
            }

            return View();
        }
        //===============================================================================================================
        //====================Thiet ke
        public ActionResult Thietke(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            //return View(db.SACHes.ToList());
            return View(db.THIETKEs.ToList().OrderBy(n => n.MaTK).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Themmoithietke()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Themmoithietke(THIETKE tk)
        {
            if (ModelState.IsValid)
            {
                //chen du lieu vao bang khach hang
                db.THIETKEs.InsertOnSubmit(tk);
                //luu vao co so du lieu
                db.SubmitChanges();

                return RedirectToAction("Thietke", "Admin");
            }

            return View();
        }

        //Xóa thiet ke
        [HttpGet]
        public ActionResult Xoathietke(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            THIETKE tk = db.THIETKEs.SingleOrDefault(n => n.MaTK == id);
            ViewBag.Masach = tk.MaTK;
            if (tk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tk);
        }

        [HttpPost, ActionName("Xoathietke")]
        public ActionResult XNxoathietke(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            THIETKE tk  = db.THIETKEs.SingleOrDefault(n => n.MaTK == id);
            ViewBag.MaTK = tk.MaTK;
            if (tk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.THIETKEs.DeleteOnSubmit(tk);
            db.SubmitChanges();
            return RedirectToAction("Thietke");
        }

        [HttpGet]
        public string getImage(int id)
        {
            return db.HOAs.SingleOrDefault(n => n.Mahoa == id).Anhbia;
        }
        public ActionResult Suasp(int id)
        {
            HOA sp = db.HOAs.SingleOrDefault(n => n.Mahoa == id);
            ViewBag.MaSanPham = sp.Mahoa;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaTK = new SelectList(db.THIETKEs.ToList().OrderBy(n => n.KieuThietKe), "MaTK", "KieuThietKe", sp.MaTK);
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sp.MaCD);
            return View(sp);
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Suasp(HOA sp, HttpPostedFileBase fileUpload)
        {

            HOA spm = db.HOAs.SingleOrDefault(n => n.Mahoa == sp.Mahoa);
            ViewBag.MaSanPham = sp.Mahoa;
            var Tenhoa = sp.Tenhoa;
            var Matk = sp.MaTK;
            var Macd = sp.MaCD;
            var Mota = sp.Mota;
            var Gia = sp.Giaban;
            var Soluongton = sp.Soluongton;
            var Anh = sp.Anhbia;
            var ngaycn = sp.Ngaycapnhat;

            spm.Ngaycapnhat = ngaycn;
            spm.Tenhoa = Tenhoa;
            spm.Anhbia = Anh;
            spm.MaTK = Matk;
            spm.MaCD = Macd;
            spm.Mota = Mota;
            spm.Giaban = Gia;
            spm.Soluongton = Soluongton;

            if (ModelState.IsValid)
            {
                ViewBag.MaTK = new SelectList(db.THIETKEs.ToList().OrderBy(n => n.KieuThietKe), "MaTK", "KieuThietKe");
                ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
                if (fileUpload == null)
                {
                    spm.Anhbia = getImage(sp.Mahoa);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        var path = Path.Combine(Server.MapPath("~/images/"), fileName); 
                        if (System.IO.File.Exists(path))
                        {
                            sp.Anhbia = fileName;
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                            return View(sp);
                        }
                        else
                        {
                            fileUpload.SaveAs(path);
                            spm.Anhbia = fileName;
                        }
                    }
                }
                db.SubmitChanges();
            }
            return RedirectToAction("Hoa");
        }
    }
}