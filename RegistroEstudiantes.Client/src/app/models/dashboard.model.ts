export interface CourseResponse {
  id: number;
  name: string;
  professorId: number;
  professorName: string;
  credits: number;
  classmates: string[];
}


export interface MyDashboard {
  studentName?: string;
  totalCredits: number;
  enrolledCoursesCount: number;
  myCourses: CourseResponse[];
}