import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'request',
    pathMatch: 'full'
  },
  {
    path: 'request',
    loadComponent: () =>
      import('./features/subscription-request/subscription-request').then(
        (m) => m.SubscriptionRequestComponent
      )
  },
  {
    path: 'status',
    loadComponent: () =>
      import('./features/subscription-status/subscription-status').then(
        (m) => m.SubscriptionStatusComponent
      )
  },
  {
    path: 'confirmation',
    loadComponent: () =>
      import('./features/submission-confirmation/submission-confirmation').then(
        (m) => m.SubmissionConfirmationComponent
      )
  },
  {
    path: '**',
    redirectTo: 'request'
  }
];
