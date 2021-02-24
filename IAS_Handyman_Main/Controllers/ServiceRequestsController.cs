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

            return View(serviceRequest);
        }

        // POST: ServiceRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,Description,ScheduleDate,IsEmergency,StartDateTime,EndDateTime,Hours,CreationDateTime,ModificationDateTime,RegisterStatus,SelectedTechnicianId")] ServiceRequest serviceRequest)
        {
            if (ModelState.IsValid)
            {
                if (serviceRequest.SelectedTechnicianId > 0)
                {
                    if(serviceRequest.Responsable == null || serviceRequest.Responsable.Id != serviceRequest.SelectedTechnicianId)
                    {
                        serviceRequest.Responsable = db.Technicians.FirstOrDefault(x => x.Id == serviceRequest.SelectedTechnicianId);
                    }
                }

                if (serviceRequest.Responsable != null)
                {
                    if (serviceRequest.StartDateTime != null && serviceRequest.EndDateTime != null)
                    {
                        // Assigns the status "Ejecutado"
                        serviceRequest.CurrentStatus = db.ServiceStatuses.FirstOrDefault(x => x.Id == 2);
                    }
                    else if(serviceRequest.CurrentStatus == null)
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
