import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { finalize } from 'rxjs';
import {
  CreateSubscriptionRequest,
  SubmissionSummary
} from '../../core/models/subscription';
import { SubscriptionService } from '../../core/services/subscription';

@Component({
  selector: 'app-subscription-request',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './subscription-request.html',
  styleUrl: './subscription-request.scss'
})
export class SubscriptionRequestComponent {
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly subscriptionService = inject(SubscriptionService);

  readonly businessTypes = [
    'Technology',
    'Finance',
    'Healthcare',
    'Education',
    'Retail',
    'Manufacturing',
    'Other'
  ];

  readonly form = this.fb.nonNullable.group({
    companyName: ['', [Validators.required, Validators.maxLength(200)]],
    contactEmail: ['', [Validators.required, Validators.email]],
    contactPerson: ['', [Validators.required, Validators.maxLength(200)]],
    businessType: ['', Validators.required]
  });

  private readonly pending = signal(false);
  private readonly error = signal<string | null>(null);

  readonly isSubmitting = computed(() => this.pending());
  readonly errorMessage = computed(() => this.error());

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const payload = this.form.getRawValue() as CreateSubscriptionRequest;
    this.pending.set(true);
    this.error.set(null);

    this.subscriptionService
      .createSubscription(payload)
      .pipe(finalize(() => this.pending.set(false)))
      .subscribe({
        next: () => this.navigateToConfirmation(payload),
        error: () =>
          this.error.set('Unable to submit your request right now. Please try again later.')
      });
  }

  private navigateToConfirmation(payload: CreateSubscriptionRequest): void {
    const summary: SubmissionSummary = {
      companyName: payload.companyName,
      contactEmail: payload.contactEmail,
      contactPerson: payload.contactPerson,
      businessType: payload.businessType,
      status: 'Pending Approval'
    };

    this.router.navigate(['/confirmation'], { state: { summary } });
  }

  fieldInvalid(controlName: keyof CreateSubscriptionRequest): boolean {
    const control = this.form.get(controlName);
    return !!control && control.invalid && (control.dirty || control.touched);
  }
}
