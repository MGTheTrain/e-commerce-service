import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faArrowLeft, faImage, faPlus } from '@fortawesome/free-solid-svg-icons';
import { ProductResponseDTO, ProductService } from '../../../generated';
import { DetailHeaderComponent } from '../../header/detail-header/detail-header.component';

@Component({
  selector: 'app-product-creation',
  standalone: true,
  imports: [FormsModule, CommonModule, FontAwesomeModule, DetailHeaderComponent],
  templateUrl: './product-creation.component.html',
  styleUrl: './product-creation.component.css'
})
export class ProductCreationComponent implements OnInit {
  product: ProductResponseDTO = { 
    categories: ['Electric Guitar'],
    name: '', 
    description: '', 
    price: 0, 
    stock: 0
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

  public faPlus: IconDefinition = faPlus;
  public faImage: IconDefinition = faImage;
  public faArrowLeft: IconDefinition = faArrowLeft;
  public isEditing: boolean = false;

  public isLoggedIn: boolean = false;
  public selectedFile: File | null = null;

  constructor(private router: Router, private productService: ProductService) { }

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;
    } 
  }

  handleNavigateBackClick(): void {
    this.router.navigate(['/']);
  }

  handleCreateProductClick(): void {
    const fileBlobs: Blob[] = [];
    if (this.selectedFile) {
      fileBlobs.push(this.selectedFile);
    }
    
    this.productService.apiV1ProductsPostForm(
      this.product.categories,
      this.product.name,
      this.product.description,
      this.product.price,
      this.product.stock,
      fileBlobs).subscribe(
      (data: ProductResponseDTO) => {
        console.log('Created product', data);
        this.router.navigate(['/']);
      },
      error => {
        console.error('Error creating product', error);
      }
    ); 
  }

  // Method to trigger file input click
  triggerImageInput(): void {
    const fileInput = document.getElementById('imageInput') as HTMLInputElement;
    fileInput.click();
  }

  // Method to handle image file change
  onImageChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      const file = input.files[0];
      this.selectedFile = file;
  
      // Create a FileReader to read the file and convert it to a Data URL for preview
      const reader = new FileReader();
      reader.onload = (e: ProgressEvent<FileReader>) => {
        // Ensure e.target is an instance of FileReader
        console.log(e);
      };
      reader.readAsDataURL(file);
    }  
  }
}
