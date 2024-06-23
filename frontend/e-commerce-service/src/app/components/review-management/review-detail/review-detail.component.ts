import { Component, Input } from '@angular/core';
import { ReviewResponseDTO } from '../../../generated';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-review-detail',
  standalone: true,
  imports: [ FormsModule, CommonModule, FontAwesomeModule ],
  templateUrl: './review-detail.component.html',
  styleUrls: ['./review-detail.component.css']
})
export class ReviewDetailComponent {
  private subscription: Subscription | null = null;

  @Input() review: ReviewResponseDTO = {
    reviewID: '1',
    productID: '1',
    userID: 'user1',
    rating: 4,
    comment: 'Great product!',
    reviewDate: new Date('2023-06-01')
  };
  
  public faTrash: IconDefinition = faTrash;
  public faEdit: IconDefinition = faEdit;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.subscription = this.route.params.subscribe(params => {
      let id = params['reviewId'];
      this.review.reviewID = id;
   });
  }

  onDelete(): void {    
    console.log('Deleting review:', this.review);    
  }

  onUpdate(): void {    
    console.log('Updating review:', this.review);    
  }
}
