import { Component, HostListener, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService } from './services/auth.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, MatSnackBarModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Registro de Materias';

  private authService = inject(AuthService);
  private snackBar = inject(MatSnackBar);

  @HostListener('window:focus', [])
  onFocus() {
    // Si el usuario está logueado, verificamos la sesión
    if (this.authService.isAuthenticated()) {
      this.checkAndNotify();
    }
  }
  private checkAndNotify() {
    const token = this.authService.getToken();
    if (!token) 
      return;
    this.authService.checkSession();
    
    if (!this.authService.isAuthenticated()) {
      this.snackBar.open('Su sesión ha expirado. Por favor, ingrese de nuevo.', 'Cerrar', {
        duration: 5000,
        verticalPosition: 'top',
        panelClass: ['warn-snackbar']
      });
    }
  }
}
