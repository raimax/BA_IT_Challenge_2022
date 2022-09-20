import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { map, take } from 'rxjs';
import { Book } from '../_models/book';
import { BookStatus } from '../_models/status';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.scss'],
})
export class BookComponent implements OnInit {
  @Input() book!: Book;
  @Input() association: string | null = null;
  @Output() onReserve = new EventEmitter<number>();
  @Output() onBorrow = new EventEmitter<number>();
  @Output() onReturn = new EventEmitter<Book>();
  @Output() onDelete = new EventEmitter<number>();
  bookActions: MenuItem[] = [];
  currentuser: User | null = null;

  constructor(public accountService: AccountService) {
    this.accountService.currentUser$
      .pipe(take(1))
      .subscribe({ next: (user) => (this.currentuser = user) });
  }

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
        {
          label: 'Delete',
          icon: 'pi pi-fw pi-times',
          command: () => {
            this.onDelete.emit(this.book.id);
          },
          visible: this.currentuser?.roles.includes('Admin'),
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
