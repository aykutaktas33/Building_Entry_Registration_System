import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SessionService } from '../../services/session.service';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-entrance-id',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './entrance-id.component.html',
  styleUrls: ['./entrance-id.component.css']
})
export class EntranceIdComponent implements OnInit {
  entranceForm: FormGroup;
  errorMessage: string = '';
  isLoading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private sessionService: SessionService,
    private apiService: ApiService
  ) {
    this.entranceForm = this.fb.group({
      entranceId: ['', [Validators.required, Validators.minLength(3)]]
    });
  }

  ngOnInit() {
    const urlParams = new URLSearchParams(window.location.search);
    const qrCode = urlParams.get('qr');

    if (qrCode) {
      this.entranceForm.patchValue({ entranceId: qrCode });
      this.onSubmit();
    }
  }

  onSubmit() {
    if (this.entranceForm.invalid) {
      return;
    }

    this.isLoading = true;
    const entranceId = this.entranceForm.value.entranceId;

    let session = this.sessionService.getSession();
    if (!session) {
      const sessionId = this.sessionService.createSession();
      session = this.sessionService.getSession();
    }

    this.apiService.validateEntrance(entranceId, session!.sessionId).subscribe({
      next: (response) => {
        // API başarılı yanıt verirse
        this.sessionService.updateSession({
          entranceId,
          currentStep: 2
        });
        this.router.navigate(['/personal-info']);
      },
      error: (error) => {
        this.errorMessage = 'Invalid entrance ID. Please try again.';
        this.isLoading = false;
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }
}
