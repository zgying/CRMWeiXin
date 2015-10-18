using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace jQueryMobileAutoComplete
{
    /// <summary>
    /// Summary description for Search
    /// </summary>
    public class Search : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (!String.IsNullOrEmpty(context.Request.QueryString["term"].ToString()))
            {
                string connStr = ConfigurationManager.ConnectionStrings["DBCONNSTRING"].ToString();
                SqlConnection sqlconn = new SqlConnection(connStr);
                SqlCommand sqlcmd = new SqlCommand();

                try
                {
                    if (sqlconn.State == ConnectionState.Closed)
                    {
                        sqlconn.Open();
                    }

                    sqlcmd.Connection = sqlconn;
                    sqlcmd.CommandType = CommandType.Text;
                    sqlcmd.CommandText = "SELECT top 10 x.CountryName as cn FROM Countries as x WHERE x.CountryName LIKE '%' + @cn + '%'";
                    sqlcmd.Parameters.AddWithValue("@cn", context.Request.QueryString["term"].ToString());
                    
                    sqlcmd.ExecuteNonQuery();

                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        string[] items = new string[dt.Rows.Count];
                        int ctr = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            items[ctr] = (string)row["cn"];
                            ctr++;
                        }

                        //convert the string array to Javascript and send it out
                        context.Response.Write(new JavaScriptSerializer().Serialize(items));
                    }
                    if (sqlconn.State == ConnectionState.Open)
                    {
                        sqlcmd.Dispose();
                        sqlconn.Close();
                        sqlconn.Dispose();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (sqlconn.State == ConnectionState.Open)
                    {
                        sqlcmd.Dispose();
                        sqlconn.Close();
                        sqlconn.Close();
                    }
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}