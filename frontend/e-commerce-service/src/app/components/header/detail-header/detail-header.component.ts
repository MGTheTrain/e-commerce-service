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
  public accessToken: string | null = ''; 

  constructor(private router: Router, public auth: AuthService) {}

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;
      this.accessToken = localStorage.getItem("accessToken");
    } else {
      this.isLoggedIn = false;
      this.accessToken = '';
    }
  }

  handleLogoClick(): void {
    this.router.navigate(['/']);
  }

  handleLoginClick(): void {
    // this.router.navigate(['/user/login']);
    this.auth.loginWithRedirect();
  }

  handleLogoutClick(): void {
    this.auth.logout();
    // Simulate a logout process
    // You would typically have a service to handle the actual logout process
    // For now, we will just remove the isLoggedIn flag from local storage
    localStorage.removeItem('isLoggedIn');
    this.isLoggedIn = false;
    this.router.navigate(['/']);
  }
}
