import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailHeaderComponent } from './detail-header.component';
import { AuthModule } from '@auth0/auth0-angular';
import { environment } from '../../../../../environments/environment';
import { CartService } from '../../../generated';
import { HttpClientModule } from '@angular/common/http';

describe('DetailHeaderComponent', () => {
  let component: DetailHeaderComponent;
  let fixture: ComponentFixture<DetailHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientModule,
        DetailHeaderComponent,
        AuthModule.forRoot({
          domain: environment.auth0.domain,
          clientId: environment.auth0.clientId,
          authorizationParams: {
            redirect_uri: window.location.origin,
            audience: environment.auth0.audience,
          }
        })
      ],
      providers:[
        CartService
      ]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DetailHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
