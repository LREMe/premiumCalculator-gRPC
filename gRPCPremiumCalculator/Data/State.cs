using System.ComponentModel.DataAnnotations;

namespace gRPCPremiumCalculator.Data
{
    /// <summary>
    /// Class to map State
    /// </summary>
    public class State
    {

        [Key]
        public string? StateId { get; set; }
        public string? StateName { get; set; }
    }
}
