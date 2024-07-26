import { Component, Input, OnInit } from '@angular/core';
import { ProductResponseDTO, ProductService, ReviewRequestDTO, ReviewResponseDTO, ReviewService } from '../../../generated';
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

  @Input() review: ReviewResponseDTO = {};

  @Input() product: ProductResponseDTO = {};

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
  
  public faTrash: IconDefinition = faTrash;
  public faEdit: IconDefinition = faEdit;
  public faArrowLeft: IconDefinition = faArrowLeft;
  public isEditing: boolean = false;

  public isLoggedIn: boolean = false;
  public productReviewsUrl: string = "";
  public isReviewOwner: boolean = false;

  constructor(private router: Router, private route: ActivatedRoute, private reviewService: ReviewService, private productService: ProductService) { }

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;
    } 
    
    this.subscription = this.route.params.subscribe(params => {
      this.review.reviewID = params['reviewId'];

      this.reviewService.apiV1ReviewsReviewIdUserGet(this.review.reviewID!).subscribe(
        (data: ReviewResponseDTO) => {
          console.log("Review owned by:", data.userID);
          this.isReviewOwner = true;
        },
        error => {
          console.error('Error: User is not the product owner', error);
        }
      );

      this.reviewService.apiV1ReviewsReviewIdGet(this.review.reviewID!).subscribe(
        (data: ReviewResponseDTO) => {
          this.review = data;
          this.productService.apiV1ProductsProductIdGet(this.review.productID!).subscribe(
            (data: ProductResponseDTO) => {
              this.product = data;
              this.productReviewsUrl = `/products/${this.review.productID}/reviews`;
              console.log("productReviewsUrl", this.productReviewsUrl);
            },
            error => {
              console.error('Error retrieving product', error);
            }
          );
        },
        error => {
          console.error('Error retrieving review', error);
        }
      );
   });
  }

  handleNavigateBackClick(): void {
    this.router.navigate([this.productReviewsUrl]);
  }

  handleEditClick(): void {
    this.isEditing = !this.isEditing;
  }

  handleUpdateReviewClick(): void {    
    if(localStorage.getItem('isLoggedIn') === 'true') {
      const reviewRequestDto: ReviewRequestDTO = {
        productID: this.product.productID!,
        rating: this.review.rating!,
        comment: this.review.comment!,
      };
      this.reviewService.apiV1ReviewsReviewIdPut(this.review.reviewID!, reviewRequestDto).subscribe(
        (data: ReviewResponseDTO) => {
          console.log("Updated review with id", data.reviewID!);
          this.router.navigate([this.productReviewsUrl]);
        },
        error => {
          console.error('Error updating review', error);
        }
      );
    }
  }

  handleDeleteReviewClick(): void {    
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.reviewService.apiV1ReviewsReviewIdDelete(this.review.reviewID!).subscribe(
        () => {
          console.log("Deleted review");
          this.router.navigate([this.productReviewsUrl]);
        },
        error => {
          console.error('Error deleting review', error);
        }
      );
    } 
  }
}
