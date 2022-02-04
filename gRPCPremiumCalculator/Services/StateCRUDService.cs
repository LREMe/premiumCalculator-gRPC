namespace gRPCPremiumCalculator.Services
{

    using Grpc.Core;
    using System.Threading.Tasks;
    using Data = gRPCPremiumCalculator.Data;
    public class StateCRUDService:StateCRUD.StateCRUDBase
    {

        private Data.PremiumRolDbContext db = null;

        public StateCRUDService(Data.PremiumRolDbContext db)
        {
            this.db = db;
        }



           public override Task<States> StateList(EmptyStateParam request, ServerCallContext context)
           {
               States responseData = new States();

               var query = from state in db.States
                           select new State
                           {
                              StateId = state.StateId,
                              StateName = state.StateName
                           };

               responseData.Items.AddRange(query.ToArray());

               return Task.FromResult(responseData);
               //turn base.PeriodList(request, context);
           }

    }
}
