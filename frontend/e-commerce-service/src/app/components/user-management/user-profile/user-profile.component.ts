import { Component, Input, OnInit } from '@angular/core';
import { UserRequestDTO, UserResponseDTO, UserService } from '../../../generated';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faArrowLeft, faEdit, faEye, faEyeSlash, faTrash } from '@fortawesome/free-solid-svg-icons';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { DetailHeaderComponent } from '../../header/detail-header/detail-header.component';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [FormsModule, CommonModule, FontAwesomeModule, DetailHeaderComponent],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent implements OnInit {
  private subscription: Subscription | null = null;

  @Input() user: UserResponseDTO = { 
    userID: '',
    userName: '',
    email: '', 
    role: 'User' 
  };
  
  public password: string = '';
  public confirmPassword: string = '';
  public hidePassword: boolean = true;
  public hideConfirmPassword: boolean = true;

  public faTrash: IconDefinition = faTrash;
  public faEdit: IconDefinition = faEdit;
  public faEye: IconDefinition = faEye;
  public faEyeSlash: IconDefinition = faEyeSlash;
  public faArrowLeft: IconDefinition = faArrowLeft;

  constructor(private router: Router, private route: ActivatedRoute, private userService: UserService) { }

  ngOnInit(): void {
    this.subscription = this.route.params.subscribe(params => {
      const id = params['userId'];
      this.user.userID = id;

      this.userService.apiV1UsersIdGet(id).subscribe(
        (data: UserResponseDTO) => {
          this.user = data;
        },
        error => {
          console.error('Error fetching user with id', id, error);
        }
      );
   });
  }

  handleNavigateBackClick(): void {
    this.router.navigate(['/user']);
  }

  togglePasswordVisibility(): void {
    this.hidePassword = !this.hidePassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.hideConfirmPassword = !this.hideConfirmPassword;
  }

  handleDeleteUserClick(): void {    
    const userID = this.user.userID;
    if (userID) {
      this.userService.apiV1UsersIdDelete(userID).subscribe(
        () => {
          console.error('Successfully deleted user with id', userID);
          this.router.navigate(['/user']);
        },
        error => {
          console.error('Error deleting user with id', userID, error);
        }
      );
    } else {
      console.error('User ID is undefined');
    }
  }

  handleUpdateUserClick(): void {    
    if (this.password.length == 0 || this.confirmPassword.length == 0) {
      alert("Password fields empty!");
      return;
    }
    
    if (this.password !== this.confirmPassword) {
      alert("Passwords do not match!");
      return;
    }

    const userID = this.user.userID;
    if (userID) {
      const userRequest: UserRequestDTO = {
        userName: this.user.userName!,
        password: this.password,
        email: this.user.email!,
        role: 'User',
      };

      this.userService.apiV1UsersIdPut(userID, userRequest).subscribe(
        (data: UserResponseDTO) => {
          console.error('Successfully updated user with id', userID);
          this.router.navigate(['/user']);
        },
        error => {
          console.error('Error updating user with id', userID, error);
        }
      );
    } else {
      console.error('User ID is undefined');
    }
  }
}
