import { Component, inject, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { DashboardHeaderComponent } from '../dashboard-header/dashboard-header.component';
import { RouterOutlet } from '@angular/router';


@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, DashboardHeaderComponent, RouterOutlet],
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent {
  private authService = inject(AuthService);

  userName = computed(() => {
    const currentUser = this.authService.currentUser();
    return currentUser ? `${currentUser.studentName} ${currentUser.studentLastName}` : 'Estudiante';
  });

  onLogout() {
    this.authService.logout();
  }
}