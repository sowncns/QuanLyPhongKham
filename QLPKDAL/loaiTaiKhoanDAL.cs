using QLPKDTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLPKDAL
{
    public class loaiTaiKhoanDAL
    {
        private string connectionString;

        public loaiTaiKhoanDAL()
        {
            connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
        }
        public string ConnectionString
        {
            get => connectionString;
            set => connectionString = value;
        }
        public List<loaiTaiKhoanDTO> select()
        {
            List<loaiTaiKhoanDTO> lsloaitk = new List<loaiTaiKhoanDTO>();
            string query = "SELECT * FROM v_DanhSachRoles"; // Sử dụng View

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
                                loaiTaiKhoanDTO loaitk = new loaiTaiKhoanDTO();
                                loaitk.TenLoaiTaiKhoan = reader["tenRole"].ToString();
                                loaitk.MaRole = int.Parse(reader["maRole"].ToString());
                                lsloaitk.Add(loaitk);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi tải danh sách loại tài khoản: " + ex.Message);
                    }
                }
            }
            return lsloaitk;
        }
    }
}
