import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Team } from '../models/team.model';
import { Visitor } from '../models/visitor.model';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  // Proxy kullanıyorsanız sadece /api ile başlayın
  private apiUrl = '/api/v1.0';

  constructor(private http: HttpClient) {}

  validateEntrance(entranceId: string, sessionId: string): Observable<any> {
    const body = { sessionId, entranceId };
    console.log('API Call to:', `${this.apiUrl}/Entrance/validate`);
    console.log('Validating entrance with body:', JSON.stringify(body));
    return this.http.post(`${this.apiUrl}/Entrance/validate`, body);
  }

  // Diğer metodlar aynı kalacak...
  getTeams(skip?: number, top?: number): Observable<Team[]> {
    let url = `${this.apiUrl}/Teams`;
    const params: any = {};

    if (skip !== undefined) params.skip = skip.toString();
    if (top !== undefined) params.top = top.toString();

    console.log('API Call to:', url);
    console.log('Get Teams:', JSON.stringify(params));

    return this.http.get<Team[]>(url, { params });
  }

  savePersonalInfo(sessionId: string, name: string, email: string, company: string): Observable<any> {
    const body = { sessionId, name, email, company };

    console.log('API Call to:', `${this.apiUrl}/Visitor/save-personal-info`);
    console.log('Saving personal info:', JSON.stringify(body));

    return this.http.post(`${this.apiUrl}/Visitor/save-personal-info`, body);
  }

  selectTeam(sessionId: string, teamId: string): Observable<any> {
    const body = { sessionId, teamId };

    console.log('API Call to:', `${this.apiUrl}/Visitor/select-team`);
    console.log('Selecting team:', JSON.stringify(body));

    return this.http.post(`${this.apiUrl}/Visitor/select-team`, body);
  }

  acceptRules(sessionId: string, accepted: boolean): Observable<any> {
    const body = { sessionId, accepted };

    console.log('API Call to:', `${this.apiUrl}/Visitor/accept-rules`);
    console.log('Accepting rules:', JSON.stringify(body));

    return this.http.post(`${this.apiUrl}/Visitor/accept-rules`, body);
  }

  checkIn(sessionId: string): Observable<any> {
    const body = { sessionId };

    console.log('API Call to:', `${this.apiUrl}/Visitor/checkin`);
    console.log('Checking in:', JSON.stringify(body));

    return this.http.post(`${this.apiUrl}/Visitor/checkin`, body);
  }

  getReview(sessionId: string): Observable<any> {

    console.log('API Call to (getReview):', `${this.apiUrl}/Visitor/${sessionId}/review`);

    return this.http.get(`${this.apiUrl}/Visitor/${sessionId}/review`);
  }

  getVisitor(id: string): Observable<Visitor> {

    console.log('API Call to (getVisitor):', `${this.apiUrl}/Visitor/${id}`);

    return this.http.get<Visitor>(`${this.apiUrl}/Visitor/${id}`);
  }
}
