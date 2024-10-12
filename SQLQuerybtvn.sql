-- Tạo cơ sở dữ liệu qlysinhvien
CREATE DATABASE qlysinhvien;

-- Sử dụng cơ sở dữ liệu qlysinhvien
USE qlysinhvien;

-- Tạo bảng Faculty
CREATE TABLE Faculty (
    FacultyID INT PRIMARY KEY,
    FacultyName NVARCHAR(100) NOT NULL
);

-- Tạo bảng Major
CREATE TABLE Major (
    MajorID INT PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    FacultyID INT,
    FOREIGN KEY (FacultyID) REFERENCES Faculty(FacultyID)
);

-- Tạo bảng Student
CREATE TABLE Student (
    StudentID INT PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    AverageScore FLOAT,
    FacultyID INT,
    MajorID INT,
    Avatar NVARCHAR(255),
    FOREIGN KEY (FacultyID) REFERENCES Faculty(FacultyID),
    FOREIGN KEY (MajorID) REFERENCES Major(MajorID)
);
SELECT *
FROM Faculty

INSERT INTO Faculty (FacultyID, FacultyName) VALUES (1, N'Công nghệ thông tin');
INSERT INTO Faculty (FacultyID, FacultyName) VALUES (2, N'Ngành ngôn ngữ Anh');
INSERT INTO Faculty (FacultyID, FacultyName) VALUES (3, N'Marketing');

INSERT INTO Major (MajorID, Name, FacultyID) VALUES (1, N'Kỹ thuật phần mềm', 1);
INSERT INTO Major (MajorID, Name, FacultyID) VALUES (2, N'Mạng máy tính', 1);
INSERT INTO Major (MajorID, Name, FacultyID) VALUES (3, N'Ngôn ngữ Anh', 2);
INSERT INTO Major (MajorID, Name, FacultyID) VALUES (4, N'Văn hóa Anh', 2);
INSERT INTO Major (MajorID, Name, FacultyID) VALUES (5, N'Tiếp thị', 3);
INSERT INTO Major (MajorID, Name, FacultyID) VALUES (6, N'Quản trị kinh doanh', 3);

INSERT INTO Student (StudentID, FullName, AverageScore, FacultyID, MajorID)
VALUES
(1, N'Nguyễn Văn A', 8.5, 1, 1),  -- Sinh viên 1
(2, N'Trần Thị B', 7.0, 1, 2),     -- Sinh viên 2
(3, N'Phạm Văn C', 9.2, 2, 1),     -- Sinh viên 3
(4, N'Lê Thị D', 6.8, 2, 3);   

SELECT * FROM Student