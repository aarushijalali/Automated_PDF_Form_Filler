import { Injectable } from '@angular/core';



@Injectable({
  providedIn: 'root'
})
export class DataSharingService {



  private username: string = '';
  private userid: number = -1;



  constructor() { }



  setUsername(username: string) {
    this.username = username;
  }



  getUsername() {
    return this.username;
  }



  setUserId(userid: number) {
    this.userid = userid;
  }



  getUserId() {
    return this.userid;
  }
}
