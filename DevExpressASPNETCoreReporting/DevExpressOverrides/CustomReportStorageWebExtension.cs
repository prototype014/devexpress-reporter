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
            // Check if the specified URL is valid for the current report storage.
            // In this example, a URL should be a string containing a numeric value that is used as a data row primary key.
            int n;
            return int.TryParse(url, out n);
        }


        public override void SetData(XtraReport report, string url)
        {
            /*
            // Write a report to the storage under the specified URL.
            DataRow row = reportsTable.Rows.Find(int.Parse(url));

            if (row != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    report.SaveLayoutToXml(ms);
                    row["LayoutData"] = ms.GetBuffer();
                }
                reportsTableAdapter.Update(catalogDataSet);
                catalogDataSet.AcceptChanges();
            }

            //Now save this row as an entity
            */
        }


        public override string SetNewData(XtraReport report, string defaultUrl)
        {
            /*
            // Save a report to the storage under a new URL. 
            // The defaultUrl parameter contains the report display name specified by a user.
            DataRow row = reportsTable.NewRow();

            row["DisplayName"] = defaultUrl;
            using (MemoryStream ms = new MemoryStream())
            {
                report.SaveLayoutToXml(ms);
                row["LayoutData"] = ms.GetBuffer();
            }

            reportsTable.Rows.Add(row);
            reportsTableAdapter.Update(catalogDataSet);
            catalogDataSet.AcceptChanges();

            // Refill the dataset to obtain the actual value of the new row's autoincrement key field.
            reportsTableAdapter.Fill(catalogDataSet.Reports);
            return catalogDataSet.Reports.FirstOrDefault(x => x.DisplayName == defaultUrl).ReportID.ToString();
            */
            return "";
        }
    }
}
