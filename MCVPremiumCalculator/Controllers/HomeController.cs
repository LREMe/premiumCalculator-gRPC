using Grpc.Net.Client;
using MCVPremiumCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using gRPCPremiumCalculator;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MCVPremiumCalculator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            HomeViewModel m = new HomeViewModel();

            //ViewBag.BaseAddress = baseAddress;

            // Add values to the front end
            m.DateOfBirth = DateTime.Now;

            try
            {
                // get a client to connect
                var channel = GrpcChannel.ForAddress("http://localhost:5017");
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
            catch (Exception e) {
                _logger.LogError(e, "gRPC server unable to respond");
                m.States = GetDefaultList("StateId", "StateName");
                m.Plans = GetDefaultList("PlanId", "PlanName");
                m.Periods = GetDefaultList("Factor", "NamePeriod");

            }


            return View(m);
        }

        private SelectList GetDefaultList(string voidid, string voidvalue) {

           // string voidResponse = "[{\"" + voidid + "\":\" \",\"" + voidvalue + "\":\" \"}]";
           return new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Selected = false, Text = "", Value = ""}
             }, "Value", "Text");
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