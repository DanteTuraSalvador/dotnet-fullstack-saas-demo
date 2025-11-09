import type {
  CreateSubscriptionRequest,
  SubscriptionResponse
} from '../types/subscription';

const DEFAULT_API_BASE = 'https://localhost:7264';
const API_BASE_URL = (import.meta.env.VITE_API_BASE_URL as string) ?? DEFAULT_API_BASE;

async function handleResponse<T>(response: Response): Promise<T> {
  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Request failed with status ${response.status}`);
  }

  return (await response.json()) as T;
}

export async function createSubscription(
  payload: CreateSubscriptionRequest
): Promise<void> {
  const response = await fetch(`${API_BASE_URL}/api/subscriptions`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  });

  await handleResponse(response);
}

export async function getSubscriptionsByEmail(
  email: string
): Promise<SubscriptionResponse[]> {
  const response = await fetch(`${API_BASE_URL}/api/subscriptions`);
  const subscriptions = await handleResponse<SubscriptionResponse[]>(response);

  return subscriptions.filter((subscription) =>
    subscription.contactEmail?.toLowerCase().includes(email.toLowerCase())
  );
}
