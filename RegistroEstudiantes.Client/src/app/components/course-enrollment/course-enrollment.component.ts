import { Component, inject, computed, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { CommonModule } from '@angular/common'; // Importante para el *ngFor
import { EnrollmentService } from '../../services/enrollment.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-course-enrollment',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule, MatChipsModule],
  templateUrl: './course-enrollment.component.html'
})
export class CourseEnrollmentComponent {
  private enrollmentService = inject(EnrollmentService);
  private snackBar = inject(MatSnackBar)
    ;
  isProcessing = signal<boolean>(false);

  // Consumimos los signals directamente del servicio
  allCourses = this.enrollmentService.allCourses;
  dashboard = this.enrollmentService.dashboardData;

  // Computadas internas
  totalCredits = computed(() => this.dashboard()?.totalCredits || 0);
  enrolledIds = computed(() => this.dashboard()?.myCourses?.map((c: any) => c.id) || []);
  enrollmentCount = computed(() => this.dashboard()?.enrolledCoursesCount || 0);


  isAlreadyEnrolled(courseId: number): boolean {
    return this.enrolledIds().includes(courseId);
  }

  onEnroll(course: any) {

    if (this.enrollmentCount() >= 3) {
      this.showSnackBar('Error: Máximo de 3 materias alcanzado.');
      return;
    }

    const isProfessorTaken = this.dashboard()?.myCourses.some(
      (c: any) => c.professorId === course.professorId
    );

    if (isProfessorTaken) {
      this.showSnackBar(`Error: Ya tienes clase con el Prof. ${course.professorName}`);
      return;
    }

    this.isProcessing.set(true);

    this.enrollmentService.enroll(course.id).subscribe({
      next: () => {
        this.showSnackBar(`Inscrito en ${course.name}`);
        this.enrollmentService.loadDashboard().subscribe({
          next: () => this.isProcessing.set(false),
          error: () => this.isProcessing.set(false)
        });
      },
      error: (err) => {
        this.showSnackBar(err.error?.message || 'Error al inscribir');
        this.isProcessing.set(false);
      }
    });
  }

  onCancel(id: number) {
    this.isProcessing.set(true);

    this.enrollmentService.cancelEnrollment(id).subscribe({
      next: () => {
        this.showSnackBar('Inscripción cancelada');
        this.enrollmentService.loadDashboard().subscribe({
          next: () => this.isProcessing.set(false),
          error: () => this.isProcessing.set(false)
        });
      },
      error: () => {
        this.isProcessing.set(false);
      }
    });
  }

  private showSnackBar(msg: string) {
    this.snackBar.open(msg, 'Cerrar', { duration: 3000 });
  }
}