import { Component, Input, OnInit } from '@angular/core';
import { ProductResponseDTO, ReviewResponseDTO, UserResponseDTO } from '../../../generated';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faArrowLeft, faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DetailHeaderComponent } from '../../header/detail-header/detail-header.component';

@Component({
  selector: 'app-review-detail',
  standalone: true,
  imports: [ FormsModule, CommonModule, FontAwesomeModule, DetailHeaderComponent ],
  templateUrl: './review-detail.component.html',
  styleUrls: ['./review-detail.component.css']
})
export class ReviewDetailComponent implements OnInit {
  private subscription: Subscription | null = null;

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
  
  public faTrash: IconDefinition = faTrash;
  public faEdit: IconDefinition = faEdit;
  public faArrowLeft: IconDefinition = faArrowLeft;
  public isEditing: boolean = false;

  public isLoggedIn: boolean = false;
  public accessToken: string | null = ''; 

  constructor(private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;
      this.accessToken = localStorage.getItem("accessToken");
      console.log("accessToken: ", this.accessToken);
    } else {
      this.isLoggedIn = false;
      this.accessToken = '';
    }
    
    this.subscription = this.route.params.subscribe(params => {
      const id = params['reviewId'];
      this.review.reviewID = id;
   });
  }

  handleNavigateBackClick(): void {
    this.router.navigate(['/reviews']);
  }

  handleEditClick(): void {
    this.isEditing = !this.isEditing;
  }

  handleDeleteReviewClick(): void {    
    console.log('Deleting review:', this.review);    
  }

  handleUpdateReviewClick(): void {    
    console.log('Updating review:', this.review);    
  }
}
