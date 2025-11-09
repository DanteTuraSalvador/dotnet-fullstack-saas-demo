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
    switch ((status ?? '').toLowerCase()) {
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
