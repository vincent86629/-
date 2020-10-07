import { Injectable } from '@angular/core';
import { AdminService} from './admin.service';
import { Resolve } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AdminResolverService implements Resolve<any> {

  constructor(private memberService: AdminService) { }

  resolve() {
    return this.memberService.selectedMember;
  }
}
