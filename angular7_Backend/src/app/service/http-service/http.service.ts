import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  _url = 'https://localhost:44374/';
  _token = '';
  _role = '';
  _id = '';
  _permissions: string[] = [];

  constructor(private http: HttpClient, private cookieService: CookieService) {
    // 資料先從localStorage取得
    this.setToken(localStorage.getItem('token')),
      this.setPermission(JSON.parse(localStorage.getItem('pages'))),
      this.setRole(JSON.parse(localStorage.getItem('role')));
  }

  createAuthorizationHeader(headers: HttpHeaders) {
    if (this._token) {
      headers.append('Authorization', 'Bearer ' + this._token);
    }
  }

  // 設定token
  setToken(token) {
    this._token = token;
  }

  // 設定規則
  setRole(role) {
    this._role = role;
  }

  // 設定ID
  setId(id) {
    this._id = id;
  }

  // 設定權限URL
  setPermission(permissions) {
    this._permissions = permissions;
  }

  // 檢查URL是否有在URL權限清單中
  isAvailableLink(link) {
    return this._permissions.indexOf(link) > -1;
  }

  TempDeleteMedNetCookie() {
    //瀏覽器安全因素，無法刪除 Cookie ，改更新成空白
    this.cookieService.set("ezA_gender", "", null, "/", ".med-net.com");
  }


  get<T>(path) {

    this.TempDeleteMedNetCookie();

    const fullPath = `${this._url}${path}`;
    const headers = new HttpHeaders();
    this.createAuthorizationHeader(headers);
    return this.http.get<T>(fullPath, { headers: headers });
  }

  post<T>(path, data) {

    this.TempDeleteMedNetCookie();

    const fullPath = `${this._url}${path}`;
    const headers = new HttpHeaders();
    this.createAuthorizationHeader(headers);
    return this.http.post(fullPath, data, { headers: headers });
  }

  put<T>(path, data) {

    this.TempDeleteMedNetCookie();

    const fullPath = `${this._url}${path}`;
    const headers = new HttpHeaders();
    this.createAuthorizationHeader(headers);
    headers.append('Content-Type', 'application/json');
    return this.http.put(fullPath, data, { headers: headers });
  }

  delete<T>(path) {

    this.TempDeleteMedNetCookie();

    const fullPath = `${this._url}${path}`;
    const headers = new HttpHeaders();
    this.createAuthorizationHeader(headers);
    headers.append('Content-Type', 'application/json');
    return this.http.delete(fullPath, { headers: headers });
  }

  upload<T>(path, file: FormData) {

    this.TempDeleteMedNetCookie();

    const fullPath = `${this._url}${path}`;
    const headers = new HttpHeaders();
    this.createAuthorizationHeader(headers);
    return this.http.post(fullPath, file, { headers: headers });
  }
}
