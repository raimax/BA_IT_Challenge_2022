import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { Book } from '../_models/book';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-book-tr',
  templateUrl: './book-tr.component.html',
  styleUrls: ['./book-tr.component.scss'],
})
export class BookTrComponent implements OnInit {
  @Input() book!: Book;
  @Output() onReserve = new EventEmitter<number>();
  @Output() onBorrow = new EventEmitter<number>();
  bookActions: MenuItem[] = [];

  constructor(public accountService: AccountService) {
    this.bookActions = [
      {
        label: 'Reserve',
        icon: 'pi pi-fw pi-clock',
        command: () => {
          this.onReserve.emit(this.book.id);
        },
      },
      {
        label: 'Borrow',
        icon: 'pi pi-fw pi-book',
        command: () => {
          this.onBorrow.emit(this.book.id);
        },
      },
    ];
  }

  ngOnInit(): void {}
}
