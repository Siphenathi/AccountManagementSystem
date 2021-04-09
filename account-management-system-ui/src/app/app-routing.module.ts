import { NgModule } from '@angular/core';
// import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { ContactComponent } from './contact/contact.component';
import {PersonComponent} from './person/person.component';
import {AccountComponent} from './account/account.component';
import {TransactionComponent} from './transaction/transaction.component';

const routes: Routes = [
  {path: 'home', component: HomeComponent},
  {path: '', redirectTo: '/home', pathMatch: 'full' },
  {path: '***', redirectTo: '/home', pathMatch: 'full' },
  {path: 'about', component: AboutComponent },
  {path: 'contact', component: ContactComponent },
  {path: 'people', component: PersonComponent },
  {path: 'account', component: AccountComponent },
  {path: 'transaction', component: TransactionComponent }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
