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
    public class ToathuocDAL
    {
        private string connectionString;

        public ToathuocDAL()
        {
            connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        }
        public string ConnectionString { get => connectionString; set => connectionString = value; }

        public bool them(toathuocDTO tt)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ThemToaThuoc", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ngayKeToa", tt.NgayKetoa);
                    cmd.Parameters.AddWithValue("@maPKB", tt.MaPkb);

                    try
                    {
                        con.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            tt.MaToa = Convert.ToInt32(result);
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi thêm toa thuốc: " + ex.Message);
                    }
                }
            }
        }

        public List<toathuocDTO> select()
        {
            string query = "SELECT maToaThuoc, ngayKeToa, maPKB FROM ToaThuoc"; // Không dùng SELECT *
            List<toathuocDTO> lsToaThuoc = new List<toathuocDTO>();

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
                                toathuocDTO tt = new toathuocDTO();
                                tt.MaToa = int.Parse(reader["maToaThuoc"].ToString());
                                tt.NgayKetoa = Convert.ToDateTime(reader["ngayKeToa"]);
                                tt.MaPkb = int.Parse(reader["maPKB"].ToString());
                                lsToaThuoc.Add(tt);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi tải danh sách toa thuốc: " + ex.Message);
                    }
                }
            }
            return lsToaThuoc;
        }


    }
}
