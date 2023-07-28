// authentication.service.ts
import { Injectable } from '@angular/core';



@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private isAuthenticated = false;



  // Simulate a login by setting isAuthenticated to true
  login() {
    this.isAuthenticated = true;
  }



  logout() {
    this.isAuthenticated = false;
  }



  isLoggedIn(): boolean {
    return this.isAuthenticated;
  }
}
