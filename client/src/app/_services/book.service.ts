import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import {
  getPaginatedResult,
  getPaginationHeaders,
} from '../_helpers/paginationHelper';
import { Book } from '../_models/book';
import { BookParams } from '../_models/bookParams';

@Injectable({
  providedIn: 'root',
})
export class BookService {
  baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getBooks(bookParams: BookParams) {
    let params = getPaginationHeaders(
      bookParams.pageNumber,
      bookParams.pageSize
    );

    params = params.append('title', bookParams.title || '');
    params = params.append('author', bookParams.author || '');
    params = params.append('publisher', bookParams.publisher || '');
    params = params.append('publishingDate', bookParams.publishingDate || '');
    params = params.append('genre', bookParams.genre || '');
    params = params.append('isbn', bookParams.isbn || '');
    params = params.append('status', bookParams.status || 0);

    return getPaginatedResult<Book[]>(
      this.baseUrl + 'books',
      params,
      this.http
    );
  }

  reserveBook(bookId: number) {
    return this.http.put(
      this.baseUrl + 'books/' + `${bookId}` + '/reserve',
      {}
    );
  }

  borrowBook(bookId: number) {
    return this.http.put(this.baseUrl + 'books/' + `${bookId}` + '/borrow', {});
  }
}
