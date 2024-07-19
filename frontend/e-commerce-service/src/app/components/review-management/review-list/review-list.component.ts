import { Component, Input, OnInit } from '@angular/core';
import { ProductResponseDTO, ReviewResponseDTO, ReviewService } from '../../../generated';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { v4 as uuidv4 } from 'uuid';
import { HeaderComponent } from '../../header/header.component';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

@Component({
  selector: 'app-review-list',
  standalone: true,
  imports: [ CommonModule, FormsModule, FontAwesomeModule, HeaderComponent ],
  templateUrl: './review-list.component.html',
  styleUrl: './review-list.component.css'
})
export class ReviewListComponent implements OnInit {
  @Input() reviews: ReviewResponseDTO[] = [];

  @Input() products: ProductResponseDTO[] = [];

  availableCategories: string[] = [
    'Acoustic Guitar',
    'Electric Guitar',
    'Bass Guitar',
    'Classical Guitar',
    '12-String Guitar',
    '7-String Guitar',
    'Jazz Guitar',
    'Blues Guitar',
    'Metal Guitar',
    'Rock Guitar'
  ];

  public faPlus: IconDefinition = faPlus;

  public isLoggedIn: boolean = false;

  constructor(private router: Router, private reviewService: ReviewService) {}

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;
    } 

    this.reviewService.apiV1ReviewsGet().subscribe(
      (data: ReviewResponseDTO[]) => {
        this.reviews = data;
      },
      error => {
        console.error('Error fetching reviews', error);
      }
    );
  }

  handleCreateReviewClick(): void {
    this.router.navigate(['/reviews/creation']);
  }

  // getUserName(userID: string | undefined): string | undefined {
  //   const user = this.users.find(user => user.userID === userID);
  //   return user ? user.userName : 'Unknown User';
  // }

  getProductName(productID: string | undefined): string | undefined {
    const product = this.products.find(product => product.productID === productID);
    return product ? product.name
     : 'Unknown User';
  }

  handleReviewClick(review: ReviewResponseDTO): void {
    this.router.navigate(['/reviews', review.reviewID]);
  }
}
