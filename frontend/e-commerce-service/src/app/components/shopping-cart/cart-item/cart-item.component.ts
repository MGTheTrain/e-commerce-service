import { Component, Input, OnInit } from '@angular/core';
import { CartItemResponseDTO, ProductResponseDTO, ProductService } from '../../../generated';
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
  @Input() cartItem: CartItemResponseDTO = {};

  @Input() product: ProductResponseDTO = {};

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

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;

      this.productService.apiV1ProductsProductIdGet(this.cartItem.productID!).subscribe(
        (data: ProductResponseDTO) => {
            this.product = data;
        }
      ); 
    }
  }

  handleDeleteItemClick(): void {
    console.log('Removing cart item:', this.cartItem);
  }
}
