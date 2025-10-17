import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface RouteResponse {
  shortestPath: string[];
  totalDistance: number;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class RouteService {
  private apiUrl = 'http://localhost:5105/api/Route';  // CHANGE THIS PORT to match your Visual Studio port

  constructor(private http: HttpClient) { }

  getNodes(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/nodes`);
  }

  getShortestRoute(start: string, end: string): Observable<RouteResponse> {
    return this.http.get<RouteResponse>(`${this.apiUrl}?start=${start}&end=${end}`);
  }
}