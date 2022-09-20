import { Paged } from './paged';

export class BookParams extends Paged {
  title: string | null = '';
  author: string | null = '';
  publisher: string | null = '';
  publishingDate: Date | null = null;
  genre: string | null = '';
  isbn: string | null = '';
  status: number | null = 0;
}
