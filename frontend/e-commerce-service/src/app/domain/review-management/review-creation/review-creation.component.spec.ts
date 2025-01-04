import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ReviewCreationComponent } from './review-creation.component';
import { CartService, ProductService, ReviewService } from '../../../generated';
import { AuthModule } from '@auth0/auth0-angular';
import { environment } from '../../../../../environments/environment';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

class ActivatedRouteStub {
  params = of({ id: 1 }); // some mocked data
}

describe('ReviewCreationComponent', () => {
  let component: ReviewCreationComponent;
  let fixture: ComponentFixture<ReviewCreationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        ReviewCreationComponent,
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

    fixture = TestBed.createComponent(ReviewCreationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
