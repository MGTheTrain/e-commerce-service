import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faSignInAlt } from '@fortawesome/free-solid-svg-icons';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [FormsModule, CommonModule, FontAwesomeModule],
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent {
  email: string = '';
  public faSignInAlt: IconDefinition = faSignInAlt;

  constructor(private router: Router) {}

  handleLogoClick(): void {
    this.router.navigate(['/']);
  }

  onSubmit(): void {
    console.log('Reset password for email:', this.email);
  }

  onLogin(): void {
    console.log('Login:', this.email);
  }

  onRegister(): void {
    console.log('Register:', this.email);
  }
}
