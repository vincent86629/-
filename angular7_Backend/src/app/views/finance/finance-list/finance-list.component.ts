import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpService } from '../../../service/http-service/http.service';
import { MatPaginator, MatSort, MatTableDataSource, MatDialog } from '@angular/material';
import { FinanceReport } from '../../../data/financedata';
import { AppService } from '../../../service/app-service/app.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-finance-list',
  templateUrl: './finance-list.component.html',
  styleUrls: ['./finance-list.component.css']
})
export class FinanceListComponent implements OnInit {
  search = new SearchQuery();
  adminId: number;
  //Angular Material Table
  dataSource = new MatTableDataSource<FinanceReport>(); //資料來源
  @ViewChild(MatSort) sort: MatSort; //排序
  @ViewChild(MatPaginator) paginator: MatPaginator; //分頁
  statusOptions: any[] = //狀態下拉選單
    [
      { 'value': null, 'text': '請選擇' },
      { 'value': 0, 'text': '草稿' },
      { 'value': 1, 'text': '已完成' },
      { 'value': 2, 'text': '作廢' }
    ];
  permissionOptions: any[] = [];
  yearMonthOptions: any[] = [{ 'value': null, 'text': '請選擇' },];

  constructor(
    private httpService: HttpService,
    private appService: AppService,
    private router: Router,
  ) { }

  ngOnInit() {
    this.adminId = this.appService.loginResponse.adminInfo.id;
    this.getPermissionOptions();
    this.getYearMonthOptions();
  }
  getPermissionOptions() {
    this.appService.loginResponse.adminInfo.permissions.forEach(a => {
      if (a.value == '1') {
        this.permissionOptions.push({ 'value': '0', 'text': '全部' });
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
  getData() {

  }
  createReport() {
    this.router.navigate(['finance-edit/0']);
  }
}
class SearchQuery {
  createBy: string;
  status: string;
  permission: string;
  yearMonth: string;
}