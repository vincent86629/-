import { Component, OnInit, ViewChild } from '@angular/core';
import { AdminData, GroupCheckData } from '../../../data/admindata';
import { APIReturn } from '../../../data/apidata';
import { Router } from '@angular/router';
import { HttpService } from '../../../service/http-service/http.service';
import { LogService } from '../../../service/log-service/log.service';
import { MatPaginator, MatSort, MatTableDataSource, MatDialog } from '@angular/material';
import { AdminDetailComponent } from './adminDetail/adminDetail.component';
import { AdminPermissionComponent } from '../adminPermission/adminPermissionList.component';
import { AddPostDialogComponent } from '../add-post-dialog/add-post-dialog.component';
import { LogRecordComponent } from '../logRecord/logRecord.component';
import { text } from '@angular/core/src/render3/instructions';

@Component({
  selector: 'app-adminList',
  templateUrl: './adminList.component.html',
  styleUrls: ['./adminList.component.css']
})
export class AdminListComponent implements OnInit {

  //API 路徑
  postUrl: string = "api/Admin/Admin/"; //API 路徑前綴
  getAdminURL: string = this.postUrl + 'GetAdmins'; //取得所有 admin 資料的 API 路徑

  //資料
  adminData: FormModel = new FormModel(); //前端要用到的資料清單

  //查詢條件
  groupData: GroupCheckData[] = []; //勾選的權限
  searchCondition: AdminData = new AdminData();
  isSelectAllGroup: boolean = true; //權現是否全選                          
  statusOptions: any[] = //狀態下拉選單
    [
      { 'isEnable': null, 'optionName': '請選擇' },
      { 'isEnable': true, 'optionName': '啟用' },
      { 'isEnable': false, 'optionName': '停用' }
    ];

  //Angular Material Table
  dataSource = new MatTableDataSource<AdminData>(); //資料來源
  @ViewChild(MatSort) sort: MatSort; //排序
  @ViewChild(MatPaginator) paginator: MatPaginator; //分頁



  //建構子 (DI 由此注入)
  constructor(private httpService: HttpService, private logService: LogService, private router: Router, public dialog: MatDialog) { }

  //頁面進入點
  ngOnInit() {

    //取得 後台管理者清單
    this.httpService.post<any>(this.getAdminURL, null).subscribe(
      (adminList: any) => {

        //前端要用到的資料清單
        this.adminData.adminList = adminList;
        //把資料餵給 Angular Material Table
        this.dataSource.data = this.adminData.adminList;
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;

        //所有的會員資料
        this.adminData.adminList.forEach(admin => {

          //所有的群組
          admin.groups.forEach(group => {

            //如果不包含這個群組
            if (group.isChecked && (this.groupData.filter(a => a.id === group.id).length === 0)) {

              //加到清單裡面
              var _group = new GroupCheckData();
              _group.id = group.id;
              _group.codeName = group.codeName;
              _group.isChecked = group.isChecked;

              this.groupData.push(_group);
            }
          });
        });
      });
  }


  //====================以下自訂函數=====================//

  Search() {

    var data = this.adminData.adminList;

    //姓名
    if (this.searchCondition.name && this.searchCondition.name.length > 0) {
      data = data.filter(a => a.name.indexOf(this.searchCondition.name) > -1);
    }
    //帳號
    if (this.searchCondition.account && this.searchCondition.account.length > 0) {
      data = data.filter(a => a.account.indexOf(this.searchCondition.account) > -1);
    }
    //電子郵件
    if (this.searchCondition.email && this.searchCondition.email.length > 0) {
      data = data.filter(a => a.email.indexOf(this.searchCondition.email) > -1);
    }

    //狀態
    if (this.searchCondition && (this.searchCondition.isEnable != null)) {
      data = data.filter(a => (a.isEnable == this.searchCondition.isEnable));
    }

    //權限
    if (this.groupData) {

      //暫存用清單
      var tempList: AdminData[] = [];

      //檢查每個勾選條件
      this.groupData.forEach(group => {
        //如果有打勾
        if (group.isChecked) {

          //找出含有這個權限的資料
          var _res = data.filter(a => a.groups.filter(b => (b.isChecked === group.isChecked) && (b.id === group.id)).length > 0);

          //如果結果清單沒有就加入 (不重複)
          _res.forEach(item => {
            if (tempList.filter(a => a.id == item.id).length <= 0) {
              tempList.push(item);
            }
          });
        }
      });

      //把結果設定回 Data
      data = tempList;

    }

    this.dataSource.data = data;
  }

  //勾選全選時，自動勾選所有權限
  SelectAllGroups() {
    this.groupData.forEach(group => {
      group.isChecked = this.isSelectAllGroup;
    });
  }

  //任一個權限取消勾選時，取消全選勾勾
  DisSelectAllGroups() {
    this.isSelectAllGroup = this.groupData.every(group => group.isChecked);
  }


  // 編輯會員資料
  EditAdmin(item) {
    // 導向admindata並帶參數
    this.router.navigate(['/admin/adminDetail', { state: 'edit', id: item.id }]);
  }

  // 檢視會員資料
  DetailAdmin(item) {
    // 導向Admindata並帶參數
    this.router.navigate(['/admin/adminDetail', { state: 'detail', id: item.id }]);
  }

  // 新增會員資料
  CreateAdmin() {
    // 導向admindata並帶參數
    this.router.navigate(['/admin/adminDetail', { state: 'create', id: '0' }]);
  }
  // 刪除會員資料
  DeleteAdmin(admin: AdminData) {
    const dialogRef = this.dialog.open(AddPostDialogComponent, {
      data: {
        title: '是否刪除',
        messages:
          [
            '刪除帳號:' + admin.account,
            '此動作無法復原，是否要刪除？',
            '若要停用此帳號可於編輯中停用此帳號'
          ],
        bt_confirm: '確認',
        bt_cancel: '取消'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // 刪除會員資料
        this.httpService.post<any>(this.postUrl + 'DeleteAdmin', admin.id).subscribe(
          (apiReturn: APIReturn) => {

            if (apiReturn.code == 0) {
              //alert(apiReturn.message);
              this.dialog.open(AddPostDialogComponent, {
                data: {
                  title: '執行成功',
                  messages: [apiReturn.message],
                  bt_cancel: '確定',

                }
              });

              //重load清單
              this.httpService.post<any>(this.postUrl + 'GetAdmins', null).subscribe(
                (admins: any) => {
                  this.adminData.adminList = admins;
                  this.dataSource.data = admins;
                });
            }
            else {
              //Log
              this.logService.LogInfo(apiReturn.message);
            }
          },
          (err: any) => {
            //Log
            this.logService.LogError(err);
          });
      }
    });
    // 跳出提示訊息詢問是否要刪除
  }
}
//====================以上自訂函數=====================//


//====================以下自訂型別=====================//
class FormModel {
  adminList: AdminData[];
}
//====================以上自訂型別=====================//