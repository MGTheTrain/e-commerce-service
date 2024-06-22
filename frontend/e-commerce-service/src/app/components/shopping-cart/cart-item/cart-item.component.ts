import { Component, Input } from '@angular/core';
import { CartItemResponseDTO } from '../../../generated/api';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cart-item',
  standalone: true,
  imports: [ FormsModule, CommonModule ],
  templateUrl: './cart-item.component.html',
  styleUrl: './cart-item.component.css'
})
export class CartItemComponent {
  @Input() cartItem: CartItemResponseDTO = {
    cartItemID: '1',
    cartID: 'cart1',
    productID: 'product1',
    quantity: 2,
    price: 25.0
  };

  onUpdateQuantity(newQuantity: number): void {
    this.cartItem.quantity = newQuantity;
    console.log('Updated quantity:', newQuantity);
  }

  onRemoveCartItem(): void {
    console.log('Removing cart item:', this.cartItem);
  }
}
