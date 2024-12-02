
import { Router } from '@angular/router';
import { PersonViewModel } from '../../../models/person-view-model';
import { PersonService } from '../../../services/person.service';
import { AddPersonComponent } from '../add-person/add-person.component';
import { CommonModule } from '@angular/common';

import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';


@Component({
  selector: 'app-person-list',
  standalone: true,
  imports: [AddPersonComponent, CommonModule],
  templateUrl: './person-list.component.html',
  styleUrl: './person-list.component.scss'
})

export class PersonListComponent implements OnInit {

  constructor(private personService: PersonService, private router: Router) { }

  selectedPerson: PersonViewModel = {
    personId: '',
    firstName: '',
    lastName: '',
    dateOfBirth: '',
    departmentId: 0,
    departmentName: ''
  };

  @Input() persons: PersonViewModel[] = [];
  @Output() edit = new EventEmitter<PersonViewModel>();
  @Output() delete = new EventEmitter<string>();

  ngOnInit(): void {
    this.loadPersons();
  }

  loadPersons(): void {
    this.personService.getPersons().subscribe({
      next: (data) => {
        console.log('Persons received:', data);
        this.persons = data;
      },
      error: (err) => {
        console.error('Error fetching persons:', err);
      }
    });
  }
  
  selectPerson(person: PersonViewModel): void {
    this.selectedPerson = person;
  }

  editPerson(personId: string): void {
    this.router.navigate(['/persons/edit', personId]); // Navigate to edit route
  }

  deletePerson(personId: string): void {
    this.delete.emit(personId);
  }
}
