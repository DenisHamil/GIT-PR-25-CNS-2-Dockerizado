using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Models
{
    public class Vote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdVote { get; set; }
        public string StatusVote { get; set; }
        public int IdUser { get; set; }
        public int IdRequest { get; set; }
        public decimal PointVote { get; set; }
        public string? Administration { get; set; }
        public string? doses { get; set; }
        public string? Recommendation { get; set; }
    }
}
