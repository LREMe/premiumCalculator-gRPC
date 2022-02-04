using System.ComponentModel.DataAnnotations;

namespace gRPCPremiumCalculator.Data
{
    /// <summary>
    /// Class to map Plan
    /// </summary>
    public class Plan
    {
        [Key]
        public string? PlanId { get; set; }
        public string? PlanName { get; set; }
    }
}
