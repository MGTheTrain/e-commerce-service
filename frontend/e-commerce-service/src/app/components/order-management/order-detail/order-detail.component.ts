import { Component, Input, OnInit } from '@angular/core';
import { OrderItemResponseDTO, OrderRequestDTO, OrderResponseDTO, OrderService, ProductResponseDTO, ProductService } from '../../../generated';
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

  @Input() order: OrderResponseDTO = {};

  @Input() orderItems: OrderItemResponseDTO[] = [];

  public faTrash: IconDefinition = faTrash;
  public faEdit: IconDefinition = faEdit;
  public faArrowLeft: IconDefinition = faArrowLeft;
  public isEditing: boolean = false;

  public isLoggedIn: boolean = false;
  public isOrderOwner: boolean = false;

  constructor(private router: Router, private route: ActivatedRoute, private orderService: OrderService, private productService: ProductService) { }

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;
      this.subscription = this.route.params.subscribe(params => {
        const id = params['orderId'];
        this.order.orderID = id;
        
        this.orderService.apiV1OrdersOrderIdUserGet(this.order.orderID!).subscribe(
          (data: OrderResponseDTO) => {
            console.log("Order owned by:", data.userID);
            this.isOrderOwner = true;
          },
          error => {
            console.error('Error: User is not the order owner', error);
          }
        );

        this.orderService.apiV1OrdersOrderIdGet(this.order.orderID!).subscribe(
          (data: OrderResponseDTO) => {
            this.order = data;
          },
          error => {
            console.error('Error retrieving order', error);
          }
        );
      });
      this.calculateTotalAmount();
    } 
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
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.orderService.apiV1OrdersOrderIdDelete(this.order.orderID!).subscribe(
        () => {
          console.log("Deleted order");
          this.router.navigate(['/orders']);
        },
        error => {
          console.error('Error deleting order', error);
        }
      );
    }
  }

  handleUpdateOrderClick(): void {    
    if(localStorage.getItem('isLoggedIn') === 'true') {
      const orderRequestDto: OrderRequestDTO = {
        totalAmount: this.order.totalAmount!,
        orderStatus: this.order.orderStatus!,
        currencyCode: this.order.currencyCode,
        referenceId: this.order.referenceId,
        addressLine1: this.order.addressLine1,
        addressLine2: this.order.addressLine2,
        adminArea2: this.order.adminArea2,
        adminArea1: this.order.adminArea1,
        postalCode: this.order.postalCode,
        countryCode: this.order.countryCode!,
      };
      this.orderService.apiV1OrdersOrderIdPut(this.order.orderID!, orderRequestDto).subscribe(
        (data: OrderResponseDTO) => {
          console.log("Updated order with id", data.orderID!);
          this.router.navigate(['/orders']);
        },
        error => {
          console.error('Error updating review', error);
        }
      );
    }
  }
}