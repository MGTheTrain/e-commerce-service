import { TestBed } from '@angular/core/testing';
import { TokenInterceptor } from './token.service';

describe('TokenInterceptor', () => {
  let service: TokenInterceptor;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TokenInterceptor]
    });
    service = TestBed.inject(TokenInterceptor);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
