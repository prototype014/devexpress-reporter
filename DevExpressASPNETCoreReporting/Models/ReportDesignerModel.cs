using DevExpress.XtraReports.UI;

namespace DevExpressASPNETCoreReporting.Models {
    public class ReportDesignerModel
    {
        public XtraReport Report { get; set; }
        public object DataSource { get; set; }
    }
}
