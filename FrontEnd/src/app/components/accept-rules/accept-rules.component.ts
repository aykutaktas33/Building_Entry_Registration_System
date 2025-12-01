import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SessionService } from '../../services/session.service';
import { ApiService } from '../../services/api.service'; // EKLENDİ

@Component({
  selector: 'app-accept-rules',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="rules-container">
      <div class="card">
        <h2>Accept Rules & Policies</h2>
        <form [formGroup]="rulesForm" (ngSubmit)="onSubmit()">
          <div class="checkbox-group">
            <input type="checkbox" id="acceptRules" formControlName="accepted">
            <label for="acceptRules">
              I accept the terms and conditions, privacy policy, and building rules.
            </label>
          </div>
          <div class="actions">
            <button type="button" class="btn-outline" (click)="goBack()">Back</button>
            <button type="submit" [disabled]="rulesForm.invalid" class="btn-primary">Next</button>
          </div>
        </form>
      </div>
    </div>
  `,
  styles: [`
    .rules-container {
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
    .checkbox-group {
      margin: 30px 0;
      display: flex;
      align-items: flex-start;
      gap: 10px;
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
  `]
})
export class AcceptRulesComponent implements OnInit {
  rulesForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private sessionService: SessionService,
    private apiService: ApiService // EKLENDİ
  ) {
    this.rulesForm = this.fb.group({
      accepted: [false, Validators.requiredTrue]
    });
  }

  ngOnInit() {
    const session = this.sessionService.getSession();
    if (!session || !session.entranceId) {
      this.router.navigate(['/entrance']);
    }
  }

  onSubmit() {
    const session = this.sessionService.getSession();
    if (!session) return;

    this.apiService.acceptRules(
      session.sessionId,
      true
    ).subscribe({
      next: () => {
        this.sessionService.updateSession({
          visitorData: { ...session.visitorData, rulesAccepted: true },
          currentStep: 5
        });

        this.router.navigate(['/review']);
      },
      error: (error: any) => {
        console.error('Error accepting rules:', error);
      }
    });
  }

  goBack() {
    this.router.navigate(['/select-team']);
  }
}
