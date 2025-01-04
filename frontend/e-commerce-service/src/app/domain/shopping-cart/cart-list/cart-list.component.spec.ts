import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { CartListComponent } from './cart-list.component';
import { CartService } from '../../../generated';
import { AuthModule } from '@auth0/auth0-angular';
import { environment } from '../../../../../environments/environment';

describe('CartListComponent', () => {
  let component: CartListComponent;
  let fixture: ComponentFixture<CartListComponent>;

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
        CartService
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CartListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
