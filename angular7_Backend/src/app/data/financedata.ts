/*-----管理者左邊menu-----*/
export class FinanceReport {
  id: number;
  yearMonth: string;
  community: string;
  communityId: number;
  createBy: string;
  updateTime: Date;
  status: string;
  statusId: number
  constructor() { }
}
export class FinanceEditData {
  id: number;
  lastMonthBalance: number;
  thisMonthBalance: number;
  statusId: number
  bankSaving: any[] = [];
  blocks: Block[] = [];
  permission: string;
  yearMonth: string;
  createBy: number;
  constructor() { }
}
export class Block {
  blockName: string;
  totalName: string;
  total: number;
  rows: Row[] = []
  constructor() {
    for (let i = 0; i < 15; i++) {
      this.rows.push(new Row());
    }
  }
}

export class Row {
  date: string;
  name: string;
  value: number;
}
export class SearchQuery {
  createBy: string;
  statusId: string;
  communityId: string;
  yearMonth: string;
}