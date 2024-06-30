import { Component, Input, OnInit } from '@angular/core';
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
export class CartItemComponent implements OnInit {
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
    price: 3999.99,
    stock: 10,
    imageUrl: 'https://www.musicconnection.com/wp-content/uploads/2021/01/dean-dime-620x420.jpg'
  };

  public faTrash: IconDefinition = faTrash;
  public faEdit: IconDefinition = faEdit;
  @Input() public isEditing: boolean = false;

  public isLoggedIn: boolean = false;
  public accessToken: string | null = ''; 

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;
      this.accessToken = localStorage.getItem("accessToken");
      console.log("accessToken: ", this.accessToken);
    } else {
      this.isLoggedIn = false;
      this.accessToken = '';
    }
  }

  handleDeleteItemClick(): void {
    console.log('Removing cart item:', this.cartItem);
  }
}
