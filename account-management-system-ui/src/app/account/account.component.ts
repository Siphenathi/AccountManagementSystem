import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import {RepositoryService} from '../shared/repository.service';
import { MatTableDataSource, MatTable } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { DialogBoxComponent } from './dialog-box/dialog-box.component';
import {account} from './_interface/account.model';
import { MatSort } from '@angular/material/sort';
import { NotificationService } from '../shared/Notification.service';
import { NotificationType } from '../shared/notification.message';
import { Router } from '@angular/router';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {
  public displayedColumns = ['code', 'name', 'surname', 'id_number', 'details', 'update', 'delete'];
  public dataSource = new MatTableDataSource<account>();
  public numberOfRowsAffected = 0;

  @ViewChild(MatTable) table: MatTable<account>;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private service: RepositoryService, public dialog: MatDialog, private notificationService: NotificationService, 
    private changeDetectorRefs: ChangeDetectorRef, private router: Router) { 
      console.log(this.router.getCurrentNavigation().extras.state)
    }

  public ngOnInit(): void {
    console.log(this.router.getCurrentNavigation().extras.state)
    // console.log(this.router.getCurrentNavigation().extras.state);
    this.getAccountsWithParentKey();
  }
  
  public openDialog(action: any, data: any) {
    data.action = action;
    const dialogRef = this.dialog.open(DialogBoxComponent, {
      width: '284px',
      data: data
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result.event == 'add')
        this.addRowData(result.data);
      else if(result.event == 'delete')
        this.deleteRowData(result.data.code);
      else if(result.event == 'detail')
        this.getRowData(result.data.code)
    });
  }

  deleteRowData(account: account) {
    this.deleteAccount(account.code);
    this.getAccountsWithParentKey(account.person_code);
    this.table.renderRows();
  }

  addRowData(account: account) {
    this.saveAccount(account);
    this.getAccountsWithParentKey(account.person_code);
    this.table.renderRows();
  }
  getRowData(code: number){
    this.getAccount(code);
  }

  public ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }
  public doFilter = (value: string) => {
    this.dataSource.filter = value.trim().toLocaleLowerCase();
  }

  private getAccountsWithParentKey = (parentCode?: number) => {
    if(parentCode !== 0)
    this.service.getData(`account/GetAccountWithParentKey/${parentCode}`).subscribe( res => {
      this.dataSource.data = res as account[];   
    });
    else
    this.service.getData(`account/get`).subscribe( res => {
      this.dataSource.data = res as account[];   
    });
  }
  private getAccount = (code: number) => {
    this.service.getData(`account/get/${code}`).subscribe( res => {
      this.numberOfRowsAffected = res as number;
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
