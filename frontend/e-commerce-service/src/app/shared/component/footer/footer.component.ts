import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.css'
})
export class FooterComponent {
  @Output() pageNumberChanged = new EventEmitter<number>();
  public pageNumber: number = 1;

  loadMore(): void {
    this.pageNumber++;
    this.pageNumberChanged.emit(this.pageNumber);
  }
}
