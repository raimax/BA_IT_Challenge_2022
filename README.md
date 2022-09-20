# BooksSpot - simple library management system
Made for BA IT Challenge 2022

## Technologies used
- Back-End: ASP.NET Core 6
- Front-End: Angular 14
- DBMS: Microsoft SQL Server

## Startup instructions

### Requirements
- Visual Studio 2022 with ASP.NET ([link](https://visualstudio.microsoft.com/vs/))
- Visual Studio Code ([link](https://code.visualstudio.com/))
- Node.js (for npm) ([link](https://nodejs.org/en/))

### Launch instructions
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

## Useful information
Upon first launch, some data is seeded into the database for testing (books, roles, users).

Default credentials for admin account:
```
username: admin, password: password
```
Default credentials for regular account:
```
username: user, password: password
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
