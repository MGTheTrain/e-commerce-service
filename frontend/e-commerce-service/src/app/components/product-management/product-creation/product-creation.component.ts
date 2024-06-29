import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faArrowLeft, faImage, faPlus } from '@fortawesome/free-solid-svg-icons';
import { ProductRequestDTO, ProductResponseDTO, ProductService } from '../../../generated';

@Component({
  selector: 'app-product-creation',
  standalone: true,
  imports: [FormsModule, CommonModule, FontAwesomeModule],
  templateUrl: './product-creation.component.html',
  styleUrl: './product-creation.component.css'
})
export class ProductCreationComponent {
  productRequest: ProductRequestDTO = { 
    categoryID: '', // TBD: remove Category entity
    name: '', 
    description: '', 
    price: 0, 
    stock: 0, 
    imageUrl: 'https://ralfvanveen.com/wp-content/uploads/2021/06/Placeholder-_-Glossary.svg' 
  };

  public faPlus: IconDefinition = faPlus;
  public faImage: IconDefinition = faImage;
  public faArrowLeft: IconDefinition = faArrowLeft;
  public isEditing: boolean = false;

  constructor(private router: Router, private productService: ProductService) { }

  handleNavigateBackClick(): void {
    this.router.navigate(['/products']);
  }

  handleCreateProductClick(): void {
    this.productService.apiV1ProductsPost(this.productRequest).subscribe(
      (data: ProductResponseDTO) => {
        console.log('Created product', data);
        this.router.navigate(['/products']);
      },
      error => {
        console.error('Error creating product', error);
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
      const file = input.files[0];
      const reader = new FileReader();
      reader.onload = (e: any) => {
        // this.product.imageUrl = e.target.result;
      };
      // reader.readAsDataURL(file);
    }
  }
}
