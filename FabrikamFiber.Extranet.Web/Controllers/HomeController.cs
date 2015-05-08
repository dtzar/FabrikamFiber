namespace FabrikamFiber.Extranet.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using FabrikamFiber.DAL.Data;

    public class HomeController : Controller
    {
        private readonly IServiceTicketRepository serviceTicketRepository;

        private readonly IScheduleItemRepository scheduleItemRepository;

        public HomeController(IServiceTicketRepository serviceTicketRepository, IScheduleItemRepository scheduleItemRepository)
        {
            this.serviceTicketRepository = serviceTicketRepository;
            this.scheduleItemRepository = scheduleItemRepository;
        }

        public ActionResult Index()
        {
            return View(this.serviceTicketRepository.All.Take(5));
        }
    }
}