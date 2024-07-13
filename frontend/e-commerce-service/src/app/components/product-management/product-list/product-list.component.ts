import { Component, Input, OnInit } from '@angular/core';
import { CartRequestDTO, CartResponseDTO, CartService, ProductResponseDTO, ProductService } from '../../../generated';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HeaderComponent } from '../../header/header.component';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [ CommonModule, FormsModule, FontAwesomeModule, HeaderComponent ],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  @Input() products: ProductResponseDTO[] = [
    { productID: '1', categories: ['Electric Guitar'], name: 'Dean Razorback Guitar Blue', description: 'Description of Product A', price: 4999.99, stock: 10, imageUrl: 'https://s.yimg.com/ny/api/res/1.2/jVphTvtt1LwM3foboVcs_w--/YXBwaWQ9aGlnaGxhbmRlcjt3PTEyMDA7aD02MDA-/https://media.zenfs.com/en-US/homerun/consequence_of_sound_458/830585263f74148d1ac63c91bfe6e2f4' },
    { productID: '2', categories: ['Electric Guitar'], name: 'Dean Razorback Guitar White', description: 'Description of Product B', price: 2999.99, stock: 5, imageUrl: 'https://www.musicconnection.com/wp-content/uploads/2021/01/dean-dime-620x420.jpg' },
    { productID: '3', categories: ['Electric Guitar'], name: 'ESP LTD Greeny Black', description: 'Description of Product C', price: 3999.99, stock: 15, imageUrl: 'https://media.sound-service.eu/Artikelbilder/Shopsystem/1200x837/ALEXI%20LAIHO%20GREENY_1.jpg' },
    { productID: '4', categories: ['Electric Guitar'], name: 'Jackson Guitar Blue', description: 'Description of Product D', price: 1099.99, stock: 2, imageUrl: 'https://m.media-amazon.com/images/I/51EnIfqKY7L.jpg' },
    { productID: '5', categories: ['Electric Guitar'], name: 'Gibson Les Paul Black', description: 'Description of Product E', price: 999.99, stock: 8, imageUrl: 'https://morningsideschoolofmusic.co.uk/wp-content/uploads/2022/05/Gibson-Guitars-1024x576.jpg' },
  ];

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

  public faPlus: IconDefinition = faPlus;
  public isLoggedIn: boolean = false;
  public accessToken: string | null = ''; 

  constructor(private router: Router, private productService: ProductService, private cartService: CartService) {}

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;

      if(localStorage.getItem('cartId') === null) {
        const cartRequestDto: CartRequestDTO = {
          totalAmount: 0
        };
        this.cartService.apiV1CartsPost(cartRequestDto).subscribe(
          (data: CartResponseDTO) => {
            localStorage.setItem("cartId", data.cartID!.toString());
          },
          error => {
            console.error('Error creating cart', error);
          }
        );
      }
    } 
    this.productService.apiV1ProductsGet().subscribe(
      (data: ProductResponseDTO[]) => {
        this.products = data;
      },
      error => {
        console.error('Error fetching products', error);
      }
    );
  }

  handleCreateProductClick(): void {
    this.router.navigate(['/products/creation']);
  }

  handleViewClick(product: ProductResponseDTO): void {
    this.router.navigate(['/products', product.productID]);
  }

  handleCartAddToCartClick(product: ProductResponseDTO): void {
    console.log('About to handle cart added', product);
  }
}
