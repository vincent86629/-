import { Component, OnInit } from '@angular/core';
import { AppService } from '../../service/app-service/app.service';
import { HttpService } from '../../service/http-service/http.service';
import { AdminMenuData } from '../../data/admindata';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  postUrl: string = "api/Admin/Admin/";
  id: number;
  adminMenuData: AdminMenuData[];
  constructor(private appService: AppService, private httpService: HttpService) { }

   ngOnInit() {
    this.id = this.appService.loginResponse.adminInfo.id;
    this.httpService.post<any>(this.postUrl + 'GetAdminMenuByID', this.id).subscribe(
      (data: AdminMenuData[]) => {
        this.adminMenuData = data;
      }
      ,(err: any) => {
        console.log(err);
      });
   }

}