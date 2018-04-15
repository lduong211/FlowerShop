﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlowerShop.Models;

namespace FlowerShop.Controllers
{
    public class FlowerShopController : Controller
    {
        //tao doi tuong chua toan bo csdl flowershop
        dbFlowerShopDataContext data = new dbFlowerShopDataContext();
        //lay sach moi
        private List<HOA> Layhoa(int count)
        {
            // sap xep giam dan theo ngay cap nhat
            return data.HOAs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        // GET: FlowerShop
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Single(int id)
        {
            var hoa = from s in data.HOAs where s.Mahoa == id select s;
            return View(hoa.Single());
        }
        public ActionResult Products()
        {
            var hoamoi = Layhoa(1000);
            var anhbia = from Anhbia in data.HOAs select Anhbia; 
            return View(hoamoi);
        }
        public ActionResult Mycart()
        {
            return View();
        }
        public ActionResult Chude(int id)
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }
        private List<HOA> Layhoaton(int count)
        {
            return data.HOAs.OrderBy(a => a.Soluongton).Take(count).ToList();
        }
        public ActionResult Hoaton()
        {
            var hoaton = Layhoaton(6);
            return PartialView(hoaton);
        }
        private List<HOA> Layhoamoi(int count)
        {
            return data.HOAs.OrderBy(a => a.Ngaycapnhat).Take(count).ToList();
        }
        public ActionResult Hoamoi()
        {
            var hoamoi = Layhoamoi(6);
            return PartialView(hoamoi);
        }
    }
}