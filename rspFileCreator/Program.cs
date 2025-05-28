using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace rspFileCreator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataTable dt = GetMaxFaxIdentifier(args[1]);
            //int cnt = dt.Rows.Count;
            foreach(DataRow dr in dt.Rows)
            {
                StringBuilder stb = new StringBuilder();
                string maxFaxId = String.Format("{0:D5}", (int)dr["FaxIdentifier"]);
                stb.AppendLine(maxFaxId);
                stb.AppendLine("0354717007   ");
                stb.AppendLine(args[0].ToString());
                stb.AppendLine("0000");
                stb.AppendLine("DP            ");
                File.WriteAllText(string.Format("1A1{0}.rsp", maxFaxId), stb.ToString());
                stb = null;
            }
        }

        private static DataTable GetMaxFaxIdentifier(string SenderName)
        {
            DataTable dt = null;
            string dbconstr = rspFileCreator.Properties.Settings.Default.DBConnectString;
            using (SqlConnection sqlcon = new SqlConnection(dbconstr))
            using (SqlCommand cmd = sqlcon.CreateCommand())
            {
                try
                {
                    sqlcon.Open();
                    cmd.CommandTimeout = rspFileCreator.Properties.Settings.Default.DBCommandTimeout;
                    cmd.CommandText = "SELECT TOP 1 FaxIdentifier FROM FCCGWFaxService WITH(NOLOCK) WHERE SenderName = '" + SenderName + "' AND Status = 2 ORDER BY RequestTime DESC";
                    cmd.CommandType = System.Data.CommandType.Text;
                    dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                }
                catch (Exception exp)
                {
                    throw exp;
                }
                finally
                {
                    sqlcon.Close();
                }
                return dt;
            }
        }
    }
}
