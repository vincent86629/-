import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Block, FinanceEditData, FinanceFile, SearchQuery } from '../../../data/financedata';
import { HttpService } from '../../../service/http-service/http.service';
import { AppService } from '../../../service/app-service/app.service';
import { MatDialog } from '@angular/material';
import { AddPostDialogComponent } from '../../../shared/add-post-dialog/add-post-dialog.component';
import { APIReturn } from '../../../data/apidata';
import { Location } from '@angular/common';
import * as XLSX from 'xlsx';
import { Router } from '@angular/router';

@Component({
  selector: 'app-finance-edit',
  templateUrl: './finance-edit.component.html',
  styleUrls: ['./finance-edit.component.css']
})
export class FinanceEditComponent implements OnInit {
  editField: string;
  indexList: number[] = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14];
  data: FinanceEditData;
  id: number;
  tab = 0;
  income = 0;
  expenses = 0;
  total = 0;
  permissionOptions: any[] = [];
  yearMonthOptions: any[] = [];
  permissionName: string;
  isCreator = false;
  fileData: File = null;
  constructor(
    private route: ActivatedRoute,
    private httpService: HttpService,
    private appService: AppService,
    public dialog: MatDialog,
    private location: Location,
    private router: Router,

  ) { }
  ngOnInit() {
    this.data = new FinanceEditData();
    this.id = +this.route.snapshot.paramMap.get('id');
    this.getPermissionOptions();
    this.getYearMonthOptions();
    this.getData();
  }
  getData() {
    this.httpService.post<any>('api/Finance/GetFinanceEditData', this.id).subscribe(
      (data: FinanceEditData) => {
        this.data = data;
        this.data.files = [];

        console.log(this.data.files);
        if (this.data.createBy != 0) {
          this.isCreator = this.data.createBy == this.appService.loginResponse.adminInfo.id;
        } else {
          this.data.createBy = this.appService.loginResponse.adminInfo.id;
          this.isCreator = true;
        }
        if (this.data.yearMonth == null) {
          this.data.yearMonth = this.yearMonthOptions[this.yearMonthOptions.length - 1].value;
        }
        if (this.data.permission == null) {
          this.data.permission = this.permissionOptions[0].value;
        }
        this.permissionChange();
        this.calculate();
      }
      , (err: any) => {
        console.log(err);
      });

  }
  calculate() {
    this.income = 0;
    this.expenses = 0;
    this.data.blocks.forEach((block, index) => {
      block.total = block.rows.reduce(function (a, b) { return +a + +b.value }, 0)
    });
    this.income = this.data.blocks[0].total + this.data.blocks[2].total;
    this.expenses = this.data.blocks[3].total;
    this.data.thisMonthBalance = this.data.lastMonthBalance + this.income - this.expenses + this.data.blocks[4].total - this.data.blocks[5].total;
    this.total = this.data.thisMonthBalance + this.data.bankSaving.reduce(function (a, b) { return +a + +b.value }, 0);
    console.log(this.total);
  }
  permissionChange() {
    this.permissionName = this.permissionOptions.find(a => a.value == this.data.permission).text;
  }
  getPermissionOptions() {
    this.appService.loginResponse.adminInfo.permissions.forEach(a => {
      if (a.value == '1') {
      } else {
        this.permissionOptions.push(a);
      }
    })
  }
  getYearMonthOptions() {
    var start_year = 2020;
    var start_month = 10;
    var dateObj = new Date();
    var month = dateObj.getUTCMonth() + 1; //months from 1-12
    var year = dateObj.getUTCFullYear();
    for (let i = start_year; i <= year; i++) {

      if (year == start_year) {
        for (let m = start_month; m <= month; m++) {
          let strYear = i.toString();
          let strMonth = String("0" + m).slice(-2);
          this.yearMonthOptions.push({ 'value': strYear + '/' + strMonth, 'text': strYear + '年' + strMonth + '月' })
        }
      }
      else if (i == year) {
        for (let m = 1; m <= month; m++) {
          let strYear = i.toString();
          let strMonth = String("0" + m).slice(-2);
          this.yearMonthOptions.push({ 'value': strYear + '/' + strMonth, 'text': strYear + '年' + strMonth + '月' })
        }
      }
      else if (i == start_year) {
        for (let m = start_month; m <= 12; m++) {
          let strYear = i.toString();
          let strMonth = String("0" + m).slice(-2);
          this.yearMonthOptions.push({ 'value': strYear + '/' + strMonth, 'text': strYear + '年' + strMonth + '月' })
        }
      }

      if (year > i && i > start_year) {
        for (let m = 1; m <= 12; m++) {
          let strYear = i.toString();
          let strMonth = String("0" + m).slice(-2);
          this.yearMonthOptions.push({ 'value': strYear + '/' + strMonth, 'text': strYear + '年' + strMonth + '月' })
        }
      }

    }
  }
  save(statusId: number) {
    let title = '暫存報表';
    let message = '是否要暫時儲存當前報表?';
    if (statusId === 1) {
      title = '完成報表';
      message = '完成後將不能修改,只能作廢重填,確認完成嗎?';
    }
    if (statusId === 2) {
      title = '作廢報表';
      message = '作廢後需重新填寫,確認作廢嗎?';
    }

    const dialogRef = this.dialog.open(AddPostDialogComponent, {
      data: {
        title: title,
        messages:
          [message
          ],
        bt_confirm: '確認',
        bt_cancel: '取消'
      }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.data.statusId = statusId;
        this.httpService.post<any>('api/Finance/SaveFinanceEditData', this.data).subscribe(
          (data: APIReturn) => {
            if (data.code == 0) {
              alert('儲存成功!');
              this.router.navigate(['finance-edit/' + data.message]);

            } else {
              alert('儲存失敗!錯誤訊息:' + data.message);
            }
          }
          , (err: any) => {
            console.log(err);
          });
      }
    });
  }
  goBack() {
    this.location.back();
  }
  exportexcel(): void {
    /* table id is passed over here */
    let element = document.getElementById('excel-table');
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element);

    /* generate workbook and add the worksheet */
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');

    /* save to file */
    XLSX.writeFile(wb, this.permissionName + this.data.yearMonth + '收支表.xlsx');

  }
  upLoadPicture(event) {
    const selectedFile: any = event.target.files[0];

    if (selectedFile != null) {
      const type = selectedFile.type.split('/')[1];
      const typeList = ['jpg', 'jpeg', 'png'];

      if (typeList.indexOf(type) === -1) {
        alert('檔案格式只接受 : jpg, jpeg, png');
        return;
      }
      const uploadData = new FormData();
      uploadData.append('uploads', selectedFile, selectedFile.name);
      console.log(uploadData);
      this.httpService.upload<any>('api/Finance/PictureUpload', uploadData).subscribe(
        (apiReturn: APIReturn) => {
          this.data.files.push({ path: apiReturn.message, id: this.data.files.length + 1 })
        },
        (error: any) => {
          console.log(error);
        });
    }
  }
  removeFile(id: number) {
    this.data.files = this.data.files.filter(a => a.id != id);
  }
}
