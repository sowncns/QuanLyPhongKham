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
    public class ThuocDAL
    {
        private string connectionString;
        public ThuocDAL()
        {
            connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        }
        public string ConnectionString { get => connectionString; set => connectionString = value; }

        public bool them(thuocDTO th)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ThemThuoc", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tenThuoc", th.TenThuoc);
                    cmd.Parameters.AddWithValue("@donGia", th.DonGia);
                    cmd.Parameters.AddWithValue("@maCachDung", th.MaCachDung);
                    cmd.Parameters.AddWithValue("@maDonVi", th.MaDonVi);
                    cmd.Parameters.AddWithValue("@soLuong", th.SoLuong);
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi thêm thuốc: " + ex.Message);
                    }
                }
            }
        }

        public bool sua(thuocDTO th, int maThuocold) // Changed parameter to int
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_SuaThuoc", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@maThuoc", maThuocold);
                    cmd.Parameters.AddWithValue("@tenThuoc", th.TenThuoc);
                    cmd.Parameters.AddWithValue("@donGia", th.DonGia);
                    cmd.Parameters.AddWithValue("@maCachDung", th.MaCachDung);
                    cmd.Parameters.AddWithValue("@maDonVi", th.MaDonVi);
                    cmd.Parameters.AddWithValue("@soLuong", th.SoLuong);
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi sửa thuốc: " + ex.Message);
                    }
                }
            }
        }

        public bool xoa(thuocDTO th)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_XoaThuoc", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@maThuoc", th.MaThuoc);
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi xóa thuốc: " + ex.Message);
                    }
                }
            }
        }

        public List<thuocDTO> select()
        {
            List<thuocDTO> lsThuoc = new List<thuocDTO>();
            string query = "SELECT * FROM v_DanhSachThuoc"; // Using View

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
                                thuocDTO th = new thuocDTO();
                                th.MaThuoc = int.Parse(reader["maThuoc"].ToString());
                                th.TenThuoc = reader["tenThuoc"].ToString();
                                th.MaDonVi = int.Parse(reader["maDonVi"].ToString());
                                th.MaCachDung = int.Parse(reader["maCachDung"].ToString());
                                th.DonGia = float.Parse(reader["donGia"].ToString());
                                th.SoLuong = int.Parse(reader["soLuong"].ToString());
                                lsThuoc.Add(th);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi tải danh sách thuốc: " + ex.Message);
                    }
                }
            }
            return lsThuoc;
        }

        public List<thuocDTO> selectByKeyWord(string sKeyword)
        {
            List<thuocDTO> lsThuoc = new List<thuocDTO>();
            string query = "SELECT * FROM v_DanhSachThuoc WHERE tenThuoc LIKE @key"; // Using View

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@key", "%" + sKeyword + "%");
                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                thuocDTO th = new thuocDTO();
                                th.MaThuoc = int.Parse(reader["maThuoc"].ToString());
                                th.TenThuoc = reader["tenThuoc"].ToString();
                                th.MaDonVi = int.Parse(reader["maDonVi"].ToString());
                                th.MaCachDung = int.Parse(reader["maCachDung"].ToString());
                                th.DonGia = float.Parse(reader["donGia"].ToString());
                                th.SoLuong = int.Parse(reader["soLuong"].ToString());
                                lsThuoc.Add(th);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi tìm kiếm thuốc: " + ex.Message);
                    }
                }
            }
            return lsThuoc;
        }



        public List<thuocDTO> selectbypkb(int mapkb)
        {
            List<thuocDTO> lsThuoc = new List<thuocDTO>();
            // Using View to simplify the join
            string query = @"
                SELECT v.* 
                FROM v_DanhSachThuoc v
                JOIN ChiTietDonThuoc ct ON v.maThuoc = ct.maThuoc
                JOIN ToaThuoc t ON ct.maToaThuoc = t.maToaThuoc
                WHERE t.maPKB = @mapkb";

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
                                thuocDTO th = new thuocDTO();
                                th.MaThuoc = int.Parse(reader["maThuoc"].ToString());
                                th.TenThuoc = reader["tenThuoc"].ToString();
                                th.MaDonVi = int.Parse(reader["maDonVi"].ToString());
                                th.MaCachDung = int.Parse(reader["maCachDung"].ToString());
                                th.DonGia = float.Parse(reader["donGia"].ToString());
                                lsThuoc.Add(th);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi lấy thuốc theo PKB: " + ex.Message);
                    }
                }
            }
            return lsThuoc;
        }

        public List<thuocDTO> baocaobymonth(string month, string year)
        {
            List<thuocDTO> lsThuoc = new List<thuocDTO>();
            // Using View to simplify
            string query = @"
                SELECT v.maThuoc, v.tenThuoc, v.maDonVi 
                FROM ToaThuoc T 
                JOIN ChiTietDonThuoc KT ON T.maToaThuoc=KT.maToaThuoc 
                JOIN v_DanhSachThuoc v ON KT.maThuoc=v.maThuoc 
                WHERE MONTH(T.ngayKeToa)=@month AND YEAR(T.ngayKeToa)=@year 
                GROUP BY v.maThuoc, v.tenThuoc, v.maDonVi";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@month", month);
                    cmd.Parameters.AddWithValue("@year", year);
                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                thuocDTO th = new thuocDTO();
                                th.MaThuoc = int.Parse(reader["maThuoc"].ToString());
                                th.TenThuoc = reader["tenThuoc"].ToString();
                                th.MaDonVi = int.Parse(reader["maDonVi"].ToString());
                                lsThuoc.Add(th);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi lấy báo cáo thuốc: " + ex.Message);
                    }
                }
            }
            return lsThuoc;
        }

        public bool KiemTraTenThuocDaTonTai(string tenThuoc)
        {
            string query = "SELECT COUNT(*) FROM Thuoc WHERE tenThuoc = @tenThuoc";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@tenThuoc", tenThuoc);
                    try
                    {
                        con.Open();
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }

        public bool truSoLuong(int maThuoc, int soLuongTru)
        {
            // Note: This is now handled by Trigger in the database when inserting into ChiTietDonThuoc.
            // But if called directly, we can use the old logic or call a procedure.
            // Let's keep the inline query as fallback or use it.
            string query = @"UPDATE Thuoc
                             SET soLuong = soLuong - @soLuongTru
                             WHERE maThuoc = @maThuoc
                             AND soLuong >= @soLuongTru";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@maThuoc", maThuoc);
                    cmd.Parameters.AddWithValue("@soLuongTru", soLuongTru);
                    try
                    {
                        con.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }
    }
}
