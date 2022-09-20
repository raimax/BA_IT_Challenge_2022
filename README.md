# BooksSpot - simple library management system
Made for BA IT Challenge 2022

## Startup instructions

1. **Launch back-end development server**.
- Open 'BookSpot_server.sln' with Visual Studio
- Start the project

2. **Launch front-end development server**.
- Open 'client' folder
- Open terminal and type:
```
npm install
```
then
```
ng serve --open
```

## System functions
- anonymous:
  - book search (by title, author, publisher, publishing date, genre, isbn, status)
- registered user:
  - same as anonymous
  - book reservation
  - book borrowing
- admin:
  - same as registered
  - list all reserved books
  - list all borrowed books
  - un-reserve reserved books
  - return borrowed books
  - add books
  - delete books
  
## Database Schema (without asp.net core entities)
![Database Schema](https://i.imgur.com/TzWQ1mD.jpeg)
