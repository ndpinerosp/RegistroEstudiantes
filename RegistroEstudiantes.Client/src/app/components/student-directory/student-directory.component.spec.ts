import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentDirectoryComponent } from './student-directory.component';

describe('StudentDirectoryComponent', () => {
  let component: StudentDirectoryComponent;
  let fixture: ComponentFixture<StudentDirectoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudentDirectoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudentDirectoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
