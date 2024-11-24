export interface EmployerProfile {
    id: string;
    date: Date;
    latestUpdate: Date;
    jobProfileId: string;
    name: string;
    title: string;
    address: string;
    city: string;
    state: string;
    zip: string;
    phone: string;
    email: string;
    website: string;
    jobAction: ActionResult;
    details: Details;
  }
  
  export interface ActionResult {
    id: string;
    employerProfileId: string;
    date: string;
    latestUpdate: string;
    action: string;
    method: string;
    actionResult: string;
  }
  
  export interface Details {
    id: string;
    employerProfileId: string;
    date: string;
    latestUpdate: string;
    comments: string;
    updates: string;
  }