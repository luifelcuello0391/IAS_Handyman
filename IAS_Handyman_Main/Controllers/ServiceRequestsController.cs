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
        public ActionResult Create([Bind(Include = "Id,Code,Description,ScheduleDate,IsEmergency,StartDateTime,EndDateTime,Hours,CreationDateTime,ModificationDateTime,RegisterStatus")] ServiceRequest serviceRequest)
        {
            if (ModelState.IsValid)
            {
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
            return View(serviceRequest);
        }

        // POST: ServiceRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,Description,ScheduleDate,IsEmergency,StartDateTime,EndDateTime,Hours,CreationDateTime,ModificationDateTime,RegisterStatus")] ServiceRequest serviceRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceRequest).State = EntityState.Modified;
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

        public async Task<ActionResult> GetTechnician(int identification)
        {
            try
            {
                Technician technician = await db.Technicians.FirstOrDefaultAsync(x => x.Id == identification);

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
    }
}
