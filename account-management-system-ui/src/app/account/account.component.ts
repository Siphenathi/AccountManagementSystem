import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import {RepositoryService} from '../shared/repository.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { AccountDialogBoxComponent } from './dialog-box/dialog-box.component';
import {account} from './_interface/account.model';
import { NotificationService } from '../shared/Notification.service';
import { NotificationType } from '../shared/notification.message';
import { Router } from '@angular/router';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {
  public displayedColumns = ['code', 'person_Code', 'account_Number', 'outstanding_Balance', 'details', 'update', 'delete'];
  public dataSource = new MatTableDataSource<account>();
  public numberOfRowsAffected = 0;
  private personCode = 0;
  public tempArray : Array<any> = []; 

  constructor(private service: RepositoryService, public dialog: MatDialog, private notificationService: NotificationService, 
    private changeDetectorRefs: ChangeDetectorRef, private router: Router) { 

      var state = this.router.getCurrentNavigation().extras.state;
      this.personCode = state.id as unknown as number
    }

  public redirectToTransaction(data: any){
    this.router.navigateByUrl('/transaction', {state:{id:data.code}});
  }

  public ngOnInit(): void {
    this.getAccount();    
  }
  
  public openDialog(action: any, data: any) {
    data.action = action;
    const dialogRef = this.dialog.open(AccountDialogBoxComponent, {
      width: '284px',
      data: data
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result.event == 'add')
        this.addRowData(result.data);
      else if(result.event == 'delete')
        this.deleteRowData(result.data.code);
      else if(result.event == 'detail')
        this.getAccount();
    });
  }

  deleteRowData(account: account) {
    this.deleteAccount(account.code);
    this.getAccount();
  }

  addRowData(account: account) {
    this.saveAccount(account);
    this.getAccount();
  }

  private getAccount = () => {
    this.service.getData(`account/GetAccountWithParentKey/${this.personCode}`).subscribe( res => {
      this.tempArray.push(res);
      this.dataSource.data = this.tempArray as account[];
      this.tempArray = [];
    },
    error => {
      this.notificationService.sendMessage({
        message: error,
        type: NotificationType.error
      });
    });
  }
  private saveAccount = (account: account) => {
    this.service.create("account/save", account).subscribe( res => {
      this.numberOfRowsAffected = res as number;
      this.changeDetectorRefs.detectChanges();
    },
    error => {
      this.notificationService.sendMessage({
        message: error,
        type: NotificationType.error
      });
    });
  }

  private deleteAccount = (code: number) => {
    this.service.delete(`account/delete/${code}`).subscribe( res => {
      this.numberOfRowsAffected = res as number;
      this.changeDetectorRefs.detectChanges();
    },
    error => {
        this.notificationService.sendMessage({
          message: error,
          type: NotificationType.error
        });
    });
  }
}
