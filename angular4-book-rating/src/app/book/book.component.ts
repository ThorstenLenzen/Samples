import { Book } from './../shared/book';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'br-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.css']
})
export class BookComponent implements OnInit {
  @Input() book: Book;
  @Output() rated: EventEmitter<Book>;

  constructor() {
    this.rated = new EventEmitter<Book>();
  }

  ngOnInit() {
  }

  public rateUp(): void {
    this.book.rateUp();
    this.rated.emit(this.book);
  }

  public rateDown(): void {
    this.book.rateDown();
    this.rated.emit(this.book);
  }

  public isRateUpDisabled(): boolean {
    if (this.book.rating > 4) {
      return true;
    }
    return false;
  }

  public isRateDownDisabled(): boolean {
    if (this.book.rating < 1) {
      return true;
    }
    return false;
  }

}
