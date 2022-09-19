import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Book } from '../_models/book';
import { BookParams } from '../_models/bookParams';
import { Pagination } from '../_models/pagination';
import { BookStatus, Status } from '../_models/status';
import { AccountService } from '../_services/account.service';
import { BookService } from '../_services/book.service';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-search-page',
  templateUrl: './search-page.component.html',
  styleUrls: ['./search-page.component.scss'],
})
export class SearchPageComponent implements OnInit {
  isLoading: boolean = false;
  status: Status[] = [];
  selectedStatus: Status = { name: 'all', id: 0 };
  advancedSearchActive: boolean = true;
  pagination!: Pagination;
  pageNumber: number = 1;
  pageSize: number = 10;
  books: Book[] = [];

  constructor(
    private fb: FormBuilder,
    private bookService: BookService,
    public accountService: AccountService
  ) {
    this.status = [
      { name: 'all', id: 0 },
      { name: 'available', id: BookStatus.AVAILABLE },
      { name: 'reserved', id: BookStatus.RESERVED },
      { name: 'borrowed', id: BookStatus.BORROWED },
    ];
  }

  searchForm = this.fb.group({
    title: [''],
    author: [''],
    publisher: [''],
    publishingDate: [''],
    genre: [''],
    isbn: [''],
    status: [0],
  });

  ngOnInit(): void {}

  search() {
    this.isLoading = true;

    let bookParams: BookParams = {
      title: this.searchForm.controls['title'].value,
      author: this.searchForm.controls['author'].value,
      publisher: this.searchForm.controls['publisher'].value,
      publishingDate: this.searchForm.controls['publishingDate'].value,
      genre: this.searchForm.controls['genre'].value,
      isbn: this.searchForm.controls['isbn'].value,
      status: this.searchForm.controls['status'].value,
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
    };

    this.bookService.getBooks(bookParams).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.pagination = response.pagination;
        this.books = response.result;
      },
      error: (error) => {
        this.isLoading = false;
        console.log(error);
      },
    });
  }

  toggleAdvancedSearch() {
    this.advancedSearchActive = !this.advancedSearchActive;
  }

  paginate(event: any) {
    this.pageNumber = event.page + 1;
    this.pageSize = event.rows;
    this.search();
  }

  reserveBook(bookId: number) {
    this.isLoading = true;
    this.bookService.reserveBook(bookId).subscribe({
      next: (response) => {
        this.isLoading = false;
        console.log(response);
      },
      error: (error) => {
        this.isLoading = false;
        console.log(error);
      },
    });
  }

  borrowBook(bookId: number) {
    this.isLoading = true;
    this.bookService.borrowBook(bookId).subscribe({
      next: (response) => {
        this.isLoading = false;
        console.log(response);
      },
      error: (error) => {
        this.isLoading = false;
        console.log(error);
      },
    });
  }
}
