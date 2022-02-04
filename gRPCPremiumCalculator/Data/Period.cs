using System.ComponentModel.DataAnnotations;

namespace gRPCPremiumCalculator.Data
{
    /// <summary>
    /// Class to map Period
    /// </summary>
    public class Period
    {

        [Key]
        public string? IdPeriod { get; set; }
        public string? NamePeriod { get; set; }
        public int Factor { get; set; }
    }
}
