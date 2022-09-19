import { Author } from "./author";
import { Publisher } from "./publisher";
import { Status } from "./status";

export interface Book {
  id: number;
  title: string;
  author: Author;
  publisher: Publisher;
  publishingDate: Date;
  genre: string;
  isbn: string;
  status: Status;
}
