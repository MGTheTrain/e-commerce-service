import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faApple, faGoogle, faMicrosoft } from '@fortawesome/free-brands-svg-icons';
import { faSignInAlt, faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-user-login',
  standalone: true,
  imports: [FormsModule, CommonModule, FontAwesomeModule],
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.css']
})
export class UserLoginComponent {
  public email: string = '';
  public password: string = '';
  public hidePassword: boolean = true;

  public faSignInAlt: IconDefinition = faSignInAlt;
  public faEye: IconDefinition = faEye;
  public faEyeSlash: IconDefinition = faEyeSlash;
  public faMicrosoft: IconDefinition = faMicrosoft;
  public faApple: IconDefinition = faApple;
  public faGoogle: IconDefinition = faGoogle;

  constructor(private router: Router) {}

  onLogin(): void {
    // Simulate a login process
    // You would typically have a service to handle the actual login process
    // For now, we will just set the isLoggedIn flag in local storage
    localStorage.setItem('isLoggedIn', 'true');
    this.router.navigate(['/']);
  }

  onSSOLogin(provider: string): void {
    console.log('Logging in with', provider);
  }

  onForgotPassword(): void {
    console.log('Forgot password');
  }

  onRegister(): void {
    console.log('Navigating to register');
  }

  togglePasswordVisibility(): void {
    this.hidePassword = !this.hidePassword;
  }
}
