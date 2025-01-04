import { Component, Input, OnInit } from '@angular/core';
import { CartResponseDTO, CartService } from '../../../generated';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CartItemComponent } from '../cart-item/cart-item.component';
import { HeaderComponent } from '../../../shared/component/header/header.component';

@Component({
  selector: 'app-cart-list',
  standalone: true,
  imports: [ FormsModule, CommonModule, CartItemComponent, HeaderComponent ],
  templateUrl: './cart-list.component.html',
  styleUrl: './cart-list.component.css'
})
export class CartListComponent implements OnInit {
  @Input() carts: CartResponseDTO[] = [];

  public isLoggedIn: boolean = false;
  public enableSearch: boolean = false;

  constructor(private router: Router, private cartService: CartService) {}

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;
      this.cartService.apiV1CartsGet().subscribe(
        (data: CartResponseDTO[]) => {
          this.carts = data;
        },
        error => {
          console.error('Error fetching carts', error);
        }
      );
    } 
  }

  // getUserName(userID: string | undefined): string | undefined {
  //   const user = this.users.find(user => user.userID === userID);
  //   return user ? user.userName : 'Unknown User';
  // }

  handleCart(cart: CartResponseDTO): void {
    this.router.navigate(['/carts', cart.cartID]);
  }
}
