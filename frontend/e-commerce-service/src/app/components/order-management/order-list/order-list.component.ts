import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/free-brands-svg-icons';
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import { OrderResponseDTO, OrderService } from '../../../generated';
import { Router } from '@angular/router';
import { HeaderComponent } from '../../header/header.component';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [ FormsModule, CommonModule, FontAwesomeModule, HeaderComponent ],
  templateUrl: './order-list.component.html',
  styleUrl: './order-list.component.css'
})
export class OrderListComponent implements OnInit {
  orders: OrderResponseDTO[] = [];

  public faSearch: IconDefinition = faSearch;
  public searchText: string = '';
  public filterOption: string = '';
  public isLoggedIn: boolean = false;
  public accessToken: string | null = ''; 

  public enableSearch: boolean = false;

  constructor(private router: Router, private orderService: OrderService) {}

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;
      this.orderService.apiV1OrdersUserGet().subscribe(
        (data: OrderResponseDTO[]) => {
          this.orders = data;
        },
        error => {
          console.error('Error fetching orders', error);
        }
      );
    } 
  }

  // getUserName(userID: string | undefined): string | undefined {
  //   const user = this.users.find(user => user.userID === userID);
  //   return user ? user.userName : 'Unknown User';
  // }

  handleOrderClick(order: OrderResponseDTO): void {
    this.router.navigate(['/orders', order.orderID]);
  }
}
