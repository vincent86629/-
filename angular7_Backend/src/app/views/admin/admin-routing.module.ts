import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminListComponent } from './admininformation/adminList.component';
import { AdminPermissionComponent } from './adminPermission/adminPermissionList.component';
import { AdminDetailComponent } from './admininformation/adminDetail/adminDetail.component';
import { AdminPermissionDetailComponent } from './adminPermission/adminPermissionDetail/adminPermissionDetail.component';
import { LogRecordComponent } from './logRecord/logRecord.component';


const routes: Routes = [
  {
    path: '',
    data: {
      title: '會員管理'
    },
    children: [
      {
        path: 'adminList',
        component: AdminListComponent,
        data: {
          title: '會員資料維護清單'
        }
      },
      {
        path: 'adminDetail',
        component: AdminDetailComponent,
        data: {
          title: '會員資料維護'
        }
      },
      {
        path: 'adminPermissionList',
        component: AdminPermissionComponent,
        data: {
          title: '群組管理'
        }
      },
      {
        path: 'adminPermissionDetail',
        component: AdminPermissionDetailComponent,
        data: {
          title: '群組管理維護'
        }
      },
      {
        path: 'logRecord',
        component: LogRecordComponent,
        data: {
          title: '後台Log紀錄'
        }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
