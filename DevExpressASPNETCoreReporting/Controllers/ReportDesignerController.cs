using System.Threading.Tasks;
using DevExpress.XtraReports.Web.ReportDesigner.Native.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevExpressASPNETCoreReporting.Controllers {
    public class ReportDesignerController : RequestControllerBase {
        public ReportDesignerController(IReportDesignerRequestManager requestManager) : base(requestManager) { }

        [Route("DXXRD")]
        public Task<IActionResult> ReportDesignerInvoke() {
            return base.Invoke();
        }
    }
}