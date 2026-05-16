using QLPKDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace QLPKDAL
{
    public class ChiTietToaThuocDAL
    {
        private string connectionString;
        public ChiTietToaThuocDAL() 
        {
            connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        }
        public string ConnectionString { get => connectionString; set => connectionString = value;}

        public bool kethuoc(ChiTietToaThuocDTO kt)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                // Sử dụng Procedure sp_KeThuocVaoDon
                using (SqlCommand cmd = new SqlCommand("sp_KeThuocVaoDon", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@maThuoc", kt.MaThuoc);
                    cmd.Parameters.AddWithValue("@maToaThuoc", kt.MaToa);
                    cmd.Parameters.AddWithValue("@soLuong", kt.SoLuong);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi kê thuốc: " + ex.Message);
                    }
                }
            }
        }

        public List<ChiTietToaThuocDTO> selectbypkb(int mapkb)
        {
            // Sử dụng View v_ChiTietDonThuoc
            string query = "SELECT maToaThuoc, maThuoc, soLuong FROM v_ChiTietDonThuoc WHERE maPKB = @mapkb";
            List<ChiTietToaThuocDTO> lskethuoc = new List<ChiTietToaThuocDTO>();

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@mapkb", mapkb);
                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ChiTietToaThuocDTO kt = new ChiTietToaThuocDTO();
                                kt.SoLuong = int.Parse(reader["soLuong"].ToString());
                                kt.MaToa = int.Parse(reader["maToaThuoc"].ToString());
                                kt.MaThuoc = int.Parse(reader["maThuoc"].ToString());
                                lskethuoc.Add(kt);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi lấy chi tiết toa thuốc theo PKB: " + ex.Message);
                    }
                }
            }
            return lskethuoc;
        }

        public List<ChiTietToaThuocDTO> baocaobymonth(string month, string year)
        {
            List<ChiTietToaThuocDTO> lskethuoc = new List<ChiTietToaThuocDTO>();

            // Sử dụng Procedure sp_BaoCaoSuDungThuoc
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_BaoCaoSuDungThuoc", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@month", int.Parse(month));
                    cmd.Parameters.AddWithValue("@year", int.Parse(year));
                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ChiTietToaThuocDTO kt = new ChiTietToaThuocDTO();
                                kt.SoLuong = int.Parse(reader["soLuong"].ToString());
                                kt.MaThuoc = int.Parse(reader["maThuoc"].ToString());
                                // DTO có thể không có trường TenThuoc, nếu có thì gán vào.
                                // Giả sử DTO chỉ cần MaThuoc và SoLuong cho báo cáo.
                                lskethuoc.Add(kt);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi lấy báo cáo sử dụng thuốc: " + ex.Message);
                    }
                }
            }
            return lskethuoc;
        }

        public int solandungbymonth(int mathuoc, string month, string year)
        {
            int SLD = 0;
            // Sử dụng Procedure sp_DemSoLanKeThuoc
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DemSoLanKeThuoc", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mathuoc", mathuoc);
                    cmd.Parameters.AddWithValue("@month", int.Parse(month));
                    cmd.Parameters.AddWithValue("@year", int.Parse(year));
                    try
                    {
                        con.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            SLD = Convert.ToInt32(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi đếm số lần kê thuốc: " + ex.Message);
                    }
                }
            }
            return SLD;
        }

        public List<ChiTietToaThuocDTO> selectByDate(DateTime ngay)
        {
            List<ChiTietToaThuocDTO> list = new List<ChiTietToaThuocDTO>();
            // Sử dụng View v_ChiTietDonThuoc
            string query = "SELECT maThuoc, maToaThuoc, soLuong FROM v_ChiTietDonThuoc WHERE CAST(ngayKeToa AS DATE) = @ngay";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ngay", ngay.Date);
                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ChiTietToaThuocDTO kt = new ChiTietToaThuocDTO();
                                kt.SoLuong = int.Parse(reader["soLuong"].ToString());
                                kt.MaToa = int.Parse(reader["maToaThuoc"].ToString());
                                kt.MaThuoc = int.Parse(reader["maThuoc"].ToString());
                                list.Add(kt);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi lấy chi tiết toa thuốc theo ngày: " + ex.Message);
                    }
                }
            }
            return list;
        }
    }
}
