import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Block, FinanceEditData } from '../../../data/financedata';

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

  constructor(private route: ActivatedRoute) { }
  ngOnInit() {
    this.data = new FinanceEditData();
    this.id = +this.route.snapshot.paramMap.get('id');
    this.getData();
  }
  getData() {
    var test1 = new Block();
    test1.blockName = 'Block1';
    test1.totalName = 'Total1';
    test1.total = 1111;
    var test2 = new Block();
    test2.blockName = 'Block2';
    test2.totalName = 'Total2';
    test2.total = 2222;
    test1.rows = [
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
    ];
    test2.rows = [
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
      { date: new Date('2020-11-16'), name: '住戶管理費收入', value: 100 },
    ];

    this.data.blocks.push(test1);
    this.data.blocks.push(test2);

  }
}
