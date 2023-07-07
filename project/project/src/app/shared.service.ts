import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from 'src/app/user-class';


@Injectable({
  providedIn: 'root'
})

//class User {
//  Username: string = "";
//  Useremail: string = "";
//  Userpassword: string = "";
//}

export class SharedService {

  readonly APIUrl = "http://localhost:5121/api";
  constructor(private http: HttpClient) { }

  addUser(val: any) {
    return this.http.post(this.APIUrl + '/login', val)
  }
  getUser(usrnamelogin: string): Observable<User[]> {

    return this.http.get<User[]>(this.APIUrl + '/login?usrname=' + usrnamelogin);

  }

  getCol(): Observable<string[]> {

    return this.http.get<string[]>(this.APIUrl+'/UploadExcel');

  }

  sendData(excelVal: string, pdfVal: string) {

    //const data 

    return this.http.post(this.APIUrl + '/Map', { excel: excelVal, pdf: pdfVal });
  }

  fieldGet() {
    return this.http.get(this.APIUrl + '/Map');
  }


}
