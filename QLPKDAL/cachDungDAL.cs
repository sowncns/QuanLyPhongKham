using QLPKDTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLPKDAL
{
    public class cachDungDAL
    {
        private string connectionString;
        public cachDungDAL() 
        {
            connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        }

        public string ConnectionString { get => connectionString; set => connectionString = value; }

        public List<cachdungDTO> getcachdung()
        {
            List<cachdungDTO> lscd = new List<cachdungDTO>();
            string query = "SELECT * FROM v_DanhSachCachDung"; // Sử dụng View

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cachdungDTO cd = new cachdungDTO(); 
                                cd.TenCachDung = reader["tenCachDung"].ToString(); 
                                cd.MaCachDung = int.Parse(reader["maCachDung"].ToString()); 
                                lscd.Add(cd);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi tải danh sách cách dùng: " + ex.Message);
                    }
                }
            }
            return lscd;
        }
    }
}
