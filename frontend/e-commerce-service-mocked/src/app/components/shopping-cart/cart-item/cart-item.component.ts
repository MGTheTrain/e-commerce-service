import { Component, Input } from '@angular/core';
import { CartItemResponseDTO, ProductResponseDTO } from '../../../generated';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

@Component({
  selector: 'app-cart-item',
  standalone: true,
  imports: [ FormsModule, CommonModule, FontAwesomeModule ],
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
    name: 'Dean Razorback Guitar',
    description: 'Product Description',
    price: 99.99,
    stock: 10,
    imageUrl: 'https://www.musicconnection.com/wp-content/uploads/2021/01/dean-dime-620x420.jpg'
  };

  public faTrash: IconDefinition = faTrash;
  public faEdit: IconDefinition = faEdit;

  ngOnInit(): void {
    // GET api/v1/cart/:cartId/item
    // GET api/v1/product/:productId
  }

  handleDeleteItemClick(): void {
    console.log('Removing cart item:', this.cartItem);
  }
}
