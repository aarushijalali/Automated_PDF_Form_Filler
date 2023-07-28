import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from 'src/app/user-class';





@Injectable({
  providedIn: 'root'
})



export class SharedService {





  readonly APIUrl = "http://localhost:5121/api";
  constructor(private http: HttpClient) { }





  addUser(val: any) {
    return this.http.post(this.APIUrl + '/login', val)
  }
  getUser(usrnamelogin: string): Observable<User[]> {
    return this.http.get<User[]>(this.APIUrl + '/login?usrname=' + usrnamelogin);
  }





  getCol(userid: number, selectedFile: string): Observable<string[]> {
    return this.http.get<string[]>(this.APIUrl + '/UploadExcel/?userid=' + userid + '&filenameexcel=' + selectedFile);
  }





  sendData(excelVal: string, pdfVal: string) {
    return this.http.post(this.APIUrl + '/Map/ReceiveMapData', { excel: excelVal, pdf: pdfVal });
  }







  getRows(): Observable<string[]> {
    return this.http.get<string[]>(this.APIUrl + '/Map/ExcelRowExtract');
  }





  getPdfFields(selectedPdf: string, userid: number): Observable<string[]> {
    return this.http.get<string[]>(this.APIUrl + '/UploadPdf/?fileName=' + selectedPdf + '&userid=' + userid);
  }



  getSessionFields(userid: number): Observable<any> {
    return this.http.get<string[][]>(this.APIUrl + '/Session/?userid=' + userid);
  }



  getSessionFileNames(userid: number): Observable<any> {
    return this.http.get<any>(this.APIUrl + '/FileName/?userid=' + userid);
  }
}
