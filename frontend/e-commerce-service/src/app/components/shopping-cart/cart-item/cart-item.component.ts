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
    categories: ['Electric Guitar'],
    name: 'Dean Razorback Guitar',
    description: 'Product Description',
    price: 3999.99,
    stock: 10,
    imageUrl: 'https://www.musicconnection.com/wp-content/uploads/2021/01/dean-dime-620x420.jpg'
  };

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

  public faTrash: IconDefinition = faTrash;
  public faEdit: IconDefinition = faEdit;
  @Input() public isEditing: boolean = false;

  public isLoggedIn: boolean = false;

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;
    } 
  }

  handleDeleteItemClick(): void {
    console.log('Removing cart item:', this.cartItem);
  }
}
