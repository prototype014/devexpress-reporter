using System.Collections.Generic;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.ClientControls;
using DevExpress.XtraReports.Web.ReportDesigner;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using DevExpressASPNETCoreReporting.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevExpressASPNETCoreReporting.Controllers {
    public class HomeController : Controller {
        public IActionResult Index() {
            return View();
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

        public IActionResult ReportViewer() {
            var clientSideModelGenerator = new WebDocumentViewerClientSideModelGenerator();
            var clientSideModelSettings = new ClientSideModelSettings { IncludeLocalization = false };
            //var report = new CategoriesReport();
            var modelString = clientSideModelGenerator.GetJsonModelScript((XtraReport) null/*report*/, "/DXXRDV", clientSideModelSettings);
            var model = new ClientControlModel {
                ModelJson = modelString
            };
            return View(model);
        }

        public IActionResult Error() {
            return View();
        }
    }
}
