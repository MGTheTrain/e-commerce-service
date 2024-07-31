import { CommonModule } from '@angular/common';
import { Component, ElementRef, EventEmitter, Input, OnInit, Output, Renderer2, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faList, faSearch, faShoppingCart, faSignIn } from '@fortawesome/free-solid-svg-icons';
import { CartResponseDTO, CartService } from '../../generated';
import { ProductListFilter } from '../../models/product-list-filter';

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
  public faList: IconDefinition = faList;

  public isLoggedIn: boolean = false;
  public accessToken: string | null = ''; 

  // filter and pagination properties
  @Input() enableSearch: boolean = true;
  @Input() availableCategories: string[] = [];
  @Output() searchChanged = new EventEmitter<ProductListFilter>();
  @ViewChild('collapseElement') collapseElement!: ElementRef;

  public searchText: string = '';
  public selectedCategory: string = '';
  public minPrice: number | null = null;
  public maxPrice: number | null = null;

  constructor(private router: Router, private renderer: Renderer2, public auth: AuthService, private cartService: CartService) {}

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;
      this.accessToken = localStorage.getItem("accessToken");
    } else {
      this.isLoggedIn = false;
      this.accessToken = '';
      this.auth.isAuthenticated$.subscribe((isAuthenticated: boolean) => {
        if (isAuthenticated) {
          localStorage.setItem('isLoggedIn', 'true');
          this.auth.getAccessTokenSilently().subscribe(
            (accessToken: string) => {
              localStorage.setItem("accessToken", accessToken);

              window.location.reload();
            },
            (error) => {
              console.error('Error getting access token:', error);
            }
          );
        }
      });
    }
  }

  handleLogoClick(): void {
    this.router.navigate(['/']);
  }

  handleLoginClick(): void {
    this.auth.loginWithRedirect();
  }

  handleLogoutClick(): void {
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

  handleCartClick(): void {
    this.router.navigate(['/cart']);
  }

  handleNavigateToOrdersClick(): void {
    this.router.navigate(['/orders']);
  }

  handleSearch(): void {
    this.searchChanged.emit({
      searchText: this.searchText,
      category: this.selectedCategory,
      minPrice: this.minPrice,
      maxPrice: this.maxPrice
    });
  }

  toggleCollapse(): void {
    if (this.collapseElement.nativeElement.classList.contains('show')) {
      this.renderer.removeClass(this.collapseElement.nativeElement, 'show');
    } else {
      this.renderer.addClass(this.collapseElement.nativeElement, 'show');
    }
  }
}
