import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faSearch, faSignIn } from '@fortawesome/free-solid-svg-icons';
import { CartResponseDTO, CartService } from '../../../../generated';

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

  constructor(private router: Router, public auth: AuthService, private cartService: CartService) {}

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;
    } else {
      this.isLoggedIn = false;
    }
  }

  handleLogo(): void {
    this.router.navigate(['/']);
  }

  handleLogin(): void {
    this.auth.loginWithRedirect();
  }

  handleLogout(): void {
    const cartId = localStorage.getItem('cartId')?.toString();
    this.cartService.apiV1CartsCartIdDelete(cartId!).subscribe(
      (data: CartResponseDTO) => {
        localStorage.setItem("cartId", data.cartID!.toString());
      },
      error => {
        console.error('Error deleting cart', error);
      }
    );
    this.auth.logout();
    localStorage.removeItem('isLoggedIn');
    localStorage.removeItem('accessToken');
    localStorage.removeItem('cartId');
    this.isLoggedIn = false;
    this.router.navigate(['/']);
  }
}
