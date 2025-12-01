import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-success',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="success-container">
      <div class="card">
        <div class="success-icon">✓</div>
        <h2>Check-in Successful!</h2>
        <p>Thank you for checking in. You may now enter the building.</p>
        <p class="timestamp">{{displayTime}}</p>
        <button type="button" (click)="startNewCheckIn()" class="btn-primary">New Check-in</button>
      </div>
    </div>
  `,
  styles: [`
    .success-container {
      max-width: 400px;
      margin: 0 auto;
      padding: 20px;
      min-height: calc(100vh - 150px);
      display: flex;
      align-items: center;
      justify-content: center;
    }
    .success-icon {
      font-size: 60px;
      color: #28a745;
      text-align: center;
      margin-bottom: 20px;
    }
    .timestamp {
      color: #666;
      font-size: 14px;
      text-align: center;
      margin: 20px 0;
    }
  `]
})
export class SuccessComponent {
  displayTime: string;

  constructor(private router: Router) {
    this.displayTime = new Date().toLocaleString();
  }

  startNewCheckIn() {
    this.router.navigate(['/entrance']);
  }
}
