import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
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
export class HeaderComponent implements OnInit {
  public faSearch: IconDefinition = faSearch;
  public faShoppingCart: IconDefinition = faShoppingCart;
  public faSignIn: IconDefinition = faSignIn;

  public searchText: string = ""; 
  @Output() isLoggedInChange: EventEmitter<boolean> = new EventEmitter<boolean>();

  private _isLoggedIn: boolean = false;

  @Input()
  set isLoggedIn(value: boolean) {
    this._isLoggedIn = value;
    this.isLoggedInChange.emit(value); // Emitting value change
  }

  get isLoggedIn(): boolean {
    return this._isLoggedIn;
  }

  constructor(private router: Router) {}

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
    this.router.navigate(['/user/login']);
  }

  handleLogoutClick(): void {
    // Simulate a logout process
    // You would typically have a service to handle the actual logout process
    // For now, we will just remove the isLoggedIn flag from local storage
    localStorage.removeItem('isLoggedIn');
    this.isLoggedIn = false;
    this.router.navigate(['/']);
  }

  handleCartClick(): void {
    console.log('Navigating to cart');
  }
}
