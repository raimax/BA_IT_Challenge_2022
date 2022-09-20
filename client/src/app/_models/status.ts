export interface Status {
  name: string;
  id: number;
}

export enum BookStatus {
  AVAILABLE = 1,
  RESERVED,
  BORROWED,
}
