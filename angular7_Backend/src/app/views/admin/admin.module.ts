import '../../../polyfills';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { HttpClientModule } from '@angular/common/http';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminListComponent } from './admininformation/adminList.component';
import { AdminPermissionComponent } from './adminPermission/adminPermissionList.component';
import { AdminDetailComponent } from './admininformation/adminDetail/adminDetail.component';

import { AdminPermissionDetailComponent } from './adminPermission/adminPermissionDetail/adminPermissionDetail.component';
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

import { SharedModule } from '../../shared/shared.module';
import { LogRecordComponent } from './logRecord/logRecord.component';
import { AddPostDialogComponent } from './add-post-dialog/add-post-dialog.component';



@NgModule({
  exports: [
    //要用 Angular Material 加在這
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
    CommonModule,
    AdminRoutingModule, //後台管理者 Routing Module
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    SharedModule,
    MaterialModule, //這邊匯入上面定義的 Angular Material Module
    //AddPostDialogComponent 
  ],
  declarations: [
    AdminListComponent,
    AdminPermissionComponent,
    AdminDetailComponent,
    AdminPermissionDetailComponent,
    LogRecordComponent,
    AddPostDialogComponent 
  ],
  entryComponents: [AdminPermissionDetailComponent, LogRecordComponent,AddPostDialogComponent
   ],
})
export class AdminModule { }
