import { Component, Inject, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { transaction } from '../_interface/transaction.model';

@Component({
  selector: 'app-dialog-box',
  templateUrl: './dialog-box.component.html',
  styleUrls: ['./dialog-box.component.css']
})
export class TransactionDialogBoxComponent {
  action: string;
  local_data: any;

  constructor(    public dialogRef: MatDialogRef<TransactionDialogBoxComponent>,
  @Optional() @Inject(MAT_DIALOG_DATA) public data: transaction) { 
    this.local_data = {...data};
    this.action = this.local_data.action;
  }

  doAction(){
    this.dialogRef.close({event:this.action, data:this.local_data});
  }

  closeDialog(){
    this.dialogRef.close({event:'Cancel'});
  }
}
