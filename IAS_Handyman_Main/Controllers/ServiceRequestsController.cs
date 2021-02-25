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
            return View(serviceRequest);
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
            return View(serviceRequest);
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
