namespace MTAIntranetMVC.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using MTAIntranet.Shared;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TraineeEvalModel
    {
        [Key]
        public int EvalID { get; set; }

        [Column(TypeName = "int")]
        public int EmployeeNumber { get; set; }

        [Column(TypeName = "nvarchar (50)")]
        public string? EmployeeFirstName { get; set; }

        [Column(TypeName = "nvarchar (50)")]
        public string? EmployeeLastName { get; set; }

        [Column(TypeName = "nvarchar (50)")]
        public string? Department { get; set; }
        public DateTime Date { get; set; }

        [Column(TypeName = "nvarchar (3)")]
        [MaxLength(3)]
        public string? VehicleType { get; set; }

        [Column(TypeName = "nvarchar (50)")]
        public string? DIName { get; set; }

        [Column(TypeName = "int")]
        public int PrePostTrips { get; set; }

        [Column(TypeName = "int")]
        public int RadioEtiquette { get; set; }

        [Column(TypeName = "int")]
        public int CustomerService { get; set; }

        [Column(TypeName = "int")]
        public int DefensiveDriving { get; set; }

        [Column(TypeName = "int")]
        public int RRCrossings { get; set; }

        [Column(TypeName = "int")]
        public int WheelChairSecurements { get; set; }

        [Column(TypeName = "nvarchar (255)")]
        public string? Comments { get; set; }


        public TraineeEvalModel()
        {

        }
    }
}