using QLPKDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLPKDTO;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace QLPKDAL
{
    public class DichvuDAL
    {
        private string connectionString;

        public DichvuDAL()
        {
            connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        }
        public string ConnectionString { get => connectionString; set => connectionString = value; }

        public bool them(dichvuDTO dv)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ThemDichVu", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tenDichVu", dv.TenDichVu);
                    cmd.Parameters.AddWithValue("@tienDichVu", dv.TienDichVu);
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi thêm dịch vụ: " + ex.Message);
                    }
                }
            }
        }

        public bool sua(dichvuDTO dv, int maDichVuOld)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_SuaDichVu", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@maDichVu", maDichVuOld);
                    cmd.Parameters.AddWithValue("@tenDichVu", dv.TenDichVu);
                    cmd.Parameters.AddWithValue("@tienDichVu", dv.TienDichVu);
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi sửa dịch vụ: " + ex.Message);
                    }
                }
            }
        }

        public bool xoa(dichvuDTO dv)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_XoaDichVu", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@maDichVu", dv.MaDichVu);
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi xóa dịch vụ: " + ex.Message);
                    }
                }
            }
        }

        public List<dichvuDTO> select()
        {
            List<dichvuDTO> lsDichVu = new List<dichvuDTO>();
            string query = "SELECT * FROM v_DanhSachDichVu"; // Sử dụng View

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
                                dichvuDTO dv = new dichvuDTO();
                                dv.MaDichVu = int.Parse(reader["maDichVu"].ToString());
                                dv.TenDichVu = reader["tenDichVu"].ToString();
                                dv.TienDichVu = float.Parse(reader["tienDichVu"].ToString());
                                lsDichVu.Add(dv);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi tải danh sách dịch vụ: " + ex.Message);
                    }
                }
            }
            return lsDichVu;
        }

        public List<dichvuDTO> selectByKeyWord(string sKeyword)
        {
            List<dichvuDTO> lsDichVu = new List<dichvuDTO>();
            string query = "SELECT * FROM v_DanhSachDichVu WHERE tenDichVu LIKE @key"; // Sử dụng View

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
                                dichvuDTO dv = new dichvuDTO();
                                dv.MaDichVu = int.Parse(reader["maDichVu"].ToString());
                                dv.TenDichVu = reader["tenDichVu"].ToString();
                                dv.TienDichVu = float.Parse(reader["tienDichVu"].ToString());
                                lsDichVu.Add(dv);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi tìm kiếm dịch vụ: " + ex.Message);
                    }
                }
            }
            return lsDichVu;
        }


    }
}
