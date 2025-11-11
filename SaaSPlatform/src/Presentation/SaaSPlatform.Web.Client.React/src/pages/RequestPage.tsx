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
  const [showValidation, setShowValidation] = useState(false);
  const navigate = useNavigate();

  const updateField = (field: keyof CreateSubscriptionRequest, value: string) => {
    setForm((prev) => ({ ...prev, [field]: value }));
    setShowValidation(false);
    setError(null);
  };

  const fieldInvalid = (field: keyof CreateSubscriptionRequest) =>
    showValidation && !form[field].trim();

  const handleSubmit = async (event: FormEvent) => {
    event.preventDefault();

    if (!form.companyName || !form.contactEmail || !form.contactPerson || !form.businessType) {
      setShowValidation(true);
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
      setShowValidation(false);
    } catch (err) {
      setError('Unable to submit your request right now. Please try again later.');
    } finally {
      setSubmitting(false);
    }
  };

  const inputBaseClass =
    'mt-1 block w-full rounded-2xl border border-slate-200 bg-white/70 px-4 py-3 text-slate-900 shadow-sm transition focus:border-primary focus:ring-2 focus:ring-primary/40';

  return (
    <div className="mx-auto max-w-3xl space-y-6">
      <div className="space-y-2 text-center">
        <p className="text-xs font-semibold uppercase tracking-[0.3em] text-primary">
          Azure SaaS
        </p>
        <h1 className="section-heading">Subscription Request</h1>
        <p className="section-subheading">
          Fill out this form to request Azure infrastructure provisioning for your organization.
        </p>
      </div>

      {error && (
        <div className="rounded-2xl border border-rose-200 bg-rose-50/80 p-4 text-rose-700">
          {error}
        </div>
      )}

      <div className="surface-card">
        <form className="space-y-5" onSubmit={handleSubmit} noValidate>
          <div>
            <label className="block text-sm font-semibold text-slate-600" htmlFor="companyName">
              Company Name
            </label>
            <input
              id="companyName"
              type="text"
              className={`${inputBaseClass} ${
                fieldInvalid('companyName') ? 'border-rose-400 ring-rose-200' : ''
              }`}
              value={form.companyName}
              onChange={(e) => updateField('companyName', e.target.value)}
              required
            />
            {fieldInvalid('companyName') && (
              <p className="mt-2 text-sm text-rose-600">Company name is required.</p>
            )}
          </div>

          <div>
            <label className="block text-sm font-semibold text-slate-600" htmlFor="contactEmail">
              Contact Email
            </label>
            <input
              id="contactEmail"
              type="email"
              className={`${inputBaseClass} ${
                fieldInvalid('contactEmail') ? 'border-rose-400 ring-rose-200' : ''
              }`}
              value={form.contactEmail}
              onChange={(e) => updateField('contactEmail', e.target.value)}
              required
            />
            {fieldInvalid('contactEmail') && (
              <p className="mt-2 text-sm text-rose-600">Contact email is required.</p>
            )}
          </div>

          <div>
            <label className="block text-sm font-semibold text-slate-600" htmlFor="contactPerson">
              Contact Person
            </label>
            <input
              id="contactPerson"
              type="text"
              className={`${inputBaseClass} ${
                fieldInvalid('contactPerson') ? 'border-rose-400 ring-rose-200' : ''
              }`}
              value={form.contactPerson}
              onChange={(e) => updateField('contactPerson', e.target.value)}
              required
            />
            {fieldInvalid('contactPerson') && (
              <p className="mt-2 text-sm text-rose-600">Contact person is required.</p>
            )}
          </div>

          <div>
            <label className="block text-sm font-semibold text-slate-600" htmlFor="businessType">
              Business Type
            </label>
            <select
              id="businessType"
              className={`${inputBaseClass} ${
                fieldInvalid('businessType') ? 'border-rose-400 ring-rose-200' : ''
              }`}
              value={form.businessType}
              onChange={(e) => updateField('businessType', e.target.value)}
              required
            >
              <option value="">Select business type</option>
              {businessTypes.map((type) => (
                <option key={type}>{type}</option>
              ))}
            </select>
            {fieldInvalid('businessType') && (
              <p className="mt-2 text-sm text-rose-600">Please select a business type.</p>
            )}
          </div>

          <div className="flex flex-wrap items-center gap-3">
            <button
              type="submit"
              className="inline-flex items-center justify-center rounded-2xl bg-primary px-5 py-3 text-sm font-semibold text-white shadow-lg shadow-primary/30 transition disabled:opacity-60"
              disabled={submitting}
            >
              {submitting && (
                <span className="mr-2 h-4 w-4 animate-spin rounded-full border-2 border-white border-t-transparent" />
              )}
              Submit Subscription Request
            </button>
            <button
              type="button"
              className="rounded-2xl border border-slate-200 px-5 py-3 text-sm font-semibold text-slate-600 transition hover:border-primary hover:text-primary"
              onClick={() => navigate('/status')}
            >
              Check Request Status
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
