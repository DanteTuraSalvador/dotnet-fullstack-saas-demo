import { useLocation, useNavigate } from 'react-router-dom';
import type { SubmissionSummary } from '../types/subscription';

export function ConfirmationPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const summary = (location.state as { summary?: SubmissionSummary })?.summary;

  const badgeClass = (status: string | undefined) => {
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
  };

  return (
    <div className="mx-auto max-w-3xl space-y-6">
      <div className="rounded-2xl border border-emerald-200 bg-emerald-50/80 p-6 text-emerald-900 shadow-inner">
        <p className="text-xs font-semibold uppercase tracking-[0.25em] text-emerald-600">
          Submission received
        </p>
        <h1 className="mt-2 text-3xl font-semibold text-emerald-900">Thank you!</h1>
        <p className="mt-3 text-emerald-800">
          Your Azure SaaS subscription request has been successfully submitted. An administrator will
          review your request and contact you shortly.
        </p>
      </div>

      {summary ? (
        <div className="surface-card space-y-6">
          <div>
            <p className="text-sm font-semibold uppercase tracking-wide text-primary">
              Request details
            </p>
            <h2 className="text-2xl font-semibold text-slate-900">Submission summary</h2>
          </div>
          <dl className="grid gap-4 sm:grid-cols-2">
            <div className="rounded-2xl bg-slate-50/80 p-4">
              <dt className="text-xs font-semibold uppercase tracking-wide text-slate-500">
                Company Name
              </dt>
              <dd className="mt-2 text-base font-semibold text-slate-900">{summary.companyName}</dd>
            </div>
            <div className="rounded-2xl bg-slate-50/80 p-4">
              <dt className="text-xs font-semibold uppercase tracking-wide text-slate-500">
                Contact Email
              </dt>
              <dd className="mt-2 text-base font-semibold text-slate-900">
                {summary.contactEmail}
              </dd>
            </div>
            <div className="rounded-2xl bg-slate-50/80 p-4">
              <dt className="text-xs font-semibold uppercase tracking-wide text-slate-500">
                Contact Person
              </dt>
              <dd className="mt-2 text-base font-semibold text-slate-900">
                {summary.contactPerson}
              </dd>
            </div>
            <div className="rounded-2xl bg-slate-50/80 p-4">
              <dt className="text-xs font-semibold uppercase tracking-wide text-slate-500">
                Business Type
              </dt>
              <dd className="mt-2 text-base font-semibold text-slate-900">
                {summary.businessType}
              </dd>
            </div>
            <div className="rounded-2xl bg-slate-50/80 p-4 sm:col-span-2">
              <dt className="text-xs font-semibold uppercase tracking-wide text-slate-500">
                Status
              </dt>
              <dd className="mt-3">
                <span className={badgeClass(summary.status)}>{summary.status || 'Pending'}</span>
              </dd>
            </div>
          </dl>
        </div>
      ) : (
        <div className="rounded-2xl border border-slate-200 bg-white/80 p-5 text-slate-700">
          We could not load the request details. Please submit a new request to view the summary
          again.
        </div>
      )}

      <div className="flex flex-wrap gap-3">
        <button
          className="inline-flex items-center justify-center rounded-2xl bg-primary px-5 py-3 text-sm font-semibold text-white shadow-lg shadow-primary/30 transition hover:-translate-y-0.5"
          onClick={() => navigate('/')}
        >
          Submit Another Request
        </button>
        <button
          className="inline-flex items-center justify-center rounded-2xl border border-slate-200 px-5 py-3 text-sm font-semibold text-slate-600 transition hover:border-primary hover:text-primary"
          onClick={() => navigate('/status')}
        >
          Check Request Status
        </button>
      </div>
    </div>
  );
}
