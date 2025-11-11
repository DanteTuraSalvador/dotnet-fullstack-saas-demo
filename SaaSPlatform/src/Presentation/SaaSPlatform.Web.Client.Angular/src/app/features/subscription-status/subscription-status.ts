import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { SubscriptionResponse } from '../../core/models/subscription';
import { SubscriptionService } from '../../core/services/subscription';

@Component({
  selector: 'app-subscription-status',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './subscription-status.html',
  styleUrl: './subscription-status.scss'
})
export class SubscriptionStatusComponent {
  private readonly fb = inject(FormBuilder);
  private readonly subscriptionService = inject(SubscriptionService);

  readonly form = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]]
  });

  private readonly loading = signal(false);
  private readonly error = signal<string | null>(null);
  private readonly searchFlag = signal(false);
  private readonly results = signal<SubscriptionResponse[]>([]);

  readonly isLoading = computed(() => this.loading());
  readonly errorMessage = computed(() => this.error());
  readonly searchPerformed = computed(() => this.searchFlag());
  readonly subscriptions = computed(() => this.results());

  search(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const email = this.form.controls.email.value.trim();
    this.loading.set(true);
    this.error.set(null);
    this.searchFlag.set(true);

    this.subscriptionService
      .getSubscriptionsByEmail(email)
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (subscriptions) => this.results.set(subscriptions),
        error: () =>
          this.error.set('Unable to retrieve subscription requests at the moment. Please retry.')
      });
  }

  badgeClass(status: string | null | undefined): string {
    const base =
      'inline-flex items-center rounded-full px-3 py-1 text-xs font-semibold ring-1 ring-inset';

    switch ((status ?? '').toLowerCase()) {
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
