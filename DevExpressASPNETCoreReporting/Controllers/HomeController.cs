using System.Collections.Generic;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.ClientControls;
using DevExpress.XtraReports.Web.ReportDesigner;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using DevExpressASPNETCoreReporting.Models;
using Microsoft.AspNetCore.Mvc;
using DevExpressASPNETCoreReporting.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.IO;
using System.Text;
using DevExpress.XtraReports.Native;
using System.Data;
using DevExpress.DataAccess.Sql;

namespace DevExpressASPNETCoreReporting.Controllers {
    public class HomeController : Controller {
        public IActionResult Index() {
            ReportContext _db = GetDB();
            List<Report> model = _db.Reports.ToList();
            return View(model);
        }

        public IActionResult CreateReport() {
            var myReport = new XtraReport();
            var model = new Models.ReportDesignerModel
            {
                DataSource = CreateData(),
                // Open your report here. 
                Report = myReport
            };
            return View("ReportDesigner", model);
        }

        public IActionResult EditReport(int id)
        {
            ReportContext _db = GetDB();

            XtraReport myReport = new XtraReport();
            Report dbReport = _db.Reports.FirstOrDefault(r => r.Id == id);
            MemoryStream ms = new MemoryStream(dbReport.Content);
            var globalDataSources = new Dictionary<string, object>();
            myReport.LoadLayoutFromXml(ms);

            var model = new Models.ReportDesignerModel
            {
                DataSource = CreateData(),
                // Open your report here. 
                Report = myReport
            };
            return View("ReportDesigner", model);
        }

        public IActionResult ReportViewer(int id) {
            ReportContext _db = GetDB();

            XtraReport myReport = new XtraReport();
            myReport.Extensions[SerializationService.Guid] = DataTableSerializer.Name;
            Report dbReport = _db.Reports.FirstOrDefault(r => r.Id == id);
            MemoryStream ms = new MemoryStream(dbReport.Content);
            myReport.LoadLayoutFromXml(ms);

            var clientSideModelGenerator = new WebDocumentViewerClientSideModelGenerator();
            var clientSideModelSettings = new ClientSideModelSettings { IncludeLocalization = false };
            //var report = new CategoriesReport();
            return View(myReport);
        }

        public IActionResult Error() {
            return View();
        }

        private ReportContext GetDB()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ReportContext>();
            optionsBuilder.UseSqlServer("Data Source=LOCALHOST\\SQLExpress;Initial Catalog=ManageWithReports;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            return new ReportContext(optionsBuilder.Options);
        }

        private SqlDataSource CreateData()
        {
            SqlDataSource dataSource = new SqlDataSource("ManageConnection");
            SelectQuery query = SelectQueryFluentBuilder.AddTable("Product").SelectAllColumnsFromTable().Build("Products");
            dataSource.Queries.Add(query);
            dataSource.RebuildResultSchema();

            return dataSource;
        }
    }
}
