import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class LogService {
  constructor(private http: HttpClient) {

  }

  LogError(message: string) {
    console.log(message);
  }

  LogInfo(message: string) {
    console.log(message);
  }

}
