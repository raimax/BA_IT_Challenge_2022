import { Book } from './book';

export interface BorrowedBook {
  borrowedBy: string;
  book: Book;
}
