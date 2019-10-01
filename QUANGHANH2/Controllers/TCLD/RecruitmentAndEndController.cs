﻿using OfficeOpenXml;
using QUANGHANH2.Models;
using System;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Linq.Dynamic;
using System.Web.Mvc;
using System.Web.Routing;

namespace QUANGHANHCORE.Controllers.TCLD
{
    public class RecruitmentAndEndController : Controller
    {

        public class TDCDModel
        {
            public int Stt { get; set; }
            public string TenPhongBan { get; set; }
            public int TongTuyenDungCoDien { get; set; }
            public int TongChamDutCoDien { get; set; }
            public int TongTuyenDungKhaiThac { get; set; }
            public int TongChamDutKhaiThac { get; set; }
            public int TongTuyenDung { get; set; }
            public int TongChamDut { get; set; }
            public string GhiChu { get; set; }
            public int ChenhLech { get; set; }
            public TDCDModel()
            {
                Stt = 1;
                TenPhongBan = String.Empty;
                TongTuyenDungCoDien = 0;
                TongChamDutCoDien = 0;
                TongTuyenDungKhaiThac = 0;
                TongChamDutKhaiThac = 0;
                TongTuyenDung = 0;
                TongChamDut = 0;
                GhiChu = String.Empty;
                ChenhLech = 0;
            }
        }

        public class CountModel
        {
            public string TenPhongBan { get; set; }
            public int count { get; set; }
        }

        [Route("phong-tcld/cham-dut-va-tuyen-dung/tong-hop-cac-don-vi-cham-dut-tuyen-dung")]
        public ActionResult RecruitmentAndEnd()
        {
            ViewBag.nameDepartment = "baocao-sanluon-laodong";
            return View("/Views/TCLD/ReportRecruitmentAndEnd/RecruitmentAndEnd.cshtml");
        }

        [HttpPost]
        public ActionResult GetAll(int year = 0)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            if (year ==0 )
            {
                using (QUANGHANHABCEntities db = new QUANGHANHABCEntities())
                {
                    // them phan check nam vao day
                    var arrTuyenDungCoDien = ((from cn in db.TuyenDung_NhanVien
                                               join
                                                 n in db.NhanViens on
                                                 cn.MaNV equals n.MaNV
                                               where n.LoaiNhanVien.Equals("CNCD")
                                               join d in db.Departments on
                                               n.MaPhongBan equals d.department_id
                                               into tb1
                                               from tb2 in tb1.DefaultIfEmpty()
                                               group tb2 by tb2.department_name into groupted
                                               select new CountModel { TenPhongBan = groupted.Key, count = groupted.Count() })
                                              ).ToList();
                    var arrTuyenDungKhaiThac = ((from cn in db.TuyenDung_NhanVien
                                                 join
                                               n in db.NhanViens on
                                               cn.MaNV equals n.MaNV
                                                 where n.LoaiNhanVien.Equals("CNKT")
                                                 join d in db.Departments on
                                                 n.MaPhongBan equals d.department_id
                                                 into tb1
                                                 from tb2 in tb1.DefaultIfEmpty()
                                                 group tb2 by tb2.department_name into groupted
                                                 select new CountModel { TenPhongBan = groupted.Key, count = groupted.Count() })
                                              ).ToList();
                    var arrChamDutCoDien = ((from cn in db.ChamDut_NhanVien
                                             join
                                             n in db.NhanViens on
                                             cn.MaNV equals n.MaNV
                                             where n.LoaiNhanVien.Equals("CNCD")
                                             join d in db.Departments on
                                             n.MaPhongBan equals d.department_id
                                             into tb1
                                             from tb2 in tb1.DefaultIfEmpty()
                                             group tb2 by tb2.department_name into groupted
                                             select new CountModel { TenPhongBan = groupted.Key, count = groupted.Count() })
                                              ).ToList();
                    var arrChamDutKhaiThac = ((from cn in db.ChamDut_NhanVien
                                               join
                                               n in db.NhanViens on
                                               cn.MaNV equals n.MaNV
                                               where n.LoaiNhanVien.Equals("CNKT")
                                               join d in db.Departments on
                                               n.MaPhongBan equals d.department_id
                                               into tb1
                                               from tb2 in tb1.DefaultIfEmpty()
                                               group tb2 by tb2.department_name into groupted
                                               select new CountModel { TenPhongBan = groupted.Key, count = groupted.Count() })
                                              ).ToList();
                    var arrPhongBan = db.Departments.Select(p => new TDCDModel
                    {
                        TenPhongBan = p.department_name,
                        Stt = 1,
                        TongTuyenDungCoDien = 0,
                        TongChamDutCoDien = 0,
                        TongTuyenDungKhaiThac = 0,
                        TongChamDutKhaiThac = 0,
                        TongTuyenDung = 0,
                        TongChamDut = 0,
                        GhiChu = String.Empty,
                        ChenhLech = 0,
                    }).ToList();
                    int stt = 1;
                    int tongCD = 0, tongTD = 0, tongCDCD = 0, tongCDKT = 0, tongTDCD = 0, tongTDKT = 0;
                    foreach (var phongBan in arrPhongBan)
                    {
                        phongBan.Stt = stt++;
                        foreach (var tdcd in arrTuyenDungCoDien)
                        {
                            if (tdcd.TenPhongBan.Equals(phongBan.TenPhongBan))
                            {
                                phongBan.TongTuyenDungCoDien = tdcd.count;
                                phongBan.TongTuyenDung += tdcd.count;
                                break;
                            }
                        }
                        foreach (var tdkt in arrTuyenDungKhaiThac)
                        {
                            if (tdkt.TenPhongBan.Equals(phongBan.TenPhongBan))
                            {
                                phongBan.TongTuyenDungKhaiThac = tdkt.count;
                                phongBan.TongTuyenDung += tdkt.count;
                                break;
                            }
                        }
                        foreach (var cdcd in arrChamDutCoDien)
                        {
                            if (cdcd.TenPhongBan.Equals(phongBan.TenPhongBan))
                            {
                                phongBan.TongChamDutCoDien = cdcd.count;
                                phongBan.TongChamDut += cdcd.count;
                                break;
                            }
                        }
                        foreach (var cdkt in arrChamDutKhaiThac)
                        {
                            if (cdkt.TenPhongBan.Equals(phongBan.TenPhongBan))
                            {
                                phongBan.TongChamDutKhaiThac = cdkt.count;
                                phongBan.TongChamDut += cdkt.count;
                                break;
                            }
                        }
                        // them tong cham dut va tuyen dung cua tat ca cac phong ban
                        phongBan.ChenhLech = phongBan.TongTuyenDung - phongBan.TongChamDut;
                        tongTD += phongBan.TongTuyenDung;
                        tongCD += phongBan.TongChamDut;
                        tongCDCD += phongBan.TongChamDutCoDien;
                        tongCDKT += phongBan.TongChamDutKhaiThac;
                        tongTDCD += phongBan.TongTuyenDungCoDien;
                        tongTDKT = phongBan.TongTuyenDungKhaiThac;
                    }

                   // row cuối cùng trong dtable để tính tổng
                    TDCDModel totalModel = new TDCDModel
                    {
                        Stt = stt,
                        TenPhongBan = "Tổng cộng",
                        TongTuyenDung = tongTD,
                        TongTuyenDungCoDien = tongTDCD,
                        TongTuyenDungKhaiThac = tongTDKT,
                        TongChamDut = tongCD,
                        TongChamDutCoDien = tongCDCD,
                        TongChamDutKhaiThac = tongCDKT
                    };

                    totalModel.ChenhLech = totalModel.TongTuyenDung - totalModel.TongChamDut;

                    int totalrows = arrPhongBan.Count;
                    int totalrowsafterfiltering = arrPhongBan.Count;
                    arrPhongBan = arrPhongBan.Skip(start).Take(length).ToList<TDCDModel>();
                    arrPhongBan = arrPhongBan.OrderBy(sortColumnName + " " + sortDirection).ToList<TDCDModel>();
                    arrPhongBan.Add(totalModel);
                    return Json(new { success = true, data = arrPhongBan, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }
            }
            else // khi tìm kiếm bằng năm
            {
                using (QUANGHANHABCEntities db = new QUANGHANHABCEntities())
                {
                    // them phan check nam vao day
                    var arrTuyenDungCoDien = ((from cn in db.TuyenDung_NhanVien
                                               join
                                                 n in db.NhanViens on
                                                 cn.MaNV equals n.MaNV
                                               where n.LoaiNhanVien.Equals("CNCD")
                                               && cn.NgayTuyenDung.Year == year
                                               join d in db.Departments on
                                               n.MaPhongBan equals d.department_id
                                               into tb1
                                               from tb2 in tb1.DefaultIfEmpty()
                                               group tb2 by tb2.department_name into groupted
                                               select new CountModel { TenPhongBan = groupted.Key, count = groupted.Count() })
                                              ).ToList();
                    var arrTuyenDungKhaiThac = ((from cn in db.TuyenDung_NhanVien
                                                 join
                                               n in db.NhanViens on
                                               cn.MaNV equals n.MaNV
                                                 where n.LoaiNhanVien.Equals("CNKT")
                                                 && cn.NgayTuyenDung.Year == year
                                                 join d in db.Departments on
                                                 n.MaPhongBan equals d.department_id
                                                 into tb1
                                                 from tb2 in tb1.DefaultIfEmpty()
                                                 group tb2 by tb2.department_name into groupted
                                                 select new CountModel { TenPhongBan = groupted.Key, count = groupted.Count() })
                                              ).ToList();
                    var arrChamDutCoDien = ((from cn in db.ChamDut_NhanVien
                                             join
                                             n in db.NhanViens on
                                             cn.MaNV equals n.MaNV
                                             where n.LoaiNhanVien.Equals("CNCD")
                                             && cn.NgayChamDut.Year == year
                                             join d in db.Departments on
                                             n.MaPhongBan equals d.department_id
                                             into tb1
                                             from tb2 in tb1.DefaultIfEmpty()
                                             group tb2 by tb2.department_name into groupted
                                             select new CountModel { TenPhongBan = groupted.Key, count = groupted.Count() })
                                              ).ToList();
                    var arrChamDutKhaiThac = ((from cn in db.ChamDut_NhanVien
                                               join
                                               n in db.NhanViens on
                                               cn.MaNV equals n.MaNV
                                               where n.LoaiNhanVien.Equals("CNKT")
                                               && cn.NgayChamDut.Year == year
                                               join d in db.Departments on
                                               n.MaPhongBan equals d.department_id
                                               into tb1
                                               from tb2 in tb1.DefaultIfEmpty()
                                               group tb2 by tb2.department_name into groupted
                                               select new CountModel { TenPhongBan = groupted.Key, count = groupted.Count() })
                                              ).ToList();

                    var arrPhongBan = db.Departments.Select(p => new TDCDModel
                    {
                        TenPhongBan = p.department_name,
                        Stt = 1,
                        TongTuyenDungCoDien = 0,
                        TongChamDutCoDien = 0,
                        TongTuyenDungKhaiThac = 0,
                        TongChamDutKhaiThac = 0,
                        TongTuyenDung = 0,
                        TongChamDut = 0,
                        GhiChu = String.Empty,
                        ChenhLech = 0,
                    }).ToList();
                    int stt = 1;
                    int tongCD =0 , tongTD = 0 ,tongCDCD = 0,tongCDKT =0,tongTDCD =0,tongTDKT =0;
                    foreach (var phongBan in arrPhongBan)
                    {
                        phongBan.Stt = stt++;
                        foreach (var tdcd in arrTuyenDungCoDien)
                        {
                            if (tdcd.TenPhongBan.Equals(phongBan.TenPhongBan))
                            {
                                phongBan.TongTuyenDungCoDien = tdcd.count;
                                phongBan.TongTuyenDung += tdcd.count;
                                break;
                            }
                        }
                        foreach (var tdkt in arrTuyenDungKhaiThac)
                        {
                            if (tdkt.TenPhongBan.Equals(phongBan.TenPhongBan))
                            {
                                phongBan.TongTuyenDungKhaiThac = tdkt.count;
                                phongBan.TongTuyenDung += tdkt.count;
                                break;
                            }
                        }
                        foreach (var cdcd in arrChamDutCoDien)
                        {
                            if (cdcd.TenPhongBan.Equals(phongBan.TenPhongBan))
                            {
                                phongBan.TongChamDutCoDien = cdcd.count;
                                phongBan.TongChamDut += cdcd.count;
                                break;
                            }
                        }
                        foreach (var cdkt in arrChamDutKhaiThac)
                        {
                            if (cdkt.TenPhongBan.Equals(phongBan.TenPhongBan))
                            {
                                phongBan.TongChamDutKhaiThac = cdkt.count;
                                phongBan.TongChamDut += cdkt.count;
                                break;
                            }
                        }
                        // them tong cham dut va tuyen dung cua tat ca cac phong ban
                        phongBan.ChenhLech = phongBan.TongTuyenDung - phongBan.TongChamDut;
                        tongTD += phongBan.TongTuyenDung;
                        tongCD += phongBan.TongChamDut;
                        tongCDCD += phongBan.TongChamDutCoDien;
                        tongCDKT += phongBan.TongChamDutKhaiThac;
                        tongTDCD += phongBan.TongTuyenDungCoDien;
                        tongTDKT = phongBan.TongTuyenDungKhaiThac;
                    }
                    // row cuối cùng trong dtable để tính tổng
                    TDCDModel totalModel = new TDCDModel
                    {
                        Stt = stt,
                        TenPhongBan = "Tổng cộng",
                        TongTuyenDung = tongTD,
                        TongTuyenDungCoDien = tongTDCD,
                        TongTuyenDungKhaiThac = tongTDKT,
                        TongChamDut = tongCD,
                        TongChamDutCoDien = tongCDCD,
                        TongChamDutKhaiThac = tongCDKT
                    };

                    totalModel.ChenhLech = totalModel.TongTuyenDung - totalModel.TongChamDut;

                    int totalrows = arrPhongBan.Count;
                    int totalrowsafterfiltering = arrPhongBan.Count;
                    arrPhongBan = arrPhongBan.Skip(start).Take(length).ToList<TDCDModel>();
                    arrPhongBan = arrPhongBan.OrderBy(sortColumnName + " " + sortDirection).ToList<TDCDModel>();
                    arrPhongBan.Add(totalModel);
                    return Json(new { success = true, data = arrPhongBan, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public void ExportExcel(int year = 0)
        {
            string path = HostingEnvironment.MapPath("/excel/TCLD/CD_TD_Report.xlsx");
            FileInfo file = new FileInfo(path);

            using (ExcelPackage excelPackage = new ExcelPackage(file))
            {
                ExcelWorkbook excelWorkbook = excelPackage.Workbook;
                ExcelWorksheet excelWorksheet = excelWorkbook.Worksheets.First();

                using (QUANGHANHABCEntities db = new QUANGHANHABCEntities())
                {
                   if(year == 0)
                    {
                        var arrTuyenDungCoDien = ((from cn in db.TuyenDung_NhanVien
                                                   join
                                                   n in db.NhanViens on
                                                   cn.MaNV equals n.MaNV
                                                   where n.LoaiNhanVien.Equals("CNCD")
                                                   join d in db.Departments on
                                                   n.MaPhongBan equals d.department_id
                                                   into tb1
                                                   from tb2 in tb1.DefaultIfEmpty()
                                                   group tb2 by tb2.department_name into groupted
                                                   select new CountModel { TenPhongBan = groupted.Key, count = groupted.Count() })
                                                  ).ToList();
                        var arrTuyenDungKhaiThac = ((from cn in db.TuyenDung_NhanVien
                                                     join
                                                   n in db.NhanViens on
                                                   cn.MaNV equals n.MaNV
                                                     where n.LoaiNhanVien.Equals("CNKT")
                                                     join d in db.Departments on
                                                     n.MaPhongBan equals d.department_id
                                                     into tb1
                                                     from tb2 in tb1.DefaultIfEmpty()
                                                     group tb2 by tb2.department_name into groupted
                                                     select new CountModel { TenPhongBan = groupted.Key, count = groupted.Count() })
                                                  ).ToList();
                        var arrChamDutCoDien = ((from cn in db.ChamDut_NhanVien
                                                 join
                                                 n in db.NhanViens on
                                                 cn.MaNV equals n.MaNV
                                                 where n.LoaiNhanVien.Equals("CNCD")
                                                 join d in db.Departments on
                                                 n.MaPhongBan equals d.department_id
                                                 into tb1
                                                 from tb2 in tb1.DefaultIfEmpty()
                                                 group tb2 by tb2.department_name into groupted
                                                 select new CountModel { TenPhongBan = groupted.Key, count = groupted.Count() })
                                                  ).ToList();
                        var arrChamDutKhaiThac = ((from cn in db.ChamDut_NhanVien
                                                   join
                                                   n in db.NhanViens on
                                                   cn.MaNV equals n.MaNV
                                                   where n.LoaiNhanVien.Equals("CNKT")
                                                   join d in db.Departments on
                                                   n.MaPhongBan equals d.department_id
                                                   into tb1
                                                   from tb2 in tb1.DefaultIfEmpty()
                                                   group tb2 by tb2.department_name into groupted
                                                   select new CountModel { TenPhongBan = groupted.Key, count = groupted.Count() })
                                                  ).ToList();
                        var arrPhongBan = db.Departments.Select(p => new TDCDModel
                        {
                            TenPhongBan = p.department_name,
                            Stt = 1,
                            TongTuyenDungCoDien = 0,
                            TongChamDutCoDien = 0,
                            TongTuyenDungKhaiThac = 0,
                            TongChamDutKhaiThac = 0,
                            TongTuyenDung = 0,
                            TongChamDut = 0,
                            GhiChu = String.Empty,
                            ChenhLech = 0,
                        }).ToList();
                        int stt = 1;
                        int tongCD = 0, tongTD = 0, tongCDCD = 0, tongCDKT = 0, tongTDCD = 0, tongTDKT = 0;
                        foreach (var phongBan in arrPhongBan)
                        {
                            phongBan.Stt = stt++;
                            foreach (var tdcd in arrTuyenDungCoDien)
                            {
                                if (tdcd.TenPhongBan.Equals(phongBan.TenPhongBan))
                                {
                                    phongBan.TongTuyenDungCoDien = tdcd.count;
                                    phongBan.TongTuyenDung += tdcd.count;
                                    break;
                                }
                            }
                            foreach (var tdkt in arrTuyenDungKhaiThac)
                            {
                                if (tdkt.TenPhongBan.Equals(phongBan.TenPhongBan))
                                {
                                    phongBan.TongTuyenDungKhaiThac = tdkt.count;
                                    phongBan.TongTuyenDung += tdkt.count;
                                    break;
                                }
                            }
                            foreach (var cdcd in arrChamDutCoDien)
                            {
                                if (cdcd.TenPhongBan.Equals(phongBan.TenPhongBan))
                                {
                                    phongBan.TongChamDutCoDien = cdcd.count;
                                    phongBan.TongChamDut += cdcd.count;
                                    break;
                                }
                            }
                            foreach (var cdkt in arrChamDutKhaiThac)
                            {
                                if (cdkt.TenPhongBan.Equals(phongBan.TenPhongBan))
                                {
                                    phongBan.TongChamDutKhaiThac = cdkt.count;
                                    phongBan.TongChamDut += cdkt.count;
                                    break;
                                }
                            }
                            phongBan.ChenhLech = phongBan.TongTuyenDung - phongBan.TongChamDut;
                            tongTD += phongBan.TongTuyenDung;
                            tongCD += phongBan.TongChamDut;
                            tongCDCD += phongBan.TongChamDutCoDien;
                            tongCDKT += phongBan.TongChamDutKhaiThac;
                            tongTDCD += phongBan.TongTuyenDungCoDien;
                            tongTDKT = phongBan.TongTuyenDungKhaiThac;
                        }

                        TDCDModel totalModel = new TDCDModel
                        {
                            Stt = stt,
                            TenPhongBan = "Tổng cộng",
                            TongTuyenDung = tongTD,
                            TongTuyenDungCoDien = tongTDCD,
                            TongTuyenDungKhaiThac = tongTDKT,
                            TongChamDut = tongCD,
                            TongChamDutCoDien = tongCDCD,
                            TongChamDutKhaiThac = tongCDKT
                        };
                        arrPhongBan.Add(totalModel);
                        int k = 0;
                        int totalCD = 0;
                        int totalTD = 0;
                        for (int i = 3; i < arrPhongBan.Count + 3; i++)
                        {
                            excelWorksheet.Cells[i, 1].Value = arrPhongBan.ElementAt(k).Stt;
                            excelWorksheet.Cells[i, 2].Value = arrPhongBan.ElementAt(k).TenPhongBan;
                            excelWorksheet.Cells[i, 3].Value = arrPhongBan.ElementAt(k).TongChamDut;
                            excelWorksheet.Cells[i, 4].Value = arrPhongBan.ElementAt(k).TongChamDutKhaiThac;
                            excelWorksheet.Cells[i, 5].Value = arrPhongBan.ElementAt(k).TongChamDutCoDien;
                            excelWorksheet.Cells[i, 6].Value = arrPhongBan.ElementAt(k).TongTuyenDung;
                            excelWorksheet.Cells[i, 7].Value = arrPhongBan.ElementAt(k).TongTuyenDungKhaiThac;
                            excelWorksheet.Cells[i, 8].Value = arrPhongBan.ElementAt(k).TongTuyenDungCoDien;
                            excelWorksheet.Cells[i, 9].Value = arrPhongBan.ElementAt(k).ChenhLech;
                            excelWorksheet.Cells[i, 10].Value = arrPhongBan.ElementAt(k).GhiChu;
                            totalCD += arrPhongBan.ElementAt(k).TongChamDut;
                            totalTD += arrPhongBan.ElementAt(k).TongTuyenDung;
                            k++;
                        }

                        string location = HostingEnvironment.MapPath("/excel/TCLD/download");
                        excelPackage.SaveAs(new FileInfo(location + "/CD_TD_Report_Total.xlsx"));
                    }
                    else
                    {
                        var arrTuyenDungCoDien = ((from cn in db.TuyenDung_NhanVien
                                                   join
                                                   n in db.NhanViens on
                                                   cn.MaNV equals n.MaNV
                                                   where n.LoaiNhanVien.Equals("CNCD")
                                                   && cn.NgayTuyenDung.Year == year
                                                   join d in db.Departments on
                                                   n.MaPhongBan equals d.department_id
                                                   into tb1
                                                   from tb2 in tb1.DefaultIfEmpty()
                                                   group tb2 by tb2.department_name into groupted
                                                   select new CountModel { TenPhongBan = groupted.Key, count = groupted.Count() })
                                                  ).ToList();
                        var arrTuyenDungKhaiThac = ((from cn in db.TuyenDung_NhanVien
                                                     join
                                                   n in db.NhanViens on
                                                   cn.MaNV equals n.MaNV
                                                     where n.LoaiNhanVien.Equals("CNKT")
                                                     && cn.NgayTuyenDung.Year == year
                                                     join d in db.Departments on
                                                     n.MaPhongBan equals d.department_id
                                                     into tb1
                                                     from tb2 in tb1.DefaultIfEmpty()
                                                     group tb2 by tb2.department_name into groupted
                                                     select new CountModel { TenPhongBan = groupted.Key, count = groupted.Count() })
                                                  ).ToList();
                        var arrChamDutCoDien = ((from cn in db.ChamDut_NhanVien
                                                 join
                                                 n in db.NhanViens on
                                                 cn.MaNV equals n.MaNV
                                                 where n.LoaiNhanVien.Equals("CNCD")
                                                 && cn.NgayChamDut.Year == year
                                                 join d in db.Departments on
                                                 n.MaPhongBan equals d.department_id
                                                 into tb1
                                                 from tb2 in tb1.DefaultIfEmpty()
                                                 group tb2 by tb2.department_name into groupted
                                                 select new CountModel { TenPhongBan = groupted.Key, count = groupted.Count() })
                                                  ).ToList();
                        var arrChamDutKhaiThac = ((from cn in db.ChamDut_NhanVien
                                                   join
                                                   n in db.NhanViens on
                                                   cn.MaNV equals n.MaNV
                                                   where n.LoaiNhanVien.Equals("CNKT")
                                                   && cn.NgayChamDut.Year == year
                                                   join d in db.Departments on
                                                   n.MaPhongBan equals d.department_id
                                                   into tb1
                                                   from tb2 in tb1.DefaultIfEmpty()
                                                   group tb2 by tb2.department_name into groupted
                                                   select new CountModel { TenPhongBan = groupted.Key, count = groupted.Count() })
                                                  ).ToList();
                        var arrPhongBan = db.Departments.Select(p => new TDCDModel
                        {
                            TenPhongBan = p.department_name,
                            Stt = 1,
                            TongTuyenDungCoDien = 0,
                            TongChamDutCoDien = 0,
                            TongTuyenDungKhaiThac = 0,
                            TongChamDutKhaiThac = 0,
                            TongTuyenDung = 0,
                            TongChamDut = 0,
                            GhiChu = String.Empty,
                            ChenhLech = 0,
                        }).ToList();
                        int stt = 1;
                        int tongCD = 0, tongTD = 0, tongCDCD = 0, tongCDKT = 0, tongTDCD = 0, tongTDKT = 0;
                        foreach (var phongBan in arrPhongBan)
                        {
                            phongBan.Stt = stt++;
                            foreach (var tdcd in arrTuyenDungCoDien)
                            {
                                if (tdcd.TenPhongBan.Equals(phongBan.TenPhongBan))
                                {
                                    phongBan.TongTuyenDungCoDien = tdcd.count;
                                    phongBan.TongTuyenDung += tdcd.count;
                                    break;
                                }
                            }
                            foreach (var tdkt in arrTuyenDungKhaiThac)
                            {
                                if (tdkt.TenPhongBan.Equals(phongBan.TenPhongBan))
                                {
                                    phongBan.TongTuyenDungKhaiThac = tdkt.count;
                                    phongBan.TongTuyenDung += tdkt.count;
                                    break;
                                }
                            }
                            foreach (var cdcd in arrChamDutCoDien)
                            {
                                if (cdcd.TenPhongBan.Equals(phongBan.TenPhongBan))
                                {
                                    phongBan.TongChamDutCoDien = cdcd.count;
                                    phongBan.TongChamDut += cdcd.count;
                                    break;
                                }
                            }
                            foreach (var cdkt in arrChamDutKhaiThac)
                            {
                                if (cdkt.TenPhongBan.Equals(phongBan.TenPhongBan))
                                {
                                    phongBan.TongChamDutKhaiThac = cdkt.count;
                                    phongBan.TongChamDut += cdkt.count;
                                    break;
                                }
                            }
                            phongBan.ChenhLech = phongBan.TongTuyenDung - phongBan.TongChamDut;
                            tongTD += phongBan.TongTuyenDung;
                            tongCD += phongBan.TongChamDut;
                            tongCDCD += phongBan.TongChamDutCoDien;
                            tongCDKT += phongBan.TongChamDutKhaiThac;
                            tongTDCD += phongBan.TongTuyenDungCoDien;
                            tongTDKT = phongBan.TongTuyenDungKhaiThac;
                        }

                        TDCDModel totalModel = new TDCDModel
                        {
                            Stt = stt,
                            TenPhongBan = "Tổng cộng",
                            TongTuyenDung = tongTD,
                            TongTuyenDungCoDien = tongTDCD,
                            TongTuyenDungKhaiThac = tongTDKT,
                            TongChamDut = tongCD,
                            TongChamDutCoDien = tongCDCD,
                            TongChamDutKhaiThac = tongCDKT
                        };
                        arrPhongBan.Add(totalModel);
                        int k = 0;
                        for (int i = 3; i < arrPhongBan.Count + 3; i++)
                        {
                            excelWorksheet.Cells[i, 1].Value = arrPhongBan.ElementAt(k).Stt;
                            excelWorksheet.Cells[i, 2].Value = arrPhongBan.ElementAt(k).TenPhongBan;
                            excelWorksheet.Cells[i, 3].Value = arrPhongBan.ElementAt(k).TongChamDut;
                            excelWorksheet.Cells[i, 4].Value = arrPhongBan.ElementAt(k).TongChamDutKhaiThac;
                            excelWorksheet.Cells[i, 5].Value = arrPhongBan.ElementAt(k).TongChamDutCoDien;
                            excelWorksheet.Cells[i, 6].Value = arrPhongBan.ElementAt(k).TongTuyenDung;
                            excelWorksheet.Cells[i, 7].Value = arrPhongBan.ElementAt(k).TongTuyenDungKhaiThac;
                            excelWorksheet.Cells[i, 8].Value = arrPhongBan.ElementAt(k).TongTuyenDungCoDien;
                            excelWorksheet.Cells[i, 9].Value = arrPhongBan.ElementAt(k).ChenhLech;
                            excelWorksheet.Cells[i, 10].Value = arrPhongBan.ElementAt(k).GhiChu;
                            k++;
                        }
                        string location = HostingEnvironment.MapPath("/excel/TCLD/download");
                        excelPackage.SaveAs(new FileInfo(location + "/CD_TD_Report_"+year+".xlsx"));
                    }
                }

            }

        }

        [Route("phong-tcld/cham-dut-va-tuyen-dung/tong-hop-tuyen-dung")]
        public ActionResult Recruitment()
        {
            ViewBag.nameDepartment = "baocao-sanluon-laodong";
            return View("/Views/TCLD/ReportRecruitmentAndEnd/Recruitment.cshtml");
        }

        [Route("phong-tcld/cham-dut-va-tuyen-dung/tong-hop-cham-dut")]
        public ActionResult End()
        {
            ViewBag.nameDepartment = "baocao-sanluon-laodong";
            return View("/Views/TCLD/ReportRecruitmentAndEnd/End.cshtml");
        }

        [Route("phong-tcld/cham-dut-va-tuyen-dung/tang-giam-lao-dong")]
        public ActionResult ListFrequency()
        {
            ViewBag.nameDepartment = "baocao-sanluon-laodong";
            return View("/Views/TCLD/ReportRecruitmentAndEnd/ListFrequency.cshtml");
        }
        [Route("phong-tcld/cham-dut-va-tuyen-dung/tang-giam-lao-dong/theo-quy")]
        public ActionResult Frequency()
        {
            ViewBag.nameDepartment = "baocao-sanluon-laodong";
            return View("/Views/TCLD/ReportRecruitmentAndEnd/Frequency.cshtml");
        }
    }
}