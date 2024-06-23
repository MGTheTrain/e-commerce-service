import { Component } from '@angular/core';
import { UserResponseDTO } from '../../../generated';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faUserPlus, faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { faApple, faGoogle, faMicrosoft } from '@fortawesome/free-brands-svg-icons';

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

  onRegister(): void {
    if (this.password !== this.confirmPassword) {
      alert("Passwords do not match!");
      return;
    }
    console.log('Registering user:', this.user);
    // Add your registration logic here
  }

  togglePasswordVisibility(): void {
    this.hidePassword = !this.hidePassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.hideConfirmPassword = !this.hideConfirmPassword;
  }
}
