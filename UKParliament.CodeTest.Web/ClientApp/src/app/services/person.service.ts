import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PersonViewModel } from '../models/person-view-model';
import { Result } from '../models/result.model';

@Injectable({
  providedIn: 'root'
})
export class PersonService {
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  url: string = this.baseUrl + `api/person/`;

  // Below is some sample code to help get you started calling the API
  getById(id: string): Observable<PersonViewModel> {
    return this.http.get<Result<PersonViewModel>>(this.url + `${id}`)
      .pipe(map((response) => response.Data));
  }


  getPersons(): Observable<PersonViewModel[]> {
    return this.http.get<Result<PersonViewModel[]>>(this.url).pipe(
      map((response) => {
        if (response && response.Success) {
          return response.Data || [];
        } else {
          throw new Error(response.Message || 'Unknown error occurred');
        }
      })
    );
  }

  getPersonDepartment(personId: string): Observable<{ departmentId: number; departmentName: string }> {
    return this.http.get<Result<{ departmentId: number; departmentName: string }>>(`${this.url}/${personId}/department`).pipe(
      map((response) => response.Data)
    );
  }

  updatePerson(person: PersonViewModel): Observable<void> {
    return this.http.put<Result<void>>(`${this.url}/${person.personId}`, person).pipe(
      map((response) => response.Data)
    );
  }

  addPerson(person: PersonViewModel): Observable<PersonViewModel> {
    return this.http.post<Result<PersonViewModel>>(this.url, person).pipe(
      map((response) => response.Data)
    );
  }
}
