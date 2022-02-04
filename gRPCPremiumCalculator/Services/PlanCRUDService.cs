namespace gRPCPremiumCalculator.Services
{
    using Grpc.Core;
    using System.Threading.Tasks;
    using Data = gRPCPremiumCalculator.Data;

    public class PlanCRUDService : PlanCRUD.PlanCRUDBase
    {

        private Data.PremiumRolDbContext db = null;

        public PlanCRUDService(Data.PremiumRolDbContext db)
        {
            this.db = db;
        }

        public override Task<Plans> PlanList(EmptyPlanParam request, ServerCallContext context)
        {
            Plans responseData = new Plans();

            var query = from plan in db.Plans
                        select new Plan
                        {
                            PlanId = plan.PlanId,
                            PlanName = plan.PlanName

                        };

            responseData.Items.AddRange(query.ToArray());

            return Task.FromResult(responseData);
        }

    }
}
