import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { OrderItemComponent } from './order-item.component';
import { OrderService, ProductService } from '../../../generated';

describe('OrderItemComponent', () => {
  let component: OrderItemComponent;
  let fixture: ComponentFixture<OrderItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule, 
        OrderItemComponent
      ],
      providers: [
        OrderService,
        ProductService
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrderItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
