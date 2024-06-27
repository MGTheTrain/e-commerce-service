import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/free-brands-svg-icons';
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import { OrderResponseDTO, OrderService, UserResponseDTO } from '../../../generated';
import { Router } from '@angular/router';
import { v4 as uuidv4 } from 'uuid';
import { HeaderComponent } from '../../header/header.component';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [ FormsModule, CommonModule, FontAwesomeModule, HeaderComponent ],
  templateUrl: './order-list.component.html',
  styleUrl: './order-list.component.css'
})
export class OrderListComponent {
  orders: OrderResponseDTO[] = [
    { orderID: uuidv4(), userID: '1', orderDate: new Date('2023-06-01'), totalAmount: 250.75, orderStatus: 'pending' },
    { orderID: uuidv4(), userID: '2', orderDate: new Date('2024-06-10'), totalAmount: 150.50, orderStatus: 'shipped' },
    { orderID: uuidv4(), userID: '3', orderDate: new Date('2022-06-15'), totalAmount: 300.00, orderStatus: 'delivered' },
    { orderID: uuidv4(), userID: '4', orderDate: new Date('2021-06-20'), totalAmount: 100.99, orderStatus: 'pending' },
    { orderID: uuidv4(), userID: '5', orderDate: new Date('2023-06-25'), totalAmount: 200.20, orderStatus: 'shipped' },
  ];

  users: UserResponseDTO[] = [
    { userID: '1', userName: 'John Doe', email: 'john.doe@example.com', role: 'Admin' },
    { userID: '2', userName: 'Jane Smith', email: 'jane.smith@example.com', role: 'User' },
    { userID: '3', userName: 'Mike Johnson', email: 'mike.johnson@example.com', role: 'User' },
    { userID: '4', userName: 'Emily Brown', email: 'emily.brown@example.com', role: 'Admin' },
    { userID: '5', userName: 'Chris Lee', email: 'chris.lee@example.com', role: 'User' },
  ];

  public faSearch: IconDefinition = faSearch;
  public searchText: string = '';
  public filterOption: string = '';

  constructor(private router: Router, private orderService: OrderService) {}

  ngOnInit(): void {
    this.orderService.apiV1OrdersGet().subscribe(
      (data: OrderResponseDTO[]) => {
        this.orders = data;
      },
      error => {
        console.error('Error fetching products', error);
      }
    );
  }

  getUserName(userID: string | undefined): string | undefined {
    const user = this.users.find(user => user.userID === userID);
    return user ? user.userName : 'Unknown User';
  }

  handleOrderClick(order: OrderResponseDTO): void {
    this.router.navigate(['/orders', order.orderID]);
  }
}
