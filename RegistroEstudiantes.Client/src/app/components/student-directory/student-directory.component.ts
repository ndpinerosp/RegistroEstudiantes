import { Component, input } from '@angular/core';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-student-directory',
  standalone: true,
  imports: [MatTableModule],
  templateUrl: './student-directory.component.html'
})
export class StudentDirectoryComponent {
  data = input<any[]>([]);
}