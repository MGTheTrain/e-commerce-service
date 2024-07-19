import { Component, Input, OnInit } from '@angular/core';
import { CartItemResponseDTO, CartService, ProductResponseDTO, ProductService } from '../../../generated';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart-item',
  standalone: true,
  imports: [ FormsModule, CommonModule, FontAwesomeModule ],
  templateUrl: './cart-item.component.html',
  styleUrl: './cart-item.component.css'
})
export class CartItemComponent implements OnInit {
  @Input() cartItem: CartItemResponseDTO = {};

  @Input() product: ProductResponseDTO = {};

  availableCategories: string[] = [
    'Acoustic Guitar',
    'Electric Guitar',
    'Bass Guitar',
    'Classical Guitar',
    '12-String Guitar',
    '7-String Guitar',
    'Jazz Guitar',
    'Blues Guitar',
    'Metal Guitar',
    'Rock Guitar'
  ];

  public faTrash: IconDefinition = faTrash;
  public faEdit: IconDefinition = faEdit;
  @Input() public isEditing: boolean = false;

  public isLoggedIn: boolean = false;

  constructor(private router: Router, private productService: ProductService, private cartService: CartService) {}

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;

      this.productService.apiV1ProductsProductIdGet(this.cartItem.productID!).subscribe(
        (data: ProductResponseDTO) => {
            this.product = data;
        }
      ); 
    }
  }

  handleDeleteItemClick(cartItem: CartItemResponseDTO): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      const cartId = localStorage.getItem('cartId')?.toString();
      this.cartService.apiV1CartsCartIdItemsItemIdDelete(cartId!, cartItem.cartItemID!).subscribe(
        (data: CartItemResponseDTO) => {
          console.log("Delete cart item with id", data.cartItemID, "from cart with id", data.cartID);
        }
      );
    }
    window.location.reload();
  }
}
