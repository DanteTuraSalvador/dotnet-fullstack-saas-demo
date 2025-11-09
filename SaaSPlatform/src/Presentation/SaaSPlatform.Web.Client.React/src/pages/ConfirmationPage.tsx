import { useLocation, useNavigate } from 'react-router-dom';
import type { SubmissionSummary } from '../types/subscription';

export function ConfirmationPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const summary = (location.state as { summary?: SubmissionSummary })?.summary;

  return (
    <div className="row justify-content-center">
      <div className="col-xl-7">
        <div className="alert alert-success shadow-sm" role="alert">
          <h2 className="alert-heading">Thank you!</h2>
          <p>Your Azure SaaS subscription request has been successfully submitted.</p>
          <p className="mb-0">An administrator will review your request and contact you shortly.</p>
        </div>

        {summary ? (
          <div className="card shadow-sm">
            <div className="card-header">
              <h5 className="mb-0">Request Details</h5>
            </div>
            <div className="card-body">
              <p>
                <strong>Company Name:</strong> {summary.companyName}
              </p>
              <p>
                <strong>Contact Email:</strong> {summary.contactEmail}
              </p>
              <p>
                <strong>Contact Person:</strong> {summary.contactPerson}
              </p>
              <p>
                <strong>Business Type:</strong> {summary.businessType}
              </p>
              <p>
                <strong>Status:</strong>{' '}
                <span className="badge bg-warning text-dark">{summary.status}</span>
              </p>
            </div>
          </div>
        ) : (
          <div className="alert alert-info">
            We could not load the request details. Please submit a new request to view the summary
            again.
          </div>
        )}

        <div className="d-flex gap-3 mt-4 flex-wrap">
          <button className="btn btn-primary" onClick={() => navigate('/')}>
            Submit Another Request
          </button>
          <button className="btn btn-outline-secondary" onClick={() => navigate('/status')}>
            Check Request Status
          </button>
        </div>
      </div>
    </div>
  );
}
