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
  @Input() reviews: ReviewResponseDTO[] = [
    { reviewID: uuidv4(), productID: '1', userID: '1', rating: 4, comment: 'Great product!', reviewDate: new Date('2023-06-01') },
    { reviewID: uuidv4(), productID: '4', userID: '2', rating: 5, comment: 'Excellent service!', reviewDate: new Date('2023-06-02') },
    { reviewID: uuidv4(), productID: '2', userID: '3', rating: 3, comment: 'Could be better.', reviewDate: new Date('2023-06-03') },
    { reviewID: uuidv4(), productID: '3', userID: '4', rating: 5, comment: 'Highly recommended!', reviewDate: new Date('2023-06-04') },
    { reviewID: uuidv4(), productID: '2', userID: '5', rating: 4, comment: 'Good value for money.', reviewDate: new Date('2023-06-05') },
  ];

  @Input() products: ProductResponseDTO[] = [
    { productID: '1', categories: ['Electric Guitar'], name: 'Dean Razorback Guitar Blue', description: 'Description of Product A', price: 4999.99, stock: 10, imageUrl: 'https://s.yimg.com/ny/api/res/1.2/jVphTvtt1LwM3foboVcs_w--/YXBwaWQ9aGlnaGxhbmRlcjt3PTEyMDA7aD02MDA-/https://media.zenfs.com/en-US/homerun/consequence_of_sound_458/830585263f74148d1ac63c91bfe6e2f4' },
    { productID: '2', categories: ['Electric Guitar'], name: 'Dean Razorback Guitar White', description: 'Description of Product B', price: 2999.99, stock: 5, imageUrl: 'https://www.musicconnection.com/wp-content/uploads/2021/01/dean-dime-620x420.jpg' },
    { productID: '3', categories: ['Electric Guitar'], name: 'ESP LTD Greeny Black', description: 'Description of Product C', price: 3999.99, stock: 15, imageUrl: 'https://media.sound-service.eu/Artikelbilder/Shopsystem/1200x837/ALEXI%20LAIHO%20GREENY_1.jpg' },
    { productID: '4', categories: ['Electric Guitar'], name: 'Jackson Guitar Blue', description: 'Description of Product D', price: 1099.99, stock: 2, imageUrl: 'https://m.media-amazon.com/images/I/51EnIfqKY7L.jpg' },
    { productID: '5', categories: ['Electric Guitar'], name: 'Gibson Les Paul Black', description: 'Description of Product E', price: 999.99, stock: 8, imageUrl: 'https://morningsideschoolofmusic.co.uk/wp-content/uploads/2022/05/Gibson-Guitars-1024x576.jpg' },
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
