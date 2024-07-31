import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faArrowLeft, faPlus } from '@fortawesome/free-solid-svg-icons';
import { ProductResponseDTO, ProductService, ReviewRequestDTO, ReviewResponseDTO, ReviewService } from '../../../generated';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { DetailHeaderComponent } from '../../header/detail-header/detail-header.component';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-review-creation',
  standalone: true,
  imports: [FormsModule, CommonModule, FontAwesomeModule, DetailHeaderComponent],
  templateUrl: './review-creation.component.html',
  styleUrl: './review-creation.component.css'
})
export class ReviewCreationComponent implements OnInit {
  private subscription: Subscription | null = null;

  @Input() review: ReviewResponseDTO = {};

  @Input() product: ProductResponseDTO = {};
  
  public faPlus: IconDefinition = faPlus;
  public faArrowLeft: IconDefinition = faArrowLeft;

  public isLoggedIn: boolean = false;

  constructor(private router: Router, private route: ActivatedRoute, private productService: ProductService, private reviewService: ReviewService) { }

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;

      this.subscription = this.route.params.subscribe(params => {
        if (params['productId']) {
            const productId: string = params['productId'];
            
            // update product name
            this.productService.apiV1ProductsProductIdGet(productId).subscribe(
              (data: ProductResponseDTO) => {
                this.product = data;
              },
              error => {
                console.error('Error fetching product', error);
              }
            );
        }
      });
    } 
  }

  handleNavigateBack(): void {
    this.router.navigate(['/']);
  }

  handleCreateReview(): void {
    // create review
    const reviewRequestDto: ReviewRequestDTO = {
      productID: this.product.productID!,
      rating: this.review.rating!,
      comment: this.review.comment!,
    };
    this.reviewService.apiV1ReviewsPost(reviewRequestDto).subscribe(
      (data: ReviewResponseDTO) => {
        console.log("Created review ", data)
      },
      error => {
        console.error('Error creating review', error);
      }
    );
    this.router.navigate(['/']);
  }
}
