import { Component, OnInit, Input } from '@angular/core';
import { SharedService } from 'src/app/shared.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-user-upload',
  templateUrl: './user-upload.component.html',
  styleUrls: ['./user-upload.component.css']
})


export class UserUploadComponent {

  selectedFile: File = new File([],'');
  constructor(private service: SharedService, private http: HttpClient) {
    
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }



  uploadFile() {
    if (this.selectedFile) {
      const formData = new FormData();
      formData.append('file', this.selectedFile);



      
      this.http.post("http://localhost:5121/api/UploadExcel", formData).subscribe(
        
        (response) => {
          console.log('File uploaded successfully');
          alert("Uploaded");
        },
        (error) => {
          console.error('Error uploading file:', error);

        }
      );




      //// Clear the file input field
      //const fileInput = document.getElementById('fileInput') as HTMLInputElement;
      //fileInput.value = '';
      //this.selectedFile = new File([], '');
    }
  }

  getExcelColumns() {
          this.service.getCol().subscribe((res) => {

            this.cols = res;
            this.colsize = this.cols.length;
      //alert(this.selectedOption);

      })

  }

  ngOnInit() {
    

  }

  cols: string[] = [];
  colsize: number= -1;
  selectedOption: any;
  selectedOptionIndex: number = -1;

  fields: string[] = ["field1", "field2", "field3"];
  selectedField: any;
  


  //getCols() {
   

  //  this.service.getCol().subscribe((res) => {
     
  //    this.cols = res;
      
  //    alert(this.selectedOption);
     
  //  })

  //}
  myMap = new Map<number, string>();
  //myArr: string[] = [];
  addValuesToTable() {

    for (let i = 0; i < this.colsize; i++) {
      if (this.selectedOption == this.cols[i]) {
        this.selectedOptionIndex = i;
      }
    }

    
    this.myMap.set(this.selectedOptionIndex, this.selectedField);
    var table = document.getElementById("valuesTable") as HTMLTableElement;
    var row = table.insertRow(-1);
    var cell1 = row.insertCell(0);
    var cell2 = row.insertCell(1);
    cell1.innerHTML = this.selectedOption;
    cell2.innerHTML = this.selectedField;
    //this.myArr = Array.from(this.myMap.values());
    //alert(this.myArr);

    //alert(JSON.stringify(this.myMap));


  }
  submit() {
    this.service.sendData(this.selectedOption, this.selectedField).subscribe(res => {
      alert(res.toString());
    })

    
  }
}
