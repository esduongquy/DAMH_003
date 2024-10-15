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
using System.Windows.Forms.DataVisualization.Charting;

namespace DAMH_02
{
    public partial class FormKQHT : Form
    {
        //Thêm biến toàn cục để lưu mã sinh viên
        private string maSinhVien; // Biến lưu mã sinh viên

        // Hàm khởi tạo nhận mã sinh viên
        public FormKQHT(string maSV)
        {
            InitializeComponent();
            maSinhVien = maSV;
            LoadKetQuaHocTap(); // Gọi hàm để tải dữ liệu lịch sử học tập
        }
        // Hàm để tải dữ liệu lịch sử học tập
        private void LoadKetQuaHocTap()
        {
            try
            {
                string connectionString = @"Data Source=LAPTOP-8MJ97B04;Initial Catalog=DAMH03;Integrated Security=True";
                // Truy vấn để lấy dữ liệu từ bảng KetQuaHocTapMonHoc và MonHoc
                string query = @"
                    SELECT 
                        KetQuaHocTapMonHoc.KyHoc,
                        MonHoc.TenMH,
                        KetQuaHocTapMonHoc.DiemThi,
                        KetQuaHocTapMonHoc.DiemTrungBinh
                    FROM 
                        KetQuaHocTapMonHoc
                    JOIN 
                        MonHoc ON KetQuaHocTapMonHoc.MaMH = MonHoc.MaMH
                    WHERE 
                        KetQuaHocTapMonHoc.MaSV = @MaSV";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    da.SelectCommand.Parameters.AddWithValue("@MaSV", maSinhVien);

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataKetQuaHocTap.DataSource = dt;
                    // Tùy chỉnh tiêu đề các cột
                    dataKetQuaHocTap.Columns["KyHoc"].HeaderText = "Kỳ Học";
                    dataKetQuaHocTap.Columns["TenMH"].HeaderText = "Môn Học";
                    dataKetQuaHocTap.Columns["DiemThi"].HeaderText = "Điểm Thi";
                    dataKetQuaHocTap.Columns["DiemTrungBinh"].HeaderText = "Điểm Trung Bình Môn";
                    // Tự động điều chỉnh kích thước cột
                    dataKetQuaHocTap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    // Căn chỉnh dữ liệu giữa các cột
                    dataKetQuaHocTap.Columns["KyHoc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataKetQuaHocTap.Columns["TenMH"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataKetQuaHocTap.Columns["DiemThi"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataKetQuaHocTap.Columns["DiemTrungBinh"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    // Điều chỉnh chiều cao hàng
                    dataKetQuaHocTap.RowTemplate.Height = 30;
                    // Ngăn người dùng thêm hàng mới
                    dataKetQuaHocTap.AllowUserToAddRows = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi tải dữ liệu: " + ex.Message);
            }
        }

        public FormKQHT()
        {
            InitializeComponent();
        }

        private void FormKQHT_Load(object sender, EventArgs e)
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

        private void dashboard_btn_Click(object sender, EventArgs e)
        {
            // Tạo đối tượng FormTTSV và truyền mã sinh viên vào constructor
            FormTTSV formTTSV = new FormTTSV(maSinhVien.ToString()); // Truyền mã sinh viên
            formTTSV.Show();

            // Ẩn form hiện tại (FormLSHT)
            this.Hide();
        }

        private void salary_btn_Click(object sender, EventArgs e)
        {
            // Tạo đối tượng FormLSHT và hiển thị
            FormLSHT formLSHT = new FormLSHT(maSinhVien.ToString());
            formLSHT.Show();

            // Ẩn form hiện tại (FormLSHT)
            this.Hide();
        }

        private void btnPhanTich_Click(object sender, EventArgs e)
        {
            // Tạo đối tượng FormPT và truyền mã sinh viên vào constructor
            FormPT formPT = new FormPT(maSinhVien.ToString());
            formPT.Show();

            // Ẩn form hiện tại (FormKQHT)
            this.Hide();
        }
    }
}