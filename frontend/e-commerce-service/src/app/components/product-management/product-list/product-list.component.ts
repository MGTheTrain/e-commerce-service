import { Component, Input, OnInit } from '@angular/core';
import { CartRequestDTO, CartResponseDTO, CartService, ProductResponseDTO, ProductService } from '../../../generated';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HeaderComponent } from '../../header/header.component';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faPlus, faStar } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ProductListFilter } from '../../../models/product-list-filter';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [ CommonModule, FormsModule, FontAwesomeModule, HeaderComponent ],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  @Input() products: ProductResponseDTO[] = [];

  public productListFilter: ProductListFilter = {
    category: null,
    minPrice: null,
    maxPrice: null,
    searchText: null
  };

  public availableCategories: string[] = [
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
  public faStar: IconDefinition = faStar;
  public isLoggedIn: boolean = false;
  public accessToken: string | null = ''; 

  public enableSearch: boolean = true;

  constructor(private router: Router, private productService: ProductService, private cartService: CartService) {}

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;

      if(localStorage.getItem('cartId') === null) {
        const cartRequestDto: CartRequestDTO = {
          totalAmount: 0
        };

        this.cartService.apiV1CartsUserGet().subscribe(
          (data: CartResponseDTO[]) => {
            if (data.length === 0) {
              this.cartService.apiV1CartsPost(cartRequestDto).subscribe(
                (newCart: CartResponseDTO) => {
                  localStorage.setItem("cartId", newCart.cartID!.toString());
                },
                error => {
                  console.error('Error creating cart', error);
                }
              );
            } else {
              localStorage.setItem("cartId", data[0].cartID!.toString());
              console.log('User already has carts.');
            }
          },
          error => {
            console.error('Error fetching user carts', error);
          }
        );
      }
    } 

    const pageSize = 20;
    this.productService.apiV1ProductsGet(1, pageSize).subscribe(
      (data: ProductResponseDTO[]) => {
        this.products = data;
      },
      error => {
        console.error('Error fetching products', error);
      }
    );
  }

  handleCreateProduct(): void {
    this.router.navigate(['/products/creation']);
  }

  handleViewProduct(product: ProductResponseDTO): void {
    this.router.navigate(['/products', product.productID]);
  }

  handleViewReviews(product: ProductResponseDTO): void {
    this.router.navigate(['/products', product.productID, 'reviews']);
  }

  handleSearchChanged(productListFilter: ProductListFilter): void {
    this.productListFilter = productListFilter;
    // console.log(this.productListFilter);
    const pageSize = 20;
    this.productService.apiV1ProductsGet(1, pageSize, this.productListFilter.category!, this.productListFilter.searchText!, this.productListFilter.minPrice!, this.productListFilter.maxPrice!).subscribe(
      (data: ProductResponseDTO[]) => {
        this.products = data;
      },
      error => {
        console.error('Error fetching products', error);
      }
    );
  }

}
