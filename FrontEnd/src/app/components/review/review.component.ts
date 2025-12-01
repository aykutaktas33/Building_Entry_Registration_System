import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { SessionService } from '../../services/session.service';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-review',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="review-container">
      <div class="card">
        <h2>Review Information</h2>
        <div class="review-summary">
          <div class="review-item">
            <div class="review-label">Name:</div>
            <div class="review-value">{{visitorData?.name || 'Not provided'}}</div>
          </div>
          <div class="review-item">
            <div class="review-label">Email:</div>
            <div class="review-value">{{visitorData?.email || 'Not provided'}}</div>
          </div>
          <div class="review-item">
            <div class="review-label">Company:</div>
            <div class="review-value">{{visitorData?.company || 'Not provided'}}</div>
          </div>
          <div class="review-item">
            <div class="review-label">Team:</div>
            <div class="review-value">{{getTeamName(visitorData?.teamId)}}</div>
          </div>
        </div>
        <div class="actions">
          <button type="button" class="btn-outline" (click)="goBack()">Back</button>
          <button type="button" (click)="checkIn()" [disabled]="isLoading" class="btn-primary">
            {{isLoading ? 'Processing...' : 'Check In'}}
          </button>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .review-container {
      max-width: 400px;
      margin: 0 auto;
      padding: 20px;
      min-height: calc(100vh - 150px);
      display: flex;
      align-items: center;
      justify-content: center;
    }

    .card {
      background: white;
      border-radius: 12px;
      padding: 30px;
      box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
      width: 100%;
    }

    h2 {
      margin: 0 0 20px 0;
      color: #333;
      text-align: center;
    }

    .review-summary {
      background: #f8f9fa;
      border-radius: 8px;
      padding: 20px;
      margin-bottom: 30px;
    }

    .review-item {
      margin-bottom: 15px;
      padding-bottom: 15px;
      border-bottom: 1px solid #dee2e6;
    }

    .review-item:last-child {
      margin-bottom: 0;
      padding-bottom: 0;
      border-bottom: none;
    }

    .review-label {
      font-weight: 600;
      color: #495057;
      margin-bottom: 5px;
      font-size: 14px;
    }

    .review-value {
      color: #212529;
      font-size: 16px;
    }

    .actions {
      display: flex;
      gap: 10px;
    }

    .btn-primary {
      flex: 1;
      padding: 14px;
      background: #007bff;
      color: white;
      border: none;
      border-radius: 8px;
      font-size: 16px;
      font-weight: 600;
      cursor: pointer;
      transition: background 0.3s;
    }

    .btn-primary:hover:not(:disabled) {
      background: #0056b3;
    }

    .btn-primary:disabled {
      background: #ccc;
      cursor: not-allowed;
    }

    .btn-outline {
      flex: 1;
      padding: 14px;
      background: transparent;
      color: #007bff;
      border: 2px solid #007bff;
      border-radius: 8px;
      font-size: 16px;
      font-weight: 600;
      cursor: pointer;
      transition: all 0.3s;
    }

    .btn-outline:hover {
      background: #007bff;
      color: white;
    }
  `]
})
export class ReviewComponent implements OnInit {
  visitorData: any;
  isLoading = false;

  constructor(
    private router: Router,
    private sessionService: SessionService,
    private apiService: ApiService
  ) {}

  ngOnInit() {
    const session = this.sessionService.getSession();
    if (!session || !session.entranceId) {
      this.router.navigate(['/entrance']);
      return;
    }
    this.visitorData = session.visitorData;
  }

  getTeamName(teamId: string): string {
    const teams: any = {
      '1': 'Development Team',
      '2': 'Design Team',
      '3': 'Marketing Team',
      '4': 'Sales Team',
      '5': 'Support Team'
    };
    return teams[teamId] || 'Not selected';
  }

  checkIn() {
    this.isLoading = true;
    const session = this.sessionService.getSession();

    if (!session) {
      this.router.navigate(['/entrance']);
      return;
    }

    // DÜZELTME: Sadece sessionId string olarak gönder
    this.apiService.checkIn(session.sessionId).subscribe({
      next: (response) => {
        console.log('Check-in successful:', response);
        this.sessionService.clearSession();
        this.router.navigate(['/success']);
      },
      error: (error) => {
        console.error('Check-in error:', error);
        this.isLoading = false;
      }
    });
  }

  goBack() {
    this.router.navigate(['/accept-rules']);
  }
}
