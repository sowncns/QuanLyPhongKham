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
    public class taiKhoanDAL
    {
        private string connectionString;
        public taiKhoanDAL()
        {
            connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        }
        public string ConnectionString { get => connectionString; set => connectionString = value; }

        public bool KiemTraTonTai(taiKhoanDTO tk)
        {
            string query = "SELECT COUNT(*) FROM TaiKhoan WHERE Username = @TenDN";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TenDN", tk.Username);
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

        public bool them(taiKhoanDTO tk)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ThemTaiKhoan", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userName", tk.Username);
                    cmd.Parameters.AddWithValue("@passWord", tk.Password); // Bạn nên hash password trước khi gọi
                    cmd.Parameters.AddWithValue("@name", tk.Name);
                    cmd.Parameters.AddWithValue("@maRole", tk.MaLoai);
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi thêm tài khoản: " + ex.Message);
                    }
                }
            }
        }

        public bool sua(taiKhoanDTO tk, int maTaiKhoanold) // Changed parameter to int
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_SuaTaiKhoan", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@maTaiKhoan", maTaiKhoanold);
                    cmd.Parameters.AddWithValue("@userName", tk.Username);
                    cmd.Parameters.AddWithValue("@passWord", tk.Password);
                    cmd.Parameters.AddWithValue("@name", tk.Name);
                    cmd.Parameters.AddWithValue("@maRole", tk.MaLoai);
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi sửa tài khoản: " + ex.Message);
                    }
                }
            }
        }

        public bool xoa(taiKhoanDTO tk)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_XoaTaiKhoan", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@maTaiKhoan", tk.MaTK); // Sử dụng MaTK thay vì Username
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi xóa tài khoản: " + ex.Message);
                    }
                }
            }
        }

        public List<taiKhoanDTO> select()
        {
            List<taiKhoanDTO> lsTK = new List<taiKhoanDTO>();
            string query = "SELECT * FROM v_DanhSachTaiKhoan"; // Sử dụng View

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
                                taiKhoanDTO tk = new taiKhoanDTO();
                                tk.Name = reader["name"].ToString();
                                tk.Username = reader["userName"].ToString();
                                // Mật khẩu có thể không trả về trong View v_DanhSachTaiKhoan tùy thiết kế
                                // Nếu View không trả về, gán chuỗi rỗng hoặc xử lý phù hợp
                                tk.Password = reader["passWord"] != DBNull.Value ? reader["passWord"].ToString() : "";
                                tk.MaLoai = int.Parse(reader["maRole"].ToString());
                                tk.MaTK = int.Parse(reader["maTaiKhoan"].ToString());
                                lsTK.Add(tk);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi tải danh sách tài khoản: " + ex.Message);
                    }
                }
            }
            return lsTK;
        }

        public List<taiKhoanDTO> selectByKeyWord(string sKeyword)
        {
            List<taiKhoanDTO> lsTaiKhoan = new List<taiKhoanDTO>();
            string query = "SELECT * FROM v_DanhSachTaiKhoan WHERE name LIKE @key OR userName LIKE @key"; // Sử dụng View

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
                                taiKhoanDTO tk = new taiKhoanDTO();
                                tk.MaTK = int.Parse(reader["maTaiKhoan"].ToString());
                                tk.Username = reader["userName"].ToString();
                                tk.Password = reader["passWord"] != DBNull.Value ? reader["passWord"].ToString() : "";
                                tk.Name = reader["name"].ToString();
                                tk.MaLoai = int.Parse(reader["maRole"].ToString());
                                lsTaiKhoan.Add(tk);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi tìm kiếm tài khoản: " + ex.Message);
                    }
                }
            }
            return lsTaiKhoan;
        }


    }
}
