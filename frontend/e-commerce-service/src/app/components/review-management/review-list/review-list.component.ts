import { Component } from '@angular/core';
import { ReviewResponseDTO } from '../../../generated/api';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import { Router } from '@angular/router';

@Component({
  selector: 'app-review-list',
  standalone: true,
  imports: [ CommonModule, FormsModule, FontAwesomeModule ],
  templateUrl: './review-list.component.html',
  styleUrl: './review-list.component.css'
})
export class ReviewListComponent {
  reviews: ReviewResponseDTO[] = [
    { reviewID: '1', productID: '1', userID: 'user1', rating: 4, comment: 'Great product!', reviewDate: new Date('2023-06-01') },
    { reviewID: '2', productID: '1', userID: 'user2', rating: 5, comment: 'Excellent service!', reviewDate: new Date('2023-06-02') },
    { reviewID: '3', productID: '2', userID: 'user1', rating: 3, comment: 'Could be better.', reviewDate: new Date('2023-06-03') },
    { reviewID: '4', productID: '3', userID: 'user3', rating: 5, comment: 'Highly recommended!', reviewDate: new Date('2023-06-04') },
    { reviewID: '5', productID: '2', userID: 'user2', rating: 4, comment: 'Good value for money.', reviewDate: new Date('2023-06-05') },
  ];

  public faSearch: IconDefinition = faSearch;
  public searchText: string = '';

  constructor(private router: Router) {}

  filteredReviews() {
    return this.reviews.filter(review => {
      return (
        Object.values(review).some(val => 
          typeof val === 'string' && val.toLowerCase().includes(this.searchText.toLowerCase())
        )
      );
    })
  }

  handleReviewClick(review: ReviewResponseDTO): void {
    this.router.navigate(['/reviews', review.reviewID]);
  }
}
