import { Component, OnInit, Input } from '@angular/core';
import { SharedService } from 'src/app/shared.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { DataSharingService } from 'src/app/data-sharing.service';
import { AuthenticationService } from 'src/app/authentication.service';





@Component({
  selector: 'app-user-upload',
  templateUrl: './user-upload.component.html',
  styleUrls: ['./user-upload.component.css']
})





export class UserUploadComponent {



  showSuccessAlert = false;
  showErrorAlert = false;
  selectedFile: File = new File([], '');
  selectedPdf: File = new File([], '');
  username: string = '';
  userid: number = -1;
  selectedPdfName: string = '';
  selectedExcelName: string = '';
  cols: string[] = [];
  colsize: number = -1;
  selectedOption: any;
  selectedOptionIndex: number = -1;
  fields: string[] = [];
  selectedField: any;
  myMap = new Map<string, number>()
  displayusrname: any;



  sessionres: any;
  pdffieldsold: string[] = [];
  excelfieldsold: string[] = [];
  constructor(private service: SharedService, private http: HttpClient, private datashare: DataSharingService, private authservice: AuthenticationService) {





  }





  onPdfSelected(event: any) {
    this.selectedPdf = event.target.files[0];
    this.selectedPdfName = this.selectedPdf?.name;
    this.clearTableAndMap();
  }



  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
    this.selectedExcelName = this.selectedFile?.name;
    this.clearTableAndMap();
  }



  uploadPdf() {
    if (this.selectedPdf) {
      const formData = new FormData();
      formData.append('file', this.selectedPdf);





      this.http.post("http://localhost:5121/api/UploadPdf", formData).subscribe(





        (response) => {
          console.log('File uploaded successfully');
          this.showSuccessAlert = true;
        },
        (error) => {
          console.error('Error uploading file:', error);
          this.showErrorAlert = true;



        }
      );
    }
  }



  uploadFile() {
    if (this.selectedFile) {
      const formData = new FormData();
      formData.append('file', this.selectedFile);



      this.http.post("http://localhost:5121/api/UploadExcel", formData).subscribe(



        (response) => {
          console.log('File uploaded successfully');
          this.showSuccessAlert = true;
        },
        (error) => {
          console.error('Error uploading file:', error);
          this.showErrorAlert = true;
        }
      );
    }
  }



  getExcelColumns() {
    this.service.getCol(this.userid, this.selectedFile.name).subscribe((res) => {
      this.cols = res;
      this.colsize = this.cols.length;
      //alert(this.selectedOption);
    })
  }



  ngOnInit() {



    this.username = this.datashare.getUsername();
    this.userid = this.datashare.getUserId();
    this.displayusrname = document.getElementById("displayusrname");
    this.displayusrname.innerHTML = "Welcome " + this.username + "!";



    this.service.getSessionFields(this.userid).subscribe((res) => {
      this.sessionres = JSON.stringify(res);
      //alert(this.sessionres);



      res.forEach((item: any) => {
        if (item.userFields !== null) {
          //alert(item.);
          this.pdffieldsold.push(item.userFields);
        }



        if (item.userFieldExcel !== null) {
          this.excelfieldsold.push(item.userFieldExcel.trim());
        }
      });
      this.cols = this.excelfieldsold;
      this.fields = this.pdffieldsold;
      this.colsize = this.cols.length;
      //alert(this.pdffieldsold)
    })



    this.service.getSessionFileNames(this.userid).subscribe((res) => {
      this.selectedPdfName = res[0].userPdf;
      this.selectedExcelName = res[0].userExcel;
    })



  }



  addValuesToTable() {



    for (let i = 0; i < this.colsize; i++) {
      if (this.selectedOption == this.cols[i]) {
        this.selectedOptionIndex = i;
      }
    }
    this.myMap.set(this.selectedField, this.selectedOptionIndex);
    var table = document.getElementById("valuesTable") as HTMLTableElement;
    var row = table.insertRow(-1);
    var cell1 = row.insertCell(0);
    var cell2 = row.insertCell(1);
    cell1.innerHTML = this.selectedOption;
    cell2.innerHTML = this.selectedField;





  }



  dismissSuccessAlert() {
    this.showSuccessAlert = false;
  }





  dismissErrorAlert() {
    this.showErrorAlert = false;
  }



  signout() {
    this.authservice.logout();
  }





  submit() {
    this.sendMapData(this.myMap, this.selectedPdfName, this.selectedExcelName, this.userid);
    //this.sendMapData(this.myMap, this.selectedPdf.name, this.selectedFile.name, this.username);
    alert("Submitted!");
  }







  getPdfFields() {
    if (this.selectedPdfName) {
      this.service.getPdfFields(this.selectedPdf.name, this.userid).subscribe((res) => {
        this.fields = res;
      });
    }
  }





  sendMapData(myMap: Map<string, number>, pdfname: string, excelname: string, userid: number) {
    const jsonData: { [key: string]: number } = {};





    Array.from(myMap.entries()).forEach(([key, value]) => {
      jsonData[key] = value;
    });





    this.http.post("http://localhost:5121/api/Map/ReceiveMapData", { myMap: jsonData, pdfName: pdfname, excelName: excelname, userId: userid }).subscribe(
      (response) => {
        console.log('Map data sent successfully:', response);
      },
      (error) => {
        console.error('Error sending map data:', error);
      }
    );
  }





  getRows() {
    this.service.getRows().subscribe((res) => {
      alert("Downloaded!");
    })
  }



  clearTableAndMap() {
    this.myMap.clear();
    var table = document.getElementById("valuesTable") as HTMLTableElement;
    var rowCount = table.rows.length;





    // Start from index 1 to skip the header row (index 0)
    for (var i = rowCount - 1; i >= 1; i--) {
      table.deleteRow(i);
    }
  }
}
  


