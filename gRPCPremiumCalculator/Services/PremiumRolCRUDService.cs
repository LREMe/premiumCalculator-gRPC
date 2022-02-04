namespace gRPCPremiumCalculator.Services
{
    using Grpc.Core;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Data = gRPCPremiumCalculator.Data;
    public class PremiumRolCRUDService:PremiumRolCRUD.PremiumRolCRUDBase
    {
        private Data.PremiumRolDbContext db = null;
        private readonly ILogger<PremiumRolCRUDService> _logger;
        public PremiumRolCRUDService(Data.PremiumRolDbContext db, ILogger<PremiumRolCRUDService> logger)
        {
            this.db = db;
            this._logger = logger;
        }


        public override Task<PremiumRols> PremiumList(PremiumQ request, ServerCallContext context)
        {
            // return base.PremiumList(request, context);

            string month = "";
            var today = DateTime.Today;
            // Calculate the age.
            
            var age = today.Year - request.DoB.ToDateTime().Year;
            //not the same agen
            if (age != request.Age)
            {
                //  return Task<PremiumRols>.FromResult(new PremiumRols { new PremiumRol { Age = "0", Carrier = "", MonthOfBirth = "", Plan = "", Premium = 0, PremiumRolID = 0, State = "" } });
                //return Task.FromCanceled()

                //                return Task<PremiumRols>.FromCanceled()
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;
                tokenSource.Cancel();
                return (Task<PremiumRols>)Task.FromCanceled(token);
            }

            //get the month
            CultureInfo ci = new CultureInfo("en-US");
            month = request.DoB.ToDateTime().ToString("MMMM", ci);

            try
            {
                PremiumRols responseData = new PremiumRols();
                //Usage of Entity framework to make it independent from the DB
                //If I was going to use a defined database, I will use stored procedures to allow the database to optimize the query.
                var select = from premiumRol in db.PremiumRols
                             where (premiumRol.MonthOfBirth.Equals(month) || premiumRol.MonthOfBirth.Equals("*"))
                              && (premiumRol.State.Equals(request.State) || premiumRol.State.Equals("*"))
                              && (premiumRol.Plan.Contains(request.Plan) || premiumRol.Plan.Equals("*"))
                              && (
                                      (
                                      premiumRol.Age.Contains("-")
                                      &&
                                      String.Compare(request.Age.ToString(), premiumRol.Age.Substring(0, premiumRol.Age.IndexOf("-"))) >= 0
                                      &&
                                      String.Compare(request.Age.ToString(), premiumRol.Age.Substring(premiumRol.Age.IndexOf("-") + 1)) <= 0
                                      )
                                      ||
                                      (
                                      premiumRol.Age.EndsWith("+")
                                      &&
                                      String.Compare(request.Age.ToString(), premiumRol.Age.Substring(0, premiumRol.Age.IndexOf("+"))) >= 0
                                      )
                              )
                             select new PremiumRolResult
                             {
                                 Carrier = premiumRol.Carrier,
                                 Premium = premiumRol.Premium

                             };
                _logger.LogInformation("Premium success!");

                //<PremiumRolResult>

                responseData.Items.AddRange(select.ToArray());
                //return Results.Ok(await select.ToListAsync());
                return Task.FromResult(responseData);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Premium Exception");
                // return Results.NotFound();
                return (Task<PremiumRols>)Task.FromException(e);
            }

        }

        /*public override Task<PremiumRol> PremiumList(EmptyPremiumRolParam request, ServerCallContext context)
        {
            return base.PremiumList(request, context);
        }*/

    }
}
