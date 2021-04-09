import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import {RepositoryService} from '../shared/repository.service';
import { MatTableDataSource, MatTable } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { DialogBoxComponent } from './dialog-box/dialog-box.component';
import {person} from './_interface/person.model';
import { MatSort } from '@angular/material/sort';
import { NotificationService } from '../shared/Notification.service';
import { NotificationType } from '../shared/notification.message';
import { Router } from '@angular/router';

@Component({
  selector: 'app-person',
  templateUrl: './person.component.html',
  styleUrls: ['./person.component.css']
})
export class PersonComponent implements OnInit {
  public displayedColumns = ['code', 'name', 'surname', 'id_number', 'details', 'update', 'delete'];
  public dataSource = new MatTableDataSource<person>();
  public numberOfRowsAffected = 0;

  @ViewChild(MatTable) table: MatTable<person>;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private service: RepositoryService, public dialog: MatDialog, private notificationService: NotificationService, 
    private changeDetectorRefs: ChangeDetectorRef, private router: Router) { }

  public ngOnInit(): void {
    this.getAllPeople();
  }

  public redirectToAccount(data: any){
    this.router.navigateByUrl('/account', {state:{id:data.code}});
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
      else if(result.event == 'detail')
        this.getRowData(result.data.code)
    });
  }

  deleteRowData(code: number) {
    this.deletePerson(code);
    this.getAllPeople();
    this.table.renderRows();
  }
  updateRowData(person: person) {
    this.updatePerson(person);
    this.getAllPeople();
    this.table.renderRows();
  }
  addRowData(person: person) {
    this.savePerson(person);
    this.getAllPeople();
    this.table.renderRows();
  }
  getRowData(code: number){
    this.getPerson(code);
  }

  public ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }
  public doFilter = (value: string) => {
    this.dataSource.filter = value.trim().toLocaleLowerCase();
  }

  private getAllPeople = () => {
    this.service.getData("person").subscribe( res => {
      this.dataSource.data = res as person[];      
    });
  }
  private getPerson = (code: number) => {
    this.service.getData(`person/get/${code}`).subscribe( res => {
      this.numberOfRowsAffected = res as number;
    },
    error => {
      this.notificationService.sendMessage({
        message: error,
        type: NotificationType.error
      });
    });
  }
  private savePerson = (person: person) => {
    this.service.create("person/save", person).subscribe( res => {
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
  private updatePerson = (person: person) => {
    this.service.update("person/update", person).subscribe( res => {
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
    this.service.delete(`person/delete/${code}`).subscribe( res => {
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

