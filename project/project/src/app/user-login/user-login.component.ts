import { Component } from '@angular/core';
import { SharedService } from 'src/app/shared.service';
import { User } from 'src/app/user-class';
import { } from 'json-schema';

 

import { Router } from '@angular/router';

import { DataSharingService } from 'src/app/data-sharing.service';
import { AuthenticationService } from 'src/app/authentication.service';

 

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.css']
})
export class UserLoginComponent {
  constructor(private service: SharedService, private router: Router, private datashare: DataSharingService, private authservice : AuthenticationService) { }

 

  dismissable: boolean = false;

 

  usr: any;
  user_string: string = "";


 

  usrnamelogin: any;
  usrpasslogin: any;

 

  UserNameLogin: string = "";
  UserPassLogin: string = "";
  pass: string = "";
  UserId: number = -1;

 

 

  display() {
    alert("Enter 8 digits");
  }

 

   getUser() {

 

    this.usrnamelogin = <HTMLInputElement>document.getElementById("usrnamelogin");
    this.UserNameLogin = this.usrnamelogin.value;

 

    this.usrpasslogin = <HTMLInputElement>document.getElementById("usrpasslogin");
    this.UserPassLogin = this.usrpasslogin.value;

 

 

 

    this.service.getUser(this.UserNameLogin).subscribe(res => {
      const users: User[] = res;
      //alert(JSON.stringify(users[0]));
      this.user_string = JSON.stringify(users[0].userPassword);
      this.user_string = this.user_string.replaceAll('"', '');
      this.UserId = parseInt(JSON.stringify(users[0].userId).replaceAll('"',''));
      //alert(this.UserId);
      if (this.user_string == this.UserPassLogin) {

 

 

        this.datashare.setUsername(this.UserNameLogin);
        this.datashare.setUserId(this.UserId);

 

        this.authservice.login();

 

        this.router.navigate(['/', 'upload']);

 

      }

 

      else {

        this.dismissable = true;
        this.router.navigate(['']);

 

      }

 

    })
  }

 

  dismissAlert() {
    this.dismissable = false;
  }
  ngOnInit() {
    this.UserNameLogin = "";
  }

 

  }

 

