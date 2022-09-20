import { Book } from "./book";

export interface ReservedBook {
	reservedBy: string,
	book: Book
}