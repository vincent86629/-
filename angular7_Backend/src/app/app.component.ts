import { Component } from '@angular/core';
import { AppService } from './service/app-service/app.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  forgetPassword: boolean = false; //點選忘記密碼、重置密碼

  constructor(private appService: AppService) { }
  ngOnInit() {

    //檢查是否登入逾時，未逾時就自動登入
    this.appService.CheckLoginIsExpire();

    //先更新一次待審核專家數量
    this.appService.GetWaitApprovalExpertsCount();

    //之後每 3 分鐘，更新待審核專家數量
    setInterval(() => {

      this.appService.GetWaitApprovalExpertsCount();
    }, 3 * 60 * 1000);

  }

  //點選忘記密碼、重置密碼，畫面呈現重置密碼表單
  ForgetPassword(checked: boolean) {
    this.forgetPassword = checked;
    this.appService.loginRequest.email = "";
    this.appService.loginRequest.password = "";
    this.appService.loginResponse.messages = [];
  }
}


