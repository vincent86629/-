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
  title: string;
  lastMonthBalance: number;
  thisMonthBalance: number;
  statusId: number
  bankSaving: any[] = [];
  blocks: Block[] = [];
  constructor() { }
}
export class Block {
  blockName: string;
  totalName: string;
  total: number;
  rows: any[] = []
  constructor() {
    for (let i = 0; i < 15; i++) {
      this.rows.push({});
    }
  }
}

