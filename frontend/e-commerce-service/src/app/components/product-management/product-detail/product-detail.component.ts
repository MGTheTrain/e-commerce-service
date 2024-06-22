import { Component } from '@angular/core';
import { ProductResponseDTO } from '../../../generated/api';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [ FormsModule, CommonModule, FontAwesomeModule ],
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.css'
})
export class ProductDetailComponent {
  private subscription: Subscription | null = null;

  product: ProductResponseDTO = {
    productID: '1',
    categoryID: '1',
    name: 'Product Name',
    description: 'Product Description',
    price: 99.99,
    stock: 10,
    imageUrl: 'https://www.musicconnection.com/wp-content/uploads/2021/01/dean-dime-620x420.jpg'
  };

  public faTrash: IconDefinition = faTrash;
  public faEdit: IconDefinition = faEdit;

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
}
