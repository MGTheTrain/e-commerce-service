import { Component, Input } from '@angular/core';
import { OrderItemResponseDTO, OrderResponseDTO } from '../../../generated';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { OrderItemComponent } from '../order-item/order-item.component';

@Component({
  selector: 'app-order-detail',
  standalone: true,
  imports: [ FormsModule, CommonModule, FontAwesomeModule, OrderItemComponent ],
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.css']
})
export class OrderDetailComponent {
  private subscription: Subscription | null = null;
  
  @Input() order: OrderResponseDTO = {
    orderID: '1',
    userID: 'user1',
    orderDate: new Date('2023-06-01'),
    totalAmount: 250.75,
    orderStatus: 'pending'
  };

  orderItems: OrderItemResponseDTO[] = [
    { orderItemID: '1', orderID: '1', productID: '1', quantity: 2, price: 100 },
    { orderItemID: '2', orderID: '1', productID: '2', quantity: 1, price: 50 }
  ];

  public faTrash: IconDefinition = faTrash;
  public faEdit: IconDefinition = faEdit;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.subscription = this.route.params.subscribe(params => {
      let id = params['orderId'];
      this.order.orderID = id;
   });
  }

  onDelete(): void {    
    console.log('Deleting order:', this.order);    
  }

  onUpdate(): void {    
    console.log('Updating order:', this.order);    
  }
}
