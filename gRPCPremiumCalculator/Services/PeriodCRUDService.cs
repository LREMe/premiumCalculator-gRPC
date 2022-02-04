namespace gRPCPremiumCalculator.Services
{
    using Grpc.Core;
    using System.Threading.Tasks;
    using Data = gRPCPremiumCalculator.Data;

    public class PeriodCRUDService: PeriodCRUD.PeriodCRUDBase
    {

        private Data.PremiumRolDbContext db = null;

        public PeriodCRUDService(Data.PremiumRolDbContext db) {
            this.db = db;
        }


        public override Task<Periods> PeriodList(Empty request, ServerCallContext context)
        {
            Periods responseData = new Periods();

            var query = from period in db.Periods
                        select new Period
                        {
                            IdPeriod = period.IdPeriod,
                            NamePeriod = period.NamePeriod,
                            Factor = period.Factor
                        };

            responseData.Items.AddRange(query.ToArray());

            return Task.FromResult(responseData);
            //turn base.PeriodList(request, context);
        }

    }
}
