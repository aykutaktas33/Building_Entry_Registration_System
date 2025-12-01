import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SessionService } from '../../services/session.service';
import { ApiService } from '../../services/api.service'; // EKLENDİ
import { Team } from '../../models/team.model';

@Component({
  selector: 'app-select-team',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './select-team.component.html',
  styleUrls: ['./select-team.component.css']
})
export class SelectTeamComponent implements OnInit {
  teamForm: FormGroup;
  teams: Team[] = [];
  isLoading: boolean = true;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private sessionService: SessionService,
    private apiService: ApiService // EKLENDİ
  ) {
    this.teamForm = this.fb.group({
      teamId: ['', Validators.required]
    });
  }

  ngOnInit() {
    const session = this.sessionService.getSession();
    if (!session || !session.entranceId) {
      this.router.navigate(['/entrance']);
      return;
    }

    // Load teams from API
    this.apiService.getTeams().subscribe({
      next: (teams) => {
        this.teams = teams;
        this.isLoading = false;

        // Pre-select if returning
        if (session.visitorData?.teamId) {
          this.teamForm.patchValue({ teamId: session.visitorData.teamId });
        }
      },
      error: (error: any) => {
        console.error('Error loading teams:', error);
        this.isLoading = false;
      }
    });
  }

  onSubmit() {
    if (this.teamForm.invalid) {
      return;
    }

    const session = this.sessionService.getSession();
    if (!session) return;

    this.apiService.selectTeam(
      session.sessionId,
      this.teamForm.value.teamId
    ).subscribe({
      next: () => {
        // Update session
        this.sessionService.updateSession({
          visitorData: {
            ...session.visitorData,
            teamId: this.teamForm.value.teamId
          },
          currentStep: 4
        });

        this.router.navigate(['/accept-rules']);
      },
      error: (error: any) => {
        console.error('Error selecting team:', error);
      }
    });
  }

  goBack() {
    this.router.navigate(['/personal-info']);
  }
}
