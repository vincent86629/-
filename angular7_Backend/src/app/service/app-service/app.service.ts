import { Injectable } from '@angular/core';
import { AdminData, AdminLog } from '../../data/admindata';
import { HttpService } from '../http-service/http.service';

@Injectable({
  providedIn: 'root'
})
export class AppService {


  adminLoginPostUrl: string = "api/Admin/Login/";
  adminLogPostUrl: string = "api/Admin/AdminLog/";
  private adminAPIPostURL = 'api/Admin/Expert/';

  loginRequest: LoginRequest = new LoginRequest(); //頁面資訊
  loginResponse: LoginResponse = new LoginResponse(); //登入資訊

  waitApprovalExpertsCount: number = 999; //待審核專家數量

  constructor(private httpService: HttpService) {
  }

  //檢查是否登入逾時
  CheckLoginIsExpire(): boolean {

    //取得現在時間
    var now = new Date();

    //取得上次登入後寫入的逾期時間
    var Login_ExpireDate = JSON.parse(localStorage.getItem("Login_ExpireDate"),
      (key, value: Date) => {
        return new Date(value); //一定要在包一層 new Date
      });

    //如果之前曾經登入過，而且還沒逾期
    if (Login_ExpireDate > now) {

      //取出上次的登入資訊
      var Login_Info = JSON.parse(localStorage.getItem("Login_Info"),
        (key, value: AdminData) => {

          //設定登入資訊
          this.loginResponse.isLogin = true;
          this.loginResponse.messages = [];
          this.loginResponse.adminInfo = value;

          return value;
        });

      //登入未逾期
      return false;
    } else {

      //登入逾期
      return true;
    }
  }


  //登入
  Login() {

    //如果沒有輸入帳號密碼，就不登入
    if (!this.loginRequest
      || this.loginRequest.account == null || this.loginRequest.account.length == 0
      || this.loginRequest.password == null || this.loginRequest.password.length == 0) {

      this.loginResponse.messages = [];
      this.loginResponse.messages.push("請輸入帳號密碼");

      return;
    } else {
      this.loginResponse.messages = [];
    }


    //如果沒登入過，或是之前的登入逾期
    if (this.CheckLoginIsExpire()) {
      this.httpService.post<any>(this.adminLoginPostUrl + 'AdminLoginVefify', this.loginRequest).subscribe(
        (result: LoginResponse) => {

          //設定登入資訊
          this.loginResponse.isLogin = result.isLogin;
          this.loginResponse.messages = result.messages;
          this.loginResponse.adminInfo = result.adminInfo;

          //如果登入成功先把資訊紀錄在 Local Storage，避免重新整理就要重新登入
          if (result.isLogin == true) {

            //計算逾時時間
            var expireDate = new Date();

            // (預設 30分)
            expireDate.setMinutes(expireDate.getMinutes() + 30);

            //寫入登入資訊到 LocalStorage
            localStorage.setItem("Login_Info", JSON.stringify(result.adminInfo));
            localStorage.setItem("Login_ExpireDate", JSON.stringify(expireDate));
            location.href="/index";
          }
        });
    }
  }

  //登入
  Logout() {

    this.AddAdminLog("Logout", "LogoutSuccess", "登出成功", "");

    //清除登入資訊
    this.loginResponse.isLogin = false;
    this.loginResponse.messages = [];
    this.loginResponse.adminInfo = new AdminData();

    //取得現在時間
    var now = new Date();

    //寫入登入資訊到 LocalStorage
    localStorage.setItem("Login_Info", JSON.stringify(null));
    localStorage.setItem("Login_ExpireDate", JSON.stringify(now));
  }

  //變更密碼
  ResetPassword() {

    //如果沒有輸入資料，就不變更
    if (!this.loginRequest
      || this.loginRequest.account == null || this.loginRequest.account.length == 0
      || this.loginRequest.email == null || this.loginRequest.email.length == 0
      || this.loginRequest.password == null || this.loginRequest.password.length == 0) {

      this.loginResponse.messages = [];
      this.loginResponse.messages.push("請輸入帳號、信箱、新密碼");

      return;
    } else {
      this.loginResponse.messages = [];
      this.httpService.post<any>(this.adminLoginPostUrl + 'ResetPassword', this.loginRequest).subscribe(
        (result: LoginResponse) => {

          this.loginResponse.messages = result.messages;

        });
    }
  }

  //寫入後台使用者 Log 
  AddAdminLog(type: string, code: string, name: string, remark: string) {

    var log = new AdminLog();
    log.operator = this.loginResponse.adminInfo.id;
    log.type = type;
    log.code = code;
    log.name = name;
    log.remark = remark;

    this.httpService.post<any>(this.adminLogPostUrl + 'AddAdminLog', log).subscribe(
      (res: any) => {
      });
  }

  //取得待審核專家數量
  GetWaitApprovalExpertsCount() {
    this.httpService.post<any>(this.adminAPIPostURL + 'GetWaitApprovalExpertsCount', null)
      .subscribe(
        (count: any) => {
          this.waitApprovalExpertsCount = count;
        });
  }

}


export class LoginRequest {
  //帳號
  account: string;
  //密碼
  password: string;
  //email
  email: string;
  //記住我
  rememberMe: boolean;

  constructor() {
    this.account = "";
    this.password = "";
    this.email = "";
    this.rememberMe = false;
  }
}

export class LoginResponse {
  //登入成功
  isLogin: boolean;
  //登入訊息
  messages: string[];
  //人員資訊
  adminInfo: AdminData;

  constructor() {
    this.isLogin = false;
    this.messages = [];
    this.adminInfo = new AdminData();
  }
}
