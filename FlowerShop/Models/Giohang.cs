using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlowerShop.Models
{
    public class Giohang
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
        public Giohang(int mahoa)
        {
            iMahoa = mahoa;
            HOA hoa = data.HOAs.Single(n => n.Mahoa == iMahoa);
            sTenhoa = hoa.Tenhoa;
            sAnhbia = hoa.Anhbia;
            dDongia = double.Parse(hoa.Giaban.ToString());
            iSoluong = 1;
        }
    }
}