import { Component, Input } from '@angular/core';
import { UserResponseDTO } from '../../../generated';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [FormsModule, CommonModule, FontAwesomeModule],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent {
  private subscription: Subscription | null = null;

  @Input() user: UserResponseDTO = { userID: '1', userName: 'John Doe', email: 'john.doe@example.com', role: 'Admin' };
  
  public faTrash: IconDefinition = faTrash;
  public faEdit: IconDefinition = faEdit;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.subscription = this.route.params.subscribe(params => {
      let id = params['userId'];
      this.user.userID = id;
   });
  }

  onDelete(): void {    
    console.log('Deleting user:', this.user);    
  }

  onUpdate(): void {    
    console.log('Updating user:', this.user);    
  }

}
