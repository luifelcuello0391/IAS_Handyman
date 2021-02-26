using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IAS_Handyman.Models;
using IAS_Handyman.Repository;

namespace IAS_Handyman_Main.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        private DateTime[] WeekDays(int Year, int WeekNumber)
        {
            //DateTime start = new DateTime(Year, 1, 1).AddDays(7 * WeekNumber);
            //start = start.AddDays(-((int)start.DayOfWeek));
            //return Enumerable.Range(1, 7).Select(num => start.AddDays(num)).ToArray();

            DateTime start = new DateTime(Year, 1, 4);
            start = start.AddDays(-((int)start.DayOfWeek));
            start = start.AddDays(7 * (WeekNumber - 1));
            return Enumerable.Range(1, 7).Select(num => start.AddDays(num)).ToArray();
        }

        private IEnumerable<WeeklyHoursTechnicianReport> OrganizeReportInformation(List<ServiceDataForReport> data, int year, int week, DateTime startdate, DateTime enddate)
        {
            // The list must ordered by attention start date
            data = data.OrderBy(x => x.StartDateTime.Value).ToList();

            if(data != null && data.Count > 0)
            {
                List<WeeklyHoursTechnicianReport> report = new List<WeeklyHoursTechnicianReport>();

                WeeklyHoursTechnicianReport report_data = new WeeklyHoursTechnicianReport();
                report_data.Year = year;
                report_data.YearWeek = week;
                report_data.TechnicianIdentification = data[0].Responsable.Identification;
                report_data.TechnicianId = data[0].Responsable.Id;
                report_data.TechnicianName = data[0].Responsable.FullName;
                report_data.WeekStartDate = startdate;
                report_data.WeekEndDate = enddate;

                int SumatoryOfNormalHours = 0;
                int SumatoryOfNightHours = 0;
                int SumatoryOfSundayHours = 0;

                int SumatoryOfExtraNormalHours = 0;
                int SumatoryOfExtraNightHours = 0;
                int sumatoryOfExtraSundayHours = 0;

                // Obtains the normal hours
                foreach(var register in data)
                {
                    if(register != null)
                    {
                        // Validates if the attention date range is a sunday
                        if(register.StartDateTime.Value.DayOfWeek == DayOfWeek.Sunday || register.EndDateTime.Value.DayOfWeek == DayOfWeek.Sunday)
                        {
                            // Validates if the technician already has worked 48 hours in the week
                            if((SumatoryOfNormalHours + SumatoryOfNightHours + SumatoryOfSundayHours) >= 48)
                            {
                                sumatoryOfExtraSundayHours += register.Hours;
                            }
                            else
                            {
                                if ((SumatoryOfNormalHours + SumatoryOfNightHours + SumatoryOfSundayHours + register.Hours) >= 48)
                                {
                                    int over48hours = (SumatoryOfNormalHours + SumatoryOfNightHours + SumatoryOfSundayHours + register.Hours) - 48;
                                    int before48hours = 48 - (SumatoryOfNormalHours + SumatoryOfNightHours + SumatoryOfSundayHours);

                                    SumatoryOfSundayHours += before48hours;
                                    sumatoryOfExtraSundayHours += over48hours;
                                }
                                else
                                {
                                    SumatoryOfSundayHours += register.Hours;
                                }
                            }
                        }
                        else // Monday to saturday
                        {
                            // Validates if it is between 07:00 AM and 08:00 PM using the 24H format
                            if(int.Parse(register.StartDateTime.Value.ToString("HH")) >= 7 && int.Parse(register.EndDateTime.Value.ToString("HH")) <= 20)
                            {
                                // Validates if the technician already has worked 48 hours in the week
                                if ((SumatoryOfNormalHours + SumatoryOfNightHours + SumatoryOfSundayHours) >= 48)
                                {
                                    SumatoryOfExtraNormalHours += register.Hours;
                                }
                                else
                                {
                                    if((SumatoryOfNormalHours + SumatoryOfNightHours + SumatoryOfSundayHours + register.Hours) >= 48)
                                    {
                                        int over48hours = (SumatoryOfNormalHours + SumatoryOfNightHours + SumatoryOfSundayHours + register.Hours) - 48;
                                        int before48hours = 48 - (SumatoryOfNormalHours + SumatoryOfNightHours + SumatoryOfSundayHours);

                                        SumatoryOfNormalHours += before48hours;
                                        SumatoryOfExtraNormalHours += over48hours;
                                    }
                                    else
                                    {
                                        SumatoryOfNormalHours += register.Hours;
                                    }
                                }

                                if(SumatoryOfNormalHours > 48)
                                {
                                    SumatoryOfExtraNormalHours += (SumatoryOfNormalHours - 48);
                                    SumatoryOfNormalHours = 48;
                                }
                            }
                            else // 08:01 PM to 06:59 AM
                            {
                                // Validates if the technician already has worked 48 hours in the week
                                if ((SumatoryOfNormalHours + SumatoryOfNightHours + SumatoryOfSundayHours) >= 48)
                                {
                                    SumatoryOfExtraNightHours += register.Hours;
                                }
                                else
                                {
                                    if ((SumatoryOfNormalHours + SumatoryOfNightHours + SumatoryOfSundayHours + register.Hours) >= 48)
                                    {
                                        int over48hours = (SumatoryOfNormalHours + SumatoryOfNightHours + SumatoryOfSundayHours + register.Hours) - 48;
                                        int before48hours = 48 - (SumatoryOfNormalHours + SumatoryOfNightHours + SumatoryOfSundayHours);

                                        SumatoryOfNightHours += before48hours;
                                        SumatoryOfExtraNightHours += over48hours;
                                    }
                                    else
                                    {
                                        SumatoryOfNightHours += register.Hours;
                                    }
                                }
                            }
                        }
                    }
                }

                // Assigns the normal hours
                report_data.NormalHours = SumatoryOfNormalHours;
                report_data.NightHours = SumatoryOfNightHours;
                report_data.SundayHours = SumatoryOfSundayHours;

                // Assigns the extra hours
                report_data.ExtraNormalHours = SumatoryOfExtraNormalHours;
                report_data.ExtraNightHours = SumatoryOfExtraNightHours;
                report_data.ExtraSundayHours = sumatoryOfExtraSundayHours;

                report.Add(report_data);
                return report as IEnumerable<WeeklyHoursTechnicianReport>;
            }

            return null;
        }

        // GET: WeeklyWorkHoursByTechnician
        public ActionResult WeeklyWorkHoursByTechnician(int? year, int? week, int? technicianId)
        {
            DateTime[] weekDays = null;

            if(year != null)
            {
                if(week != null)
                {
                    // Obtains the corresponding dates for week
                    weekDays = WeekDays(year.Value, week.Value);

                    if(technicianId != null)
                    {
                        if (weekDays != null && weekDays.Length > 0)
                        {
                            weekDays[weekDays.Length - 1] = new DateTime(weekDays[weekDays.Length - 1].Year, weekDays[weekDays.Length - 1].Month, weekDays[weekDays.Length - 1].Day, 23, 59, 0);

                            var weekStartDate = weekDays[0];
                            var weekEndDate = weekDays[weekDays.Length - 1];

                            // Execute the report
                            var query = db.Services
                                        // Obtains the services of technician that status = 'Ejecutado'
                                        .Where(x => x.Responsable.Id == technicianId && x.CurrentStatus != null && x.CurrentStatus.Id == 2 &&
                                        // AND startdatetime is between week start date AND week end date
                                        ((x.StartDateTime.Value.CompareTo(weekStartDate) >= 0 && x.StartDateTime.Value.CompareTo(weekEndDate) <= 0) ||
                                        // OR enddatetime is between week start date AND week end date
                                        (x.EndDateTime.Value.CompareTo(weekStartDate) >= 0 && x.EndDateTime.Value.CompareTo(weekEndDate) <= 0))
                                        )
                                        .Select(r => new ServiceDataForReport()
                                        {
                                            Id = r.Id,
                                            Code = r.Code,
                                            CurrentStatus = r.CurrentStatus,
                                            Description = r.Description,
                                            EndDateTime = r.EndDateTime,
                                            Hours = r.Hours,
                                            IsEmergency = r.IsEmergency,
                                            Responsable = r.Responsable,
                                            ScheduleDate = r.ScheduleDate,
                                            StartDateTime = r.StartDateTime
                                        }).OrderBy(x => x.StartDateTime.Value).ToList();

                            IEnumerable<WeeklyHoursTechnicianReport> report = OrganizeReportInformation(query, year.Value, week.Value, weekStartDate, weekEndDate);

                            if (report != null && report.Count() > 0)
                            {
                                return View("WeeklyWorkHoursByTechnician", report);
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = string.Format("No fué posible obtener el rango de fechas para el año {0} y la semana {1}", year.Value, week.Value);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Debe seleccionar un técnico para generar el reporte";
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Debe especificar la semana del año para generar el reporte";
                }
            }
            else // year = null
            {
                if(week != null)
                {
                    ViewBag.ErrorMessage = "Debe especificar el año para generar el reporte";
                }

                // week = null
                if(technicianId != null)
                {
                    ViewBag.ErrorMessage = "Debe especificar el año y la semana para generar el reporte";
                }
            }

            if(technicianId != null)
            {
                Technician technician = db.Technicians.FirstOrDefault(x => x.Id == technicianId);
                if(technician != null)
                {
                    List<WeeklyHoursTechnicianReport> dummyReport = new List<WeeklyHoursTechnicianReport>();
                    WeeklyHoursTechnicianReport data = new WeeklyHoursTechnicianReport();
                    data.TechnicianId = technician.Id;
                    data.TechnicianIdentification = technician.Identification;
                    data.TechnicianName = technician.FullName;

                    if(weekDays != null && weekDays.Length >= 2)
                    {
                        data.WeekStartDate = weekDays[0];
                        data.WeekEndDate = weekDays[weekDays.Length - 1];
                    }

                    data.Year = year;
                    data.YearWeek = week;

                    dummyReport.Add(data);
                    return View("WeeklyWorkHoursByTechnician", dummyReport);
                }
            }

            return View("WeeklyWorkHoursByTechnician");
        }

        // GET: ServiceReport
        public ActionResult ServiceReport()
        {
            return View("ServiceReport", db.Services.Where(x => x.CurrentStatus != null && x.CurrentStatus.Id == 2).ToList());
        }

        // GET: ServiceRequests
        public ActionResult Index()
        {
            return View(db.Services.ToList());
        }

        // GET: ServiceRequests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceRequest serviceRequest = db.Services.Find(id);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }
            return View("Details", serviceRequest);
        }

        // GET: ServiceRequests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServiceRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,Description,ScheduleDate,IsEmergency,StartDateTime,EndDateTime,Hours,CreationDateTime,ModificationDateTime,RegisterStatus,SelectedTechnicianId")] ServiceRequest serviceRequest)
        {
            if (ModelState.IsValid)
            {
                if(serviceRequest.SelectedTechnicianId > 0)
                {
                    serviceRequest.Responsable = db.Technicians.FirstOrDefault(x => x.Id == serviceRequest.SelectedTechnicianId);
                }

                if (serviceRequest.Responsable != null)
                {
                    if (serviceRequest.CurrentStatus == null)
                    {
                        serviceRequest.CurrentStatus = db.ServiceStatuses.FirstOrDefault(x => x.Id == 1);
                    }
                }

                db.Services.Add(serviceRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(serviceRequest);
        }

        // GET: ServiceRequests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceRequest serviceRequest = db.Services.Find(id);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }

            if(serviceRequest.Responsable != null)
            {
                serviceRequest.SelectedTechnicianId = serviceRequest.Responsable.Id;
            }

            if(serviceRequest.CurrentStatus != null)
            {
                serviceRequest.SelectedCurrentStatusId = serviceRequest.CurrentStatus.Id;
            }

            return View(serviceRequest);
        }

        // POST: ServiceRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,Description,ScheduleDate,IsEmergency,StartDateTime,EndDateTime,Hours,CreationDateTime,ModificationDateTime,RegisterStatus,SelectedTechnicianId,StartTimeHour,StartTimeMinute,EndTimeHour,EndTimeMinute,SelectedCurrentStatusId")] ServiceRequest serviceRequest)
        {
            if (ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "";

                // Obtains the current status
                if(serviceRequest.SelectedCurrentStatusId != null)
                {
                    serviceRequest.CurrentStatus = db.ServiceStatuses.FirstOrDefault(x => x.Id == serviceRequest.SelectedCurrentStatusId);
                }

                if (serviceRequest.SelectedTechnicianId > 0)
                {
                    if(serviceRequest.Responsable == null || serviceRequest.Responsable.Id != serviceRequest.SelectedTechnicianId)
                    {
                        serviceRequest.Responsable = db.Technicians.FirstOrDefault(x => x.Id == serviceRequest.SelectedTechnicianId);
                    }
                }

                // Obtains the start date time
                if (serviceRequest.StartTimeHour != null && serviceRequest.StartTimeMinute != null)
                {
                    if (serviceRequest.StartDateTime != null)
                    {
                        serviceRequest.StartDateTime = new DateTime(serviceRequest.StartDateTime.Value.Year,
                                                                    serviceRequest.StartDateTime.Value.Month,
                                                                    serviceRequest.StartDateTime.Value.Day,
                                                                    serviceRequest.StartTimeHour.Value,
                                                                    serviceRequest.StartTimeMinute.Value,
                                                                    0);
                    }
                }
                else
                {
                    if(serviceRequest.StartDateTime != null)
                    {
                        ViewBag.ErrorMessage = "No se han ingresado las horas o los minutos para la fecha de inicio de la atención";
                        return View(serviceRequest);
                    }
                }

                // Obtains the end date time
                if (serviceRequest.EndTimeHour != null && serviceRequest.EndTimeMinute != null)
                {
                    if (serviceRequest.EndDateTime != null)
                    {
                        serviceRequest.EndDateTime = new DateTime(serviceRequest.EndDateTime.Value.Year,
                                                                    serviceRequest.EndDateTime.Value.Month,
                                                                    serviceRequest.EndDateTime.Value.Day,
                                                                    serviceRequest.EndTimeHour.Value,
                                                                    serviceRequest.EndTimeMinute.Value,
                                                                    0);
                    }
                }
                else
                {
                    if (serviceRequest.EndDateTime != null)
                    {
                        ViewBag.ErrorMessage = "No se han ingresado las horas o los minutos para la fecha de finalización de la atención";
                        return View(serviceRequest);
                    }
                }

                // Compare the dates, end date must be after than start date
                if (serviceRequest.StartDateTime != null && serviceRequest.EndDateTime != null)
                {
                    if(serviceRequest.StartDateTime.Value.CompareTo(DateTime.Now) > 0)
                    {
                        ViewBag.ErrorMessage = "La fecha de inicio de la atención debe ser inferior o igual a la fecha actual";
                        return View(serviceRequest);
                    }

                    if(serviceRequest.EndDateTime.Value.CompareTo(DateTime.Now) > 0)
                    {
                        ViewBag.ErrorMessage = "La fecha de finalización de la atención debe ser inferior o igual a la fecha actual";
                        return View(serviceRequest);
                    }

                    if (serviceRequest.StartDateTime.Value.CompareTo(serviceRequest.EndDateTime.Value) >= 0)
                    {
                        ViewBag.ErrorMessage = "La fecha de inicio de la atención debe ser inferior a la fecha de finalización de la atención";
                        return View(serviceRequest);
                    }

                    if(serviceRequest.StartDateTime.Value.CompareTo(serviceRequest.ScheduleDate) < 0)
                    {
                        ViewBag.ErrorMessage = "La fecha de inicio de la atención no puede ser inferior a la fecha de programación del servicio";
                        return View(serviceRequest);
                    }

                    if (serviceRequest.EndDateTime.Value.CompareTo(serviceRequest.ScheduleDate) < 0)
                    {
                        ViewBag.ErrorMessage = "La fecha de finalización de la atención no puede ser inferior a la fecha de programación del servicio";
                        return View(serviceRequest);
                    }
                }
                else
                {
                    if(serviceRequest.StartDateTime != null)
                    {
                        ViewBag.ErrorMessage = "Debe especificar la fecha de finalización de la atención";
                        return View(serviceRequest);
                    }

                    if (serviceRequest.EndDateTime != null)
                    {
                        ViewBag.ErrorMessage = "Debe especificar la fecha de inicio de la atención";
                        return View(serviceRequest);
                    }
                }

                if (serviceRequest.Responsable != null)
                {
                    if (serviceRequest.StartDateTime != null && serviceRequest.EndDateTime != null)
                    {
                        // Assigns the status "Ejecutado"
                        serviceRequest.CurrentStatus = db.ServiceStatuses.FirstOrDefault(x => x.Id == 2);

                        TimeSpan dateDifference = serviceRequest.EndDateTime.Value.Subtract(serviceRequest.StartDateTime.Value);
                        serviceRequest.Hours = (int)dateDifference.TotalHours;
                    }
                    else if (serviceRequest.CurrentStatus == null)
                    {
                        // Assigns the status "Asignado"
                        serviceRequest.CurrentStatus = db.ServiceStatuses.FirstOrDefault(x => x.Id == 1);
                    }
                }

                var data = db.Services.Find(serviceRequest.Id);

                if (data != null)
                {
                    data.Hours = serviceRequest.Hours;
                    data.IsEmergency = serviceRequest.IsEmergency;
                    data.ModificationDateTime = serviceRequest.ModificationDateTime;
                    data.Responsable = serviceRequest.Responsable;
                    data.ScheduleDate = serviceRequest.ScheduleDate;
                    data.SelectedTechnicianId = serviceRequest.SelectedTechnicianId;
                    data.StartDateTime = serviceRequest.StartDateTime;
                    data.EndDateTime = serviceRequest.EndDateTime;
                    data.CurrentStatus = serviceRequest.CurrentStatus;
                    data.Code = serviceRequest.Code;
                    data.Description = serviceRequest.Description;
                    data.CreationDateTime = serviceRequest.CreationDateTime;
                    data.RegisterStatus = serviceRequest.RegisterStatus;
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(serviceRequest);
        }

        // GET: ServiceRequests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceRequest serviceRequest = db.Services.Find(id);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }
            return View("Delete", serviceRequest);
        }

        // POST: ServiceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServiceRequest serviceRequest = db.Services.Find(id);
            db.Services.Remove(serviceRequest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult GetTechnician(int identification)
        {
            try
            {
                Technician technician = db.Technicians.FirstOrDefault(x => x.Id == identification);

                return PartialView("TechnicianData", technician);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error on ServiceRequestController.GetTechnician >> " + ex.ToString());
            }

            return PartialView("TechnicianData", null);
        }

        public string GetTechnicianData(int identification)
        {
            try
            {
                Technician technician = db.Technicians.FirstOrDefault(x => x.Id == identification);

                if(technician != null)
                {
                    return string.Format("{0}||NAME||{1}", technician.Identification, technician.FullName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error on ServiceRequestController.GetTechnician >> " + ex.ToString());
            }

            return null;
        }

        public async Task<ActionResult> GetTechnicians()
        {
            try
            {
                IEnumerable<Technician> technicians = await db.Technicians.ToListAsync();

                return PartialView("TechnicianSelectionList", technicians);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error on ServiceRequestController.GetTechnicians >> " + ex.ToString());
            }

            return PartialView("TechnicianSelectionList", null);
        }

        public string CancelService(int identification)
        {
            if (identification > 0)
            {
                var data = db.Services.Find(identification);

                if (data != null)
                {
                    data.CurrentStatus = db.ServiceStatuses.FirstOrDefault(x => x.Id == 3);
                }

                db.SaveChanges();
                return "OK";
            }

            return "Error: Identificación no encontrada.";
        }
    }
}
