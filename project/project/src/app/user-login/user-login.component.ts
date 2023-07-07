import { Component } from '@angular/core';
import { SharedService } from 'src/app/shared.service';
import { User } from 'src/app/user-class';
import { } from 'json-schema';

import { Router } from '@angular/router';

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.css']
})
export class UserLoginComponent {
  constructor(private service: SharedService, private router: Router) { }

  usr: any;
  user_string: string = "";
  

  usrnamelogin: any;
  usrpasslogin: any;

  UserNameLogin: string = "";
  UserPassLogin: string = "";
  pass: string = "";



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

      this.user_string = JSON.stringify(users[0].UserPassword);
      this.user_string = this.user_string.replaceAll('"', '');


      if (this.user_string == this.UserPassLogin) {



        this.router.navigate(['/', 'upload']);

      }

      else {
   
        alert("Wrong Password try again")
        this.router.navigate(['']);

      }



    })


    }





  ngOnInit() { }



  }


