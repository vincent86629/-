import { Component, ViewChild } from '@angular/core';
import { AppService } from './service/app-service/app.service';
import { VerifycodeComponent } from './views/verifycode/verifycode.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  forgetPassword: boolean = false; //點選忘記密碼、重置密碼
  @ViewChild('verifyCode') verifyCode: VerifycodeComponent; //获取页面中的验证码组件
  verifycode: string;

  constructor(private appService: AppService) { }
  ngOnInit() {
    //檢查是否登入逾時，未逾時就自動登入
    this.appService.CheckLoginIsExpire();
  }
  Login() {
    if (!this.verifyCode.validate(this.verifycode)) { //this.code.value为用户输入的验证码
      alert('驗證碼不正確');
      return;

    }
    this.appService.Login();
  }
  //點選忘記密碼、重置密碼，畫面呈現重置密碼表單
  ForgetPassword(checked: boolean) {
    this.forgetPassword = checked;
    this.appService.loginRequest.email = "";
    this.appService.loginRequest.password = "";
    this.appService.loginResponse.messages = [];
  }
}


