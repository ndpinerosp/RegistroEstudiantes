import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { EnrollmentService } from './enrollment.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'auth_token';
  private readonly USER_KEY = 'user_data';
  private readonly API_URL = 'api/auth';
  private enrollmentService = inject(EnrollmentService)
  private router = inject(Router);

  currentUser = signal<any>(JSON.parse(localStorage.getItem(this.USER_KEY) || 'null'));
  isAuthenticated = computed(() => !!this.currentUser());

  constructor(private http: HttpClient) { }

  login(credentials: any) {
    return this.http.post<any>('api/auth/login', credentials).pipe(
      tap(res => {
        localStorage.setItem(this.TOKEN_KEY, res.token);
        localStorage.setItem(this.USER_KEY, JSON.stringify(res));
        this.currentUser.set(res);
      })
    );
  }

  updateProfile(userData: any) {
    return this.http.put<any>(`${this.API_URL}/update-profile`, userData).pipe(
      tap(res => {
        const currentData = this.currentUser();

        //Actualiza el nombre en la pagina
        const updatedData = {
          ...currentData,
          studentName: userData.name,
          studentLastName: userData.lastName,
          email: userData.email
        };

        localStorage.setItem(this.USER_KEY, JSON.stringify(updatedData));

        this.currentUser.set(updatedData);
      })
    );
  }

  deleteAccount() {
    return this.http.delete<any>(`${this.API_URL}/delete-account`).pipe(
      tap(() => {
        this.logout();
      })
    );
  }

  logout(expired: boolean = false) {
    localStorage.clear();
    this.enrollmentService.clearData();
    this.currentUser.set(null);
    this.router.navigate(['/login']);

    return expired;
  }

  getToken() {
    return localStorage.getItem(this.TOKEN_KEY);
  }
  
  checkSession() {
    const token = this.getToken();
    if (!token)
      return;

    try {
      const decoded: any = jwtDecode(token);
      const now = Math.floor(Date.now() / 1000);

      if (decoded.exp < now) {
        this.logout(true);
      }
    } catch (e) {
      this.logout();
    }
  }
}
