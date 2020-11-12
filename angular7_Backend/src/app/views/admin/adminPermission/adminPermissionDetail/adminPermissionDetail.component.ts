import { Component, OnInit, Injectable } from '@angular/core';
import { PermissionData } from '../../../../data/admindata';
import { APIReturn } from '../../../../data/apidata';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpService } from '../../../../service/http-service/http.service';
import { LogService } from '../../../../service/log-service/log.service';

import { SelectionModel } from '@angular/cdk/collections';
import { FlatTreeControl } from '@angular/cdk/tree';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
// tslint:disable-next-line:import-blacklist
import { BehaviorSubject } from 'rxjs';
import { AddPostDialogComponent } from '../../../../shared/add-post-dialog/add-post-dialog.component';
import { MatDialog } from '@angular/material';

// 樹的結構
export class TodoItemNode {
  children: TodoItemNode[];
  id: number;
  parentId: number;
  item: string;
  isChecked: boolean;
}

// 樹的單一節點
export class TodoItemFlatNode {
  id: number;
  parentId: number;
  item: string;
  level: number;
  expandable: boolean;
  isChecked: boolean;
}

// 整棵樹在這邊產生，包含新增跟更新項目的方法
@Injectable()
export class ChecklistDatabase {
  dataChange = new BehaviorSubject<TodoItemNode[]>([]);

  get data(): TodoItemNode[] { return this.dataChange.value; }

  constructor() {
    //this.initialize();
  }

  initialize() {


    // 樹的假資料
    const TREE_DATA =
      [
        {
          id: 91,
          name: "會員管理",
          isChecked: true,
          child: [
            {
              id: 911,
              name: "使用者管理",
              isChecked: true,
              child: null
            },
            {
              id: 912,
              name: "群組管理",
              isChecked: true,
              child: null
            }
          ]

        },
        {
          id: 92,
          name: "專家管理",
          isChecked: true,
          child: [
            {
              id: 921,
              name: "專家申請",
              isChecked: true,
              child: null
            },
            {
              id: 922,
              name: "科系管理",
              isChecked: true,
              child: null
            },
            {
              id: 923,
              name: "test",
              isChecked: true,
              child: null
            }
          ]

        }
      ]


    // 從資料建立樹
    const data = this.buildFileTree(TREE_DATA, 0);

    // 監視資料變更
    this.dataChange.next(data);
  }

  // 從 JSON 資料建立樹狀結構資料
  buildFileTree(obj: any, level: number): TodoItemNode[] {
    return Object.keys(obj).reduce<TodoItemNode[]>((accumulator, key) => {
      const nowobj = obj[key];
      const _id = parseInt(nowobj["id"]);
      const _parentId = parseInt(nowobj["parentId"]);
      const _name = nowobj["name"];
      const _child = nowobj["child"];
      const _isChecked = nowobj["isChecked"];

      const _node = new TodoItemNode();
      _node.id = _id;
      _node.item = _name;
      _node.isChecked = _isChecked;
      _node.parentId = _parentId;
      if (_child != null) {
        if (typeof _child === 'object' && _child.length > 0) {
          _node.children = this.buildFileTree(_child, level + 1);
        }
      }
      return accumulator.concat(_node);

    }, []);
  }

  // [樹] 新增樹節點
  insertItem(parent: TodoItemNode, name: string) {
    if (parent.children) {
      parent.children.push({ item: name } as TodoItemNode);
      this.dataChange.next(this.data);
    }
  }

  // 儲存節點項目
  updateItem(node: TodoItemNode, name: string) {
    node.item = name;
    this.dataChange.next(this.data);
  }
}


@Component({
  selector: 'app-admin-permissionDetail',
  templateUrl: './adminPermissionDetail.component.html',
  styleUrls: ['./adminPermissionDetail.component.css'],
  providers: [ChecklistDatabase],
})
export class AdminPermissionDetailComponent implements OnInit {

  permissionListURL = '/admin/adminPermissionList';
  postUrl: string = "api/Admin/AdminPermission/";
  title: string;
  state: string;
  submitButtonText: string = '';
  id: number;
  formData: FormModel;
  /** Map from flat node to nested node. This helps us finding the nested node to be modified */
  flatNodeMap = new Map<TodoItemFlatNode, TodoItemNode>();

  /** Map from nested node to flattened node. This helps us to keep the same object for selection */
  nestedNodeMap = new Map<TodoItemNode, TodoItemFlatNode>();

  /** A selected parent node to be inserted */
  selectedParent: TodoItemFlatNode | null = null;

  /** The new item's name */
  newItemName = '';

  treeControl: FlatTreeControl<TodoItemFlatNode>;

  treeFlattener: MatTreeFlattener<TodoItemNode, TodoItemFlatNode>;

  dataSource: MatTreeFlatDataSource<TodoItemNode, TodoItemFlatNode>;

  /** The selection for checklist */
  checklistSelection = new SelectionModel<TodoItemFlatNode>(true /* multiple */);

  constructor(private database: ChecklistDatabase, private router: Router, private route: ActivatedRoute,
    private logService: LogService, private httpService: HttpService, public dialog: MatDialog) {
    this.treeFlattener = new MatTreeFlattener(this.transformer, this.getLevel,
      this.isExpandable, this.getChildren);
    this.treeControl = new FlatTreeControl<TodoItemFlatNode>(this.getLevel, this.isExpandable);
    this.dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

    database.dataChange.subscribe(data => {
      this.dataSource.data = data;
    });
  }

  ngOnInit() {
    // 取得router參數
    this.state = this.route.snapshot.params['state'];
    this.id = parseInt(this.route.snapshot.params['id']);

    // 此畫面使用的formData
    this.formData = new FormModel();
    this.formData.permissionData = new PermissionData();

    // 由傳入的狀態來判斷執行狀態
    if (this.state === 'create') {

      this.title = '新增權限';
      this.submitButtonText = '新增';

    } else if (this.state === 'edit') {

      this.title = '編輯權限';
      this.submitButtonText = '編輯';

    } else if (this.state === 'detail') {

      this.title = '檢視權限';
      this.submitButtonText = '';

    }

    // 會員清單從共用service取得
    this.httpService.post<any>(this.postUrl + 'GetAdminPermissionByID', this.id).subscribe(
      (permissionData: PermissionData) => {
        this.formData.permissionData = permissionData;

        if (this.formData.permissionData.permissionMenus != null) {
          // 從資料建立樹
          const data = this.database.buildFileTree(this.formData.permissionData.permissionMenus, 0);
          // 監視資料變更
          this.database.dataChange.next(data);

          //勾選該勾選的資料
          this.initSelected();

          //預設展開所有節點
          this.treeControl.expandAll();
        }

      });

  }

  getLevel = (node: TodoItemFlatNode) => node.level;

  isExpandable = (node: TodoItemFlatNode) => node.expandable;

  getChildren = (node: TodoItemNode): TodoItemNode[] => node.children;

  hasChild = (_: number, _nodeData: TodoItemFlatNode) => _nodeData.expandable;

  hasNoContent = (_: number, _nodeData: TodoItemFlatNode) => _nodeData.item === '';

  // [樹] 把樹的結構，轉成單一的節點
  transformer = (node: TodoItemNode, level: number) => {
    const existingNode = this.nestedNodeMap.get(node);
    const flatNode = (existingNode && existingNode.item === node.item)
      ? existingNode : new TodoItemFlatNode();

    flatNode.id = node.id; //設定節點 id
    flatNode.item = node.item; //設定節點 項目
    flatNode.level = level; //設定節點 層級數
    flatNode.expandable = !!node.children; //設定節點可展開下層 (有子節點)
    flatNode.isChecked = !!node.isChecked; //設定節點 是否選取
    flatNode.parentId = node.parentId; //設定節點 是否選取

    this.flatNodeMap.set(flatNode, node);
    this.nestedNodeMap.set(node, flatNode);
    return flatNode;
  }

  // [樹] DB撈出來時的打勾狀態DB撈出來時的打勾狀態
  initSelected(): void {
    this.dataSource._flattenedData.forEach(nodes => {
      nodes.forEach(node => {
        node.isChecked
          ? this.checklistSelection.select(node)
          : this.checklistSelection.deselect(node);
      });
    });
  }

  // [樹] 表示子節點被全部選取
  descendantsAllSelected(node: TodoItemFlatNode): boolean {
    const descendants = this.treeControl.getDescendants(node);
    const all = descendants.every(child => this.checklistSelection.isSelected(child));
    all
      ? this.checklistSelection.select(node)
      : this.checklistSelection.deselect(node);

    return all
  }

  // [樹] 表示子節點被部分選取
  descendantsPartiallySelected(node: TodoItemFlatNode): boolean {
    const descendants = this.treeControl.getDescendants(node);
    const some = descendants.some(child => this.checklistSelection.isSelected(child));
    const all = this.descendantsAllSelected(node);
    const partial = some && !all;

    return partial;
  }

  // [樹] 點選父節點，選取所有子節點
  todoItemSelectionToggle(node: TodoItemFlatNode): void {
    this.checklistSelection.toggle(node);
    const descendants = this.treeControl.getDescendants(node);

    this.checklistSelection.isSelected(node)
      ? this.checklistSelection.select(...descendants)
      : this.checklistSelection.deselect(...descendants);

  }

  // [樹] 新增樹節點
  addNewItem(node: TodoItemFlatNode) {
    const parentNode = this.flatNodeMap.get(node);
    this.database.insertItem(parentNode!, '');
    this.treeControl.expand(node);
  }

  // [樹] 儲存節點項目
  saveNode(node: TodoItemFlatNode, itemValue: string) {
    const nestedNode = this.flatNodeMap.get(node);
    this.database.updateItem(nestedNode!, itemValue);
  }

  submitGroupData() {
    // 跳出提示訊息詢問是否要送出
    const dialogRef = this.dialog.open(AddPostDialogComponent, {
      data: {
        title: '是否送出',
        messages:
          ['是否要'+ this.submitButtonText + '群組'],
        bt_confirm: '確認',
        bt_cancel: '取消'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        //取得每個節點是否打勾
        const checkedMenus: number[] = [];
        this.dataSource._flattenedData.forEach(nodes => {
          nodes.forEach(node => {
            if (this.checklistSelection.isSelected(node) && node.parentId !== 0) {
              checkedMenus.push(node.id);
            }
          })
        })
        this.formData.permissionData.checkedMenus = checkedMenus;

        this.httpService.post<any>(this.postUrl + 'UpdateOrCreateAdminPermission', this.formData.permissionData).subscribe(
          (apiReturn: APIReturn) => {
            if (apiReturn.code == 0) {

              this.dialog.open(AddPostDialogComponent, {
                data: {
                  title: '執行成功',
                  messages: [apiReturn.message],
                  bt_cancel: '確定',

                }
              });

              // 回清單頁
              this.router.navigateByUrl(this.permissionListURL);
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
            this.logService.LogError(err);

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
    // if (confirm('是否要更新群組')) {
    // }
  }

  // 回群組清單頁面
  backPreviousPage() {
    this.router.navigateByUrl(this.permissionListURL);
  }

}

class FormModel {
  permissionList: PermissionData[];
  permissionData: PermissionData;
}



