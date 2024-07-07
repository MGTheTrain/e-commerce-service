import { Component, Input, OnInit } from '@angular/core';
import { OrderItemResponseDTO, OrderResponseDTO } from '../../../generated';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faArrowLeft, faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { OrderItemComponent } from '../order-item/order-item.component';
import { v4 as uuidv4 } from 'uuid';
import { DetailHeaderComponent } from '../../header/detail-header/detail-header.component';

@Component({
  selector: 'app-order-detail',
  standalone: true,
  imports: [FormsModule, CommonModule, FontAwesomeModule, OrderItemComponent, DetailHeaderComponent],
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.css']
})
export class OrderDetailComponent implements OnInit {
  private subscription: Subscription | null = null;

  @Input() order: OrderResponseDTO = {
    orderID: uuidv4(),
    userID: '1',
    orderDate: new Date('2023-06-01'),
    totalAmount: 0,
    orderStatus: 'pending'
  };

  @Input() orderItems: OrderItemResponseDTO[] = [
    { orderItemID: '1', orderID: '1', productID: '1', quantity: 2, price: 100 },
    { orderItemID: '2', orderID: '1', productID: '2', quantity: 1, price: 50 }
  ];

  public faTrash: IconDefinition = faTrash;
  public faEdit: IconDefinition = faEdit;
  public faArrowLeft: IconDefinition = faArrowLeft;
  public isEditing: boolean = false;

  public isLoggedIn: boolean = false;

  constructor(private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;
    } 

    this.subscription = this.route.params.subscribe(params => {
      const id = params['orderId'];
      this.order.orderID = id;
    });
    this.calculateTotalAmount();
  }

  handleNavigateBackClick(): void {
    this.router.navigate(['/orders']);
  }

  handleEditClick(): void {
    this.isEditing = !this.isEditing;
  }

  calculateTotalAmount(): void {
    this.order.totalAmount = this.orderItems.reduce((total, item) => {
      const quantity = item.quantity ?? 0;
      const price = item.price ?? 0;
      return total + (quantity * price);
    }, 0);
  }

  handleDeleteOrderClick(): void {    
    console.log('Deleting order:', this.order);    
  }

  handleUpdateOrderClick(): void {    
    console.log('Updating order:', this.order);    
  }
}