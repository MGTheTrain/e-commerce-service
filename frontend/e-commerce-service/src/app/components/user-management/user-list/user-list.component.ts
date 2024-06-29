import { Component } from '@angular/core';
import { UserResponseDTO, UserService } from '../../../generated';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HeaderComponent } from '../../header/header.component';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [ CommonModule, FormsModule, HeaderComponent ],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css'
})
export class UserListComponent {
  users: UserResponseDTO[] = [
    { userID: '1', userName: 'John Doe', email: 'john.doe@example.com', role: 'Admin' },
    { userID: '2', userName: 'Jane Smith', email: 'jane.smith@example.com', role: 'User' },
    { userID: '3', userName: 'Mike Johnson', email: 'mike.johnson@example.com', role: 'User' },
    { userID: '4', userName: 'Emily Brown', email: 'emily.brown@example.com', role: 'Admin' },
    { userID: '5', userName: 'Chris Lee', email: 'chris.lee@example.com', role: 'User' },
  ];

  constructor(private router: Router, private userService: UserService) {}

  ngOnInit(): void {
    this.userService.apiV1UsersGet().subscribe(
      (data: UserResponseDTO[]) => {
        this.users = data;
      },
      error => {
        console.error('Error fetching users', error);
      }
    );
  }

  handleUserClick(user: UserResponseDTO): void {
    this.router.navigate(['/user', user.userID]);
  }
}
