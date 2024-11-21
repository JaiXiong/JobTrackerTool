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
    jobaction: ActionResult;
    details: Details;
  }
  
  export interface ActionResult {
    id: string;
    employerprofileid: string;
    date: string;
    latestUpdate: string;
    action: string;
    method: string;
    actionresult: string;
  }
  
  export interface Details {
    id: string;
    employerprofileid: string;
    date: string;
    latestUpdate: string;
    comments: string;
    updates: string;
  }