import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpService } from '../../../service/http-service/http.service';
import { MatPaginator, MatSort, MatTableDataSource, MatDialog } from '@angular/material';
import { FinanceReport } from '../../../data/financedata';

@Component({
  selector: 'app-finance-list',
  templateUrl: './finance-list.component.html',
  styleUrls: ['./finance-list.component.css']
})
export class FinanceListComponent implements OnInit {
  search = new SearchQuery();
  //Angular Material Table
  dataSource = new MatTableDataSource<FinanceReport>(); //資料來源
  @ViewChild(MatSort) sort: MatSort; //排序
  @ViewChild(MatPaginator) paginator: MatPaginator; //分頁
  statusOptions: any[] = //狀態下拉選單
    [
      { 'value': null, 'optionName': '請選擇' },
      { 'value': 0, 'optionName': '草稿' },
      { 'value': 1, 'optionName': '已完成' },
      { 'value': 2, 'optionName': '作廢' }
    ];

  constructor(private httpService: HttpService) { }

  ngOnInit() {

  }
  getData() {

  }
}
class SearchQuery {
  name: string;
  account: string;
  status: number;
}