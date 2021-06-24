namespace XmlToSQL
{
    using System.Data;
    using System.Data.SqlClient;

    class Program
    {
        static void Main() {
            using (var con = new SqlConnection(@"server=(localdb)\MyInstance;database=CalibStore;integrated security=true")) {
                var ds = new DataSet();
                ds.ReadXml("C:/Users/GLJ/Desktop/A001.calib.xml");

                var dtProj = ds.Tables["Project"];
                var dtLogger = ds.Tables["Logger"];
                var dtTrasducer = ds.Tables["Transducer"];
                var dtCalib = ds.Tables["Calibration"];

                con.Open();

                using (var bc = new SqlBulkCopy(con)) {
                    bc.DestinationTableName = "Calibrations";
                    bc.ColumnMappings.Add("date", "Date");
                    bc.ColumnMappings.Add("temperature", "Temperature");
                    bc.ColumnMappings.Add("steps", "Steps");
                    bc.ColumnMappings.Add("callow", "CalLow");
                    bc.ColumnMappings.Add("calhigh", "CalHigh");
                    bc.ColumnMappings.Add("factor", "Factor");
                    bc.ColumnMappings.Add("minv", "MinV");
                    bc.ColumnMappings.Add("maxv", "MaxV");
                    bc.WriteToServer(dtCalib);
                }
                using (var bc = new SqlBulkCopy(con)) {
                    bc.DestinationTableName = "Loggers";
                    bc.ColumnMappings.Add("name", "Name");
                    bc.ColumnMappings.Add("Serial", "Serial");
                    bc.ColumnMappings.Add("idx", "Idx");
                    bc.WriteToServer(dtLogger);
                }
                using (var bc = new SqlBulkCopy(con)) {
                    bc.DestinationTableName = "Projects";
                    bc.ColumnMappings.Add("No", "No");
                    bc.ColumnMappings.Add("Ini", "Ini");
                    bc.ColumnMappings.Add("CalibrationType", "CalibrationType");
                    bc.WriteToServer(dtProj);
                }
                using (var bc = new SqlBulkCopy(con)) {
                    bc.DestinationTableName = "Transducers";
                    bc.ColumnMappings.Add("type", "Type");
                    bc.ColumnMappings.Add("serial", "Serial");
                    bc.ColumnMappings.Add("nomlow", "NomLow");
                    bc.ColumnMappings.Add("nomhigh", "NomHigh");
                    bc.WriteToServer(dtTrasducer);
                }
            }
        }
    }
}
