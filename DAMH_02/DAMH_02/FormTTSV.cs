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
    public partial class FormTTSV : Form
    {
        public FormTTSV()
        {
            InitializeComponent();
        }

        private string maSinhVien; // Lưu mã sinh viên từ form login

        // Constructor, nhận mã sinh viên
        public FormTTSV(string maSV)
        {
            InitializeComponent();
            maSinhVien = maSV;
            LoadThongTinSinhVien();
        }

        private void LoadThongTinSinhVien()
        {
            try
            {
                string connectionString = @"Data Source=LAPTOP-8MJ97B04;Initial Catalog=DAMH02;Integrated Security=True";
                string query = "SELECT MaSV, HoTen, NgaySinh, GioiTinh, LopHoc, SoDienThoai, Email, DiaChi FROM SinhVien WHERE MaSV = @MaSV";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@MaSV", maSinhVien);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Gán giá trị từ cơ sở dữ liệu vào các TextBox tương ứng
                        txtMaSinhVien.Text = reader["MaSV"].ToString();
                        txtHoTen.Text = reader["HoTen"].ToString();
                        txtNgaySinh.Text = Convert.ToDateTime(reader["NgaySinh"]).ToString("dd/MM/yyyy");
                        txtGioiTinh.Text = reader["GioiTinh"].ToString();
                        txtLopHoc.Text = reader["LopHoc"].ToString();
                        txtDienThoai.Text = reader["SoDienThoai"].ToString();
                        txtEmail.Text = reader["Email"].ToString();
                        txtDiaChi.Text = reader["DiaChi"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy thông tin sinh viên.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi tải thông tin sinh viên: " + ex.Message);
            }
        }
        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormTTSV_Load(object sender, EventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Tạo đối tượng FormLogin và hiển thị
            FormLogin formLogin = new FormLogin();
            formLogin.Show();

            // Ẩn form hiện tại (FormLSHT)
            this.Hide();
        }
    }
}
