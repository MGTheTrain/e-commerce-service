import { Component, Input, OnInit } from '@angular/core';
import { CartItemRequestDTO, CartItemResponseDTO, CartRequestDTO, CartResponseDTO, CartService, OrderItemRequestDTO, OrderItemResponseDTO, OrderRequestDTO, OrderResponseDTO, OrderService } from '../../../generated';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CartItemComponent } from '../cart-item/cart-item.component';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faEdit, faShoppingCart } from '@fortawesome/free-solid-svg-icons';
import { DetailHeaderComponent } from '../../../shared/component/header/detail-header/detail-header.component';

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

  public disableCheckout: boolean = false;

  @Input() cart: CartResponseDTO = {};

  @Input() cartItems: CartItemResponseDTO[] = [];

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

  public isLoggedIn: boolean = false;
  public isEditing: boolean = true;

  constructor(private router: Router, private route: ActivatedRoute, private cartService: CartService, private orderService: OrderService) { }

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;

      let cartId = localStorage.getItem('cartId')?.toString();
      
      this.subscription = this.route.params.subscribe(params => {
          if (params['cartId']) {
              cartId = params['cartId'];
          } 
      });
      
      this.cartService.apiV1CartsCartIdGet(cartId!).subscribe(
        (data: CartResponseDTO) => {
          this.cart = data;
        },
        error => {
          console.error('Error fetching carts', error);
          this.router.navigate(['/']);
        }
      );

      this.cartService.apiV1CartsCartIdItemsGet(cartId!).subscribe(
        (data: CartItemResponseDTO[]) => {
          this.cartItems = data;

          if(this.cartItems.length == 0) { // disable checkout button
            this.disableCheckout = true;
          }
          this.calculateTotalAmount();
        },
        error => {
          console.error('Error fetching carts', error);
        }
      );
    } 
  }

  handleCheckout(): void {
    if(this.cartItems.length > 0) {
      this.updateCart();
      const orderRequestDto: OrderRequestDTO = {
        totalAmount: this.cart.totalAmount! + 0.0,
        orderStatus: "Pending",
        currencyCode: "USD",
        referenceId: "tmp",
        addressLine1: "tmp",
        addressLine2: "tmp",
        adminArea2: "tmp",
        adminArea1: "tmp",
        postalCode: "tmp",
        countryCode: "US",
      };
      
      this.orderService.apiV1OrdersPost(orderRequestDto).subscribe(
        (data: OrderResponseDTO) => {
          console.log("Created order", data);

          for(const cartItem of this.cartItems) {
            const orderItemRequestDTO: OrderItemRequestDTO = {
              orderID: data.orderID!,
              productID: cartItem.productID!,
              quantity: cartItem.quantity!,
              price: cartItem.price!,
            };
            this.orderService.apiV1OrdersOrderIdItemsPost(data.orderID!, orderItemRequestDTO).subscribe(
              (data2: OrderItemResponseDTO) => {
                console.log("Created order item", data2, "for order with id", data.orderID!)
              },
              error => {
                console.error('Error creating order item', error);
              }
            );
          }

          window.location.href = data.checkoutNowHref!;
        },
        error => {
          console.error('Error creating order', error);
        }
      );
    }
  }

  updateCart(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      const cartId = localStorage.getItem('cartId')?.toString();
      const cartRequestDto: CartRequestDTO = {
        totalAmount: this.cart.totalAmount!
      };
      this.cartService.apiV1CartsCartIdPut(cartId!, cartRequestDto).subscribe(
        (data: CartResponseDTO) => {
          console.log("Updated cart with cart id", data.cartID!);
        }
      );

      for(const cartItem of this.cartItems) {
        const cartItemRequestDTO: CartItemRequestDTO = {
          cartID: cartItem.cartID!,
          productID: cartItem.productID!,
          quantity: cartItem.quantity!,
          price: cartItem.price!,
        };
        this.cartService.apiV1CartsCartIdItemsItemIdPut(cartId!, cartItem.cartItemID!, cartItemRequestDTO).subscribe(
          (data: CartItemResponseDTO) => {
            console.log("Updated cart item with cart item id", data.cartItemID!);
          }
        );
      }
    }
  }

  calculateTotalAmount(): void {
    this.cart.totalAmount = this.cartItems.reduce((total, item) => {
      const quantity = item.quantity ?? 0;
      const price = item.price ?? 0;
      return total + (quantity * price);
    }, 0);
  }
}