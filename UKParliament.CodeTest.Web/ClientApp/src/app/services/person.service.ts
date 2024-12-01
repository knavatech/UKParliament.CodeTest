import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PersonViewModel } from '../models/person-view-model';

@Injectable({
  providedIn: 'root'
})
export class PersonService {
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  // Below is some sample code to help get you started calling the API
  getById(id: number): Observable<PersonViewModel> {
    return this.http.get<PersonViewModel>(this.baseUrl + `api/person/${id}`)
  }


  getPersons(): Observable<PersonViewModel[]> {
    return this.http.get<PersonViewModel[]>(this.baseUrl);
  }

  getPersonDepartment(personId: string): Observable<{ departmentId: number; departmentName: string }> {
    return this.http.get<{ departmentId: number; departmentName: string }>(`${this.baseUrl}/${personId}/department`);
  }

  updatePerson(person: PersonViewModel): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${person.personId}`, person);
  }

  addPerson(person: PersonViewModel): Observable<PersonViewModel> {
    return this.http.post<PersonViewModel>(this.baseUrl, person);
  }
}
