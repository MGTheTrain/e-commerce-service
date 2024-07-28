import { Component, Input, OnInit } from '@angular/core';
import { ProductResponseDTO, ProductService, ReviewResponseDTO, ReviewService } from '../../../generated';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HeaderComponent } from '../../header/header.component';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-review-list',
  standalone: true,
  imports: [ CommonModule, FormsModule, FontAwesomeModule, HeaderComponent ],
  templateUrl: './review-list.component.html',
  styleUrl: './review-list.component.css'
})
export class ReviewListComponent implements OnInit {
  private subscription: Subscription | null = null;
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
  public productId: string | null = null;
  public enableSearch: boolean = false;

  constructor(private router: Router, private route: ActivatedRoute, private reviewService: ReviewService, private productService: ProductService) {}

  ngOnInit(): void {
    if(localStorage.getItem('isLoggedIn') === 'true') {
      this.isLoggedIn = true;

      this.subscription = this.route.params.subscribe(params => {
        if (params['productId']) {
            this.productId = params['productId'];
            this.reviewService.apiV1ReviewsProductProductIdGet(this.productId!).subscribe(
              (data: ReviewResponseDTO[]) => {
                this.reviews = data;
                this.getReviewsForProduct();
              },
              error => {
                console.error('Error fetching reviews', error);
              }
            );
        } else {
          this.reviewService.apiV1ReviewsGet().subscribe(
            (data: ReviewResponseDTO[]) => {
              this.reviews = data;
              this.getReviewsForProduct();
            },
            error => {
              console.error('Error fetching reviews', error);
            }
          );
        } 
      });
    } 
  }

  getReviewsForProduct(): void {
    for(const review of this.reviews) {
      this.productService.apiV1ProductsProductIdGet(review.productID!).subscribe(
        (data: ProductResponseDTO) => {
          this.products.push(data);
        }
      );
    }
  }

  handleCreateReviewClick(): void {
    if(this.productId != null) {
      this.router.navigate(['/products', this.productId! , 'review', 'creation']);
    }
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
