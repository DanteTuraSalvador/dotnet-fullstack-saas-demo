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

  const badgeClass = (status: string) => {
    switch (status?.toLowerCase()) {
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
  };

  const handleSearch = async (event: FormEvent) => {
    event.preventDefault();
    if (!email) {
      setError('Please enter an email address.');
      return;
    }

    setIsLoading(true);
    setError(null);
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

  return (
    <div className="row justify-content-center">
      <div className="col-xl-10">
        <h1 className="mt-2">Subscription Request Status</h1>
        <p className="lead text-muted">Enter the email address used in your subscription request.</p>

        {error && (
          <div className="alert alert-danger" role="alert">
            {error}
          </div>
        )}

        <div className="card shadow-sm mb-4">
          <div className="card-body p-4">
            <form className="row g-3" onSubmit={handleSearch}>
              <div className="col-md-8">
                <label className="form-label" htmlFor="statusEmail">
                  Email Address
                </label>
                <input
                  id="statusEmail"
                  type="email"
                  className="form-control"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                />
              </div>
              <div className="col-md-4 d-flex align-items-end">
                <button type="submit" className="btn btn-primary w-100" disabled={isLoading}>
                  {isLoading ? 'Searching...' : 'Search'}
                </button>
              </div>
            </form>
          </div>
        </div>

        {searchPerformed && (
          <>
            {results.length > 0 ? (
              <div className="table-responsive shadow-sm rounded bg-white">
                <table className="table table-striped mb-0">
                  <thead>
                    <tr>
                      <th>Company</th>
                      <th>Contact Person</th>
                      <th>Business Type</th>
                      <th>Submitted Date</th>
                      <th>Status</th>
                    </tr>
                  </thead>
                  <tbody>
                    {results.map((subscription) => (
                      <tr key={subscription.id}>
                        <td>{subscription.companyName}</td>
                        <td>{subscription.contactPerson}</td>
                        <td>{subscription.businessType}</td>
                        <td>
                          {subscription.createdDate
                            ? new Date(subscription.createdDate).toLocaleDateString()
                            : '-'}
                        </td>
                        <td>
                          <span className={badgeClass(subscription.status)}>
                            {subscription.status}
                          </span>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            ) : (
              <div className="alert alert-info" role="alert">
                No subscription requests found for the provided email address.
              </div>
            )}
          </>
        )}
      </div>
    </div>
  );
}
