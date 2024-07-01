import { Component, Input, OnInit } from '@angular/core';
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
export class OrderItemComponent implements OnInit {
  public faTrash: IconDefinition = faTrash;
  @Input() public isEditing: boolean = false;
  
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
    price: 3999.99,
    stock: 10,
    imageUrl: 'https://www.musicconnection.com/wp-content/uploads/2021/01/dean-dime-620x420.jpg'
  };

  ngOnInit(): void {
    console.log("TBD");
  }

  constructor() { }
}
