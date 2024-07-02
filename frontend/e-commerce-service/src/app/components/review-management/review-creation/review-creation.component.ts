import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faArrowLeft, faPlus } from '@fortawesome/free-solid-svg-icons';
import { ProductResponseDTO, ReviewResponseDTO, UserResponseDTO } from '../../../generated';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { DetailHeaderComponent } from '../../header/detail-header/detail-header.component';

@Component({
  selector: 'app-review-creation',
  standalone: true,
  imports: [FormsModule, CommonModule, FontAwesomeModule, DetailHeaderComponent],
  templateUrl: './review-creation.component.html',
  styleUrl: './review-creation.component.css'
})
export class ReviewCreationComponent implements OnInit {
  @Input() review: ReviewResponseDTO = {
    reviewID: '1',
    productID: '1',
    userID: 'user1',
    rating: 4,
    comment: 'Great product!',
    reviewDate: new Date('2023-06-01')
  };

  @Input() user: UserResponseDTO = { 
    userID: '1', 
    userName: 'John Doe', 
    email: 'john.doe@example.com', 
    role: 'Admin' 
  };

  @Input() product: ProductResponseDTO = { 
    productID: '1', 
    categoryID: '101', 
    name: 'Dean Razorback Guitar Blue', 
    description: 'Description of Product A', 
    price: 4999.99, 
    stock: 10, 
    imageUrl: 'https://s.yimg.com/ny/api/res/1.2/jVphTvtt1LwM3foboVcs_w--/YXBwaWQ9aGlnaGxhbmRlcjt3PTEyMDA7aD02MDA-/https://media.zenfs.com/en-US/homerun/consequence_of_sound_458/830585263f74148d1ac63c91bfe6e2f4' 
  };
  
  public faPlus: IconDefinition = faPlus;
  public faArrowLeft: IconDefinition = faArrowLeft;

  public isLoggedIn: boolean = false;

  constructor(private router: Router) { }

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;
    } 
  }

  handleNavigateBackClick(): void {
    this.router.navigate(['/reviews']);
  }

  handleCreateReviewClick(): void {
    console.log('Creating review:', this.review);    
  }
}
