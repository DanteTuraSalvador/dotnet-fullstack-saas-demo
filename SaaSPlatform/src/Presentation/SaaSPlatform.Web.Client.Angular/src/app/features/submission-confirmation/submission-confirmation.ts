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
    const base =
      'inline-flex items-center rounded-full px-3 py-1 text-xs font-semibold ring-1 ring-inset';

    switch ((status ?? '').toLowerCase()) {
      case 'pending approval':
      case 'pending':
        return `${base} bg-amber-50 text-amber-700 ring-amber-200`;
      case 'approved':
        return `${base} bg-emerald-50 text-emerald-700 ring-emerald-200`;
      case 'deployed':
        return `${base} bg-sky-50 text-sky-700 ring-sky-200`;
      case 'rejected':
        return `${base} bg-rose-50 text-rose-700 ring-rose-200`;
      default:
        return `${base} bg-slate-100 text-slate-600 ring-slate-200`;
    }
  }
}
