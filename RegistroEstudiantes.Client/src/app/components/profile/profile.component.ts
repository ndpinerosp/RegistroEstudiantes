import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatIconModule, MatFormFieldModule,
    MatDialogModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {

  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private dialog = inject(MatDialog);

  profileForm!: FormGroup;
  message = '';
  isError = false;

  ngOnInit(): void {
    const user = this.authService.currentUser();
    const onlyLettersPattern = '^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$';
    

    this.profileForm = this.fb.group({
      name: [user?.studentName || '', [Validators.required,Validators.pattern(onlyLettersPattern)]],
      lastName: [user?.studentLastName || '', [Validators.required,Validators.pattern(onlyLettersPattern)]],
      email: [user?.email || '', [Validators.required, Validators.email]],
    });
  }

  onUpdate() {
    if (this.profileForm.invalid) return;

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Confirmar Actualización',
        message: '¿Estás seguro de que desea modificar tus datos de perfil?',
        confirmText: 'Actualizar',
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.executeUpdate();
      }
    });
  }

  onDelete() {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Eliminacion de cuenta',
        message: 'Esta acción borrará tu cuenta para siempre. ¿Proceder?',
        confirmText: 'Sí, eliminar cuenta',
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.executeDelete();
      }
    });
  }

  private executeUpdate() {
    this.authService.updateProfile(this.profileForm.value).subscribe({
      next: () => {
        this.message = 'Perfil actualizado correctamente';
        this.isError = false;
      },
      error: (err) => {
        this.message = err.error?.message || 'Error al actualizar';
        this.isError = true;
      }
    });
  }

  private executeDelete() {
    this.authService.deleteAccount().subscribe({
      next: () => { },
      error: (err) => {
        this.message = 'No se pudo eliminar la cuenta';
        this.isError = true;
      }
    });
  }
}
