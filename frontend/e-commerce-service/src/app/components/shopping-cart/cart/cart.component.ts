import { Component } from '@angular/core';
import { CartItemResponseDTO, CartResponseDTO } from '../../../generated/api';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CartItemComponent } from '../cart-item/cart-item.component';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [ FormsModule, CommonModule, CartItemComponent ],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent {
  cart: CartResponseDTO = {
    cartID: 'cart1',
    userID: 'user1',
    totalAmount: 50.0
  };

  cartItems: CartItemResponseDTO[] = [
    { cartItemID: '1', cartID: 'cart1', productID: 'product1', quantity: 2, price: 25.0 },
    { cartItemID: '2', cartID: 'cart1', productID: 'product2', quantity: 1, price: 20.0 }
  ];

  onUpdateCart(): void {
    console.log('Updating cart:', this.cart);
  }

  onCheckout(): void {
    console.log('Checking out cart:', this.cart);
  }
}