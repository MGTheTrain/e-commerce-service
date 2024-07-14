import { Component, OnInit } from '@angular/core';
import { ProductResponseDTO, ProductRequestDTO, ProductService } from '../../../generated';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faEdit, faTrash, faImage, faArrowLeft, faPlus } from '@fortawesome/free-solid-svg-icons';
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

  product: ProductResponseDTO = { 
    productID: '2', 
    categories: ['Electric Guitar'],
    name: 'Dean Razorback Guitar White', 
    description: 'Description of Product B', 
    price: 2999.99, 
    stock: 5, 
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
  public faImage: IconDefinition = faImage;
  public faArrowLeft: IconDefinition = faArrowLeft;
  public faPlus: IconDefinition = faPlus;
  public isEditing: boolean = false;

  public isLoggedIn: boolean = false;

  constructor(private router: Router, private route: ActivatedRoute, private productService: ProductService) { }

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;
    } 

    this.subscription = this.route.params.subscribe(params => {
      const id = params['productId'];
      this.product.productID = id;

      this.productService.apiV1ProductsProductIdGet(id).subscribe(
        (data: ProductResponseDTO) => {
          this.product = data;
        },
        error => {
          console.error('Error fetching product with id', id, error);
        }
      );
   });
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

  handleCartAddToCartClick(product: ProductResponseDTO): void {
    console.log('About to handle cart added', product);
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
