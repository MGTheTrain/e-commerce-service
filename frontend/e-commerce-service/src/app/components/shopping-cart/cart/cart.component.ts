import { Component, Input } from '@angular/core';
import { CartItemResponseDTO, CartResponseDTO, ProductResponseDTO } from '../../../generated/api';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CartItemComponent } from '../cart-item/cart-item.component';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [ FormsModule, CommonModule, CartItemComponent ],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent {
  private subscription: Subscription | null = null;

  @Input() cart: CartResponseDTO = {
    cartID: 'cart1',
    userID: 'user1',
    totalAmount: 50.0
  };

  @Input() cartItems: CartItemResponseDTO[] = [
    { cartItemID: '1', cartID: 'cart1', productID: 'product1', quantity: 2, price: 25.0 },
    { cartItemID: '2', cartID: 'cart1', productID: 'product2', quantity: 1, price: 20.0 }
  ];

  @Input() product: ProductResponseDTO = {
    productID: '1',
    categoryID: '1',
    name: 'Dean Razorback Guitar',
    description: 'Product Description',
    price: 99.99,
    stock: 10,
    imageUrl: 'https://www.musicconnection.com/wp-content/uploads/2021/01/dean-dime-620x420.jpg'
  };

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.subscription = this.route.params.subscribe(params => {
      let id = params['userId'];
      this.cart.userID = id;

      // GET /api/v1/cart/:userId/items
      // GET /api/v1/products/:productId
   });
  }

  onUpdateCart(): void {
    console.log('Updating cart:', this.cart);
  }

  onCheckout(): void {
    console.log('Checking out cart:', this.cart);
  }
}