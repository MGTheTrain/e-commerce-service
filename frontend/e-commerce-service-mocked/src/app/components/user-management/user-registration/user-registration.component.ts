import { Component } from '@angular/core';
import { UserResponseDTO } from '../../../generated';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faUserPlus, faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { faApple, faGoogle, faMicrosoft } from '@fortawesome/free-brands-svg-icons';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-registration',
  standalone: true,
  imports: [FormsModule, CommonModule, FontAwesomeModule],
  templateUrl: './user-registration.component.html',
  styleUrls: ['./user-registration.component.css']
})
export class UserRegistrationComponent {
  public user: UserResponseDTO = {
    userName: '',
    email: '',
  };
  public password: string = '';
  public confirmPassword: string = '';
  public hidePassword: boolean = true;
  public hideConfirmPassword: boolean = true;

  public faGoogle: IconDefinition = faGoogle;
  public faApple: IconDefinition = faApple;
  public faMicrosoft: IconDefinition = faMicrosoft;
  public faUserPlus: IconDefinition = faUserPlus;
  public faEye: IconDefinition = faEye;
  public faEyeSlash: IconDefinition = faEyeSlash;

  constructor(private router: Router) {}

  handleLogoClick(): void {
    this.router.navigate(['/']);
  }

  onRegister(): void {
    // Simulate a registration process
    // You would typically have a service to handle the actual registration process
    // For now, we will just set the isLoggedIn flag in local storage
    localStorage.setItem('isLoggedIn', 'true');
    this.router.navigate(['/']);
  }

  togglePasswordVisibility(): void {
    this.hidePassword = !this.hidePassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.hideConfirmPassword = !this.hideConfirmPassword;
  }
}
