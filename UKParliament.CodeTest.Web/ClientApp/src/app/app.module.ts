import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';

import { PersonListComponent } from './components/person/person-list/person-list.component';
import { AddPersonComponent } from './components/person/add-person/add-person.component';
import { PersonManagerComponent } from './components/person/person-manager/person-manager.component';



@NgModule({
  declarations: [
    AppComponent,
    HomeComponent
  ],
  bootstrap: [AppComponent],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    FormsModule,
    PersonListComponent,
    AddPersonComponent,
    PersonManagerComponent,
    RouterModule.forRoot([
      { path: '', redirectTo: '/persons', pathMatch: 'full' }, // Default route
      { path: 'persons', component: PersonListComponent }, // Person list
      { path: 'persons/add', component: AddPersonComponent }, // Add person form
      { path: 'persons/edit/:id', component: AddPersonComponent }//, // Edit person form with ID
      //{ path: '**', redirectTo: '/persons' } // Wildcard route (fallback)
    ])],
  providers: [provideHttpClient(withInterceptorsFromDi())]
})
export class AppModule { }
