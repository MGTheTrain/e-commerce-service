import { Component, OnInit } from '@angular/core';
import { ProductResponseDTO, ProductRequestDTO, ProductService, CartService, CartItemRequestDTO, CartItemResponseDTO } from '../../../generated';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faEdit, faTrash, faImage, faArrowLeft, faShoppingCart } from '@fortawesome/free-solid-svg-icons';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { DetailHeaderComponent } from '../../header/detail-header/detail-header.component';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [ FormsModule, CommonModule, FontAwesomeModule, DetailHeaderComponent ],
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {
  private subscription: Subscription | null = null;

  product: ProductResponseDTO = {};

  quantity: number = 0;
  
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
  public faImage: IconDefinition = faImage;
  public faArrowLeft: IconDefinition = faArrowLeft;
  public faShoppingCart: IconDefinition = faShoppingCart;
  public isEditing: boolean = false;

  public isLoggedIn: boolean = false;
  public isProductOwner: boolean = false;

  constructor(private router: Router, private route: ActivatedRoute, private productService: ProductService, private cartService: CartService) { }

  ngOnInit(): void {
    this.subscription = this.route.params.subscribe(params => {
      this.product.productID = params['productId'];

      this.productService.apiV1ProductsProductIdGet(this.product.productID!).subscribe(
        (data: ProductResponseDTO) => {
          this.product = data;
        },
        error => {
          console.error('Error fetching product with id', this.product.productID, error);
        }
      );
    });
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;
        this.productService.apiV1ProductsProductIdUserGet(this.product.productID!).subscribe(
          (data: ProductResponseDTO) => {
            console.log("Product owned by:", data);
            this.isProductOwner = true;
          },
          error => {
            console.error('Error: User is not the product owner', error);
          }
        );
  
        // Update quantity 
        const cartId = localStorage.getItem('cartId')?.toString();
        this.cartService.apiV1CartsCartIdProductsProductIdItemGet(cartId!, this.product.productID!).subscribe(
          (data: CartItemResponseDTO) => {
            this.quantity = data.quantity!  
          },
          error => {
            console.error('Error retrieving item from cart with id', cartId, error);
          }
        );
     } 
  }

  handleNavigateBackClick(): void {
    this.router.navigate(['/']);
  }

  handleEditClick(): void {
    this.isEditing = !this.isEditing;
  }

  handleDeleteProductClick() {
    if(this.isLoggedIn) {
      const productID = this.product.productID;
      if (productID) {
        this.productService.apiV1ProductsProductIdDelete(productID).subscribe(
          () => {
            console.error('Successfully deleted product with id', productID);
            this.router.navigate(['/']);
          },
          error => {
            console.error('Error deleting product with id', productID, error);
          }
        );
      } else {
        console.error('Product ID is undefined');
      }
    }
  }

  handleUpdateProductClick() {
    if(this.isLoggedIn) {
      const productID = this.product.productID;
      if (productID) {
        const productRequest: ProductRequestDTO = { 
          categories: this.product.categories!,
          name: this.product.name!, 
          description: this.product.description!, 
          price: this.product.price!, 
          stock: this.product.stock!, 
          imageUrl: this.product.imageUrl! 
        };

        this.productService.apiV1ProductsProductIdPut(productID, productRequest).subscribe(
          () => {
            console.error('Successfully updated product with id', productID);
            this.router.navigate(['/']);
          },
          error => {
            console.error('Error updating product with id', productID, error);
          }
        );
      } else {
        console.error('Product ID is undefined');
      }
    }
  }

  handleAddToCartClick(product: ProductResponseDTO): void {
    if(this.isLoggedIn) {
      this.removeItemFromCart();

      const cartId = localStorage.getItem('cartId')?.toString();
      const cartItemRequestDto: CartItemRequestDTO = {
        cartID: cartId!.toString(),
        productID: product.productID!,
        quantity: this.quantity,
        price: product.price!,
      };
      this.cartService.apiV1CartsCartIdItemsPost(cartId!, cartItemRequestDto).subscribe(
        (data: CartItemResponseDTO) => {
          console.log('Added item to cart', data);    
          this.router.navigate(['/']); 
        },
        error => {
          console.error('Error adding item to cart with id', cartId, error);
        }
      );
    }
  }

  handleRemoveFromCartClick(): void {
    if(this.isLoggedIn) {
      this.removeItemFromCart();
      this.router.navigate(['/']);
    }
  }

  removeItemFromCart(): void {
    const cartId = localStorage.getItem('cartId')?.toString();
    this.cartService.apiV1CartsCartIdProductsProductIdItemGet(cartId!, this.product.productID!).subscribe(
      (data: CartItemResponseDTO) => {
        this.cartService.apiV1CartsCartIdItemsItemIdDelete(cartId!, data.cartItemID!).subscribe(
          (data: CartItemResponseDTO) => {
            console.log('Delete item from cart', data);     
          },
          error => {
            console.error('Error deleting item from cart with id', cartId, error);
          }
        );
      },
      error => {
        console.error('Error retrieving cart item for cart with id', cartId, error);
      }
    );
  }

  triggerImageInput(): void {
    const fileInput = document.getElementById('imageInput') as HTMLInputElement;
    fileInput.click();
  }

  onImageChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      // const file = input.files[0];
      // const reader = new FileReader();
      // reader.onload = (e: any) => {
      //   // this.product.imageUrl = e.target.result;
      // };
      // // reader.readAsDataURL(file);
    }
  }
}
