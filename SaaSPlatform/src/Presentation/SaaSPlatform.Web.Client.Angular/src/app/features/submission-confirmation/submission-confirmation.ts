import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { SubmissionSummary } from '../../core/models/subscription';

@Component({
  selector: 'app-submission-confirmation',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './submission-confirmation.html',
  styleUrl: './submission-confirmation.scss'
})
export class SubmissionConfirmationComponent {
  private readonly router = inject(Router);
  private readonly summaryState = signal<SubmissionSummary | null>(this.extractSummary());

  readonly summary = computed(() => this.summaryState());
  readonly hasSummary = computed(() => this.summaryState() !== null);

  private extractSummary(): SubmissionSummary | null {
    const navigationState = this.router.getCurrentNavigation()?.extras?.state;
    const stateSummary = (navigationState?.['summary'] ?? window.history.state?.['summary']) as
      | SubmissionSummary
      | undefined;
    return stateSummary ?? null;
  }

  badgeClass(status: string | undefined): string {
    switch ((status ?? '').toLowerCase()) {
      case 'pending approval':
      case 'pending':
        return 'badge bg-warning text-dark';
      case 'approved':
        return 'badge bg-success';
      case 'deployed':
        return 'badge bg-info text-dark';
      case 'rejected':
        return 'badge bg-danger';
      default:
        return 'badge bg-secondary';
    }
  }
}
