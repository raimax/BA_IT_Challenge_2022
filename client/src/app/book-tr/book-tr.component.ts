import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { Book } from '../_models/book';
import { BookStatus } from '../_models/status';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-book-tr',
  templateUrl: './book-tr.component.html',
  styleUrls: ['./book-tr.component.scss'],
})
export class BookTrComponent implements OnInit {
  @Input() book!: Book;
  @Input() association: string | null = null;
  @Output() onReserve = new EventEmitter<number>();
  @Output() onBorrow = new EventEmitter<number>();
  @Output() onReturn = new EventEmitter<Book>();
  bookActions: MenuItem[] = [];

  constructor(public accountService: AccountService) {}

  ngOnInit(): void {
    if (!this.association) {
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
    } else {
      this.bookActions = [
        {
          label: `${
            this.book.status.id == BookStatus.RESERVED ? 'Un-reserve' : 'Return'
          }`,
          icon: 'pi pi-fw pi-check',
          command: () => {
            this.onReturn.emit(this.book);
          },
        },
      ];
    }
  }
}
