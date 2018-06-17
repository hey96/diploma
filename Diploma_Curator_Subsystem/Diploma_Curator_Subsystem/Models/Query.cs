using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma_Curator_Subsystem.Models
{
    [Table("Query")]
    public class Query
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("max_num_experts")]
        [Required]
        [Range(0, 15)]
        public int MaxNumExpert { get; set; }

        [Column("min_num_experts")]
        [Required]
        [Range(0, 15)]
        public int MinNumExpert { get; set; }

        [Column("min_competition_coef", TypeName = "decimal(4, 3)")]
        [DisplayFormat(NullDisplayText = "Не задан")]
        public decimal ? MinCompetitionCoef { get; set; }

        [Column("avg_competition_coef", TypeName = "decimal(4, 3)")]
        [DisplayFormat(NullDisplayText = "Не задан")]
        public decimal ? AvgCompetitionCoef { get; set; }

        [Column("required_date")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RequiredDate { get; set; }

        [Column("step")]
        [Range(0, 10)]
        [DisplayFormat(NullDisplayText = "Не задан")]
        public int ? Step { get; set; }

        [Column("last_date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true, NullDisplayText = "Не задана")]
        public DateTime ? LastDate { get; set; }

        [Column("result")]
        [DisplayFormat(NullDisplayText = "Не вычислен")]
        public string Result { get; set; }

        public int TaskID { get; set; }
        public Task Task { get; set; }
    }
}
