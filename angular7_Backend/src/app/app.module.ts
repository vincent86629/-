import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule, } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './views/nav-menu/nav-menu.component';
import { IndexComponent } from './views/index/index.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatSortModule} from '@angular/material/sort';
import { CookieService } from 'ngx-cookie-service';
import { MyNewComponentComponent } from './views/my-new-component/my-new-component.component';
import { VerifycodeComponent } from './views/verifycode/verifycode.component';




@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    IndexComponent,
    MyNewComponentComponent,
    VerifycodeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatSortModule,
    FormsModule
  ],
  providers: [
    CookieService],
  bootstrap: [AppComponent]
})
export class AppModule { }
