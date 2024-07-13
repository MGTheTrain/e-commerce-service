import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faSearch, faSignIn } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-detail-header',
  standalone: true,
  imports: [CommonModule, FormsModule, FontAwesomeModule],
  templateUrl: './detail-header.component.html',
  styleUrl: './detail-header.component.css'
})
export class DetailHeaderComponent implements OnInit {
  public faSearch: IconDefinition = faSearch;
  public faSignIn: IconDefinition = faSignIn;
  
  public isLoggedIn: boolean = false;

  constructor(private router: Router, public auth: AuthService) {}

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;
    } else {
      this.isLoggedIn = false;
    }
  }

  handleLogoClick(): void {
    this.router.navigate(['/']);
  }

  handleLoginClick(): void {
    this.auth.loginWithRedirect();
  }

  handleLogoutClick(): void {
    this.auth.logout();
    localStorage.removeItem('isLoggedIn');
    this.isLoggedIn = false;
    this.router.navigate(['/']);
  }
}
