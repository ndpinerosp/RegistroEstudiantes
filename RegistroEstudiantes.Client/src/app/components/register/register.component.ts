import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-register',
  imports: [CommonModule, ReactiveFormsModule, RouterModule,
    MatCardModule, MatInputModule, MatButtonModule, MatFormFieldModule,
    MatSnackBarModule, MatProgressSpinnerModule, MatIconModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})


export class RegisterComponent {
  private fb = inject(FormBuilder);
  private http = inject(HttpClient);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);
  private readonly soloLetras = '^[a-zA-ZÁ-ÿ ]*$';

  isLoading = signal(false);


  registerForm: FormGroup = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.pattern(this.soloLetras)]],
    lastName: ['', [Validators.required, Validators.minLength(3), Validators.pattern(this.soloLetras)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });


  onSubmit() {
    if (this.registerForm.valid) {
      this.isLoading.set(true);

      this.http.post('/api/auth/register', this.registerForm.value).subscribe({
        next: () => {
          this.snackBar.open('Registro exitoso. ¡Ya puedes iniciar sesión!', 'Cerrar', { duration: 4000 });
          this.router.navigate(['/login']);
        },
        error: (err) => {
          this.isLoading.set(false);
          const msg = err.error?.message || 'Error al registrarse. Posiblemente el email ya existe.';
          this.snackBar.open(msg, 'Cerrar', { duration: 5000 });
        }
      });
    }
  }
}
