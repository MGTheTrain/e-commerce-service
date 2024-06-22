import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faApple, faGoogle, faMicrosoft } from '@fortawesome/free-brands-svg-icons';
import { faSignInAlt } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-user-login',
  standalone: true,
  imports: [ CommonModule, FormsModule, FontAwesomeModule ],
  templateUrl: './user-login.component.html',
  styleUrl: './user-login.component.css'
})
export class UserLoginComponent {
  email: string = '';
  password: string = '';
  public faSignInAlt: IconDefinition = faSignInAlt;
  public faMicrosoft: IconDefinition = faMicrosoft;
  public faApple: IconDefinition = faApple;
  public faGoogle: IconDefinition = faGoogle;

  onLogin(): void {
    console.log('Logging in with', this.email, this.password);
  }

  onSSOLogin(provider: string): void {
    console.log('Logging in with', provider);
  }

  onForgotPassword(): void {
    console.log('Redirecting to forgot password page');
  }

  onRegister(): void {
    console.log('Redirecting to register page');
  }
}
