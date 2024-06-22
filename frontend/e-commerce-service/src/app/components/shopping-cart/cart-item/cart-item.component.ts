import { Component, Input } from '@angular/core';
import { CartItemResponseDTO, ProductResponseDTO } from '../../../generated/api';
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

  @Input() product: ProductResponseDTO = {
    productID: '1',
    categoryID: '1',
    name: 'Product Name',
    description: 'Product Description',
    price: 99.99,
    stock: 10,
    imageUrl: 'https://www.musicconnection.com/wp-content/uploads/2021/01/dean-dime-620x420.jpg'
  };

  ngOnInit(): void {
    // GET api/v1/cart/:cartId/item request
    // GET api/v1/product/:productId
  }

  onUpdateQuantity(newQuantity: number): void {
    this.cartItem.quantity = newQuantity;
    console.log('Updated quantity:', newQuantity);
  }

  onRemoveCartItem(): void {
    console.log('Removing cart item:', this.cartItem);
  }
}
