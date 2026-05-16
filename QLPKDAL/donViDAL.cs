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
    public class donViDAL
    {
        private string connectionString;
        public donViDAL()
        {
            connectionString = ConfigurationManager.AppSettings["ConnectionString"]; 
        }
        public string ConnectionString { get => connectionString; set => connectionString = value; }

        public List<donViDTO> getdonvi()
        {
            List<donViDTO> lsdv = new List<donViDTO>();
            string query = "SELECT * FROM v_DanhSachDonVi"; // Sử dụng View

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
                                donViDTO dv = new donViDTO();
                                dv.MaDonVi = int.Parse(reader["maDonVi"].ToString());
                                dv.TenDonVi = reader["tenDonVi"].ToString();
                                lsdv.Add(dv);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi tải danh sách đơn vị: " + ex.Message);
                    }
                }
            }
            return lsdv;
        }
    }
}
