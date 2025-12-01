import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SessionService } from '../../services/session.service';
import { ApiService } from '../../services/api.service'; // EKLENDİ

@Component({
  selector: 'app-personal-info',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './personal-info.component.html',
  styleUrls: ['./personal-info.component.css']
})
export class PersonalInfoComponent implements OnInit {
  personalForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private sessionService: SessionService,
    private apiService: ApiService // EKLENDİ
  ) {
    this.personalForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      company: ['', Validators.required]
    });
  }

  ngOnInit() {
    const session = this.sessionService.getSession();
    if (!session || !session.entranceId) {
      this.router.navigate(['/entrance']);
      return;
    }

    if (session.visitorData) {
      this.personalForm.patchValue(session.visitorData);
    }
  }

  onSubmit() {
    if (this.personalForm.invalid) {
      return;
    }

    const session = this.sessionService.getSession();
    if (!session) return;

    const formData = this.personalForm.value;

    // DÜZELTME: Parametreleri doğru şekilde gönder
    this.apiService.savePersonalInfo(
      session.sessionId,
      formData.name,
      formData.email,
      formData.company
    ).subscribe({
      next: () => {
        // Update session
        this.sessionService.updateSession({
          visitorData: { ...session.visitorData, ...formData },
          currentStep: 3
        });

        this.router.navigate(['/select-team']);
      },
      error: (error: any) => { // TİP EKLENDİ
        console.error('Error saving personal info:', error);
      }
    });
  }

  goBack() {
    this.router.navigate(['/entrance']);
  }
}
