import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faSearch, faShoppingCart, faSignIn } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, FormsModule, FontAwesomeModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  public faSearch: IconDefinition = faSearch;
  public faShoppingCart: IconDefinition = faShoppingCart;
  public faSignIn: IconDefinition = faSignIn;

  public searchText: string = ""; 
  public isLoggedIn: boolean = false;

  constructor(private router: Router) {}

  handleLogoClick(): void {
    this.router.navigate(['/']);
  }
  handleLoginClick(): void {
    this.router.navigate(['/user/login']);
  }

  handleLogoutClick(): void {
    console.log('Handle logout');
  }

  handleCartClick(): void {
    console.log('Navigating to cart');
  }
}
