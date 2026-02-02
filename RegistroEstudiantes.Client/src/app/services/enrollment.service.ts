import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { tap } from 'rxjs';
import { MyDashboard } from '../models/dashboard.model';
import { StudentData } from '../models/progress.model';

@Injectable({
  providedIn: 'root'
})
export class EnrollmentService {
  private http = inject(HttpClient);

  // Estados reactivos
  allCourses = signal<any[]>([]);
  dashboardData = signal<MyDashboard | null>(null);
  publicData = signal<StudentData[]>([]);

  loadAllCourses() {
    return this.http.get<any[]>('/api/courses').pipe(
      tap(courses => this.allCourses.set(courses))
    );
  }

  loadDashboard() {
    return this.http.get<MyDashboard>('/api/enrollment/my-dashboard').pipe(
      tap(data => this.dashboardData.set(data))
    );
  }

  loadPublicData() {
    return this.http.get<StudentData[]>('/api/enrollment/public').pipe(
      tap(data => this.publicData.set(data))
    );
  }

  enroll(courseId: number) {
    return this.http.post('/api/enrollment/enroll', { courseId });
  }

  cancelEnrollment(courseId: number) {
    return this.http.delete(`api/enrollment/unenroll/${courseId}`);
  }

  clearData() {
    this.allCourses.set([]);
    this.dashboardData.set(null);
    this.publicData.set([]);
  }

}