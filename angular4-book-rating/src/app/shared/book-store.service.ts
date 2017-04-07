import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Book } from './../shared/book';
import { Observable } from 'rxjs/Observable';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/retry';

@Injectable()
export class BookStoreService {

  constructor(private http: Http) { }

  public getAllBooks(): Observable<Book[]> {
    return this.http
      .get('https://book-monkey2-api.angular-buch.com/books')
      .map(response => response.json())
      .map(jsonArray => jsonArray
        .map(jsonItem => new Book(
          jsonItem.isbn,
          jsonItem.title,
          jsonItem.description,
          jsonItem.rating))
      )
      .retry(3);
  }
}
