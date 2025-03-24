export interface HdTicketChangeDTO {
  id: number;
  hdTicketId: number | undefined;
  userId: number | undefined;
  timestamp: string;
  comment: string | undefined;
  description: string | undefined;
  ownersOnly: boolean;

  userName: string | undefined;
}
