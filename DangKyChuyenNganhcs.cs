using BUS._12;
using DAL._21.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI._2_10_10_
{
    public partial class DangKyChuyenNganh : Form
    {
        private readonly FacultyService facultyService;
        private readonly StudentService studentService;
        private readonly MajorService majorService;

        public DangKyChuyenNganh()
        {
            InitializeComponent();
            studentService = new StudentService(); // Khởi tạo dịch vụ sinh viên
            facultyService = new FacultyService(); // Khởi tạo dịch vụ khoa
            majorService = new MajorService();
        }

        private void LoadFaculties()
        {
            try
            {
                var faculties = facultyService.GetAll(); // Lấy danh sách khoa từ dịch vụ
                if (faculties != null && faculties.Count > 0)
                {
                    cmbKhoa.DataSource = faculties; // Thiết lập nguồn dữ liệu cho ComboBox
                    cmbKhoa.DisplayMember = "FacultyName"; // Tên hiển thị
                    cmbKhoa.ValueMember = "FacultyID"; // Giá trị
                    cmbKhoa.SelectedIndex = -1; // Không chọn khoa nào ban đầu
                }
                else
                {
                    MessageBox.Show("Không có khoa nào trong cơ sở dữ liệu.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách khoa: " + ex.Message);
            }
        }

        private void LoadStudents(int facultyId) // Phương thức này nhận một tham số
        {
            try
            {
                // Lấy danh sách sinh viên thuộc khoa đã chọn và không có chuyên ngành
                List<Student> students = studentService.getAll()
                    .Where(s => s.FacultyID == facultyId && s.MajorID == null)
                    .ToList();

                if (students != null && students.Count > 0)
                {
                    // Thiết lập nguồn dữ liệu cho DataGridView
                    // Xóa tất cả cột hiện có trước khi thêm cột mới
                    dataGridView2.Columns.Clear();

                    // Thêm cột Select
                    DataGridViewCheckBoxColumn selectColumn = new DataGridViewCheckBoxColumn();
                    selectColumn.Name = "Select";
                    selectColumn.HeaderText = "Select";
                    dataGridView2.Columns.Add(selectColumn);

                    dataGridView2.DataSource = students.Select(s => new
                    {
                        s.StudentID,
                        s.FullName,
                        s.AverageScore,
                    }).ToList();

                   

                    // Gán giá trị cho cột "Select"
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        row.Cells["Select"].Value = false; // Gán giá trị mặc định cho cột Select
                    }
                }
                else
                {
                    MessageBox.Show("Không có sinh viên nào trong khoa này hoặc tất cả đã có chuyên ngành.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sinh viên: " + ex.Message);
            }
        }
        private void LoadMajors(int facultyId)
        {
            try
            {
                // Lấy danh sách chuyên ngành theo ID khoa đã chọn
                List<Major> majors = majorService.GetAll().Where(m => m.FacultyID == facultyId).ToList();

                // Kiểm tra nếu có chuyên ngành
                if (majors.Any())
                {
                    cmbChuyenNganh.DataSource = majors; // Thiết lập nguồn dữ liệu cho ComboBox chuyên ngành
                    cmbChuyenNganh.DisplayMember = "Name"; // Tên hiển thị
                    cmbChuyenNganh.ValueMember = "MajorID"; // Giá trị
                }
                else
                {
                    // Xóa dữ liệu trong ComboBox nếu không có chuyên ngành
                    cmbChuyenNganh.DataSource = null;
                    MessageBox.Show("Không có chuyên ngành nào cho khoa này.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách chuyên ngành: " + ex.Message);
            }
        }
        private void DangKyChuyenNganh_Load(object sender, EventArgs e)
        {

            LoadFaculties();
        }

        private void cmbKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbKhoa.SelectedItem != null)
            {
                // Lấy đối tượng khoa đã chọn
                var selectedFaculty = cmbKhoa.SelectedItem as Faculty;
                if (selectedFaculty != null)
                {
                    // Lấy FacultyID từ đối tượng
                    int facultyId = selectedFaculty.FacultyID;

                    try
                    {
                        LoadMajors(facultyId); // Tải danh sách chuyên ngành tương ứng
                        LoadStudents(facultyId); // Gọi LoadStudents với facultyId đã chọn
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                    }
                }
            }
            else if (cmbKhoa.Items.Count == 0)
            {
                // Nếu không có khoa nào được nạp
                MessageBox.Show("Không có khoa nào để chọn.");
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                // Nếu checkbox được chọn
                if (Convert.ToBoolean(row.Cells["Select"].Value))
                {
                    int studentId = Convert.ToInt32(row.Cells["StudentID"].Value); // Lấy StudentID
                    int selectedMajorId = (int)cmbChuyenNganh.SelectedValue; // Lấy MajorID từ ComboBox

                    // Gọi phương thức cập nhật chuyên ngành cho sinh viên
                    studentService.UpdateMajor(studentId, selectedMajorId); // Giả sử bạn đã có phương thức UpdateMajor trong StudentService
                }
            }

            // Thông báo cập nhật thành công
            MessageBox.Show("Cập nhật chuyên ngành thành công cho sinh viên đã chọn.");
        }
       
    }
}
