import { Component, Input, OnInit } from '@angular/core';
import { CartResponseDTO, CartService, UserResponseDTO } from '../../../generated';
import { v4 as uuidv4 } from 'uuid';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CartItemComponent } from '../cart-item/cart-item.component';
import { HeaderComponent } from '../../header/header.component';

@Component({
  selector: 'app-cart-list',
  standalone: true,
  imports: [ FormsModule, CommonModule, CartItemComponent, HeaderComponent ],
  templateUrl: './cart-list.component.html',
  styleUrl: './cart-list.component.css'
})
export class CartListComponent implements OnInit {
  @Input() carts: CartResponseDTO[] = [
    {
      cartID: uuidv4(),
      userID: '1',
      totalAmount: 50.0
    },
    {
      cartID: uuidv4(),
      userID: '2',
      totalAmount: 50.0
    },
  ];

  @Input() users: UserResponseDTO[] = [
    { userID: '1', userName: 'John Doe', email: 'john.doe@example.com', role: 'Admin' },
    { userID: '2', userName: 'Jane Smith', email: 'jane.smith@example.com', role: 'User' }
  ];

  constructor(private router: Router, private cartService: CartService) {}

  ngOnInit(): void {
    this.cartService.apiV1CartsGet().subscribe(
      (data: CartResponseDTO[]) => {
        this.carts = data;
      },
      error => {
        console.error('Error fetching carts', error);
      }
    );
  }

  getUserName(userID: string | undefined): string | undefined {
    const user = this.users.find(user => user.userID === userID);
    return user ? user.userName : 'Unknown User';
  }

  handleCartClick(cart: CartResponseDTO): void {
    this.router.navigate(['/user', cart.cartID, 'cart']);
  }
}
