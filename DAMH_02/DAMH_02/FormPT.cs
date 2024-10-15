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
    public partial class FormPT : Form
    {
        private string maSinhVien;

        public FormPT(string maSV)
        {
            InitializeComponent();
            maSinhVien = maSV;
            LoadMonHoc(); // Gọi phương thức để tải dữ liệu từ bảng MonHoc vào dataDSMH
        }

        // Phương thức để tải dữ liệu từ bảng MonHoc
        private void LoadMonHoc()
        {
            try
            {
                string connectionString = @"Data Source=LAPTOP-8MJ97B04;Initial Catalog=DAMH04;Integrated Security=True";
                string query = "SELECT MaMH, TenMH, SoTinChi FROM MonHoc";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataDSMH.DataSource = dt;

                    dataDSMH.Columns["MaMH"].HeaderText = "Mã Môn Học";
                    dataDSMH.Columns["TenMH"].HeaderText = "Tên Môn Học";
                    dataDSMH.Columns["SoTinChi"].HeaderText = "Số Tín Chỉ";

                    dataDSMH.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataDSMH.RowTemplate.Height = 30;
                    dataDSMH.AllowUserToAddRows = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi tải dữ liệu: " + ex.Message);
            }
        }

        private void btnPhanTich_Click(object sender, EventArgs e)
        {
            string maMonHoc = txtMaphantich.Text.Trim();

            if (string.IsNullOrEmpty(maMonHoc))
            {
                MessageBox.Show("Vui lòng nhập mã môn học.");
                return;
            }

            string connectionString = @"Data Source=LAPTOP-8MJ97B04;Initial Catalog=DAMH04;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Lấy điểm trung bình của các khóa
                string queryKhoaHoc = @"
        SELECT KhoaHoc, DiemTrungBinh 
        FROM DiemTrungBinhMonHocTheoKhoa 
        WHERE MaMH = @MaMH";

                double diemTBKhoa = 0;
                string khoaHocSinhVien = "";

                using (SqlCommand cmd = new SqlCommand(queryKhoaHoc, conn))
                {
                    cmd.Parameters.AddWithValue("@MaMH", maMonHoc);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string khoaHoc = reader["KhoaHoc"].ToString();
                            double diemTrungBinh = Convert.ToDouble(reader["DiemTrungBinh"]);

                            if (khoaHoc == "2021")
                            {
                                txtKhoa2021.Text = diemTrungBinh.ToString();
                            }
                            else if (khoaHoc == "2022")
                            {
                                txtKhoa2022.Text = diemTrungBinh.ToString();
                            }
                            else if (khoaHoc == "2023")
                            {
                                txtKhoa2023.Text = diemTrungBinh.ToString();
                            }

                            // Xác định sinh viên thuộc khóa nào dựa vào mã sinh viên
                            int maSV = Convert.ToInt32(maSinhVien);
                            if ((maSV >= 1 && maSV <= 150 && khoaHoc == "2021") ||
                                (maSV >= 151 && maSV <= 300 && khoaHoc == "2022") ||
                                (maSV >= 301 && maSV <= 450 && khoaHoc == "2023"))
                            {
                                diemTBKhoa = diemTrungBinh;
                                khoaHocSinhVien = khoaHoc; // Lưu lại khóa học của sinh viên
                            }
                        }
                    }
                }

                // Lấy điểm của chính sinh viên
                string queryDiemSinhVien = @"
        SELECT DiemTrungBinh 
        FROM KetQuaHocTapMonHoc 
        WHERE MaMH = @MaMH AND MaSV = @MaSV";

                double diemSinhVien = 0;

                using (SqlCommand cmd = new SqlCommand(queryDiemSinhVien, conn))
                {
                    cmd.Parameters.AddWithValue("@MaMH", maMonHoc);
                    cmd.Parameters.AddWithValue("@MaSV", maSinhVien);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        diemSinhVien = Convert.ToDouble(result);
                        txtDiemcuaban.Text = diemSinhVien.ToString();
                    }
                    else
                    {
                        txtDiemcuaban.Text = "N/A";  // Nếu sinh viên không có điểm cho môn này
                        return;
                    }
                }

                // Logic đánh giá
                if (diemSinhVien >= 6 && diemSinhVien < 8 && (diemTBKhoa - diemSinhVien < 0.5))
                {
                    txtDanhGia.Text = "Điểm môn học của bạn thấp hơn khá nhiều so với điểm trung bình của khóa. Bạn có thể đăng ký học cải thiện vào kỳ sau vì độ khả thi cao.";
                }
                else if (diemSinhVien >= 4 && diemSinhVien < 6)
                {
                    txtDanhGia.Text = "Điểm môn học của bạn khá thấp. Bạn có thể đăng ký học cải thiện vào kỳ sau.";
                }
                else if (diemSinhVien >= 0 && diemSinhVien < 4)
                {
                    txtDanhGia.Text = "Đã rớt! Bạn cần đăng ký học lại môn này vào các kỳ tiếp theo.";
                }
                else
                {
                    // So sánh điểm sinh viên với điểm trung bình của khóa
                    if (diemSinhVien < diemTBKhoa)
                    {
                        txtDanhGia.Text = "Điểm môn học này của bạn thấp hơn so với điểm trung bình cùng khóa.";
                    }
                    else
                    {
                        txtDanhGia.Text = "Điểm môn học này của bạn cao hơn so với điểm trung bình cùng khóa.";
                    }
                }
            }
        }
        private void btnKQHT_Click_1(object sender, EventArgs e)
        {
            // Tạo đối tượng FormKQHT và hiển thị
            FormKQHT formKQHT = new FormKQHT(maSinhVien.ToString());
            formKQHT.Show();

            // Ẩn form hiện tại (FormLSHT)
            this.Hide();
        }

        private void btnLSHT_Click(object sender, EventArgs e)
        {
            FormLSHT formLSHT = new FormLSHT(maSinhVien);
            formLSHT.Show();
            this.Hide();
        }

        private void btnTTSV_Click(object sender, EventArgs e)
        {
            FormTTSV formTTSV = new FormTTSV(maSinhVien);
            formTTSV.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            FormLogin formLogin = new FormLogin();
            formLogin.Show();
            this.Hide();
        }
    }
}
