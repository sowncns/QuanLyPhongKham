using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLPKDTO;

namespace QLPKDAL
{
    public class BenhNhanDAL
    {
        private string connectionString;

        public BenhNhanDAL()
        {
            connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        }

        public string ConnectionString { get => connectionString; set => connectionString = value; }

        public bool Them(BenhNhanDTO bn)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ThemBenhNhan", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tenBenhNhan", bn.TenBN);
                    cmd.Parameters.AddWithValue("@gioiTinh", bn.GtBN);
                    cmd.Parameters.AddWithValue("@ngaySinh", bn.NgsinhBN);
                    cmd.Parameters.AddWithValue("@diaChi", bn.DiachiBN);
                    cmd.Parameters.AddWithValue("@CCCD", bn.CanCuocCongDan);
                    cmd.Parameters.AddWithValue("@email", bn.Email);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi thêm bệnh nhân: " + ex.Message);
                    }
                }
            }
        }

        public bool Sua(BenhNhanDTO bn, int maBNold)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_SuaBenhNhan", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@maBenhNhan", maBNold);
                    cmd.Parameters.AddWithValue("@tenBenhNhan", bn.TenBN);
                    cmd.Parameters.AddWithValue("@gioiTinh", bn.GtBN);
                    cmd.Parameters.AddWithValue("@ngaySinh", bn.NgsinhBN);
                    cmd.Parameters.AddWithValue("@diaChi", bn.DiachiBN);
                    cmd.Parameters.AddWithValue("@CCCD", bn.CanCuocCongDan);
                    cmd.Parameters.AddWithValue("@email", bn.Email);
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi sửa bệnh nhân: " + ex.Message);
                    }
                }
            }
        }

        public bool Xoa(BenhNhanDTO bn)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_XoaBenhNhan", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@maBenhNhan", bn.MaBN);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi xóa bệnh nhân: " + ex.Message);
                    }
                }
            }
        }

        public List<BenhNhanDTO> select()
        {
            List<BenhNhanDTO> lsBenhNhan = new List<BenhNhanDTO>();
            string query = "SELECT * FROM v_DanhSachBenhNhan"; // Sử dụng View

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
                                BenhNhanDTO bn = new BenhNhanDTO();
                                bn.MaBN = int.Parse(reader["maBenhNhan"].ToString());
                                bn.TenBN = reader["tenBenhNhan"].ToString();
                                bn.GtBN = reader["gioiTinh"].ToString();
                                bn.NgsinhBN = DateTime.Parse(reader["ngaySinh"].ToString());
                                bn.DiachiBN = reader["diaChi"].ToString();
                                bn.CanCuocCongDan = reader["CCCD"].ToString();
                                bn.Email = reader["email"] != DBNull.Value ? reader["email"].ToString() : "";
                                lsBenhNhan.Add(bn);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi tải danh sách bệnh nhân: " + ex.Message);
                    }
                }
            }
            return lsBenhNhan;
        }

        public List<BenhNhanDTO> SelectByKeyWord(string sKeyword)
        {
            List<BenhNhanDTO> lsBenhNhan = new List<BenhNhanDTO>();
            // Sử dụng Procedure sp_TimKiemBenhNhan
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_TimKiemBenhNhan", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tuKhoa", sKeyword);

                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BenhNhanDTO bn = new BenhNhanDTO();
                                bn.MaBN = int.Parse(reader["maBenhNhan"].ToString());
                                bn.TenBN = reader["tenBenhNhan"].ToString();
                                bn.GtBN = reader["gioiTinh"].ToString();
                                bn.NgsinhBN = DateTime.Parse(reader["ngaySinh"].ToString());
                                bn.DiachiBN = reader["diaChi"].ToString();
                                bn.CanCuocCongDan = reader["CCCD"].ToString();
                                bn.Email = reader["email"] != DBNull.Value ? reader["email"].ToString() : "";
                                lsBenhNhan.Add(bn);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi tìm kiếm bệnh nhân: " + ex.Message);
                    }
                }
            }
            return lsBenhNhan;
        }


    }
}