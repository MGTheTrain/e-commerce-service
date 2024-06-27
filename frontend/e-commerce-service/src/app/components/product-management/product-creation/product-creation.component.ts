import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faArrowLeft, faEdit, faImage } from '@fortawesome/free-solid-svg-icons';
import { ProductResponseDTO } from '../../../generated';

@Component({
  selector: 'app-product-creation',
  standalone: true,
  imports: [FormsModule, CommonModule, FontAwesomeModule],
  templateUrl: './product-creation.component.html',
  styleUrl: './product-creation.component.css'
})
export class ProductCreationComponent {
  product: ProductResponseDTO = { 
    productID: '', 
    categoryID: '', 
    name: '', 
    description: '', 
    price: 0, 
    stock: 0, 
    imageUrl: 'https://ralfvanveen.com/wp-content/uploads/2021/06/Placeholder-_-Glossary.svg' 
  };

  public faEdit: IconDefinition = faEdit;
  public faImage: IconDefinition = faImage;
  public faArrowLeft: IconDefinition = faArrowLeft;
  public isEditing: boolean = false;

  constructor(private router: Router) { }

  handleNavigateBackClick(): void {
    this.router.navigate(['/products']);
  }

  handleCreateProductClick(): void {
    console.log('Create product');
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
