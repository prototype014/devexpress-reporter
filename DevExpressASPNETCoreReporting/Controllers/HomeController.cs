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

namespace DevExpressASPNETCoreReporting.Controllers {
    public class HomeController : Controller {
        public IActionResult Index() {
            var optionsBuilder = new DbContextOptionsBuilder<ReportContext>();
            optionsBuilder.UseSqlServer("Data Source=LOCALHOST\\SQLExpress;Initial Catalog=ManageWithReports;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            ReportContext _db = new ReportContext(optionsBuilder.Options);
            List<Report> model = _db.Reports.ToList();
            return View(model);
        }

        public IActionResult ReportDesigner() {
            var contentGenerator = new ReportDesignerClientSideModelGenerator();
            var clientSideModelSettings = new ClientSideModelSettings { IncludeLocalization = false };
            var globalDataSources = new Dictionary<string, object>();
            globalDataSources.Add("nwindDS", new CategoriesReport().DataSource);
            var report = new XtraReport();
            var modelString = contentGenerator.GetJsonModelScript(report, globalDataSources, "/DXXRD", "/DXXRDV", "/DXQB", clientSideModelSettings);
            var model = new ClientControlModel {
                ModelJson = modelString
            };
            return View(model);
        }

        public IActionResult ReportViewer(int id) {
            var optionsBuilder = new DbContextOptionsBuilder<ReportContext>();
            optionsBuilder.UseSqlServer("Data Source=LOCALHOST\\SQLExpress;Initial Catalog=ManageWithReports;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            ReportContext _db = new ReportContext(optionsBuilder.Options);

            XtraReport myReport = new XtraReport();
            Report dbReport = _db.Reports.FirstOrDefault(r => r.Id == id);
            MemoryStream ms = new MemoryStream(dbReport.Content);
            myReport.LoadLayoutFromXml(ms);

            //var clientSideModelGenerator = new WebDocumentViewerClientSideModelGenerator();
            //var clientSideModelSettings = new ClientSideModelSettings { IncludeLocalization = false };
            ////var report = new CategoriesReport();
            //var modelString = clientSideModelGenerator.GetJsonModelScript((XtraReport) null/*report*/, "/DXXRDV", clientSideModelSettings);
            //var model = new ClientControlModel {
            //    ModelJson = modelString
            //};


            return View(myReport);

        }

        public IActionResult Error() {
            return View();
        }
    }
}
