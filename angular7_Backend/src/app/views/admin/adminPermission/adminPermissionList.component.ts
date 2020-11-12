import { Component, OnInit, ViewChild } from '@angular/core';
import { PermissionData } from '../../../data/admindata';
import { APIReturn } from '../../../data/apidata';
import { Router } from '@angular/router';
import { HttpService } from '../../../service/http-service/http.service';
import { MatPaginator, MatSort, MatTableDataSource,MatDialog } from '@angular/material';
import { LogService } from '../../../service/log-service/log.service';
import { AddPostDialogComponent } from '../../../shared/add-post-dialog/add-post-dialog.component';
@Component({
  selector: 'app-admin-permission',
  templateUrl: './adminPermissionList.component.html',
  styleUrls: ['./adminPermissionList.component.css']
})
export class AdminPermissionComponent implements OnInit {

  //路徑
  permissionDetailUrl = '/admin/adminPermissionDetail'; //權限 Detail 頁面 URL

  // API URL
  postUrl: string = "api/Admin/AdminPermission/"; //取得權限 API 前綴
  deleteAdminPermissionURL: string = this.postUrl + 'DeleteAdminPermission'; //刪除權限 API 
  GetAdminPermissionsURL: string = this.postUrl + 'GetAdminPermissions'; //取得權限 API

  //資料
  permissionListData: FormModel; //頁面用的權限清單資料     
  searchCondition: PermissionData = new PermissionData();                  
  statusOptions: any[] = //狀態下拉選單
    [
      { 'isEnable': null, 'optionName': '請選擇' },
      { 'isEnable': true, 'optionName': '啟用' },
      { 'isEnable': false, 'optionName': '停用' }
    ];

  //Angular Material Table
  dataSource = new MatTableDataSource<PermissionData>(); //資料來源
  @ViewChild(MatSort) sort: MatSort; //排序
  @ViewChild(MatPaginator) paginator: MatPaginator; //分頁

  //建構子 (DI 由此注入)
  constructor(private httpService: HttpService, private logService: LogService, private router: Router,public dialog:MatDialog) { }

  //頁面進入點
  ngOnInit() {
    this.permissionListData = new FormModel();

    this.httpService.post<any>(this.postUrl + 'GetAdminPermissions', null).subscribe(
      (permissions: PermissionData[]) => {

        this.permissionListData.permissionList = permissions;
        this.dataSource.data = this.permissionListData.permissionList;
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
      });
  }

  //====================以下自訂函數=====================//

  // 編輯群組資料
  EditPermission(item) {
    // 導向adminPermissionDetail並帶參數
    this.router.navigate([this.permissionDetailUrl, { state: 'edit', id: item.id }]);
  }

  // 檢視群組資料
  DetailPermission(item) {
    // 導向adminPermissionDetail並帶參數
    this.router.navigate([this.permissionDetailUrl, { state: 'detail', id: item.id }]);
  }

  // 新增群組資料
  CreatePermission() {
    // 導向adminPermissionDetail並帶參數
    this.router.navigate([this.permissionDetailUrl, { state: 'create', id: 0 }]);
  }

  // 刪除群組資料
  DeletePermission(permission: PermissionData) {
    // 跳出提示訊息詢問是否要刪除
    const dialogRef = this.dialog.open(AddPostDialogComponent, {
      data: {
        title: '是否刪除',
        messages:
          [
            '是否要刪除 ' + permission.codeName
          ],
        bt_confirm: '確認',
        bt_cancel: '取消'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
           // 實作刪除會員資料
      this.httpService.post<any>(this.deleteAdminPermissionURL, permission.id).subscribe(
        (apiReturn: APIReturn) => {
          if (apiReturn.code == 0) {

            this.dialog.open(AddPostDialogComponent, {
              data: {
                title: '執行成功',
                messages: [apiReturn.message],
                bt_cancel: '確定',

              }
            });
            //重load清單
            this.httpService.post<any>(this.GetAdminPermissionsURL, null).subscribe(
              (permissions: PermissionData[]) => {

                this.permissionListData.permissionList = permissions;
                this.dataSource.data = this.permissionListData.permissionList;
                this.dataSource.sort = this.sort;
                this.dataSource.paginator = this.paginator;
              });
          }
          else {
            this.logService.LogError(apiReturn.message);
            
            this.dialog.open(AddPostDialogComponent, {
              data: {
                title: '執行失敗',
                messages: [apiReturn.message],
                bt_cancel: '確定',

              }
            });
          }
        },
        (err: any) => {
          this.logService.LogError(err);

          this.dialog.open(AddPostDialogComponent, {
            data: {
              title: '錯誤',
              messages: [err],
              bt_cancel: '確定',

            }
          });

        });
      }
    });
    // if (confirm('是否要刪除 ' + permission.codeName)) {   
    // }
  }

  Search() {

    var data = this.permissionListData.permissionList;

    //權限
    if (this.searchCondition.codeName && this.searchCondition.codeName.length > 0) {
      data = data.filter(a => a.codeName.indexOf(this.searchCondition.codeName) > -1);
    }

    //狀態
    if (this.searchCondition && (this.searchCondition.isEnable != null)) {
      data = data.filter(a => (a.isEnable == this.searchCondition.isEnable));
    }

    this.dataSource.data = data;
  }


}
//====================以上自訂函數=====================//

//====================以下自訂型別=====================//
class FormModel {
  permissionList: PermissionData[];
}
//====================以上自訂型別=====================//