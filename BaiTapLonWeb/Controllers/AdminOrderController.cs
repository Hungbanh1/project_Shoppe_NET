using BaiTapLonWeb.Models;
using BaiTapLonWeb.Services;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Mvc;
using static System.Web.Razor.Parser.SyntaxConstants;

namespace BaiTapLonWeb.Controllers
{

    public class AdminOrderController : Controller
    {
        private ShopeeEntities db = new ShopeeEntities();
        // GET: AdminOrder
        public ActionResult Index(string status)
        {

            // Bắt đầu với các đơn hàng của người dùng có Role là false
            var orders = db.Orders.Where(order => order.User.Role == false);
            var processingCount = orders.Count(o => o.OrderStatu.StatusName == "Ðang xử lý");
            var deliveringCount = orders.Count(o => o.OrderStatu.StatusName == "Ðang vận chuyển");
            var successCount = orders.Count(o => o.OrderStatu.StatusName == "Thành công");
            var cancelledCount = orders.Count(o => o.OrderStatu.StatusName == "Đã hủy");
            var totalCount = orders.Count();
            // Kiểm tra trạng thái và lọc theo trạng thái nếu cần
            if (!string.IsNullOrEmpty(status) && status != "all")
            {
                orders = orders.Where(o => o.OrderStatu.StatusName == status);
            }
            // Chuyển đổi thành danh sách
            var orderList = orders.ToList();

            // Đếm số lượng đơn hàng theo từng trạng thái


            // Gán các giá trị vào ViewBag
            ViewBag.ProcessingCount = processingCount;
            ViewBag.DeliveringCount = deliveringCount;
            ViewBag.SuccessCount = successCount;
            ViewBag.CancelledCount = cancelledCount;
            ViewBag.TotalCount = totalCount;

            // Trả về view với danh sách đơn hàng
            return View(orderList);

        }
        public ActionResult Details(int? id)
        {

            var orderDetails = db.OrderDetails.Where(od => od.OrderID == id).ToList();
            ViewBag.StatusID = new SelectList(db.OrderStatus, "StatusID", "StatusName");
            return View(orderDetails);
        }
        public ActionResult Dashboard()
        {

            return View(db.Orders.Where(order => order.User.Role == false));
        }
        [HttpPost]
        public ActionResult UpdateStatus(int OrderID, int StatusID)
        {
            // Lấy order từ cơ sở dữ liệu theo orderId
            var order = db.Orders.Find(OrderID);

            if (order != null)
            {
                // Cập nhật trạng thái của order
                order.StatusID = StatusID;
                db.SaveChanges();

                // Thực hiện các hành động khác sau khi cập nhật thành công

                return RedirectToAction("Dashboard", new { id = OrderID });
            }

            // Xử lý lỗi khi không tìm thấy order

            return RedirectToAction("Dashboard", "AdminOrder");
        }
        [Route("AdminOrder/Statics")]
        public ActionResult Statics()
        {
            DateTime startDate = new DateTime(2024, 1, 1);
            DateTime endDate = new DateTime(2024, 12, 31);
            //var startDate = new DateTime(2024, 1, 1);
            //var endDate = new DateTime(2024, 12, 31);
            //var ordersByMonth = db.Orders
            //    .Where(order => order.User.Role == false)
            //    .Where(order => order.DateOrder >= startDate && order.DateOrder <= endDate)
            //    .GroupBy(order => order.DateOrder.Value.Month)
            //    .Select(group => new
            //    {
            //        Month = group.Key,
            //        OrderCount = group.Count()
            //    })
            //    .OrderBy(result => result.Month)
            //    .ToList();
            var ordersByMonth = db.Orders
                 .Where(order => order.User.Role == false)
                .Where(o => o.DateOrder >= startDate && o.DateOrder <= endDate)
                .GroupBy(o => new { o.DateOrder.Value.Year, o.DateOrder.Value.Month }) // Nhóm theo năm và tháng
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    OrderCount = g.Count() // Đếm số lượng đơn hàng
                })
                .ToList();
            ViewBag.ordersByMonth = ordersByMonth;
            //foreach (var monthGroup in ordersByMonth)
            //{
            //    ViewBag.monthGroupY = monthGroup.Year;
            //    ViewBag.monthGroupM = monthGroup.Month;
            //    Console.WriteLine($"Năm: {monthGroup.Year}, Tháng: {monthGroup.Month}, Số lượng đơn hàng: {monthGroup.OrderCount}");
            //}
            return View(db.Orders.Where(order => order.User.Role == false));
            //return View(db.Orders.ToList());
        }
        [Route("AdminOrder/Report")]
        public ActionResult Report()
        {

            return View(db.Orders.Where(order => order.User.Role == false));
        }
        [Route("AdminOrder/ExportExcelAll")]
        public ActionResult ExportExcelAll()
        {

            var wb = new XLWorkbook();

            var ws = wb.Worksheets.Add("OrderDetail");

            var OrderServices = new OrderServices();
            ws.Cell("A1").Value = "Mã đơn hàng";
            ws.Cell("B1").Value = "Tên khách hàng";
            ws.Cell("C1").Value = "Tên sản phẩm";
            ws.Cell("D1").Value = "Số lượng";
            ws.Cell("E1").Value = "Giá trị";


            var ls = OrderServices.GetAllOrderForExcel();

            int row = 2;

            for (int i = 0; i < ls.Count; i++)
            {
                ws.Cell("A" + row).Value = ls[i].CodeOrder;
                ws.Cell("B" + row).Value = ls[i].UserName;
                ws.Cell("C" + row).Value = ls[i].ProductName;
                ws.Cell("D" + row).Value = ls[i].Amount;
                ws.Cell("E" + row).Value = ls[i].Price;
                row++;

            }
            string nameFile = "Export_" + DateTime.Now.Ticks + ".xlsx";
            string pathFile = Server.MapPath("~/Export/ExportExcelAll" + nameFile);
            wb.SaveAs(pathFile);

            return Json(nameFile, JsonRequestBehavior.AllowGet);
        }
        [Route("AdminOrder/ExportExcelbyId")]
        public ActionResult ExportExcelbyId(int orderId)
        {

            var wb = new XLWorkbook();

            var ws = wb.Worksheets.Add("Chi tiết đơn hàng");

            var OrderServices = new OrderServices();
            ws.Cell("A1").Value = "Mã đơn hàng";
            ws.Cell("B1").Value = "Tên khách hàng";
            ws.Cell("C1").Value = "Tên sản phẩm";
            ws.Cell("D1").Value = "Giá";
            ws.Cell("E1").Value = "Số lượng";
            ws.Cell("F1").Value = "Thời gian đặt hàng";

            ws.Column("A").Width = 20; 
            ws.Column("B").Width = 25; 
            ws.Column("C").Width = 30; 
            ws.Column("D").Width = 20; 
            ws.Column("E").Width = 20;
            ws.Column("F").Width = 20;


            var ls = OrderServices.GetOrderDetailForExcel(orderId);

            int row = 2;

            for (int i = 0; i < ls.Count; i++)
            {
                //ws.Cell("A" + row).Value = ls[i].OrderId;
                ws.Cell("A" + row).Value = ls[i].CodeOrder;
                ws.Cell("B" + row).Value = ls[i].UserName;
                ws.Cell("C" + row).Value = ls[i].ProductName;
                ws.Cell("D" + row).Value = ls[i].Price;
                ws.Cell("E" + row).Value = ls[i].Amount;
                ws.Cell("F" + row).Value = ls[i].DateOrder; // Gán giá trị DateTime

            

                row++;
            }
            string nameFile = "Export_" + DateTime.Now.Ticks + ".xlsx";
            string pathFile = Server.MapPath("~/Export/ExportExcel" + nameFile);
            wb.SaveAs(pathFile);

            return Json(nameFile, JsonRequestBehavior.AllowGet);
        }
    }
}