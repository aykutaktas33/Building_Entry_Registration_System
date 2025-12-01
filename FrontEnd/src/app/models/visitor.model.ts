export interface Visitor {
  id?: string;
  sessionId: string;
  entranceId: string;
  name: string;
  email: string;
  company: string;
  teamId: string;
  teamName?: string;
  rulesAccepted: boolean;
  checkInTime?: Date;
  checkOutTime?: Date;
  createdAt?: Date;
  updatedAt?: Date;
}
