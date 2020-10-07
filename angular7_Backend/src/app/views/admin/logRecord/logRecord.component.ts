import { Component, OnInit, ViewChild } from '@angular/core';
import {FormControl} from '@angular/forms';
import {Observable} from 'rxjs/Observable';
import { LogRecordData, SearchCondition } from '../../../data/admindata';
import { HttpService } from '../../../service/http-service/http.service';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';

@Component({
  selector: 'app-logRecord',
  templateUrl: './logRecord.component.html',
  styleUrls: ['./logRecord.component.css']
})
export class LogRecordComponent implements OnInit {

  postUrl:string = "api/Admin/AdminLog/";
  SelectAll = true;
  sortkey: string;

  formData: FormModel = new FormModel();

  myControl = new FormControl();
  options: string[] = [];
  filteredOptions: Observable<string[]>;
  maxDate: Date = new Date();

  dataSource = new MatTableDataSource<LogRecordData>();
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private httpService: HttpService) { }


  ngOnInit() {

    //初始化
    this.formData.searchCondition = new SearchCondition();     //搜尋條件

    this.httpService.post<any>( this.postUrl + 'GetAdminLogs', this.formData.searchCondition).subscribe(
      (adminLogs: LogRecordData[]) => {
        this.formData.logRecordList = adminLogs;

        this.dataSource.data = this.formData.logRecordList;
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
      });
  }

  SelectAllChange() {
    this.formData.searchCondition.SelectLogin = this.SelectAll;
    this.formData.searchCondition.SelectGiftLog = this.SelectAll;
    this.formData.searchCondition.SelectExpertApprove = this.SelectAll;
    this.formData.searchCondition.SelectLogout = this.SelectAll;
    this.formData.searchCondition.SelectExpertManage = this.SelectAll;
    this.formData.searchCondition.SelectQAlog = this.SelectAll;
  }

  //搜尋Log紀錄
  LogRecordSearch()
  {
    this.httpService.post<any>( this.postUrl + 'GetAdminLogs', this.formData.searchCondition).subscribe(
      (adminLogs: LogRecordData[]) => {

        this.formData.logRecordList = adminLogs;

        this.dataSource.data = this.formData.logRecordList;
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
      });
    
  }

  //清除搜尋選項
  ClearCondition() {
    this.formData.searchCondition = new SearchCondition();
  }
  
}


class FormModel {
  logRecordList: LogRecordData[];
  searchCondition: SearchCondition;
}