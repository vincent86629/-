import { Component, OnInit } from '@angular/core';
import { AdminData } from '../../../../data/admindata';
import { APIReturn } from '../../../../data/apidata';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpService } from '../../../../service/http-service/http.service';
import { LogService } from '../../../../service/log-service/log.service';
import { MatDialog } from '@angular/material';
import { AddPostDialogComponent } from '../../../../shared/add-post-dialog/add-post-dialog.component';

@Component({
  selector: 'app-adminDetail',
  templateUrl: './adminDetail.component.html',
  styleUrls: ['./adminDetail.component.css']
})
export class AdminDetailComponent implements OnInit {

  postUrl: string = "api/Admin/Admin/";
  state: string;
  title: string;
  id: number;
  formData: FormModel;
  constructor(private router: Router, private route: ActivatedRoute, private httpService: HttpService, private logService: LogService, public dialog: MatDialog) { }

  ngOnInit() {
    // 取得router參數
    this.state = this.route.snapshot.params['state'];
    this.id = parseInt(this.route.snapshot.params['id']);

    // 此畫面使用的formData
    this.formData = new FormModel();
    this.formData.memberData = new AdminData();

    this.httpService.post<any>(this.postUrl + 'GetAdminByID', this.id).subscribe(
      (x: any) => {
        this.formData.memberData = x;

        // 由傳入的狀態來判斷執行狀態
        if (this.state === 'create') {
          this.title = '新增使用者';
        } else if (this.state === 'edit') {
          this.title = '編輯使用者資料';
        } else if (this.state === 'detail') {
          this.title = '查看使用者資料';
        }
      });
  }

  // 回會員清單頁面
  backPreviousPage() {
    this.router.navigateByUrl('/admin/adminList');
  }

  // 送出會員資料
  submitMemberData() {

    var action = '更新';

    if (this.state == 'create') {
      action = '新增';
    }
    else if (this.state == 'edit') {
      action = '更新';
    }

    var reg = new RegExp("^[0-9a-zA-Z]+$");
    if (!reg.test(this.formData.memberData.account)) {
      this.dialog.open(AddPostDialogComponent, {
        data: {
          title: '帳號錯誤',
          messages: ["帳號必須為英數"],
          bt_cancel: '確定',

        }
      });
    }else{
      const dialogRef = this.dialog.open(AddPostDialogComponent, {
        data: {
          title: '是否' + action,
          messages:
            ['是否要' + action + '使用者'
  
            ],
          bt_confirm: '確認',
          bt_cancel: '取消'
        }
      });
  
      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.httpService.post<any>(this.postUrl + 'UpdateAdmin', this.formData.memberData).subscribe(
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
                // 回清單頁
                this.router.navigateByUrl('/admin/adminList');
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
              this.dialog.open(AddPostDialogComponent, {
                data: {
                  title: '錯誤',
                  messages: [err],
                  bt_cancel: '確定',
  
                }
              });
            }
          );
        }
      });
      // 跳出提示訊息詢問是否要送出
      // if (confirm('是否要' + action + '使用者')) {
  
  
      // }
    }
    
  }
}

class FormModel {
  memberList: AdminData[];
  memberData: AdminData;
}
