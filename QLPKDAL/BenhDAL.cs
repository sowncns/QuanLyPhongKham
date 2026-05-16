using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using QLPKDTO;
using System.Configuration;
using System.Data;
namespace QLPKDTO
{
    public class BenhDAL
    {
        private string connectionString;
        public BenhDAL()
        {
            // Đọc chuỗi kết nối từ cấu hình ứng dụng
            connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        }
        // Thuộc tính cho chuỗi kết nối
        public string ConnectionString { get => connectionString; set => connectionString = value; }
        public bool them(benhDTO be)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                // Truyền tên Stored Procedure thay vì chuỗi lệnh INSERT văn bản
                using (SqlCommand cmd = new SqlCommand("sp_ThemBenhMoi", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure; // Khai báo kiểu thủ tục

                    // Nạp tham số đầu vào
                    cmd.Parameters.AddWithValue("@tenBenh", be.TenBenh);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        // Đẩy thông báo lỗi nghiệp vụ (ví dụ: trùng tên) từ RAISERROR của SQL lên tầng BUS/GUI
                        throw new Exception(ex.Message);
                    }
                }
            }
        }
        public bool sua(benhDTO be, int maBenhold)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_SuaBenh", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Vì database maBenh là int, ta thực hiện ép kiểu từ chuỗi mã cũ sang int
                    cmd.Parameters.AddWithValue("@maBenh", maBenhold);
                    cmd.Parameters.AddWithValue("@tenBenh", be.TenBenh);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Lỗi hệ thống khi cập nhật: " + ex.Message);
                    }
                }
            }
        }
        public bool xoa(benhDTO be)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_XoaBenh", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@maBenh", be.MaBenh);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        // Bắt lỗi nếu vi phạm ràng buộc dữ liệu từ tầng SQL thảy lên
                        throw new Exception(ex.Message);
                    }
                }
            }
        }
        public List<benhDTO> select()
        {
            List<benhDTO> lsBenh = new List<benhDTO>();

            // Truy vấn trực tiếp từ VIEW v_DanhSachBenh thay vì bảng vật lý [Benh]
            string query = "SELECT * FROM v_DanhSachBenh";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text; // View dùng lệnh văn bản để quét dữ liệu

                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                benhDTO be = new benhDTO();
                                be.MaBenh = int.Parse(reader["maBenh"].ToString());
                                be.TenBenh = reader["tenBenh"].ToString();
                                lsBenh.Add(be);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi tải danh sách bệnh: " + ex.Message);
                    }
                }
            }
            return lsBenh;
        }
        public List<benhDTO> selectByKeyWord(string sKeyword)
        {
            List<benhDTO> lsBenh = new List<benhDTO>();

            // Sử dụng câu lệnh SQL quét qua VIEW v_DanhSachBenh để tăng tốc độ truy vấn
            string query = "SELECT * FROM v_DanhSachBenh WHERE maBenh LIKE @key OR tenBenh LIKE @key";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;

                    // Sử dụng Parameters để chống lỗi bảo mật SQL Injection
                    cmd.Parameters.AddWithValue("@key", "%" + sKeyword + "%");

                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                benhDTO be = new benhDTO();
                                be.MaBenh = int.Parse(reader["maBenh"].ToString());
                                be.TenBenh = reader["tenBenh"].ToString();
                                lsBenh.Add(be);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi tìm kiếm dữ liệu bệnh: " + ex.Message);
                    }
                }
            }
            return lsBenh;
        }
    }
}
