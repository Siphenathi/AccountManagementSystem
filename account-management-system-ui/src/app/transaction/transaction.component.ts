import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import {RepositoryService} from '../shared/repository.service';
import { MatTableDataSource, MatTable } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { DialogBoxComponent } from './dialog-box/dialog-box.component';
import {transaction} from './_interface/transaction.model';
import { MatSort } from '@angular/material/sort';
import { NotificationService } from '../shared/Notification.service';
import { NotificationType } from '../shared/notification.message';
import { Router } from '@angular/router';

@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.css']
})
export class TransactionComponent implements OnInit {
  public displayedColumns = ['code', 'account_Code','amount','description','transaction_Date','capture_Date','details','update','delete'];
  public dataSource = new MatTableDataSource<transaction>();
  public numberOfRowsAffected = 0;
  private accountNo = 0;

  @ViewChild(MatTable) table: MatTable<transaction>;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private service: RepositoryService, public dialog: MatDialog, private notificationService: NotificationService, 
    private changeDetectorRefs: ChangeDetectorRef, private router: Router) { 

      var state = this.router.getCurrentNavigation().extras.state;
      this.accountNo = state.id as unknown as number
    }

    public ngOnInit(): void {
      this.getAccountTransactions();
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
        else if(result.event == 'update')
          this.updateRowData(result.data);
        else if(result.event == 'delete')
          this.deleteRowData(result.data.code);
      });
    }
  
    deleteRowData(code: number) {
      this.deletePerson(code);
      this.getAccountTransactions();
      this.table.renderRows();
    }
    updateRowData(transaction: transaction) {
      this.updateTransaction(transaction);
      this.getAccountTransactions();
      this.table.renderRows();
    }
    addRowData(transaction: transaction) {
      this.saveTransaction(transaction);
      this.getAccountTransactions();
      this.table.renderRows();
    }
  
    public ngAfterViewInit(): void {
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    }
    public doFilter = (value: string) => {
      this.dataSource.filter = value.trim().toLocaleLowerCase();
    }
  
    private getAccountTransactions = () => {
      this.service.getData(`transaction/GetAccountTransaction/${this.accountNo}`).subscribe( res => {
        this.dataSource.data = res as transaction[];
      });
    }

    private saveTransaction = (transaction: transaction) => {
      this.service.create("transaction/save", transaction).subscribe( res => {
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
    private updateTransaction = (transaction: transaction) => {
      this.service.update("transaction/update", transaction).subscribe( res => {
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
    private deletePerson = (code: number) => {
      this.service.delete(`transaction/delete/${code}`).subscribe( res => {
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
  
  
