import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable, shareReplay } from 'rxjs';
import {
  CreateSubscriptionRequest,
  SubscriptionResponse
} from '../models/subscription';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SubscriptionService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = environment.apiBaseUrl.replace(/\/$/, '');

  createSubscription(request: CreateSubscriptionRequest): Observable<void> {
    return this.http.post<void>(`${this.apiBaseUrl}/api/subscriptions`, request);
  }

  /**
   * The API returns all subscriptions, so we filter client-side to mirror the Razor app.
   */
  getSubscriptionsByEmail(email: string): Observable<SubscriptionResponse[]> {
    return this.http
      .get<SubscriptionResponse[]>(`${this.apiBaseUrl}/api/subscriptions`)
      .pipe(
        map((subscriptions) =>
          (subscriptions ?? []).filter((subscription) =>
            subscription.contactEmail?.toLowerCase().includes(email.toLowerCase())
          )
        ),
        shareReplay(1)
      );
  }
}
