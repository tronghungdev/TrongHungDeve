using BUS._12;
using DAL._21.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI._2_10_10_
{
    public partial class GiaoDienNguoiDung : Form

    {
        private readonly MajorService majorService = new MajorService();
        private readonly FacultyService facultyService = new FacultyService();
        public readonly StudentService StudentSevice = new StudentService();
        public readonly StudentService StudentService = new StudentService();
        private bool isEditing = false;
        private Student currentStudent;
        public GiaoDienNguoiDung()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            LoadFacultyList();
            LoadStudentList();
       


        }
        private Image LoadImage(string path)
        {
            try
            {
                return Image.FromFile(path);
            }
            catch
            {
                return null; // Hoặc hình ảnh mặc định
            }
        }
        private void LoadFacultyList()
        {
            try
            {
                var faculties = facultyService.GetAll();
                cmbKhoa.DataSource = faculties;
                cmbKhoa.DisplayMember = "FacultyName"; // Tên hiển thị
                cmbKhoa.ValueMember = "FacultyID"; // Giá trị
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách khoa: " + ex.Message);
            }
        }
        
        private void LoadStudentList()
        {
            try
            {
                // Lấy toàn bộ danh sách sinh viên
                List<Student> students = StudentService.getAll();

                // Nếu checkbox được chọn, lọc sinh viên chưa có chuyên ngành
                if (checkBox1.Checked)
                {
                    students = students.Where(s => s.MajorID == null).ToList(); // Lọc chỉ sinh viên chưa có chuyên ngành
                }

                // Chọn danh sách hiển thị, bao gồm FacultyID
                var displayList = students.Select(s => new
                {
                    s.StudentID,
                    s.FullName,
                    s.AverageScore,
                    FacultyID = s.FacultyID, // Thêm FacultyID vào đây
                    FacultyName = s.Faculty.FacultyName, // Lấy tên khoa
                    MajorName = s.Major?.Name, // Lấy tên chuyên ngành nếu có
                    MajorID = s.MajorID // Thêm MajorID vào đây nếu cần
                }).ToList();

                // Cập nhật DataGridView
                dataGridView1.DataSource = displayList;

                // Đảm bảo cột MajorName và FacultyName luôn hiển thị
                dataGridView1.Columns["MajorName"].Visible = true;
                dataGridView1.Columns["FacultyName"].Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sinh viên: " + ex.Message);
            }
        
    }
        

        private void btnAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    txtAnh.Text = filePath;
                    pictureBox1.Image = new Bitmap(filePath); // Hiển thị hình ảnh
                }
            }
        }
        private void ClearInputFields()
        {
            txtMSSV.Clear();
            txtHoTen.Clear();
            txtDiemTB.Clear();
            txtAnh.Clear();
            pictureBox1.Image = null;
            cmbKhoa.SelectedIndex = -1; // Đặt lại giá trị ComboBox
            currentStudent = null; // Reset biến currentStudent
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string mssv = txtMSSV.Text;
            string hoTen = txtHoTen.Text;
            float diemTrungBinh;

            if (!float.TryParse(txtDiemTB.Text, out diemTrungBinh))
            {
                MessageBox.Show("Điểm trung bình không hợp lệ!");
                return;
            }

            int khoaID = (int)cmbKhoa.SelectedValue; // Lấy khoa ID từ ComboBox
            string avatarPath = txtAnh.Text; // Đường dẫn avatar từ TextBox

            // Tạo đối tượng sinh viên mới hoặc cập nhật
            Student studentToSave;

            if (currentStudent == null)
            {
                // Nếu không có sinh viên hiện tại, tạo mới
                studentToSave = new Student
                {
                    StudentID = int.Parse(mssv),
                    FullName = hoTen,
                    AverageScore = diemTrungBinh,
                    FacultyID = khoaID,
                    MajorID = null,
                    Avatar = avatarPath // Lưu đường dẫn avatar
                };

                // Gọi phương thức AddStudent
                try
                {
                    StudentService.AddStudent(studentToSave);
                    MessageBox.Show("Thêm sinh viên thành công!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm sinh viên: " + ex.Message);
                }
            }
            else
            {
                // Nếu có sinh viên hiện tại, cập nhật thông tin
                studentToSave = currentStudent;
                studentToSave.FullName = hoTen;
                studentToSave.AverageScore = diemTrungBinh;
                studentToSave.FacultyID = khoaID;
                studentToSave.Avatar = avatarPath;

                // Gọi phương thức UpdateStudent
                try
                {
                    StudentService.UpdateStudent(studentToSave);
                    MessageBox.Show("Cập nhật sinh viên thành công!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật sinh viên: " + ex.Message);
                }
            }

            LoadStudentList(); // Cập nhật lại danh sách sinh viên
            ClearInputFields(); // Xóa trường nhập liệu sau khi thêm/sửa
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            LoadStudentList(); // Cập nhật danh sách sinh viên dựa trên trạng thái của CheckBox
        }

        private void mnuChucNang_Click(object sender, EventArgs e)
        {
            DangKyChuyenNganh form2 = new DangKyChuyenNganh();
            form2.ShowDialog();
        }

        private void cmbKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                var selectedStudent = (dynamic)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                txtMSSV.Text = selectedStudent.StudentID.ToString();
                txtHoTen.Text = selectedStudent.FullName;
                txtDiemTB.Text = selectedStudent.AverageScore.ToString();

                string avatarPath = StudentService.GetAvatarPath(selectedStudent.StudentID);
                if (System.IO.File.Exists(avatarPath))
                {
                    pictureBox1.Image = Image.FromFile(avatarPath);
                }
                else
                {
                    pictureBox1.Image = null; // Hoặc hình ảnh mặc định
                }

                cmbKhoa.SelectedValue = selectedStudent.FacultyID; // Cập nhật ComboBox cho khoa

                // Cập nhật biến currentStudent
                currentStudent = new Student
                {
                    StudentID = selectedStudent.StudentID,
                    FullName = selectedStudent.FullName,
                    AverageScore = selectedStudent.AverageScore,
                    FacultyID = selectedStudent.FacultyID,
                    MajorID = selectedStudent.MajorID, // Cập nhật MajorID
                    Avatar = avatarPath // Giả sử bạn lưu đường dẫn avatar
                };
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (currentStudent == null)
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa.");
                return;
            }

            // Xác nhận xóa
            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    // Gọi phương thức xóa sinh viên
                    StudentService.DeleteStudent(currentStudent.StudentID);
                    MessageBox.Show("Đã xóa sinh viên thành công!");

                    // Tải lại danh sách sinh viên
                    LoadStudentList();
                    // Đặt currentStudent về null để xóa thông tin hiển thị
                    currentStudent = null;
                    // Xóa thông tin trên form
                    txtMSSV.Text = "";
                    txtHoTen.Text = "";
                    txtDiemTB.Text = "";
                    cmbKhoa.SelectedIndex = -1; // Đặt lại ComboBox Khoa
                    pictureBox1.Image = null; // Xóa ảnh
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa sinh viên: " + ex.Message);
                }
            }
        }
    }
}
