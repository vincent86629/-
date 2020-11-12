import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material';
@Component({
  selector: 'app-add-post-dialog',
  templateUrl: './add-post-dialog.component.html',
  styleUrls: ['./add-post-dialog.component.css']
})
export class AddPostDialogComponent implements OnInit {

  result: boolean = false;
  visiable: string ;
  constructor(@Inject(MAT_DIALOG_DATA) private data: any) {
    this.visiable=data.bt_confirm;
  }

  dialog_confirm(){
    this.result= true;   
  }
  ngOnInit() {
    if(this.visiable==undefined){
     document.getElementById('bt_confirm').style.visibility='hidden';
   }
  }



}
