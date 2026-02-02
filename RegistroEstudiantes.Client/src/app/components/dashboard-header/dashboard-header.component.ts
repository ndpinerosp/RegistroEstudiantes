import { Component, input, output } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-dashboard-header',
  standalone: true,
  imports: [MatToolbarModule, MatButtonModule, MatIconModule, MatTooltipModule, RouterModule],
  templateUrl: './dashboard-header.component.html'
})
export class DashboardHeaderComponent {
  userName = input.required<string>();
  logout = output<void>();

  onLogout() {
    this.logout.emit();
  }
}