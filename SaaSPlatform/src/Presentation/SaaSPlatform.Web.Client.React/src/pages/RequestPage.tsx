import { useState } from 'react';
import type { FormEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import type {
  CreateSubscriptionRequest,
  SubmissionSummary
} from '../types/subscription';
import { createSubscription } from '../services/subscriptionService';

const businessTypes = [
  'Technology',
  'Finance',
  'Healthcare',
  'Education',
  'Retail',
  'Manufacturing',
  'Other'
];

const initialForm: CreateSubscriptionRequest = {
  companyName: '',
  contactEmail: '',
  contactPerson: '',
  businessType: ''
};

export function RequestPage() {
  const [form, setForm] = useState<CreateSubscriptionRequest>(initialForm);
  const [error, setError] = useState<string | null>(null);
  const [submitting, setSubmitting] = useState(false);
  const navigate = useNavigate();

  const updateField = (field: keyof CreateSubscriptionRequest, value: string) => {
    setForm((prev) => ({ ...prev, [field]: value }));
  };

  const handleSubmit = async (event: FormEvent) => {
    event.preventDefault();

    if (!form.companyName || !form.contactEmail || !form.contactPerson || !form.businessType) {
      setError('Please complete all required fields.');
      return;
    }

    setSubmitting(true);
    setError(null);

    try {
      await createSubscription(form);
      const summary: SubmissionSummary = {
        ...form,
        status: 'Pending Approval'
      };
      navigate('/confirmation', { state: { summary } });
      setForm(initialForm);
    } catch (err) {
      setError('Unable to submit your request right now. Please try again later.');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="row justify-content-center">
      <div className="col-xl-8">
        <h1 className="mt-2">Azure SaaS Subscription Request</h1>
        <p className="lead text-muted">
          Fill out this form to request Azure infrastructure provisioning for your organization.
        </p>

        {error && (
          <div className="alert alert-danger" role="alert">
            {error}
          </div>
        )}

        <div className="card shadow-sm">
          <div className="card-body p-4">
            <form onSubmit={handleSubmit} noValidate>
              <div className="mb-3">
                <label className="form-label" htmlFor="companyName">
                  Company Name
                </label>
                <input
                  id="companyName"
                  type="text"
                  className="form-control"
                  value={form.companyName}
                  onChange={(e) => updateField('companyName', e.target.value)}
                  required
                />
              </div>

              <div className="mb-3">
                <label className="form-label" htmlFor="contactEmail">
                  Contact Email
                </label>
                <input
                  id="contactEmail"
                  type="email"
                  className="form-control"
                  value={form.contactEmail}
                  onChange={(e) => updateField('contactEmail', e.target.value)}
                  required
                />
              </div>

              <div className="mb-3">
                <label className="form-label" htmlFor="contactPerson">
                  Contact Person
                </label>
                <input
                  id="contactPerson"
                  type="text"
                  className="form-control"
                  value={form.contactPerson}
                  onChange={(e) => updateField('contactPerson', e.target.value)}
                  required
                />
              </div>

              <div className="mb-4">
                <label className="form-label" htmlFor="businessType">
                  Business Type
                </label>
                <select
                  id="businessType"
                  className="form-select"
                  value={form.businessType}
                  onChange={(e) => updateField('businessType', e.target.value)}
                  required
                >
                  <option value="">Select business type</option>
                  {businessTypes.map((type) => (
                    <option key={type}>{type}</option>
                  ))}
                </select>
              </div>

              <div className="d-flex gap-3 flex-wrap">
                <button type="submit" className="btn btn-primary" disabled={submitting}>
                  {submitting ? 'Submitting...' : 'Submit Subscription Request'}
                </button>
                <button
                  type="button"
                  className="btn btn-outline-secondary"
                  onClick={() => navigate('/status')}
                >
                  Check Request Status
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
}
