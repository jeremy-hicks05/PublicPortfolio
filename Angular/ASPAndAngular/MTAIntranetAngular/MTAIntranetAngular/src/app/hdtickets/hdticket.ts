export interface HdTicket {
  //id: number;
  //title: string;
  //created: string;
  //owner_id: number;
  //hdQueueId: number;
  //submitter_id: number;
  //summary: string;
  //category_id: number;
  //status_id: number;
  //impact_id: number;
  //priority_id: number;
  //machine_id: number;

  id: number;
  title: string;
  created: string;
  ownerId: number;
  hdQueueId: number;
  submitterId: number;
  summary: string;
  hdCategoryId: number;
  hdStatusId: number;
  hdImpactId: number;
  hdPriorityId: number;
  deviceId: number;

  /*customFieldValue0: string | undefined;*/
  customFieldValue0: string | undefined;
  customFieldValue1: string | undefined;
  customFieldValue2: string | undefined;
  customFieldValue3: string | undefined;
  customFieldValue4: string | undefined;
  customFieldValue5: string | undefined;
  customFieldValue6: string | undefined;
  customFieldValue7: string | undefined;
  customFieldValue8: string | undefined;
  customFieldValue9: string | undefined;
  customFieldValue10: string | undefined;
  customFieldValue11: string | undefined;
  customFieldValue12: string | undefined;
  customFieldValue13: string | undefined;
  customFieldValue14: string | undefined;
  customFieldValue15: string | undefined;

  approverId: number | undefined;
  approveState: string | undefined;
  approval: string | undefined;
  approvalNote: string | undefined;

  dueDate: string; // yyyy-mm-dd HH:mm:ss
  dueDateYear: string;
  dueDateMonth: string;
  dueDateDay: string;
  dueDateHour: string;

  firstName: string;
  lastName: string;

  userToMirror: string;
}
