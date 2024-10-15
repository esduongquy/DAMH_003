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
    public partial class FormLSHT : Form
    {
        private string maSinhVien; // Biến lưu mã sinh viên

        // Hàm khởi tạo nhận mã sinh viên
        public FormLSHT(string maSV)
        {
            InitializeComponent();
            maSinhVien = maSV;
            LoadLichSuHocTap(); // Gọi hàm để tải dữ liệu lịch sử học tập
        }

        // Hàm để tải dữ liệu lịch sử học tập
        private void LoadLichSuHocTap()
        {
            try
            {
                string connectionString = @"Data Source=LAPTOP-8MJ97B04;Initial Catalog=DAMH04;Integrated Security=True";
                string query = "SELECT KyHoc, DiemTrungBinhHocKy, DiemTichLuy FROM LichSuHocTap WHERE MaSV = @MaSV";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    da.SelectCommand.Parameters.AddWithValue("@MaSV", maSinhVien);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    // Tùy chỉnh tiêu đề các cột
                    dataGridView1.Columns["KyHoc"].HeaderText = "Kỳ Học";
                    dataGridView1.Columns["DiemTrungBinhHocKy"].HeaderText = "Điểm Trung Bình Học Kỳ";
                    dataGridView1.Columns["DiemTichLuy"].HeaderText = "Điểm Tích Lũy";

                    // Tự động điều chỉnh kích thước cột
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    // Căn chỉnh dữ liệu giữa các cột
                    dataGridView1.Columns["KyHoc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView1.Columns["DiemTrungBinhHocKy"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView1.Columns["DiemTichLuy"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    // Điều chỉnh chiều cao hàng
                    dataGridView1.RowTemplate.Height = 30;

                    // Ngăn người dùng thêm hàng mới
                    dataGridView1.AllowUserToAddRows = false;
                    // Kiểm tra nếu có dữ liệu
                    if (dt.Rows.Count > 0)
                    {
                        // Lấy giá trị điểm tích lũy kỳ học mới nhất (kỳ học cuối cùng)
                        decimal diemTichLuyMoiNhat = Convert.ToDecimal(dt.Rows[dt.Rows.Count - 1]["DiemTichLuy"]);

                        // Đưa ra nhận xét dựa trên điểm tích lũy
                        if (diemTichLuyMoiNhat < 4)
                        {
                            txtNhanXet.Text = "Kém";
                        }
                        else if (diemTichLuyMoiNhat < 5 && diemTichLuyMoiNhat >= 4)
                        {
                            txtNhanXet.Text = "Yếu";
                        }
                        else if (diemTichLuyMoiNhat >= 5 && diemTichLuyMoiNhat < 7)
                        {
                            txtNhanXet.Text = "Trung bình";
                        }
                        else if (diemTichLuyMoiNhat >= 7 && diemTichLuyMoiNhat < 8.5m)
                        {
                            txtNhanXet.Text = "Khá";
                        }
                        else if (diemTichLuyMoiNhat >= 8.5m)
                        {
                            txtNhanXet.Text = "Giỏi";
                        }
                    }
                    else
                    {
                        txtNhanXet.Text = "Không có dữ liệu lịch sử học tập.";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi tải dữ liệu: " + ex.Message);
            }
        }
        public FormLSHT()
        {
            InitializeComponent();
        }

        private void FormLSHT_Load(object sender, EventArgs e)
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

        private void btnTTSV_Click(object sender, EventArgs e)
        {
            // Tạo đối tượng FormTTSV và truyền mã sinh viên vào constructor
            FormTTSV formTTSV = new FormTTSV(maSinhVien.ToString()); // Truyền mã sinh viên
            formTTSV.Show();

            // Ẩn form hiện tại (FormLSHT)
            this.Hide();
        }

        private void btnKQHT_Click(object sender, EventArgs e)
        {
            // Tạo đối tượng FormKQHT và hiển thị
            FormKQHT formKQHT = new FormKQHT(maSinhVien.ToString());
            formKQHT.Show();

            // Ẩn form hiện tại (FormLSHT)
            this.Hide();
        }
    }
}
