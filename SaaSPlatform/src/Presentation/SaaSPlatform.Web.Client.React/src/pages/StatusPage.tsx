import { useState } from 'react';
import type { FormEvent } from 'react';
import { getSubscriptionsByEmail } from '../services/subscriptionService';
import type { SubscriptionResponse } from '../types/subscription';

export function StatusPage() {
  const [email, setEmail] = useState('');
  const [searchPerformed, setSearchPerformed] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [results, setResults] = useState<SubscriptionResponse[]>([]);
  const [showValidation, setShowValidation] = useState(false);

  const badgeClass = (status: string | null | undefined) => {
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
  };

  const handleSearch = async (event: FormEvent) => {
    event.preventDefault();
    if (!email.trim()) {
      setShowValidation(true);
      setError('Please enter an email address.');
      return;
    }

    setIsLoading(true);
    setError(null);
    setShowValidation(false);
    setSearchPerformed(true);

    try {
      const subscriptions = await getSubscriptionsByEmail(email);
      setResults(subscriptions);
    } catch {
      setError('Unable to retrieve subscription requests at the moment. Please retry.');
    } finally {
      setIsLoading(false);
    }
  };

  const inputInvalid = showValidation && !email.trim();

  return (
    <div className="mx-auto max-w-4xl space-y-6">
      <div className="space-y-2 text-center">
        <p className="text-sm font-semibold uppercase tracking-wide text-primary">Track requests</p>
        <h1 className="section-heading">Subscription Request Status</h1>
        <p className="section-subheading">
          Enter the email address used in your subscription request.
        </p>
      </div>

      {error && (
        <div className="rounded-2xl border border-rose-200 bg-rose-50/80 p-4 text-rose-700">
          {error}
        </div>
      )}

      <div className="surface-card">
        <form className="grid gap-4 md:grid-cols-3" onSubmit={handleSearch} noValidate>
          <div className="md:col-span-2">
            <label className="block text-sm font-semibold text-slate-600" htmlFor="statusEmail">
              Email Address
            </label>
            <input
              id="statusEmail"
              type="email"
              className={`mt-1 block w-full rounded-2xl border border-slate-200 bg-white/70 px-4 py-3 text-slate-900 shadow-sm transition focus:border-primary focus:ring-2 focus:ring-primary/40 ${
                inputInvalid ? 'border-rose-400 ring-rose-200' : ''
              }`}
              value={email}
              onChange={(e) => {
                setEmail(e.target.value);
                setShowValidation(false);
                setError(null);
              }}
            />
            {inputInvalid && (
              <p className="mt-2 text-sm text-rose-600">A valid email address is required.</p>
            )}
          </div>
          <div className="md:self-end">
            <button
              type="submit"
              className="h-full w-full rounded-2xl bg-primary px-5 py-3 text-sm font-semibold text-white shadow-lg shadow-primary/30 transition disabled:opacity-60"
              disabled={isLoading}
            >
              {isLoading ? 'Searching...' : 'Search'}
            </button>
          </div>
        </form>
      </div>

      {searchPerformed && (
        <>
          {results.length > 0 ? (
            <div className="overflow-hidden rounded-2xl border border-slate-100 bg-white shadow-xl shadow-slate-900/5">
              <table className="min-w-full divide-y divide-slate-100 text-sm">
                <thead className="bg-slate-50 text-left font-semibold text-slate-600">
                  <tr>
                    <th className="px-6 py-3">Company</th>
                    <th className="px-6 py-3">Contact Person</th>
                    <th className="px-6 py-3">Business Type</th>
                    <th className="px-6 py-3">Submitted Date</th>
                    <th className="px-6 py-3">Status</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-100 bg-white">
                  {results.map((subscription) => (
                    <tr key={subscription.id} className="text-slate-700">
                      <td className="px-6 py-4 font-semibold text-slate-900">
                        {subscription.companyName}
                      </td>
                      <td className="px-6 py-4">{subscription.contactPerson}</td>
                      <td className="px-6 py-4">{subscription.businessType}</td>
                      <td className="px-6 py-4">
                        {subscription.createdDate
                          ? new Date(subscription.createdDate).toLocaleDateString()
                          : '-'}
                      </td>
                      <td className="px-6 py-4">
                        <span className={badgeClass(subscription.status)}>
                          {subscription.status || 'Pending'}
                        </span>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          ) : (
            <div className="rounded-2xl border border-slate-200 bg-white/80 p-4 text-slate-600">
              No subscription requests found for the provided email address.
            </div>
          )}
        </>
      )}
    </div>
  );
}
