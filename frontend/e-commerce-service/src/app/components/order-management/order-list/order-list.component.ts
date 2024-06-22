import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/free-brands-svg-icons';
import { faSearch } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [ FormsModule, CommonModule, FontAwesomeModule ],
  templateUrl: './order-list.component.html',
  styleUrl: './order-list.component.css'
})
export class OrderListComponent {
  public faSearch: IconDefinition = faSearch;
  public searchText: string = '';
  public filterOption: string = '';
  public orders = [
    { name: 'Order 1', status: 'pending' },
    { name: 'Order 2', status: 'shipped' },
    { name: 'Order 3', status: 'delivered' },
    { name: 'Order 4', status: 'pending' },
    { name: 'Order 5', status: 'shipped' },
  ];

  filteredOrders() {
    return this.orders.filter(order => {
      return (
        (!this.searchText || order.name.toLowerCase().includes(this.searchText.toLowerCase())) &&
        (!this.filterOption || order.status === this.filterOption)
      );
    });
  }
}
