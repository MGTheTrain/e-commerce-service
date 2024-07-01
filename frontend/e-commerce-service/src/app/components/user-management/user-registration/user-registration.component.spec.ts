import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { UserRegistrationComponent } from './user-registration.component';
import { UserService } from '../../../generated';

describe('UserRegistrationComponent', () => {
  let component: UserRegistrationComponent;
  let fixture: ComponentFixture<UserRegistrationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule, 
        UserRegistrationComponent
      ],
      providers: [
        UserService
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserRegistrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
