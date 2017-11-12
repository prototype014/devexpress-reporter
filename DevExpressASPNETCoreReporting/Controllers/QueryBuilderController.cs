using System.Threading.Tasks;
using DevExpress.XtraReports.Web.QueryBuilder.Native.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevExpressASPNETCoreReporting.Controllers {
    public class QueryBuilderController : RequestControllerBase {
        public QueryBuilderController(IQueryBuilderRequestManager requestManager) : base(requestManager) { }

        [Route("DXQB")]
        public Task<IActionResult> QueryBuilderInvoke() {
            return base.Invoke();
        }
    }
}