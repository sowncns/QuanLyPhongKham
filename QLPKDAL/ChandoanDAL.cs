using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLPKDTO;

namespace QLPKDAL
{
    public class ChandoanDAL
    {
        private string connectionString;
        public ChandoanDAL()
        {
            connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        }
        public string ConnectionString { get => connectionString; set => connectionString = value; }

        public List<chandoanDTO> select()
        {
            List<chandoanDTO> lscd = new List<chandoanDTO>();
            string query = "SELECT * FROM v_DanhSachChuanDoan"; // Sử dụng View

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
                                chandoanDTO cd = new chandoanDTO();
                                cd.MaPkb = int.Parse(reader["maPKB"].ToString());
                                cd.MaBenh = int.Parse(reader["maBenh"].ToString());
                                // DTO có thể có TenChuanDoan
                                cd.TenChuanDoan = reader["tenChuanDoan"] != DBNull.Value ? reader["tenChuanDoan"].ToString() : "";
                                lscd.Add(cd);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi tải danh sách chẩn đoán: " + ex.Message);
                    }
                }
            }
            return lscd;
        }

        public bool them(chandoanDTO cd)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                // Sử dụng Procedure sp_ThemChuanDoan
                using (SqlCommand cmd = new SqlCommand("sp_ThemChuanDoan", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@maBenh", cd.MaBenh);
                    cmd.Parameters.AddWithValue("@maPKB", cd.MaPkb);
                    cmd.Parameters.AddWithValue("@tenChuanDoan", cd.TenChuanDoan);
                    
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi thêm chẩn đoán: " + ex.Message);
                    }
                }
            }
        }

        public List<chandoanDTO> selectByKeyWord(string sKeyword)
        {
            List<chandoanDTO> list = new List<chandoanDTO>();
            string query = "SELECT * FROM v_DanhSachChuanDoan WHERE tenChuanDoan LIKE @key"; // Sử dụng View

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@key", "%" + sKeyword + "%");
                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                chandoanDTO cd = new chandoanDTO();
                                cd.MaPkb = int.Parse(reader["maPKB"].ToString());
                                cd.MaBenh = int.Parse(reader["maBenh"].ToString());
                                cd.TenChuanDoan = reader["tenChuanDoan"] != DBNull.Value ? reader["tenChuanDoan"].ToString() : "";
                                list.Add(cd);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi tìm kiếm chẩn đoán: " + ex.Message);
                    }
                }
            }
            return list;
        }
    }
}
