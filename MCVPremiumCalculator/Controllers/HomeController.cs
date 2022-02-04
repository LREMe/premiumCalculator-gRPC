using Grpc.Net.Client;
using gRPCPremiumCalculator;
using MCVPremiumCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace MCVPremiumCalculator.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GrpcChannel channel;
        private readonly IConfiguration _config;
        string baseAddress = "";
        //string baseAddressLocal = "";

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _config = configuration;
            _logger = logger;
            baseAddress = _config["WebAPIConfig:WebAddressRemote"];
            //  baseAddressLocal = _config["WebAPIConfig:WebAddressLocal"];
            channel = GrpcChannel.ForAddress(baseAddress);
        }

        public IActionResult Index()
        {
            HomeViewModel m = new HomeViewModel();

            //Information from the appsettings.json

            // ViewBag.BaseAddressLocal = baseAddressLocal;

            // Add values to the front end
            m.DateOfBirth = DateTime.Now;

            try
            {
                // get a client to connect

                PeriodCRUD.PeriodCRUDClient client = new PeriodCRUD.PeriodCRUDClient(channel);
                Periods periods = client.PeriodList(new Empty());

                StateCRUD.StateCRUDClient clientState = new StateCRUD.StateCRUDClient(channel);
                States states = clientState.StateList(new EmptyStateParam());

                PlanCRUD.PlanCRUDClient clientPlan = new PlanCRUD.PlanCRUDClient(channel);
                Plans plans = clientPlan.PlanList(new EmptyPlanParam());

                //m.SelectedPeriod = "";
                m.States = new SelectList(states.Items.ToArray(), "StateId", "StateName");
                m.Plans = new SelectList(plans.Items.ToArray(), "PlanId", "PlanName");

                m.Periods = new SelectList(periods.Items.ToArray(), "Factor", "NamePeriod");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "gRPC server unable to respond");
                m.States = GetDefaultList("StateId", "StateName");
                m.Plans = GetDefaultList("PlanId", "PlanName");
                m.Periods = GetDefaultList("Factor", "NamePeriod");

            }

            return View(m);
        }

        /// <summary>
        /// Function to avoid errors on jqgrid
        /// </summary>
        /// <param name="voidid"></param>
        /// <param name="voidvalue"></param>
        /// <returns></returns>
        private SelectList GetDefaultList(string voidid, string voidvalue)
        {

            // string voidResponse = "[{\"" + voidid + "\":\" \",\"" + voidvalue + "\":\" \"}]";
            return new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Selected = false, Text = "", Value = ""}
             }, "Value", "Text");
        }

        /// <summary>
        /// Get the information for the jqgrid
        /// </summary>
        /// <param name="Dob"></param>
        /// <param name="State"></param>
        /// <param name="Age"></param>
        /// <param name="Plan"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetPremium(DateTime Dob, string State, int Age, string Plan)
        {
            try
            {
                PremiumRolCRUD.PremiumRolCRUDClient client = new PremiumRolCRUD.PremiumRolCRUDClient(channel);
                PremiumQ premiumQ = new PremiumQ();
                premiumQ.DoB = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(Dob);
                premiumQ.State = State;
                premiumQ.Age = Age;
                premiumQ.Plan = Plan;

                PremiumRols premiumRols = client.PremiumList(premiumQ);
                this._logger.LogInformation("Premium informed!");
                return Json(premiumRols.Items.ToList());
            }
            catch (Exception e)
            {
                this._logger.LogError(e, "Error connecting...");
                return Json("");
            }
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}