import { EmployerProfile } from './employer-profile.model';

export interface JobProfile {
  id: string;
  userProfileId: string;
  date: Date;
  latestUpdate: Date;
  profileName: string;
  employerProfiles: EmployerProfile[];
}