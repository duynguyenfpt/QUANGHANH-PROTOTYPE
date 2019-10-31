﻿using QUANGHANH2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using QUANGHANH2.SupportClass;

namespace QUANGHANH2.Controllers.KCM
{
    [Auther(RightID = "008")]
    public class InputPlanController : Controller
    {
        [Route("phong-kcm/nhap-ke-hoach-san-xuat")]
        public ActionResult NhapKHSX()
        {
            return View("/Views/KCM/NhapKeHoach/InputPlan_Month.cshtml");
        }

        public dynamic GetData(int month, int year, string departmentID)
        {
            using (QUANGHANHABCEntities db = new QUANGHANHABCEntities())
            {

                var sqlQuery = " select header_kh.HeaderID,TieuChi.TenTieuChi,kehoach.SanLuong,header_kh.SoNgayLamViec,[kehoach].MaTieuChi as MaTieuChiNull,TieuChi.DonViDo,[kehoach].GhiChu from (select * from header_KeHoachTungThang as header " +
                   "where header.MaPhongBan = @departmentID and header.ThangKeHoach = @month and header.NamKeHoach = @year) as header_kh " +
                   "left join (select b.*from(SELECT[HeaderID],[MaTieuChi], Max([ThoiGianNhapCuoiCung]) as [ThoiGianNhapCuoiCung] " +
                   "FROM[QUANGHANHABC].[dbo].[KeHoach_TieuChi_TheoThang] GROUP BY MaTieuChi,HeaderID) as a " +
                   "inner join[KeHoach_TieuChi_TheoThang] as b " +
                   "on a.HeaderID = b.HeaderID and a.MaTieuChi = b.MaTieuChi and a.ThoiGianNhapCuoiCung = b.ThoiGianNhapCuoiCung) as kehoach " +
                   "on header_kh.HeaderID = kehoach.HeaderID " +
                   "left join TieuChi on kehoach.MaTieuChi = TieuChi.MaTieuChi";
                return db.Database.SqlQuery<ChiTietTieuChi>(sqlQuery, new SqlParameter("departmentID", departmentID), new SqlParameter("month", month), new SqlParameter("year", year)).ToList();
            }
        }
        //
        [Route("phong-kcm/ke-hoach-san-xuat/lay-thong-tin")]
        public ActionResult getInformation()
        {
            try
            {
                var month = Int32.Parse(Request["month"]);
                var year = Int32.Parse(Request["year"]);
                var departmentID = Request["department"];
                using (QUANGHANHABCEntities db = new QUANGHANHABCEntities())
                {
                    var listAspectDepartments = (from pbtc in db.PhongBan_TieuChi
                                 .Where(x => x.MaPhongBan == departmentID)
                                                 join tieuchi in db.TieuChis on pbtc.MaTieuChi equals tieuchi.MaTieuChi
                                                 select new
                                                 {
                                                     MaTieuChi = tieuchi.MaTieuChi,
                                                     TenTieuChi = tieuchi.TenTieuChi
                                                 }).ToList();

                    var listAspect = GetData(month, year, departmentID);
                    if (listAspect.Count != 0)
                    {
                        foreach (var item in listAspect)
                        {
                            item.Identify = item.MaTieuChiNull + "-" + item.HeaderID;
                        }
                    }
                    else
                    {
                        header_KeHoachTungThang header = new header_KeHoachTungThang();
                        header.MaPhongBan = departmentID;
                        header.ThangKeHoach = month;
                        header.NamKeHoach = year;
                        header.SoNgayLamViec = 26;
                        db.header_KeHoachTungThang.Add(header);
                        db.SaveChanges();
                        var HearderID = db.header_KeHoachTungThang.Where(x => x.MaPhongBan == departmentID && x.ThangKeHoach == month && x.NamKeHoach == year).Select(x => x.HeaderID).FirstOrDefault();
                        return Json(new { data = listAspect, aspects = listAspectDepartments, totalDays = (26), headerID = HearderID }, JsonRequestBehavior.AllowGet);

                    }
                    return Json(new { data = listAspect, aspects = listAspectDepartments, totalDays = (listAspect == null ? 0 : listAspect[0].SoNgayLamViec), headerID = listAspect == null ? -1 : listAspect[0].HeaderID }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                Response.Write("Có lỗi xảy ra, xin vui lòng nhập lại");
                return new HttpStatusCodeResult(400);
            }
        }
        //
        [Route("phong-kcm/ke-hoach-san-xuat/cap-nhat-thong-tin")]
        public ActionResult UpdteInformation()
        {
            var month = Int32.Parse(Request["month"]);
            var year = Int32.Parse(Request["year"]);
            var departmentID = Request["department"];
            var headerID = Int32.Parse(Request["headerID"]);
            var totalDays = Int32.Parse(Request["totalDays"]);
            var data = Request["data"];

            List<KeHoach_TieuChi_TheoThang> listUpdate = JsonConvert.DeserializeObject<List<KeHoach_TieuChi_TheoThang>>(data);
            using (QUANGHANHABCEntities db = new QUANGHANHABCEntities())
            {
                var listAspectDepartments = (from pbtc in db.PhongBan_TieuChi
                            .Where(x => x.MaPhongBan == departmentID)
                                             join tieuchi in db.TieuChis on pbtc.MaTieuChi equals tieuchi.MaTieuChi
                                             select new
                                             {
                                                 MaTieuChi = tieuchi.MaTieuChi,
                                                 TenTieuChi = tieuchi.TenTieuChi
                                             }).ToList();

                DateTime currentTime = DateTime.Now;
                db.Configuration.ValidateOnSaveEnabled = true;
                foreach (var item in listUpdate)
                {
                    item.ThoiGianNhapCuoiCung = currentTime;

                    db.KeHoach_TieuChi_TheoThang.Add(item);
                }
                //var header = db.header_KeHoachTungThang.Where(x => x.MaPhongBan == departmentID && x.ThangKeHoach == month && x.NamKeHoach == year).FirstOrDefault();
                //header.SoNgayLamViec = totalDays;
                //db.Entry(header).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var listAspect = GetData(month, year, departmentID);
                if (listAspect.Count != 0)
                {
                    foreach (var item in listAspect)
                    {
                        item.Identify = item.MaTieuChiNull + "-" + item.HeaderID;
                    }
                }
                //
                return Json(new { data = listAspect, aspects = listAspectDepartments });
            }
        }
        [Route("phong-kcm/ke-hoach-san-xuat/returnunit")]
        [HttpPost]
        public JsonResult returnUnit(int MaTieuChi)
        {

            try
            {
                QUANGHANHABCEntities db = new QUANGHANHABCEntities();
                var ma = db.TieuChis.Where(x => x.MaTieuChi == MaTieuChi).SingleOrDefault();
                //String item = equipment.supply_name + "^" + equipment.unit;
                return Json(new
                {
                    DonViDo = ma.DonViDo
                }, JsonRequestBehavior.AllowGet); ;
            }
            catch (Exception)
            {
                return Json("Mã tiêu chí không tồn tại", JsonRequestBehavior.AllowGet);
            }

        }
    }
    public class ChiTietTieuChi : KeHoach_TieuChi_TheoThang
    {
        public Nullable<int> MaTieuChiNull { get; set; }
        public string DonViDo { get; set; }
        public string TenTieuChi { get; set; }

        public int SoNgayLamViec { get; set; }

        public string Identify { get; set; }
    }
}