import { Routes } from '@angular/router';
import { EntranceIdComponent } from './components/entrance-id/entrance-id.component';
import { PersonalInfoComponent } from './components/personal-info/personal-info.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/entrance',
    pathMatch: 'full'
  },
  {
    path: 'entrance',
    component: EntranceIdComponent
  },
  {
    path: 'personal-info',
    component: PersonalInfoComponent
  },
  {
    path: 'select-team',
    loadComponent: () => import('./components/select-team/select-team.component').then(m => m.SelectTeamComponent)
  },
  {
    path: 'accept-rules',
    loadComponent: () => import('./components/accept-rules/accept-rules.component').then(m => m.AcceptRulesComponent)
  },
  {
    path: 'review',
    loadComponent: () => import('./components/review/review.component').then(m => m.ReviewComponent)
  },
  {
    path: 'success',
    loadComponent: () => import('./components/success/success.component').then(m => m.SuccessComponent)
  },
  {
    path: '**',
    redirectTo: '/entrance'
  }
];
