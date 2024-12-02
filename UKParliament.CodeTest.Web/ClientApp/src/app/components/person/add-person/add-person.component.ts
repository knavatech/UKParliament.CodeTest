import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { DepartmentService } from '../../../services/department.service';
import { DepartmentViewModel } from '../../../models/department-view-model';
import { PersonViewModel } from '../../../models/person-view-model';
import { PersonService } from '../../../services/person.service';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-add-person',
  standalone: true,
  imports: [FormsModule, BrowserModule, HttpClientModule],
  templateUrl: './add-person.component.html',
  styleUrl: './add-person.component.scss'
})

export class AddPersonComponent implements OnInit {
  @Input() person: PersonViewModel = { personId: '', firstName: '', lastName: '', dateOfBirth: '', departmentId: 0, departmentName: '' };
  @Input() departments: DepartmentViewModel[] = [];
  @Output() save = new EventEmitter<PersonViewModel>();

  isEditMode = false;

  constructor(private departmentService: DepartmentService, private personService: PersonService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    this.departmentService.getDepartments().subscribe(data => {
      this.departments = data;
    });

    const id = this.route.snapshot.paramMap.get('id'); // Get the ID from the route
    if (id) {
      this.isEditMode = true; // Enable edit mode
      this.loadPerson(id); // Load person details
    }
  }

  loadPerson(id: string): void {
    this.personService.getById(id).subscribe((data) => {
        this.person = data;
      });
    }

  //savePerson(): void {
  //  if (!this.isEditMode) {
  //    this.person.personId = ''
  //  }
  //  this.save.emit(this.person);
  //}

  savePerson(): void {
    if (this.isEditMode) {
      this.personService.updatePerson(this.person).subscribe(() => {
        alert('Person updated successfully');
        this.router.navigate(['/persons']); // Redirect to person list
      });
    } else {
      this.personService.addPerson(this.person).subscribe(() => {
        alert('Person added successfully');
        this.router.navigate(['/persons']); // Redirect to person list
      });
    }
  }


  //save(): void {
  //  this.personService.addPerson(this.person).subscribe(() => {
  //    alert('Person added successfully!');
  //  });
  //}
}
