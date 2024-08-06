using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TodoList.Models;

namespace TodoList.Controllers
{
    public class CongViecController : Controller
    {
        // GET: CongViec
        private QL_CONGVIEC db = new QL_CONGVIEC();
        public async Task<ActionResult> Index(CONGVIEC congViec)
        {
            if (Session["IsChecked"] == null)
            {
                return RedirectToAction("Index", "TaiKhoan");
            }
            string tenDangNhap = Session["tenDangNhap"].ToString();
            string maTaiKhoan = db.TAIKHOANs.Where(t => t.TENTAIKHOAN == tenDangNhap).FirstOrDefault().MATAIKHOAN;
            ViewBag.Title = "Công việc";
            ViewBag.TitlePage = "Tất cả";
            var list = await GetAllCongViec(maTaiKhoan);
            if (list != null)
            {
                ViewBag.KhongCoCongViec = false;
                return View(list);
            }
            ViewBag.KhongCoCongViec = true;
            return View();
        }

        private async Task<List<CONGVIEC>> GetAllCongViec(string maTaiKhoan)
        {
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
                Request.ApplicationPath.TrimEnd('/') + "/";
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage res = await httpClient.GetAsync(baseUrl + "api/CONGVIECs/GetCongViecTheoTaiKhoan?maTaiKhoan=" + maTaiKhoan);
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //List<CONGVIEC> list = new List<CONGVIEC>();
                    //list = res.Content.ReadAsAsync<List<CONGVIEC>>().Result;
                    List<CONGVIEC> list = res.Content.ReadAsAsync<List<CONGVIEC>>().Result;
                    //var singleItem = res.Content.ReadAsAsync<CONGVIEC>().Result;
                    //var list = new List<CONGVIEC> { singleItem };
                    return list;
                }
                return null;
            }
        }
        private async Task<List<CONGVIEC>> GetCongViecByLoaiAsync(string maLoai)
        {
            string tenDangNhap = Session["tenDangNhap"].ToString();
            string maTaiKhoan = db.TAIKHOANs.Where(t => t.TENTAIKHOAN == tenDangNhap).FirstOrDefault().MATAIKHOAN;
            string baseUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}{Request.ApplicationPath.TrimEnd('/')}/";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}api/CONGVIECs?MALOAI={maLoai}&maTaiKhoan={maTaiKhoan}");
                    if (response.IsSuccessStatusCode)
                    {
                        List<CONGVIEC> list = await response.Content.ReadAsAsync<List<CONGVIEC>>();
                        return list;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching data: {ex.Message}");
                }

                return null;
            }
        }

        public async Task<ActionResult> ViewCongViecTheoLoai(string maLoai)
        {
            string tenLoai = db.LOAICONGVIECs.Where(t => t.MALOAI == maLoai).Select(t => t.TENLOAI).FirstOrDefault();
            ViewBag.TitlePage = tenLoai;
            var list = await GetCongViecByLoaiAsync(maLoai);
            if (list != null)
            {
                ViewBag.KhongCoCongViec = false;
                return View(list);
            }
            ViewBag.KhongCoCongViec = true;
            return View();
        }
        private string tuDongTangMaCongViec()
        {
            var maxMaCongViec = db.CONGVIECs
            .OrderByDescending(t => t.MACONGVIEC)
            .Select(t => t.MACONGVIEC)
            .FirstOrDefault();

            if (string.IsNullOrEmpty(maxMaCongViec))
            {
                return "CV01";
            }

            var numberPart = maxMaCongViec.Substring(2);
            if (int.TryParse(numberPart, out int number))
            {
                return "CV" + (number + 1).ToString();
            }
            throw new Exception("Không tạo được công việc  !!!");
        }
        public async Task<ActionResult> ThemCongViec(CONGVIEC congViec)
        {
            ViewBag.MALOAIList = db.LOAICONGVIECs.Select(l => new SelectListItem
            {
                Value = l.MALOAI.ToString(),
                Text = l.TENLOAI
            }).ToList();
            if (congViec.TENCONGVIEC == null)
            {
                return View(congViec);
            }
            string tenDangNhap = Session["tenDangNhap"]?.ToString();
            string maTaiKhoan = db.TAIKHOANs
                .Where(t => t.TENTAIKHOAN == tenDangNhap)
                .FirstOrDefault()?.MATAIKHOAN;

            if (maTaiKhoan == null)
            {
                ModelState.AddModelError("", "Không tìm thấy tài khoản.");
                return View(congViec);
            }

            congViec.MACONGVIEC = tuDongTangMaCongViec();
            congViec.MATAIKHOAN = maTaiKhoan;
            congViec.NGAYTAOCONGVIEC = DateTime.Now;
            congViec.NGAYCAPNHATCONGVIEC = DateTime.Now;

            using (var httpClient = new HttpClient())
            {
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
                    Request.ApplicationPath.TrimEnd('/') + "/";
                HttpResponseMessage res = await httpClient.PostAsJsonAsync(baseUrl + "api/CONGVIECs/CreateCongViec", congViec);

                if (res.IsSuccessStatusCode)
                {
                    
                    return RedirectToAction("Index", "CongViec");
                }
                else
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi tạo công việc.");
                    return View(congViec);
                }
            }
        }
        public async Task<ActionResult> DeleteCongViec(string maCongViec)
        {
            using (var httpClient = new HttpClient())
            {
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
                    Request.ApplicationPath.TrimEnd('/') + "/";
                HttpResponseMessage res = await httpClient.DeleteAsync($"{baseUrl}api/CONGVIECs/DeleteCongViec?maCongViec={maCongViec}");


                if (res.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Xóa công việc thành công.";
                }
                else
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi xoá công việc.");
                }
            }
            return RedirectToAction("Index", "CongViec");
        }
        public async Task<ActionResult> UpdateCongViec(string maCongViec, CONGVIEC congViec)
        {
            CONGVIEC congViecChuaUpdate = db.CONGVIECs.Where(t => t.MACONGVIEC == maCongViec).FirstOrDefault();
            ViewBag.MALOAIList = db.LOAICONGVIECs.Select(l => new SelectListItem
            {
                Value = l.MALOAI.ToString(),
                Text = l.TENLOAI
            }).ToList();
            if (congViec.TENCONGVIEC == null)
            {
                return View(congViecChuaUpdate);
            }
            if (!ModelState.IsValid)
            {
                return View(congViecChuaUpdate);
            }

            using (var httpClient = new HttpClient())
            {
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
                    Request.ApplicationPath.TrimEnd('/') + "/";
                var jsonContent = new StringContent(JsonConvert.SerializeObject(congViec), Encoding.UTF8, "application/json");

                HttpResponseMessage res = await httpClient.PutAsync($"{baseUrl}api/CONGVIECs/UpdateCongViec?maCongViec={maCongViec}", jsonContent);

                if (res.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Sửa thành công";
                }
                else
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi sửa công việc.");

                    ViewBag.MALOAIList = db.LOAICONGVIECs.Select(l => new SelectListItem
                    {
                        Value = l.MALOAI.ToString(),
                        Text = l.TENLOAI
                    }).ToList();

                    return View(congViec);
                }
            }
            return RedirectToAction("Index", "CongViec");
        }

    }
}