import { Component, Input, OnInit } from '@angular/core';
import { CartItemResponseDTO, CartResponseDTO, ProductResponseDTO } from '../../../generated';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CartItemComponent } from '../cart-item/cart-item.component';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faEdit, faShoppingCart } from '@fortawesome/free-solid-svg-icons';
import { DetailHeaderComponent } from '../../header/detail-header/detail-header.component';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [ FormsModule, CommonModule, CartItemComponent, FontAwesomeModule, DetailHeaderComponent ],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit {
  private subscription: Subscription | null = null;
  public faEdit: IconDefinition = faEdit;
  public faShoppingCart: IconDefinition = faShoppingCart;
  public isEditing: boolean = false;

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
    price: 3999.99,
    stock: 10,
    imageUrl: 'https://www.musicconnection.com/wp-content/uploads/2021/01/dean-dime-620x420.jpg'
  };

  public isLoggedIn: boolean = false;

  constructor(private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;
    } 
   this.calculateTotalAmount();
  }

  handleEditClick(): void {
    this.isEditing = !this.isEditing;
  }

  handleUpdateCartClick(): void {
    console.log('Updating cart:', this.cart);
    // pop up window
    this.router.navigate(['/']);
  }

  handleCheckoutClick(): void {
    console.log('Checking out cart:', this.cart);
  }

  calculateTotalAmount(): void {
    this.cart.totalAmount = this.cartItems.reduce((total, item) => {
      const quantity = item.quantity ?? 0;
      const price = item.price ?? 0;
      return total + (quantity * price);
    }, 0);
  }
}