import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';  
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainNavComponent } from './main-nav/main-nav.component';
import { LayoutModule } from '@angular/cdk/layout';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { AboutComponent } from './about/about.component';
import { HomeComponent } from './home/home.component';
import { MatTableModule } from '@angular/material/table';
import {HttpClientModule} from'@angular/common/http';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {MatPaginatorModule } from '@angular/material/paginator';
import {MatGridListModule} from '@angular/material/grid-list';
import {MatDialogModule} from '@angular/material/dialog';
import { ToastrModule } from "ngx-toastr";
import { ContactComponent } from './contact/contact.component';
import { PersonComponent } from './person/person.component';
import { RepositoryService } from './shared/repository.service';
import { PersonDialogBoxComponent } from './person/dialog-box/dialog-box.component';
import {AccountDialogBoxComponent} from './account/dialog-box/dialog-box.component'
import {TransactionDialogBoxComponent} from './transaction/dialog-box/dialog-box.component'
import { NotificationMessage } from './shared/notification.message';
import { AccountComponent } from './account/account.component';
import { TransactionComponent } from './transaction/transaction.component';

@NgModule({
  declarations: [
    AppComponent,
    MainNavComponent,
    HomeComponent,
    AboutComponent,
    ContactComponent,
    PersonComponent,
    PersonDialogBoxComponent,
    AccountComponent,
    TransactionComponent,
    TransactionDialogBoxComponent,
    AccountDialogBoxComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    LayoutModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MatTableModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    MatPaginatorModule,
    MatGridListModule,
    MatDialogModule,
    CommonModule,
    ToastrModule.forRoot()
  ],
  entryComponents: 
  [
    PersonDialogBoxComponent, 
    AccountDialogBoxComponent, 
    TransactionDialogBoxComponent
  ],
  providers: [RepositoryService, NotificationMessage],
  bootstrap: [AppComponent]
})
export class AppModule { }
