using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace tiki.Models
{
    public class DataModel
    {
        private string connectionString = "workstation id=DoAnPMLT.mssql.somee.com;packet size=4096;user id=BaAnhem_SQLLogin_1;pwd=tqo3flmu47;data source=DoAnPMLT.mssql.somee.com;persist security info=False;initial catalog=DoAnPMLT;TrustServerCertificate=True";
        public ArrayList get(String sql)
        {
            ArrayList datalist = new ArrayList();
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql, connection);
            connection.Open();
            using (SqlDataReader r = command.ExecuteReader())
            {
                while (r.Read())
                {
                    // Xử lý 
                    ArrayList row = new ArrayList();
                    for (int i = 0; i < r.FieldCount; i++)
                    {
                        row.Add(r.GetValue(i).ToString());
                    }
                    datalist.Add(row);
                }
            }
            connection.Close();
            return datalist;

        }
    }
}