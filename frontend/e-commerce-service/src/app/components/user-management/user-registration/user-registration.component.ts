import { Component } from '@angular/core';
import { UserRequestDTO, UserResponseDTO, UserService } from '../../../generated';
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
  public userRequest: UserRequestDTO = {
    userName: '',
    password: '',
    email: '',
    role: 'User',
  };
  public confirmPassword: string = '';
  public hidePassword: boolean = true;
  public hideConfirmPassword: boolean = true;

  public faGoogle: IconDefinition = faGoogle;
  public faApple: IconDefinition = faApple;
  public faMicrosoft: IconDefinition = faMicrosoft;
  public faUserPlus: IconDefinition = faUserPlus;
  public faEye: IconDefinition = faEye;
  public faEyeSlash: IconDefinition = faEyeSlash;

  constructor(private router: Router, private userService: UserService) {}

  handleLogoClick(): void {
    this.router.navigate(['/']);
  }

  handleRegisterClick(): void {
    if (this.userRequest.password.length == 0 || this.confirmPassword.length == 0) {
      alert("Password fields empty!");
      return;
    }

    if (this.userRequest.password !== this.confirmPassword) {
      alert("Passwords do not match!");
      return;
    }

    this.userService.apiV1UsersRegisterPost(this.userRequest).subscribe(
      (data: UserResponseDTO) => {
        console.log('Registered user', data);
        // Simulate a registration process
        // You would typically have a service to handle the actual registration process
        // For now, we will just set the isLoggedIn flag in local storage
        localStorage.setItem('isLoggedIn', 'true');
        this.router.navigate(['/']);
      },
      error => {
        console.error('Error registering user', error);
      }
    ); 
  }

  togglePasswordVisibility(): void {
    this.hidePassword = !this.hidePassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.hideConfirmPassword = !this.hideConfirmPassword;
  }
}
