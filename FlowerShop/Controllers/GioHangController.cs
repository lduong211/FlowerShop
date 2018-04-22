using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlowerShop.Models;
namespace FlowerShop.Controllers
{
    public class GioHang
    {
        //tạo đối tượng data chứa dữ liệu model dbBanSach đã tạo
        dbFlowerShopDataContext data = new dbFlowerShopDataContext();
        public int iMahoa { set; get; }
        public string sTenhoa { set; get; }
        public string sAnhbia { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }
        //khoi tao gio hang theo mahoa duoc truyen vao voi so luong mac dinh la 1
        public GioHang (int mahoa)
        {
            iMahoa = mahoa;
            HOA hoa = data.HOAs.Single(n => n.Mahoa == iMahoa);
            sTenhoa = hoa.Tenhoa;
            sAnhbia = hoa.Anhbia;
            dDongia = double.Parse(hoa.Giaban.ToString());
            iSoluong = 1;
        }
    }
    public class GioHangController : Controller
    {
        dbFlowerShopDataContext data = new dbFlowerShopDataContext();
        // GET: GioHang
        public List<GioHang>Laygiohang()
        {
            List<GioHang> lstGiohang = Session["Giohang"] as List<GioHang>;
            if(lstGiohang==null)
            {
                //neu giỏ hàng chưa tồn tại thì khởi tạo list giỏ hàng
                lstGiohang = new List<GioHang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        public ActionResult ThemGioHang(int iMahoa, string strURL)
        {
            //lay ra session gio hang
            List<GioHang> lstGiohang = Laygiohang();
            //kiểm tra sách này có tồn tại trong giỏ hàng chưa 
            GioHang sanpham = lstGiohang.Find(n => n.iMahoa == iMahoa);
            if(sanpham == null)
            {
                sanpham = new GioHang(iMahoa);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        //Tong so luong
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if(lstGiohang!=null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoLuong;
        }
        //Tong tien
        private double TongTien()
        {
            double iTongTien = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return iTongTien;
        }//xây dựng giỏ hàng
        public ActionResult GioHang()
        {
            List<GioHang> lstGiohang = Laygiohang();
            if (lstGiohang.Count==0)
            {
                RedirectToAction("Mycart","FlowerShop");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
    }
}