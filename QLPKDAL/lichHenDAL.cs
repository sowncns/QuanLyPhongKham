using QLPKDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections;

namespace QLPKDAL
{
    public class lichHenDAL
    {
        private string connectionString;
        public lichHenDAL()
        {
            connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
        }
        public string ConnectionString { get => connectionString; set => connectionString = value; }

        public List<lichHenDTO> select()
        {
            // Sử dụng View v_DanhSachLichHen
            string query = "SELECT * FROM v_DanhSachLichHen"; 
            List<lichHenDTO> lslichHen = new List<lichHenDTO>();

            using (SqlConnection con = new SqlConnection(ConnectionString)) 
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lichHenDTO lh = new lichHenDTO();
                                lh.MaLichHen = int.Parse(reader["maLichHen"].ToString());
                                lh.MaBenhNhan = int.Parse(reader["maBenhNhan"].ToString());
                                lh.MaTaiKhoan = int.Parse(reader["maTaiKhoan"].ToString());
                                lh.MaDieuDuong = int.Parse(reader["maDieuDuong"].ToString());
                                lh.NgayHen = DateTime.Parse(reader["ngayHen"].ToString());
                                lh.TrangThai = reader["trangThai"].ToString();
                                lslichHen.Add(lh);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi tải danh sách lịch hẹn: " + ex.Message);
                    }
                }
            }
            return lslichHen;
        }

        public bool them(lichHenDTO lh)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                // Gọi Stored Procedure sp_ThemLichHen
                using (SqlCommand cmd = new SqlCommand("sp_ThemLichHen", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ngayHen", lh.NgayHen);
                    cmd.Parameters.AddWithValue("@maTaiKhoan", lh.MaTaiKhoan);
                    cmd.Parameters.AddWithValue("@maBenhNhan", lh.MaBenhNhan);
                    cmd.Parameters.AddWithValue("@maDieuDuong", lh.MaDieuDuong);
                    // Procedure sp_ThemLichHen trong file 05_procedures.sql không có tham số @trangThai
                    // Nó dùng mặc định là N'Chờ khám'. Nếu cần truyền, cần cập nhật Procedure.
                    // Giả sử dùng mặc định của Procedure.

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi thêm lịch hẹn: " + ex.Message);
                    }
                }
            }
        }



        public bool xoa(lichHenDTO lh)
        {
            string query = "DELETE FROM LichHen WHERE maLichHen = @maLichHen";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@maLichHen", lh.MaLichHen);
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi xóa lịch hẹn: " + ex.Message);
                    }
                }
            }
        }

        public bool CapNhatTrangThai(int maBenhNhan, string trangThaiMoi) // Changed parameter to int
        {
            // Cập nhật theo mã bệnh nhân như code cũ của bạn
            string query = "UPDATE LichHen SET trangThai = @trangThaiMoi WHERE maBenhNhan = @maBenhNhan";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@trangThaiMoi", trangThaiMoi);
                cmd.Parameters.AddWithValue("@maBenhNhan", maBenhNhan);

                try
                {
                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi cập nhật trạng thái lịch hẹn: " + ex.Message);
                }
            }
        }

        public List<lichHenDTO> selectByDate(DateTime ngay)
        {
            string query = "SELECT * FROM LichHen WHERE CAST(ngayHen AS DATE) = @ngay";
            List<lichHenDTO> ds = new List<lichHenDTO>();

            using (SqlConnection con = new SqlConnection(ConnectionString))
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
                            lichHenDTO lh = new lichHenDTO();
                            lh.MaLichHen = int.Parse(reader["maLichHen"].ToString());
                            lh.MaBenhNhan = int.Parse(reader["maBenhNhan"].ToString());
                            lh.MaTaiKhoan = int.Parse(reader["maTaiKhoan"].ToString());
                            lh.MaDieuDuong = int.Parse(reader["maDieuDuong"].ToString());
                            lh.NgayHen = DateTime.Parse(reader["ngayHen"].ToString());
                            lh.TrangThai = reader["trangThai"].ToString();
                            ds.Add(lh);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi lấy lịch hẹn theo ngày: " + ex.Message);
                }
            }
            return ds;
        }
    }
}
