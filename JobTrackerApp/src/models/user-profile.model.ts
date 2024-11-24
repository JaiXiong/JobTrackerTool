import { JobProfile } from "./job-profile.model";

export interface UserProfile {
    id: string;
    date: Date;
    latestUpdate: Date;
    jobProfiles: JobProfile[];
    name: string;
    email: string;
    phone: string;
    address: string;
    city: string;
    state: string;
    zip: string
}
