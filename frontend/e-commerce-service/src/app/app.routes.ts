import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PageNotFoundComponent } from './components/error-pages/page-not-found/page-not-found.component';
// import { UserLoginComponent } from './components/user-management/user-login/user-login.component';
// import { UserRegistrationComponent } from './components/user-management/user-registration/user-registration.component';
// import { ResetPasswordComponent } from './components/user-management/reset-password/reset-password.component';
import { UserProfileComponent } from './components/user-management/user-profile/user-profile.component';
import { ProductListComponent } from './components/product-management/product-list/product-list.component';
import { ProductDetailComponent } from './components/product-management/product-detail/product-detail.component';
import { OrderListComponent } from './components/order-management/order-list/order-list.component';
import { OrderDetailComponent } from './components/order-management/order-detail/order-detail.component';
import { CartComponent } from './components/shopping-cart/cart/cart.component';
import { ReviewListComponent } from './components/review-management/review-list/review-list.component';
import { ReviewDetailComponent } from './components/review-management/review-detail/review-detail.component';
import { UserListComponent } from './components/user-management/user-list/user-list.component';
import { CartListComponent } from './components/shopping-cart/cart-list/cart-list.component';
import { ProductCreationComponent } from './components/product-management/product-creation/product-creation.component';
import { ReviewCreationComponent } from './components/review-management/review-creation/review-creation.component';

const routes: Routes = [
    // { path: 'user/registration', component: UserRegistrationComponent },
    // { path: 'user/login', component: UserLoginComponent },
    // { path: 'user/reset-password', component: ResetPasswordComponent },
    { path: 'user/:userId', component: UserProfileComponent },
    { path: 'user/:userId/cart', component: CartComponent },
    { path: 'user', component: UserListComponent },
    { path: '', component: ProductListComponent },
    { path: 'products/creation', component: ProductCreationComponent },
    { path: 'products/:productId', component: ProductDetailComponent },
    { path: 'orders', component: OrderListComponent },
    // { path: 'orders/creation', component: OrderCreationComponent }, // order will be created trough the cart
    { path: 'orders/:orderId', component: OrderDetailComponent },
    { path: 'carts', component: CartListComponent },
    { path: 'reviews', component: ReviewListComponent },
    { path: 'reviews/creation', component: ReviewCreationComponent },
    { path: 'reviews/:reviewId', component: ReviewDetailComponent },
    { path: '**', component: PageNotFoundComponent },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class RoutingModule { }
