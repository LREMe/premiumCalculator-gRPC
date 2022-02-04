namespace gRPCPremiumCalculator.Data
{
    /// <summary>
    /// Claas to get parameters from the client
    /// </summary>

    public class PremiumQ
    {
        public DateTime DoB { get; set; }

        public string? State { get; set; }


        public int Age { get; set; }


        public string? Plan { get; set; }
    }
}
