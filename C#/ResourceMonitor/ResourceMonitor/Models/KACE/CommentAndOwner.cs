namespace ResourceMonitor.Models.KACE
{
    public class CommentOwnerAndSubmitter
    {
        public HdTicketChange Comment;
        public long Owner;
        public long Submitter;

        public CommentOwnerAndSubmitter()
        {
            Comment = new HdTicketChange();
            Owner = 0;
            Submitter = 0;
        }

        public CommentOwnerAndSubmitter(HdTicketChange comment, long owner, long submitter)
        {
            Comment = comment;
            Owner = owner;
            Submitter = submitter;
        }
    }

    
}
