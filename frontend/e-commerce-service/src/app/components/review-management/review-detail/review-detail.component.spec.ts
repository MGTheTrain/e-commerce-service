import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ReviewDetailComponent } from './review-detail.component';
import { CartService, ProductService, ReviewService } from '../../../generated';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { AuthModule } from '@auth0/auth0-angular';
import { environment } from '../../../../../environments/environment';

class ActivatedRouteStub {
  params = of({ id: 1 }); // some mocked data
}

describe('ReviewDetailComponent', () => {
  let component: ReviewDetailComponent;
  let fixture: ComponentFixture<ReviewDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule, 
        ReviewDetailComponent,
        AuthModule.forRoot({
          domain: environment.auth0.domain,
          clientId: environment.auth0.clientId,
          authorizationParams: {
            redirect_uri: window.location.origin,
            audience: environment.auth0.audience,
          }
        })
      ],
      providers: [
        ReviewService,
        CartService,
        ProductService,
        { provide: ActivatedRoute, useClass: ActivatedRouteStub }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReviewDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
