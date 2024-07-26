import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PageNotFoundComponent } from './components/error-pages/page-not-found/page-not-found.component';
// import { UserLoginComponent } from './components/user-management/user-login/user-login.component';
// import { UserRegistrationComponent } from './components/user-management/user-registration/user-registration.component';
// import { ResetPasswordComponent } from './components/user-management/reset-password/reset-password.component';
import { ProductListComponent } from './components/product-management/product-list/product-list.component';
import { ProductDetailComponent } from './components/product-management/product-detail/product-detail.component';
import { OrderListComponent } from './components/order-management/order-list/order-list.component';
import { OrderDetailComponent } from './components/order-management/order-detail/order-detail.component';
import { CartComponent } from './components/shopping-cart/cart/cart.component';
import { ReviewListComponent } from './components/review-management/review-list/review-list.component';
import { ReviewDetailComponent } from './components/review-management/review-detail/review-detail.component';
import { CartListComponent } from './components/shopping-cart/cart-list/cart-list.component';
import { ProductCreationComponent } from './components/product-management/product-creation/product-creation.component';
import { ReviewCreationComponent } from './components/review-management/review-creation/review-creation.component';

const routes: Routes = [
    { path: '', component: ProductListComponent },

    // Product Management
    { path: 'products/creation', component: ProductCreationComponent },
    { path: 'products/:productId', component: ProductDetailComponent },

    // Cart Management
    { path: 'cart', component: CartComponent },
    { path: 'carts/:cartId', component: CartComponent },
    { path: 'carts', component: CartListComponent },

    // Order Management
    { path: 'orders', component: OrderListComponent },
    { path: 'orders/:orderId', component: OrderDetailComponent },
    
    // Review Management
    { path: 'reviews', component: ReviewListComponent }, // List all reviews
    { path: 'reviews/:reviewId', component: ReviewDetailComponent }, // View a specific review by reviewId
    { path: 'products/:productId/reviews', component: ReviewListComponent }, // List reviews for a specific product
    { path: 'products/:productId/review/creation', component: ReviewCreationComponent }, // Create a review for a specific product
    
    { path: '**', component: PageNotFoundComponent },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class RoutingModule { }
