using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TodoList
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "CongViec", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "XemThongTin",
                url: "{controller}/{action}/{tenDangNhap}",
                defaults: new { controller = "TaiKhoan", action = "XemThongTin", tenDangNhap = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CreateLoaiCongViec",
                url: "{controller}/{action}/{maTaiKhoan}",
                defaults: new { controller = "LOAICONGVIECs", action = "CreateLoaiCongViec", maTaiKhoan = UrlParameter.Optional }
            );





        }
    }
}

