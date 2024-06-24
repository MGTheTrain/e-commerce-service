import { Component } from '@angular/core';
import { ProductResponseDTO } from '../../../generated';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faEdit, faTrash, faImage } from '@fortawesome/free-solid-svg-icons';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { HeaderComponent } from '../../header/header.component';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [ FormsModule, CommonModule, FontAwesomeModule, HeaderComponent ],
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent {
  private subscription: Subscription | null = null;

  product: ProductResponseDTO = { 
    productID: '2', 
    categoryID: '102', 
    name: 'Dean Razorback Guitar White', 
    description: 'Description of Product B', 
    price: 29.99, 
    stock: 5, 
    imageUrl: 'https://www.musicconnection.com/wp-content/uploads/2021/01/dean-dime-620x420.jpg' 
  };

  public faTrash: IconDefinition = faTrash;
  public faEdit: IconDefinition = faEdit;
  public faImage: IconDefinition = faImage;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.subscription = this.route.params.subscribe(params => {
      let id = params['productId'];
      this.product.productID = id;
   });
  }

  onDelete() {
    console.log('Deleting product with ID:', this.product.productID);
  }

  onUpdate() {
    console.log('Updating product:', this.product);
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
