import { Component, OnInit } from '@angular/core';
import { PersonService } from '../../../services/person.service';
import { PersonViewModel } from '../../../models/person-view-model';
import { DepartmentViewModel } from '../../../models/department-view-model';
import { AddPersonComponent } from '../add-person/add-person.component';
import { PersonListComponent } from '../person-list/person-list.component';

@Component({
  imports: [AddPersonComponent, PersonListComponent],
  standalone: true,
  selector: 'app-person-manager',
  templateUrl: './person-manager.component.html',
  styleUrls: ['./person-manager.component.scss']
})
export class PersonManagerComponent implements OnInit {
  persons: PersonViewModel[] = [];
  departments: DepartmentViewModel[] = [];
  selectedPerson: PersonViewModel = {
    personId: '',
    firstName: '',
    lastName: '',
    dateOfBirth: '',
    departmentId: 0,
    departmentName: ''
  };

  constructor(private personService: PersonService) { }

  ngOnInit(): void {
    this.loadPersons();
    this.loadDepartments();
  }

  loadPersons(): void {
    this.personService.getPersons().subscribe((data) => (this.persons = data));
  }

  loadDepartments(): void {
    // Simulate fetching departments
    this.departments = [
      { id: 1, name: 'Sales' },
      { id: 2, name: 'Marketing' },
      { id: 3, name: 'Finance' },
      { id: 4, name: 'HR' }
    ];
  }

  handleSave(person: PersonViewModel): void {
    // Save person logic here
  }

  handleEdit(person: PersonViewModel): void {
    this.selectedPerson = { ...person };
  }

  handleDelete(personId: string): void {
    // Delete person logic here
  }
}
