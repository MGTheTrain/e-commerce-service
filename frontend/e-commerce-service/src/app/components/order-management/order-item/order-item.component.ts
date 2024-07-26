import { Component, Input, OnInit } from '@angular/core';
import { OrderItemResponseDTO, OrderService, ProductResponseDTO, ProductService } from '../../../generated';
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
  
  @Input() orderItem: OrderItemResponseDTO = {};

  @Input() product: ProductResponseDTO = {};

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

  public isLoggedIn: boolean = false;

  constructor(private productService: ProductService, private orderService: OrderService) {}

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;

      this.productService.apiV1ProductsProductIdGet(this.orderItem.productID!).subscribe(
        (data: ProductResponseDTO) => {
            this.product = data;
        }
      ); 
    } 
  }

  handleDeleteItemClick(orderItem: OrderItemResponseDTO): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.orderService.apiV1OrdersOrderIdItemsItemIdDelete(orderItem.orderID!, orderItem.orderItemID!).subscribe(
        (data: OrderItemResponseDTO) => {
          console.log("Delete order item with id", data.orderItemID, "from order with id", data.orderID);
        }
      );
    }
    window.location.reload();
  }
}
