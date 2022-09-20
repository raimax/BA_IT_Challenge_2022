import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MenuItem, MessageService } from 'primeng/api';
import { Book } from '../_models/book';
import { BorrowedBook } from '../_models/borrowedBook';
import { Pagination } from '../_models/pagination';
import { ReservedBook } from '../_models/reservedBook';
import { BookStatus } from '../_models/status';
import { BookService } from '../_services/book.service';

@Component({
  selector: 'app-admin-page',
  templateUrl: './admin-page.component.html',
  styleUrls: ['./admin-page.component.scss'],
})
export class AdminPageComponent implements OnInit {
  isLoading: boolean = false;
  items: MenuItem[] = [];
  activeItem: MenuItem = { label: 'Reserved Books', icon: 'pi pi-fw pi-clock' };
  pagination!: Pagination;
  pageNumber: number = 1;
  pageSize: number = 10;
  reservedBooks: ReservedBook[] = [];
  borrowedBooks: BorrowedBook[] = [];

  addBookForm = this.fb.group({
    title: ['', [Validators.required]],
    authorFirstName: ['', [Validators.required]],
    authorLastName: ['', [Validators.required]],
    publisher: ['', [Validators.required]],
    publishingDate: ['', [Validators.required]],
    genre: ['', [Validators.required]],
    isbn: ['', [Validators.required]],
  });

  constructor(
    private bookService: BookService,
    private messageService: MessageService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.items = [
      {
        label: 'Reserved Books',
        icon: 'pi pi-fw pi-clock',
        command: () => {
          this.activeItem = this.items[0];
          this.getReservedBooks();
        },
      },
      {
        label: 'Borrowed Books',
        icon: 'pi pi-fw pi-book',
        command: () => {
          this.activeItem = this.items[1];
          this.getBorrowedBooks();
        },
      },
      {
        label: 'Add Book',
        icon: 'pi pi-fw pi-plus',
        command: () => {
          this.activeItem = this.items[2];
        },
      },
    ];
    this.activeItem = this.items[0];

    this.getReservedBooks();
  }

  paginate(event: any) {
    this.pageNumber = event.page + 1;
    this.pageSize = event.rows;
  }

  getReservedBooks() {
    this.isLoading = true;
    this.bookService
      .getReservedBooks({
        pageNumber: this.pageNumber,
        pageSize: this.pageSize,
      })
      .subscribe({
        next: (response) => {
          this.pagination = response.pagination;
          this.reservedBooks = response.result;
          this.isLoading = false;
        },
        error: () => {
          this.isLoading = false;
        },
      });
  }

  getBorrowedBooks() {
    this.isLoading = true;
    this.bookService
      .getBorrowedBooks({
        pageNumber: this.pageNumber,
        pageSize: this.pageSize,
      })
      .subscribe({
        next: (response) => {
          this.pagination = response.pagination;
          this.borrowedBooks = response.result;
          this.isLoading = false;
        },
        error: () => {
          this.isLoading = false;
        },
      });
  }

  returnBook(book: Book) {
    this.isLoading = true;
    this.bookService.returnBook(book.id).subscribe({
      next: () => {
        switch (book.status.id) {
          case BookStatus.RESERVED:
            this.getReservedBooks();
            break;
          case BookStatus.BORROWED:
            this.getBorrowedBooks();
            break;
          default:
            break;
        }
        this.isLoading = false;
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: `Book ${
            book.status.id == BookStatus.RESERVED ? 'un-reserved' : 'returned'
          }`,
        });
      },
      error: () => {
        this.isLoading = false;
      },
    });
  }

  addBook() {
    this.isLoading = true;

    const book: Partial<Book> = {
      title: this.addBookForm.controls['title'].value!,
      author: {
        id: 0,
        firstName: this.addBookForm.controls['authorFirstName'].value!,
        lastName: this.addBookForm.controls['authorLastName'].value!,
      },
      publisher: { id: 0, name: this.addBookForm.controls['publisher'].value! },
      publishingDate: new Date(
        this.addBookForm.controls['publishingDate'].value!
      ),
      genre: this.addBookForm.controls['genre'].value!,
      isbn: this.addBookForm.controls['isbn'].value!,
    };

    this.bookService.addBook(book).subscribe({
      next: () => {
        this.isLoading = false;
				this.addBookForm.reset();
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Book Added',
        });
      },
      error: () => {
        this.isLoading = false;
      },
    });
  }
}
