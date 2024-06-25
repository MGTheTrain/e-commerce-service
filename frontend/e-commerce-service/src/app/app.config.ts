import { NgModule } from "@angular/core";
import { AppComponent } from "./app.component";
import { BrowserModule } from "@angular/platform-browser";
import { RoutingModule } from "./app.routes";
import { HttpClientModule } from "@angular/common/http";
import { ApiModule, Configuration } from "./generated";

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
        basePath: 'http://localhost:5000'
      });
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }