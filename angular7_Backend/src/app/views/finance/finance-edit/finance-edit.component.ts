import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Block, FinanceEditData, SearchQuery } from '../../../data/financedata';
import { HttpService } from '../../../service/http-service/http.service';
import { AppService } from '../../../service/app-service/app.service';
import { MatDialog } from '@angular/material';
import { AddPostDialogComponent } from '../../../shared/add-post-dialog/add-post-dialog.component';
import { APIReturn } from '../../../data/apidata';
import { Location } from '@angular/common';

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
  constructor(
    private route: ActivatedRoute,
    private httpService: HttpService,
    private appService: AppService,
    public dialog: MatDialog,
    private location: Location
  ) { }
  ngOnInit() {
    this.data = new FinanceEditData();
    this.id = +this.route.snapshot.paramMap.get('id');
    this.getData();
    this.getPermissionOptions();
    this.getYearMonthOptions();
  }
  getData() {
    this.httpService.post<any>('api/Finance/GetFinanceEditData', this.id).subscribe(
      (data: FinanceEditData) => {
        this.data = data;
        this.data.createBy = this.appService.loginResponse.adminInfo.id;
        this.data.permission = this.permissionOptions[0].value;
        this.data.yearMonth = this.yearMonthOptions[this.yearMonthOptions.length - 1].value;
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
      if (index % 2 == 0) {
        this.income += block.total;
      } else {
        this.expenses += block.total;
      }
    });
    this.data.thisMonthBalance = this.data.lastMonthBalance + this.income - this.expenses;
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
    // this.search.yearMonth = year.toString() + '/' + String("0" + month).slice(-2);
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
    this.data.statusId = statusId;
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
    console.log(this.data.blocks)
    dialogRef.afterClosed().subscribe(result => {
      this.httpService.post<any>('api/Finance/SaveFinanceEditData', this.data).subscribe(
        (data: APIReturn) => {
          if (data.code == 0) {
            alert('儲存成功!');
          } else {
            alert('儲存失敗!錯誤訊息:' + data.message);
          }
        }
        , (err: any) => {
          console.log(err);
        });
    });
  }
  goBack() {
    this.location.back();
  }
}
