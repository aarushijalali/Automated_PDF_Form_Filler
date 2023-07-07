import { Component, OnInit, Input } from '@angular/core';
import { SharedService } from 'src/app/shared.service';
import { Router } from '@angular/router';



@Component({
  selector: 'app-user-signup',
  templateUrl: './user-signup.component.html',
  styleUrls: ['./user-signup.component.css']
})
export class UserSignupComponent {

 

  constructor(private service: SharedService, private router: Router) {
  
}

  usrname: any;
  usremail: any;
  usrpassword: any;
  usrconfirm: any;
  UserName: string ="";
  UserEmail: string ="";
  UserPassword: string = "";
  UserConfirm: string = "";


  ngOnInit() {
 
  }

  
  

  addUser() {

    this.usrname = <HTMLInputElement>document.getElementById("usrname");
    this.UserName = this.usrname.value;

    this.usremail = <HTMLInputElement>document.getElementById("usremail");
    this.UserEmail = this.usremail.value;

    this.usrpassword = <HTMLInputElement>document.getElementById("usrpassword");
    this.UserPassword = this.usrpassword.value;


    this.usrconfirm = <HTMLInputElement>document.getElementById("usrconfirm");
    this.UserConfirm = this.usrconfirm.value;


    console.log(this.usrname);

    if (this.UserConfirm != this.UserPassword) {
      alert("Password does not match");
      
      this.router.navigate(['/', 'signup']);
      

    }

    else {

      

      var val = {
        UserName: this.UserName,
        UserEmail: this.UserEmail,
        UserPassword: this.UserPassword
      };

      

      this.service.addUser(val).subscribe(res => {
        alert(res.toString());
      })

      this.router.navigate([""]);
    }
    
  }


}
