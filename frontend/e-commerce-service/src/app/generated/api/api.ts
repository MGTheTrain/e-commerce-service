export * from './cart.service';
import { CartService } from './cart.service';
export * from './order.service';
import { OrderService } from './order.service';
export * from './product.service';
import { ProductService } from './product.service';
export * from './review.service';
import { ReviewService } from './review.service';
export const APIS = [CartService, OrderService, ProductService, ReviewService];
