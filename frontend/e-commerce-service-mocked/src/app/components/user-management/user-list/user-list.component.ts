import { Component } from '@angular/core';
import { UserResponseDTO } from '../../../generated';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import { Router } from '@angular/router';
import { HeaderComponent } from '../../header/header.component';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [ CommonModule, FormsModule, FontAwesomeModule, HeaderComponent ],
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

  public faSearch: IconDefinition = faSearch;
  public searchText: string = '';

  constructor(private router: Router) {}

  ngOnInit(): void {
    // GET api/v1/users
  }

  filteredUsers() {
    return this.users.filter(user => {
      return (
        Object.values(user).some(val => 
          typeof val === 'string' && val.toLowerCase().includes(this.searchText.toLowerCase())
        )
      );
    });
  }

  handleUserClick(user: UserResponseDTO): void {
    this.router.navigate(['/user', user.userID]);
  }
}
