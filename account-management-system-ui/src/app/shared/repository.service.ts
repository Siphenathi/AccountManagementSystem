import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from './../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RepositoryService {

  constructor(private http: HttpClient) { }

  public getData = (route: string) => {
    return this.http.get(this.createCompleteRoute(environment.apiUrl, route));
  }
 
  public create = (route: string, body: any) => {
    return this.http.post(this.createCompleteRoute(environment.apiUrl, route), body, this.generateHeaders());
  }
 
  public update = (route: string, body: any) => {
    return this.http.put(this.createCompleteRoute(environment.apiUrl, route), body, this.generateHeaders());
  }
 
  public delete = (route: string) => {
    return this.http.delete(this.createCompleteRoute(environment.apiUrl, route));
  }
 
  private createCompleteRoute = (apiUrl: string, route: string, ) => {
    return `${apiUrl}/${route}`;
  }
 
  private generateHeaders = () => {
    return {
      headers: new HttpHeaders({'Content-Type': 'application/json'})
    }
  }
}