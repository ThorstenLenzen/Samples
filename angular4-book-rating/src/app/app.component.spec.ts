import { TestBed, async } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { AppComponent } from './app.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('AppComponent', () => {
  let app;

  beforeEach(async(() => {
    TestBed
      .configureTestingModule({
        imports: [
          // RouterTestingModule
        ],
        declarations: [
          AppComponent
        ],
        schemas: [
          NO_ERRORS_SCHEMA
        ]
      })
      .compileComponents()
      .then(() => {
        const fixture = TestBed.createComponent(AppComponent);
        fixture.detectChanges();
        app = fixture.debugElement.componentInstance;
      });
  }));

  it('should create the app', async(() => {
    expect(app).toBeTruthy();
  }));

  it(`should have as title 'Book Rating!'`, async(() => {
    expect(app.title).toEqual('Book Rating');
  }));
});
