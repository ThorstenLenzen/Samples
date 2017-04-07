import { Book } from './../shared/book';
import { Component, EventEmitter, Output, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'br-create-book',
  templateUrl: './create-book.component.html',
  styleUrls: ['./create-book.component.less']
})
export class CreateBookComponent implements OnInit {
  public  book: Book;

  @ViewChild(NgForm)
  public form: NgForm;

  @Output()
  public bookCreated = new EventEmitter<Book>(false);

  public ngOnInit(): void {
      this.book = Book.createNew();
    }

  public add(): void {
    this.bookCreated.emit(this.book);
    this.book = Book.createNew();
    this.form.reset(this.book);
  }

}
