import { NgModule } from "@angular/core";
import { AppComponent } from "./app.component";
import { BrowserModule } from "@angular/platform-browser";
import { RoutingModule } from "./app.routes";
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { ApiModule, Configuration } from "./generated";
import { environment } from "../../environments/environment";
import { AuthModule } from "@auth0/auth0-angular";
import { TokenInterceptor } from "./shared/interceptors/token.service";

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    RoutingModule,
    HttpClientModule,
    ApiModule.forRoot(() => {
      return new Configuration({
        basePath: environment.internalWebApiBasePath
      });
    }),
    AuthModule.forRoot({
      domain: environment.auth0.domain,  
      clientId: environment.auth0.clientId,  
      authorizationParams: {
        redirect_uri: window.location.origin,
        audience: environment.auth0.audience,
      }
    }),
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }