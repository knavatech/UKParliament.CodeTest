import { Component } from '@angular/core';
import { PersonService } from '../../services/person.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  constructor(private personService: PersonService) {
    this.getPersonById('');
  }

  getPersonById(id: string): void {
    this.personService.getById(id).subscribe({
      next: (result) => console.info(`User returned: ${JSON.stringify(result)}`),
      error: (e) => console.error(`Error: ${e}`)
    });
  }
}
