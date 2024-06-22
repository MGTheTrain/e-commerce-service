import { Component, Input } from '@angular/core';
import { CartResponseDTO } from '../../../generated/api';
import { v4 as uuidv4 } from 'uuid';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CartItemComponent } from '../cart-item/cart-item.component';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

@Component({
  selector: 'app-cart-list',
  standalone: true,
  imports: [ FormsModule, CommonModule, CartItemComponent, FontAwesomeModule ],
  templateUrl: './cart-list.component.html',
  styleUrl: './cart-list.component.css'
})
export class CartListComponent {
  @Input() carts: CartResponseDTO[] = [
    {
      cartID: uuidv4(),
      userID: uuidv4(),
      totalAmount: 50.0
    },
    {
      cartID: uuidv4(),
      userID: uuidv4(),
      totalAmount: 50.0
    },
  ];

  public faSearch: IconDefinition = faSearch;
  public searchText: string = '';
  public filterOption: string = '';

  constructor(private router: Router) {}

  ngOnInit(): void {
    // GET /api/v1/cart optionally with filter
  }

  filteredCarts() {
    return this.carts.filter(cart => {
      const matchesSearchText = Object.values(cart).some(val =>
        typeof val === 'string' && val.toLowerCase().includes(this.searchText.toLowerCase())
      );
  
      const matchesFilterOption = !this.filterOption;
  
      return matchesSearchText && matchesFilterOption;
    });
  }

  handleCartClick(cart: CartResponseDTO): void {
    this.router.navigate(['/user', cart.cartID, 'cart']);
  }
}
