import { Injectable } from '@angular/core';
import { SessionData } from '../models/session.model';

@Injectable({
  providedIn: 'root'
})
export class SessionService {
  private readonly SESSION_KEY = 'building_entry_session';
  
  createSession(): string {
    const sessionId = this.generateSessionId();
    const session: SessionData = {
      sessionId,
      entranceId: '',
      currentStep: 1,
      visitorData: {}
    };
    localStorage.setItem(this.SESSION_KEY, JSON.stringify(session));
    return sessionId;
  }
  
  getSession(): SessionData | null {
    const session = localStorage.getItem(this.SESSION_KEY);
    return session ? JSON.parse(session) : null;
  }
  
  updateSession(updates: Partial<SessionData>): void {
    const session = this.getSession();
    if (session) {
      const updatedSession = { ...session, ...updates };
      localStorage.setItem(this.SESSION_KEY, JSON.stringify(updatedSession));
    }
  }
  
  clearSession(): void {
    localStorage.removeItem(this.SESSION_KEY);
  }
  
  private generateSessionId(): string {
    return 'session_' + Math.random().toString(36).substr(2, 9);
  }
}