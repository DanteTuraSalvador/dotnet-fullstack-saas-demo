export interface CreateSubscriptionRequest {
  companyName: string;
  contactEmail: string;
  contactPerson: string;
  businessType: string;
}

export interface SubscriptionResponse {
  id: string;
  companyName: string;
  contactEmail: string;
  contactPerson: string;
  businessType: string;
  status: string;
  createdDate: string;
  azureResourceGroup?: string | null;
  webAppUrl?: string | null;
}

export interface SubmissionSummary {
  companyName: string;
  contactEmail: string;
  contactPerson: string;
  businessType: string;
  status: string;
}
