import { Component, Input } from '@angular/core';
import { OrderItemResponseDTO } from '../../../generated/api';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-order-item',
  standalone: true,
  imports: [ FormsModule, CommonModule ],
  templateUrl: './order-item.component.html',
  styleUrls: ['./order-item.component.css']
})
export class OrderItemComponent {
  @Input() orderItem: OrderItemResponseDTO = {
    orderItemID: '',
    orderID: '',
    productID: '',
    quantity: 0,
    price: 0
  };

  constructor() { }
}
