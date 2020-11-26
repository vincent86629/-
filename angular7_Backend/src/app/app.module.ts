import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import {
  MatButtonModule, //要用 Angular Material 要先在這 Import，才能在下面加入參考
  MatCheckboxModule,
  MatIconModule,
  MatInputModule,
  MatRippleModule,
  MatTableModule,
  MatTreeModule,
  MatSortModule,
  MatPaginatorModule,
  MatFormFieldModule,
  MatAutocompleteModule,
  MatTooltipModule,
  MatDatepickerModule,
  MatSelectModule,
  MAT_DATE_LOCALE,
  MatDialogModule
} from '@angular/material';
import { MatMomentDateModule, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';

import { AppRoutingModule, } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './views/nav-menu/nav-menu.component';
import { IndexComponent } from './views/index/index.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CookieService } from 'ngx-cookie-service';
import { VerifycodeComponent } from './shared/verifycode/verifycode.component';
import { FinanceListComponent } from './views/finance/finance-list/finance-list.component';
import { FinanceEditComponent } from './views/finance/finance-edit/finance-edit.component';
import { AddPostDialogComponent } from './shared/add-post-dialog/add-post-dialog.component';

@NgModule({
  imports: [
    MatButtonModule,
    MatCheckboxModule,
    MatIconModule,
    MatInputModule,
    MatRippleModule,
    MatTableModule,
    MatSortModule,
    MatFormFieldModule,
    MatTreeModule,
    MatTooltipModule,
    MatPaginatorModule,
    MatCheckboxModule,
    MatAutocompleteModule,
    MatDatepickerModule,
    MatMomentDateModule,
    MatSelectModule,
    MatDialogModule
  ],
  exports: [
    MatButtonModule,
    MatCheckboxModule,
    MatIconModule,
    MatInputModule,
    MatRippleModule,
    MatTableModule,
    MatSortModule,
    MatFormFieldModule,
    MatTreeModule,
    MatTooltipModule,
    MatPaginatorModule,
    MatCheckboxModule,
    MatAutocompleteModule,
    MatDatepickerModule,
    MatMomentDateModule,
    MatSelectModule,
    MatDialogModule
  ],
  providers: [
    { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } },
    { provide: MAT_DATE_LOCALE, useValue: 'zh-TW' }
  ]
})
export class MaterialModule { }

@NgModule({
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    MaterialModule
  ],
  declarations: [
    AppComponent,
    NavMenuComponent,
    IndexComponent,
    VerifycodeComponent,
    FinanceListComponent,
    FinanceEditComponent,
    AddPostDialogComponent
  ],
  providers: [CookieService],
  bootstrap: [AppComponent],
  entryComponents:[AddPostDialogComponent]
})
export class AppModule { }
