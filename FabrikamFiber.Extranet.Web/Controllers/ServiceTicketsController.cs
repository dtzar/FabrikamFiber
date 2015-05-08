namespace FabrikamFiber.Extranet.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using FabrikamFiber.DAL.Data;
    using FabrikamFiber.DAL.Models;

    public class ServiceTicketsController : Controller
    {
        private readonly IServiceTicketRepository serviceTicketRepository;

        private readonly IScheduleItemRepository scheduleItemRepository;
        
        public ServiceTicketsController(IServiceTicketRepository serviceTicketRepository, IScheduleItemRepository scheduleItemRepository)
        {
            this.serviceTicketRepository = serviceTicketRepository;
            this.scheduleItemRepository = scheduleItemRepository;
        }

        public JsonResult GetServiceTicket(int ticketId)
        {
            var serviceTicket = this.serviceTicketRepository.FindIncluding(ticketId, serviceticket => serviceticket.AssignedTo);

            var ticketViewModel = new
            {
                id = serviceTicket.ID,
                status = serviceTicket.Status.ToString(),
                title = serviceTicket.Title,
                opened = serviceTicket.Opened,
                assignedTo = serviceTicket.AssignedTo.FullName,
                expectedVisitTime = this.CalculateVisitTime(serviceTicket),
                isClosed = serviceTicket.Status.ToString() == "Closed"
            };

            return this.Json(ticketViewModel, JsonRequestBehavior.AllowGet);
        }

        private DateTime? CalculateVisitTime(ServiceTicket serviceTicket)
        {
            var time = this.scheduleItemRepository.All.SingleOrDefault(e => e.ServiceTicketID == serviceTicket.ID);

            if (time == null) return null;

            var number = new Random((int)DateTime.Now.Ticks).Next(0, 30);
            return time.Start.AddMinutes(number % 2 == 0 ? number : -1 * number);
        }

        [HttpPost]
        [ActionName("Schedule")]
        public ActionResult AssignSchedule(int serviceTicketId, int employeeId, float startTime)
        {
            this.scheduleItemRepository.All.Where(e => e.ServiceTicketID == serviceTicketId)
                                          .ToList()
                                          .ForEach(e => this.scheduleItemRepository.Delete(e.ID));

            var serviceTicket = this.serviceTicketRepository.Find(serviceTicketId);
            var time = string.Format("Mon 16 May {0:d2}:{1:d2} {2} 2011", ((int)startTime > 12 ? (int)startTime - 12 : (int)startTime) / 1, startTime % 1 == 0.5 ? 30 : 0, startTime < 12 ? "AM" : "PM");
            var startAt = DateTime.ParseExact(time, "ddd dd MMM h:mm tt yyyy", System.Globalization.CultureInfo.InvariantCulture);
            var scheduleItem = new ScheduleItem { EmployeeID = employeeId, ServiceTicketID = serviceTicketId, Start = startAt, WorkHours = 1, AssignedOn = DateTime.Now };
            this.scheduleItemRepository.InsertOrUpdate(scheduleItem);
            serviceTicket.AssignedToID = employeeId;

            this.serviceTicketRepository.Save();
            this.scheduleItemRepository.Save();

            return RedirectToAction("Index", new { serviceTicket.ID });
        }
    }
}