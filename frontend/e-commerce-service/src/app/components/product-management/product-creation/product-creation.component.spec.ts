import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ProductCreationComponent } from './product-creation.component';
import { ProductService } from '../../../generated';

describe('ProductCreationComponent', () => {
  let component: ProductCreationComponent;
  let fixture: ComponentFixture<ProductCreationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule, 
        ProductCreationComponent
      ],
      providers: [
        ProductService 
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductCreationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
