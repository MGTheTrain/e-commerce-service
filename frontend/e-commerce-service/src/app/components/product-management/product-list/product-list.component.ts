import { Component } from '@angular/core';
import { ProductResponseDTO } from '../../../generated/api';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product-list',
  standalone: true, 
  imports: [ CommonModule, FormsModule, FontAwesomeModule ], 
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css'] // Correct the styleUrl to styleUrls, it's plural
})
export class ProductListComponent {
  products: ProductResponseDTO[] = [
    { productID: '1', categoryID: '101', name: 'Dean Razorback Guitar Blue', description: 'Description of Product A', price: 49.99, stock: 10, imageUrl: 'https://s.yimg.com/ny/api/res/1.2/jVphTvtt1LwM3foboVcs_w--/YXBwaWQ9aGlnaGxhbmRlcjt3PTEyMDA7aD02MDA-/https://media.zenfs.com/en-US/homerun/consequence_of_sound_458/830585263f74148d1ac63c91bfe6e2f4' },
    { productID: '2', categoryID: '102', name: 'Dean Razorback Guitar White', description: 'Description of Product B', price: 29.99, stock: 5, imageUrl: 'https://www.musicconnection.com/wp-content/uploads/2021/01/dean-dime-620x420.jpg' },
    { productID: '3', categoryID: '101', name: 'ESP LTD Greeny Black', description: 'Description of Product C', price: 99.99, stock: 15, imageUrl: 'https://media.sound-service.eu/Artikelbilder/Shopsystem/1200x837/ALEXI%20LAIHO%20GREENY_1.jpg' },
    { productID: '4', categoryID: '103', name: 'Jackson Guitar Blue', description: 'Description of Product D', price: 149.99, stock: 2, imageUrl: 'https://m.media-amazon.com/images/I/51EnIfqKY7L.jpg' },
    { productID: '5', categoryID: '102', name: 'Gibson Les Paul Black', description: 'Description of Product E', price: 199.99, stock: 8, imageUrl: 'https://morningsideschoolofmusic.co.uk/wp-content/uploads/2022/05/Gibson-Guitars-1024x576.jpg' },
  ];

  public faSearch: IconDefinition = faSearch;
  public searchText: string = '';

  constructor(private router: Router) {}

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

  handleProductClick(product: ProductResponseDTO): void {
    this.router.navigate(['/products', product.productID]);
  }
}
