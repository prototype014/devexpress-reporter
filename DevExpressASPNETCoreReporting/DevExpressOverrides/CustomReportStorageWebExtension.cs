using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;
using System.Data;
using DevExpressASPNETCoreReporting.Data;
using DevExpressASPNETCoreReporting.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DevExpressASPNETCoreReporting.DevExpressOverrides
{
    public class CustomReportStorageWebExtension : ReportStorageWebExtension
    {
        private ReportContext _db;

        public IConfigurationRoot Configuration { get; }

        public CustomReportStorageWebExtension()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ReportContext>();
            optionsBuilder.UseSqlServer("Data Source=LOCALHOST\\SQLExpress;Initial Catalog=ManageWithReports;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            _db = new ReportContext(optionsBuilder.Options);
        }

        public override bool CanSetData(string url)
        {
            int id = 0;
            int.TryParse(url, out id);
            // Check if the URL is available in the report storage.
            return _db.Reports.Where(r => r.Id == id) != null;
        }


        public override byte[] GetData(string url)
        {
            int id = 0;
            int.TryParse(url, out id);
            // Get the report data from the storage.
            Report report = _db.Reports.FirstOrDefault(r => r.Id == id);
            if (report == null) return null;

            byte[] reportData = (Byte[])report.Content;
            return reportData;
        }


        public override Dictionary<string, string> GetUrls()
        {
            Dictionary<string, string> reportFiles = new Dictionary<string, string>();
            // Get URLs and display names for all reports available in the storage.
            IEnumerable<Report> reports = _db.Reports.OrderBy(r => r.Name);
            foreach(Report report in reports)
            {
                reportFiles.Add(report.Id.ToString(), report.Name);
            }

            return reportFiles;
        }


        public override bool IsValidUrl(string url)
        {
            int n;
            return int.TryParse(url, out n);
        }


        public override void SetData(XtraReport report, string url)
        {
            int id = 0;
            int.TryParse(url, out id);
            Report dbReport = _db.Reports.FirstOrDefault(r => r.Id == id);
            if (dbReport != null) {
                using (MemoryStream ms = new MemoryStream())
                {
                    report.SaveLayoutToXml(ms);
                    dbReport.Content = ms.GetBuffer();
                }
                _db.Entry(dbReport).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }


        public override string SetNewData(XtraReport report, string defaultUrl)
        {
            Report dbReport = new Report()
            {
                Name = defaultUrl
            };

            using (MemoryStream ms = new MemoryStream())
            {
                report.SaveLayoutToXml(ms);
                dbReport.Content = ms.GetBuffer();
            }

            _db.Reports.Add(dbReport);
            _db.SaveChanges();
            return dbReport.Id.ToString();
        }
    }
}
