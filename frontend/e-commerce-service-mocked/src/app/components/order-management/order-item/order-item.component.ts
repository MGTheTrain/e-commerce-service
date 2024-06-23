import { Component, Input } from '@angular/core';
import { OrderItemResponseDTO, ProductResponseDTO } from '../../../generated';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

@Component({
  selector: 'app-order-item',
  standalone: true,
  imports: [ FormsModule, CommonModule, FontAwesomeModule ],
  templateUrl: './order-item.component.html',
  styleUrls: ['./order-item.component.css']
})
export class OrderItemComponent {
  public faTrash: IconDefinition = faTrash;
  
  @Input() orderItem: OrderItemResponseDTO = {
    orderItemID: '',
    orderID: '',
    productID: '',
    quantity: 0,
    price: 0
  };

  @Input() product: ProductResponseDTO = {
    productID: '1',
    categoryID: '1',
    name: 'Dean Razorback Guitar',
    description: 'Product Description',
    price: 99.99,
    stock: 10,
    imageUrl: 'https://www.musicconnection.com/wp-content/uploads/2021/01/dean-dime-620x420.jpg'
  };

  ngOnInit(): void {
    // GET api/v1/orders/:orderId/item
    // GET api/v1/product/:productId
  }

  constructor() { }
}