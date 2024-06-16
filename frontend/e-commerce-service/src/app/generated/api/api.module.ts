import { NgModule, ModuleWithProviders, SkipSelf, Optional } from '@angular/core';
import { Configuration } from './configuration';
import { HttpClient } from '@angular/common/http';


import { CartService } from './api/cart.service';
import { CategoryService } from './api/category.service';
import { OrderService } from './api/order.service';
import { ProductService } from './api/product.service';
import { ReviewService } from './api/review.service';
import { UserService } from './api/user.service';

@NgModule({
  imports:      [],
  declarations: [],
  exports:      [],
  providers: [
    CartService,
    CategoryService,
    OrderService,
    ProductService,
    ReviewService,
    UserService ]
})
export class ApiModule {
    public static forRoot(configurationFactory: () => Configuration): ModuleWithProviders<ApiModule> {
        return {
            ngModule: ApiModule,
            providers: [ { provide: Configuration, useFactory: configurationFactory } ]
        };
    }

    constructor( @Optional() @SkipSelf() parentModule: ApiModule,
                 @Optional() http: HttpClient) {
        if (parentModule) {
            throw new Error('ApiModule is already loaded. Import in your base AppModule only.');
        }
        if (!http) {
            throw new Error('You need to import the HttpClientModule in your AppModule! \n' +
            'See also https://github.com/angular/angular/issues/20575');
        }
    }
}
