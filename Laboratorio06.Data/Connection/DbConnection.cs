using System.Configuration;
using Microsoft.Data.SqlClient; // o System.Data.SqlClient

namespace Laboratorio06.Data.Connection
{
    public static class DbConnection
    {
        public static string GetConnectionString()
        {
            // Lee la cadena desde el App.config del proyecto de inicio (WPF)
            return ConfigurationManager.ConnectionStrings["NeptunoConnection"].ConnectionString;
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }
    }
}