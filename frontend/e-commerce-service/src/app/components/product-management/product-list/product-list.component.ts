import { Component } from '@angular/core';
import { ProductResponseDTO } from '../../../generated/api';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faSearch } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-product-list',
  standalone: true, 
  imports: [ CommonModule, FormsModule, FontAwesomeModule ], 
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css'] // Correct the styleUrl to styleUrls, it's plural
})
export class ProductListComponent {
  products: ProductResponseDTO[] = [
    { productID: '1', categoryID: '101', name: 'Product A', description: 'Description of Product A', price: 49.99, stock: 10, imageUrl: 'https://example.com/product-a.jpg' },
    { productID: '2', categoryID: '102', name: 'Product B', description: 'Description of Product B', price: 29.99, stock: 5, imageUrl: 'https://example.com/product-b.jpg' },
    { productID: '3', categoryID: '101', name: 'Product C', description: 'Description of Product C', price: 99.99, stock: 15, imageUrl: 'https://example.com/product-c.jpg' },
    { productID: '4', categoryID: '103', name: 'Product D', description: 'Description of Product D', price: 149.99, stock: 2, imageUrl: 'https://example.com/product-d.jpg' },
    { productID: '5', categoryID: '102', name: 'Product E', description: 'Description of Product E', price: 199.99, stock: 8, imageUrl: 'https://example.com/product-e.jpg' },
  ];

  public faSearch: IconDefinition = faSearch;
  public searchText: string = '';

  filteredProducts() {
    return this.products.filter(product => {
      return (
        Object.values(product).some(val => 
          typeof val === 'string' && val.toLowerCase().includes(this.searchText.toLowerCase())
        ) ||
        product.price?.toString().includes(this.searchText.toLowerCase()) ||
        product.stock?.toString().includes(this.searchText.toLowerCase())
      );
    });
  }
}
