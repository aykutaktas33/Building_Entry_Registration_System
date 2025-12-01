import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterModule, Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class AppComponent {
  currentStep: number = 1;
  showHeader: boolean = true;
  
  constructor(private router: Router) {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: any) => {
      this.updateStep(event.url);
    });
  }
  
  private updateStep(url: string) {
    const routes: { [key: string]: number } = {
      '/entrance': 1,
      '/personal-info': 2,
      '/select-team': 3,
      '/accept-rules': 4,
      '/review': 5,
      '/success': 6
    };
    
    this.currentStep = routes[url] || 1;
    this.showHeader = url !== '/success';
  }
}