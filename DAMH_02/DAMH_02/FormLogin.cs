using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAMH_02
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = @"Data Source=LAPTOP-8MJ97B04;Initial Catalog=DAMH02;Integrated Security=True";
                string query = "SELECT MaSV FROM SinhVien WHERE Username = @Username AND Password = @Password";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                    con.Open();
                    var maSinhVien = cmd.ExecuteScalar(); // Lấy mã sinh viên nếu đăng nhập thành công

                    if (maSinhVien != null)
                    {
                        // Đăng nhập thành công, mở form lịch sử học tập và truyền mã sinh viên
                        FormLSHT lichSuHocTapForm = new FormLSHT(maSinhVien.ToString());
                        lichSuHocTapForm.Show();
                        this.Hide(); // Ẩn form login sau khi đăng nhập thành công
                    }
                    //if (maSinhVien != null)
                    //{
                    //    // Đăng nhập thành công, mở form thông tin sinh viên và truyền mã sinh viên
                    //    FormTTSV thongTinSinhVienForm = new FormTTSV(maSinhVien.ToString());
                    //    thongTinSinhVienForm.Show();
                    //    this.Hide(); // Ẩn form login sau khi đăng nhập thành công
                    //}
                    else
                    {
                        // Đăng nhập thất bại
                        MessageBox.Show("Tài khoản hoặc mật khẩu không đúng.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
        }
    }
}
