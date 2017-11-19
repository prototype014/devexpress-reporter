using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace DevExpressASPNETCoreReporting
{
    public class DataTableSerializer : DevExpress.XtraReports.Native.IDataSerializer
    {
        public const string Name = "DataTableSerializer";
        public bool CanDeserialize(string value, string typeName, object extensionProvider)
        {
            return typeName == "System.Data.DataTable";
        }
        public bool CanSerialize(object data, object extensionProvider)
        {
            return (data is DataTable);
        }

        public object Deserialize(string value, string typeName, object extensionProvider)
        {
            DataTable table = new DataTable();
            using (XmlReader reader = XmlReader.Create(new StringReader(value)))
            {
                table.ReadXml(reader);
            }
            return table;
        }

        public string Serialize(object data, object extensionProvider)
        {
            if (data is DataTable)
            {
                DataTable table = data as DataTable;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                XmlWriter writer = XmlWriter.Create(sb);
                table.WriteXml(writer, XmlWriteMode.WriteSchema);
                return sb.ToString();
            }
            return string.Empty;
        }
    }
}
