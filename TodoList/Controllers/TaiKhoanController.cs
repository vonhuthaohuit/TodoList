using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TodoList.Models;

namespace TodoList.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly string baseUrl = "https://localhost:44397/api/taikhoans/";
        private QL_CONGVIEC db = new QL_CONGVIEC();

        // GET: TaiKhoan
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> CallLoginAPI(string tenDangNhapLogin, string matKhauLogin)
        {
            using (var httpClient = new HttpClient())
            {
                var loginModel = new TAIKHOAN
                {
                    TENTAIKHOAN = tenDangNhapLogin,
                    MATKHAU = matKhauLogin
                };

                var response = await httpClient.GetAsync(baseUrl + $"Login?tenDangNhapLogin={tenDangNhapLogin}&matKhauLogin={matKhauLogin}");

                if (response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    bool check = true;
                    Session["IsChecked"] = check;
                    Session["tenDangNhap"] = tenDangNhapLogin;
                    return RedirectToAction("Index", "CongViec");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = "Sai tài khoản hoặc mật khẩu vui lòng kiểm tra lại !!!";
                    return RedirectToAction("Index"); 
                }
            }
        }
        public async Task<ActionResult> CallRegisterAPI(string tenDangNhapDangKy, string matKhauDangKy, string xacNhanMatKhau)
        {

            if (!matKhauDangKy.Equals(xacNhanMatKhau))
            {
                TempData["ErrorMessage"] = "Mật khẩu không trùng khớp vui lòng thử lại !!!";
                return RedirectToAction("Index");
            }

            var registerModel = new TAIKHOAN
            {
                TENTAIKHOAN = tenDangNhapDangKy,
                 MATKHAU = matKhauDangKy
            };

            var json = JsonConvert.SerializeObject(registerModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.BaseAddress = new Uri(baseUrl);

                    var response = await httpClient.PostAsync("Register", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var message = await response.Content.ReadAsStringAsync();
                        Session["IsChecked"] = true;
                        Session["tenDangNhap"] = tenDangNhapDangKy;
                        return RedirectToAction("Index", "CongViec");
                    }
                    else
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        TempData["ErrorMessage"] = "Không tạo được tài khoản !!!";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra trong quá trình đăng ký. Vui lòng thử lại sau.";
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> XemThongTin()
        {
            string tenDangNhap = Session["tenDangNhap"]?.ToString();
            if (string.IsNullOrEmpty(tenDangNhap))
            {
                return RedirectToAction("Index");
            }

            var taiKhoan = db.TAIKHOANs.FirstOrDefault(t => t.TENTAIKHOAN == tenDangNhap);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }

            string maTaiKhoan = taiKhoan.MATAIKHOAN;

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{baseUrl}/XemThongTin?maTaiKhoan={maTaiKhoan}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsAsync<TAIKHOAN>().Result;
                    var list = new List<TAIKHOAN> { jsonString };
                    return View(list);
                }
            }

            return View("Error");
        }

        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("Index", "TaiKhoan");
        }

    }
}
