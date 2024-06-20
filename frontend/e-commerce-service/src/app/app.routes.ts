import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PageNotFoundComponent } from './components/error-pages/page-not-found/page-not-found.component';
import { UserLoginComponent } from './components/user-management/user-login/user-login.component';
import { UserRegistrationComponent } from './components/user-management/user-registration/user-registration.component';
import { UserProfileComponent } from './components/user-management/user-profile/user-profile.component';
import { ProductListComponent } from './components/product-management/product-list/product-list.component';
import { ProductDetailComponent } from './components/product-management/product-detail/product-detail.component';
import { OrderListComponent } from './components/order-management/order-list/order-list.component';
import { OrderDetailComponent } from './components/order-management/order-detail/order-detail.component';
import { CartComponent } from './components/shopping-cart/cart/cart.component';
import { CartItemComponent } from './components/shopping-cart/cart-item/cart-item.component';
import { ReviewListComponent } from './components/review-management/review-list/review-list.component';
import { ReviewDetailComponent } from './components/review-management/review-detail/review-detail.component';
import { HomeComponent } from './components/home/home/home.component';

const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'user/registration', component: UserRegistrationComponent },
    { path: 'user/login', component: UserLoginComponent },
    { path: 'user/profile', component: UserProfileComponent },
    { path: 'products', component: ProductListComponent },
    { path: 'products/:productId', component: ProductDetailComponent },
    { path: 'orders', component: OrderListComponent },
    { path: 'orders/:orderId', component: OrderDetailComponent },
    { path: 'cart', component: CartComponent },
    { path: 'cart/items', component: CartItemComponent },
    { path: 'reviews', component: ReviewListComponent },
    { path: 'reviews/:reviewId', component: ReviewDetailComponent },
    { path: '**', component: PageNotFoundComponent },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class RoutingModule { }
