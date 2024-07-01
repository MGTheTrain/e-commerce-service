import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ReviewCreationComponent } from './review-creation.component';
import { ReviewService } from '../../../generated';

describe('ReviewCreationComponent', () => {
  let component: ReviewCreationComponent;
  let fixture: ComponentFixture<ReviewCreationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        ReviewCreationComponent
      ],
      providers: [
        ReviewService
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
