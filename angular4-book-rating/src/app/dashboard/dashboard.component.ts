import { BookStoreService } from './../shared/book-store.service';
import { Component, OnInit } from '@angular/core';

import { Book } from './../shared/book';

@Component({
  selector: 'br-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  public books: Book[];
  public text: string;

  constructor(private bookStoreService: BookStoreService) {
    this.text = '';
    this.books = [];
   }

  ngOnInit() {
    // this.books = [
    //   new Book('12345678', 'Erstes Buch', 'Erste Beschreibung', 2),
    //   new Book('22345678', 'Zweites Buch', 'Zweite Beschreibung', 2),
    //   new Book('32345678', 'Dritte Buch', 'Dritte Beschreibung', 2),
    //   new Book('42345678', 'Vierte Buch', 'Vierte Beschreibung', 2)
    // ];

    this.bookStoreService
      .getAllBooks()
      .subscribe(books => {
        this.books = books;
        this.reorderBooks(null);
      });
  }

  public reorderBooks(book: Book): void {
    this.books.sort((a, b) => b.rating - a.rating);
  }

  public addBook(book: Book): void {
    this.books.push(book);
    this.reorderBooks(null);
  }
}
