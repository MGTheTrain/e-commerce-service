import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/free-brands-svg-icons';
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import { OrderResponseDTO } from '../../../generated/api';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [ FormsModule, CommonModule, FontAwesomeModule ],
  templateUrl: './order-list.component.html',
  styleUrl: './order-list.component.css'
})
export class OrderListComponent {
  orders: OrderResponseDTO[] = [
    { orderID: '1', userID: '101', orderDate: new Date('2023-06-01'), totalAmount: 250.75, orderStatus: 'pending' },
    { orderID: '2', userID: '102', orderDate: new Date('2024-06-10'), totalAmount: 150.50, orderStatus: 'shipped' },
    { orderID: '3', userID: '103', orderDate: new Date('2022-06-15'), totalAmount: 300.00, orderStatus: 'delivered' },
    { orderID: '4', userID: '104', orderDate: new Date('2021-06-20'), totalAmount: 100.99, orderStatus: 'pending' },
    { orderID: '5', userID: '105', orderDate: new Date('2023-06-25'), totalAmount: 200.20, orderStatus: 'shipped' },
  ];

  public faSearch: IconDefinition = faSearch;
  public searchText: string = '';
  public filterOption: string = '';

  filteredOrders() {
    return this.orders.filter(order => {
      // Convert orderDate to string for easier text matching
      const orderDateString = order.orderDate?.toLocaleDateString() || '';
      // Check if searchText exists in any order property
      return (
        Object.values(order).some(val => 
          typeof val === 'string' && val.toLowerCase().includes(this.searchText.toLowerCase())) ||
        orderDateString.includes(this.searchText.toLowerCase())
      ) &&
        (!this.filterOption || order.orderStatus === this.filterOption)
    });
  }
}
