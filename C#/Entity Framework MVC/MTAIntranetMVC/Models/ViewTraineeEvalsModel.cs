namespace MTAIntranetMVC.Models
{
    using MTAIntranet.Shared;

    public class ViewTraineeEvalsModel
    {
        public List<TraineeEval>? TraineeEvals;
        public int EmployeeNum { get; set; }

        public ViewTraineeEvalsModel()
        {

        }
    }
}