CREATE DATABASE IF NOT EXISTS StudentsRegister;
USE StudentsRegister;

DROP TABLE IF EXISTS Enrollments;
DROP TABLE IF EXISTS Students;
DROP TABLE IF EXISTS Courses;
DROP TABLE IF EXISTS Professors;

CREATE TABLE Professors (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL
);

CREATE TABLE Courses (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Credits INT DEFAULT 3,
    ProfessorId INT,
    FOREIGN KEY (ProfessorId) REFERENCES Professors(Id)
);

CREATE TABLE Students (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    PasswordHash TEXT NOT NULL
);

CREATE TABLE Enrollments (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    StudentId INT,
    CourseId INT,
    EnrollmentDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (StudentId) REFERENCES Students(Id),
    FOREIGN KEY (CourseId) REFERENCES Courses(Id),
    UNIQUE KEY unique_student_course (StudentId, CourseId)
);

-- 2. SEED DATA  (5 Profesores, 10 Materias)
INSERT INTO Professors (Id, Name) VALUES 
(1, 'Juan Pérez'),
(2, 'María González'),
(3, 'Carlos Ramírez'),
(4, 'Laura Martínez'),
(5, 'Ana Torres');

INSERT INTO Courses (Id, Name, Credits, ProfessorId) VALUES 
(1, 'Fundamentos de Programación', 3, 1),
(2, 'Estructuras de Datos', 3, 1),
(3, 'Programación Orientada a Objetos', 3, 2),
(4, 'Bases de Datos', 3, 2),
(5, 'Desarrollo Web', 3, 3),
(6, 'Programación en Python', 3, 3),
(7, 'Inteligencia Artificial', 3, 4),
(8, 'Programación Avanzada en Java', 3, 4),
(9, 'Algoritmos y Optimización', 3, 5),
(10, 'Sistemas Distribuidos', 3, 5);