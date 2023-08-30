import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable, ReplaySubject } from 'rxjs';
import { User } from '../models/identity/User'
import { map, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private currentUserSource = new ReplaySubject<User>(1);
  public currentUser$ = this.currentUserSource.asObservable();

  baseURL = environment.apiURL + 'api/account/'

  constructor(private http: HttpClient) { }

  public login(model: any): Observable<void> {
    return this.http.post<User>(this.baseURL + 'login', model).pipe(
      take(1),
      map((res: User) => {
        const user = res;
        if (user) {
          this.setCurrentUser(user);
        }
      })
    )
  }

  public logout(): void {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.currentUserSource.complete();
  }

  public setCurrentUser(user: User):void {
    localStorage.setItem('user',  JSON.stringify(user))
    this.currentUserSource.next(user);
  }
}
