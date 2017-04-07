export class Book {
  private maxRating = 5;
  private minRating = 0;

  // Book-Factory
  public static createNew(): Book {
    return new Book('000', '', '');
  }

  constructor(
    public isbn: string,
    public title: string,
    public description: string,
    public rating = 0) { }

    public rateUp(): void {
      if (this.rating < this.maxRating) {
        this.rating++;
      }
    }

    public rateDown(): void {
      if (this.rating > this.minRating) {
        this.rating--;
      }
    }
}
