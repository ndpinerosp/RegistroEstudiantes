import { Component, inject, OnInit, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EnrollmentService } from '../../services/enrollment.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTabsModule } from '@angular/material/tabs';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { CoursesListComponent } from '../courses-list/courses-list.component';
import { StudentDirectoryComponent } from '../student-directory/student-directory.component';
import { CourseEnrollmentComponent } from '../course-enrollment/course-enrollment.component';

@Component({
  selector: 'app-dashboardhome',
  standalone: true,
  imports: [CommonModule, MatSnackBarModule, MatTabsModule, MatProgressSpinnerModule,
    MatIconModule, CoursesListComponent, StudentDirectoryComponent, CourseEnrollmentComponent],
  templateUrl: './dashboardhome.component.html',
  styleUrl: './dashboardhome.component.css'
})
export class DashboardhomeComponent implements OnInit {
  private enrollmentService = inject(EnrollmentService);
  private snackBar = inject(MatSnackBar);

  publicProgress = this.enrollmentService.publicData;
  allCourses = this.enrollmentService.allCourses;
  dashboard = this.enrollmentService.dashboardData;
  loading = signal<boolean>(false);

  enrollmentCount = computed(() => this.dashboard()?.enrolledCoursesCount || 0);
  showNoCourses = computed(() => !this.loading() && (this.dashboard()?.myCourses?.length || 0) === 0);
  showNoDirectory = computed(() => !this.loading() && this.publicProgress().length === 0);
  showNoAvailable = computed(() => !this.loading() && this.allCourses().length === 0);

  ngOnInit(): void {
    this.loadActiveTabData(0);
  }

  onTabChange(event: any) {
    this.loadActiveTabData(event.index);
  }

  private loadActiveTabData(index: number) {
    let observable: any = null;
    switch (index) {
      case 0: if (!this.dashboard()?.myCourses?.length) observable = this.enrollmentService.loadDashboard(); break;
      case 1: if (!this.publicProgress().length) observable = this.enrollmentService.loadPublicData(); break;
      case 2: if (!this.allCourses().length) observable = this.enrollmentService.loadAllCourses(); break;
    }

    if (observable) {
      this.loading.set(true);
      observable.subscribe({
        next: () => this.loading.set(false),
        error: (err: any) => {
          this.loading.set(false);
          this.showSnackBar(err.error?.message || 'Error cargando datos');
        }
      });
    }
  }

  enroll(course: any) {
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

    this.enrollmentService.enroll(course.id).subscribe({
      next: () => {
        this.showSnackBar(`Inscrito en ${course.name}`);
        this.enrollmentService.loadDashboard().subscribe();
      },
      error: (err) => this.showSnackBar(err.error?.message || 'Error al inscribir')
    });
  }

  cancelEnroll(courseId: any) {
    this.enrollmentService.cancelEnrollment(courseId).subscribe({
      next: () => {
        this.showSnackBar('Inscripción cancelada con éxito');
        this.enrollmentService.loadDashboard().subscribe();
      },
      error: (err) => this.showSnackBar(err.error?.message || 'Error al cancelar')
    });
  }

  private showSnackBar(message: string) {
    this.snackBar.open(message, 'Cerrar', { duration: 3000 });
  }
}
