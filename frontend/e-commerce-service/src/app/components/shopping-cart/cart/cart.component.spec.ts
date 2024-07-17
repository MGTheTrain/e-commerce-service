import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { CartComponent } from './cart.component';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { AuthModule } from '@auth0/auth0-angular'; 
import { environment } from '../../../../../environments/environment';
import { CartService } from '../../../generated';

class ActivatedRouteStub {
  params = of({ id: 1 }); 
}

describe('CartComponent', () => {
  let component: CartComponent;
  let fixture: ComponentFixture<CartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
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
        CartComponent,
        CartService,
        { provide: ActivatedRoute, useClass: ActivatedRouteStub }
      ]
    }).compileComponents(); 

    fixture = TestBed.createComponent(CartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
