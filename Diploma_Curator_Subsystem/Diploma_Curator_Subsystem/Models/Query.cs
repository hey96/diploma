using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma_Curator_Subsystem.Models
{
    public class Query
    {
        public int ID { get; set; }

        [Required]
        [Range(0, 15)]
        public int MaxNumExpert { get; set; }

        [Range(0, 15)]
        public int MinNumExpert { get; set; }

        [Column(TypeName = "decimal(4, 3)")]
        [DisplayFormat(NullDisplayText = "Не задан")]
        public decimal ? MinCompetitionCoef { get; set; }

        [Column(TypeName = "decimal(4, 3)")]
        [DisplayFormat(NullDisplayText = "Не задан")]
        public decimal ? AvgCompetitionCoef { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RequiredDate { get; set; }

        [Required]
        [Range(0, 10)]
        public int Step { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; }

        public string Result { get; set; }

        public int TaskID { get; set; }
        public Task Task { get; set; }
    }
}
