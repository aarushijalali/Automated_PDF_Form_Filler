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
  UserName: string = "";
  UserEmail: string = "";
  UserPassword: string = "";
  UserConfirm: string = "";
  userCreated: boolean = false;









  ngOnInit() {
    this.usrname = <HTMLInputElement>document.getElementById("usrname");
    this.usremail = <HTMLInputElement>document.getElementById("usremail");
    this.usrpassword = <HTMLInputElement>document.getElementById("usrpassword");
    this.usrconfirm = <HTMLInputElement>document.getElementById("usrconfirm");





  }







  containsLowerCaseLetter(password: string): boolean {
    const lowerCaseLetters = /[a-z]/g;
    return lowerCaseLetters.test(password);
  }







  containsUpperCaseLetter(password: string): boolean {
    const upperCaseLetters = /[A-Z]/g;
    return upperCaseLetters.test(password);
  }







  containsNumber(password: string): boolean {
    const numbers = /[0-9]/g;
    return numbers.test(password);
  }







  hasMinimumLength(password: string): boolean {
    return password.length >= 8;
  }







  validatePassword() {
    const password = this.UserPassword;







    const containsLowerCase = /[a-z]/.test(password);
    const containsUpperCase = /[A-Z]/.test(password);
    const containsNumber = /\d/.test(password);
    const hasMinimumLength = password.length >= 8;







    const letterElement = document.getElementById("letter");
    const capitalElement = document.getElementById("capital");
    const numberElement = document.getElementById("number");
    const lengthElement = document.getElementById("length");







    if (letterElement) {
      letterElement.classList.toggle("invalid", !containsLowerCase);
      letterElement.classList.toggle("valid", containsLowerCase);
    }







    if (capitalElement) {
      capitalElement.classList.toggle("invalid", !containsUpperCase);
      capitalElement.classList.toggle("valid", containsUpperCase);
    }







    if (numberElement) {
      numberElement.classList.toggle("invalid", !containsNumber);
      numberElement.classList.toggle("valid", containsNumber);
    }







    if (lengthElement) {
      lengthElement.classList.toggle("invalid", !hasMinimumLength);
      lengthElement.classList.toggle("valid", hasMinimumLength);
    }
  }





  addUser() {





    this.UserName = this.usrname.value;
    this.UserEmail = this.usremail.value;
    this.UserPassword = this.usrpassword.value;
    this.UserConfirm = this.usrconfirm.value;









    //alert(this.usrname);







    //if (this.UserConfirm != this.UserPassword) {
    //  alert("Password does not match");





    //  this.router.navigate(['/', 'signup']);









    //}













    if (!(this.UserName &&
      this.UserEmail &&
      this.UserPassword &&
      this.UserConfirm &&
      this.containsLowerCaseLetter(this.UserPassword) &&
      this.containsUpperCaseLetter(this.UserPassword) &&
      this.containsNumber(this.UserPassword) &&
      this.hasMinimumLength(this.UserPassword) &&
      (this.UserConfirm == this.UserPassword))) {
      alert("Enter Correctly");





      this.router.navigate(['/', 'signup']);









    }









    else {





      var val = {
        UserName: this.UserName,
        UserEmail: this.UserEmail,
        UserPassword: this.UserPassword
      };











      this.service.addUser(val).subscribe(res => {
        alert(JSON.stringify(res));



      })





      this.router.navigate([""]);




    }





  }
}
