/*-----管理者左邊menu-----*/
export class AdminMenuData {
  name: string;
  url: string;
  childs: AdminMenuData[] = [];
  constructor() {}
}

/*-----管理者帳號管理-----*/
export class AdminData {
  id: number = 0;
  name: string = "";
  account: string = "";
  password: string = "";
  email: string = "";
  phone: string = "";
  isEnable: boolean = null;
  groups: GroupCheckData[] = [];
  permissions: SelectListItem[] = [];
  groupsName: string = "";
  constructor() {}
}

export class GroupCheckData {
  id: number;
  codeName: string;
  isChecked: boolean;
  constructor() { }
}
export class SelectListItem {
  value: string;
  text: string;
  isSelected: boolean;
  constructor() { }
}
/*-----管理者帳號管理 end-----*/

/*-----權限管理-----*/
export class PermissionData {
  id: number;
  codeName: string;
  isEnable: boolean;
  permissionMenus: Permission[] = [];  //DB撈出來的tree
  checkedMenus: number[] = []; //要存進DB的ID
  constructor() { }
}

export class Permission {
  id: number;
  parentId: number;
  name: string;
  isChecked: boolean;
  child: Permission[] = [];
  constructor() { }
}
/*-----權限管理 end-----*/

/*-----Log紀錄-----*/

export class AdminLog {
  id: number;
  type: string;
  code: string;
  name: string;
  operator: number;
  requestTime: Date;
  parameter: string;
  ip: string;
  remark: string;

  constructor() { }
}


export class LogRecordData {
  id: number;
  event: string;
  operator: string;
  requestTime: string;
  ip: string;
  remark: string;
  constructor() { }
}

export class SearchCondition {
  startDate: Date;
  endDate: Date;
  operator: string;
  ip: string;
  remark: string;
  SelectLogin: boolean = true;
  SelectGiftLog: boolean = true;
  SelectExpertApprove: boolean = true;
  SelectLogout: boolean = true;
  SelectExpertManage: boolean = true;
  SelectQAlog: boolean = true;
  constructor() { }
}
/*-----Log紀錄 end-----*/

