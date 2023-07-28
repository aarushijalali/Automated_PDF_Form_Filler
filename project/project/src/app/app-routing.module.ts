import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { UserLoginComponent } from './user-login/user-login.component';
import { UserSignupComponent } from './user-signup/user-signup.component';
import { UserUploadComponent } from './user-upload/user-upload.component';
import { AuthGuard } from './auth.guard';



const routes: Routes = [


  { path: '', component: UserLoginComponent },
  { path: 'signup', component: UserSignupComponent },
  { path: 'upload', component: UserUploadComponent, canActivate: [AuthGuard] }

 

];


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
