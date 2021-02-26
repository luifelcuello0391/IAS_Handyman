using IAS_Handyman.Models;
using IAS_Handyman_Main.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace IAS_Handyman.test
{
    [TestClass]
    public class ServiceRequestTest
    {
        [TestMethod]
        public void TestDetailsView()
        {
            // This method validates that the Details view is showed up successfully for an existing service request
            var controller = new ServiceRequestsController();
            var result = controller.Details(2) as ViewResult;
            Assert.AreEqual("Details", result.ViewName);
        }

        [TestMethod]
        public void TestDeleteView()
        {
            // This method validates that the Delete view is showed up successfully for an existing service request
            var controller = new ServiceRequestsController();
            var result = controller.Delete(2) as ViewResult;
            Assert.AreEqual("Delete", result.ViewName);
        }

        [TestMethod]
        public void TestNotfound()
        {
            // This method validates that the Not found view is showed up for a non-existing service request
            var controller = new ServiceRequestsController();
            var result = controller.Delete(1) as ViewResult;
            bool isNull = result == null;
            Assert.AreEqual(true, isNull);
        }

        [TestMethod]
        public void GetWeekDaysValidation()
        {
            var controller = new ServiceRequestsController();

            DateTime[] dates = controller.WeekDays(2021, 8);
            dates[dates.Length - 1] = new DateTime(dates[dates.Length - 1].Year, dates[dates.Length - 1].Month, dates[dates.Length - 1].Day, 23, 59, 0);

            Assert.AreEqual("2021/02/22;2021/02/28", string.Format("{0};{1}", dates[0].ToString("yyyy/MM/dd"), dates[dates.Length-1].ToString("yyyy/MM/dd")));
        }

        [TestMethod]
        public void WorkedHoursReportValidation()
        {
            var controller = new ServiceRequestsController();

            string data = "[{\"Id\":10,\"Code\":\"ACD130\",\"Description\":\"no funciona el monitor\",\"ScheduleDate\":\"2021-02-22T00:00:00\",\"IsEmergency\":true,\"CurrentStatus\":{\"Id\":2,\"Name\":\"Ejecutado\",\"CreationDateTime\":\"2021-02-22T21:13:03.437\",\"ModificationDateTime\":\"2021-02-22T21:13:03.437\",\"RegisterStatus\":1,\"RegisterStatusName\":\"Activo\"},\"Responsable\":null,\"StartDateTime\":\"2021-02-22T02:02:00\",\"EndDateTime\":\"2021-02-22T06:00:00\",\"Hours\":3},{\"Id\":9,\"Code\":\"ACD129\",\"Description\":\"Se dañó el aire acondicionado\",\"ScheduleDate\":\"2021-02-22T00:00:00\",\"IsEmergency\":false,\"CurrentStatus\":{\"Id\":2,\"Name\":\"Ejecutado\",\"CreationDateTime\":\"2021-02-22T21:13:03.437\",\"ModificationDateTime\":\"2021-02-22T21:13:03.437\",\"RegisterStatus\":1,\"RegisterStatusName\":\"Activo\"},\"Responsable\":null,\"StartDateTime\":\"2021-02-22T10:01:00\",\"EndDateTime\":\"2021-02-22T19:00:00\",\"Hours\":8},{\"Id\":11,\"Code\":\"ACD131\",\"Description\":\"no sirve la PC\",\"ScheduleDate\":\"2021-02-23T00:00:00\",\"IsEmergency\":false,\"CurrentStatus\":{\"Id\":2,\"Name\":\"Ejecutado\",\"CreationDateTime\":\"2021-02-22T21:13:03.437\",\"ModificationDateTime\":\"2021-02-22T21:13:03.437\",\"RegisterStatus\":1,\"RegisterStatusName\":\"Activo\"},\"Responsable\":null,\"StartDateTime\":\"2021-02-23T08:00:00\",\"EndDateTime\":\"2021-02-23T19:00:00\",\"Hours\":11},{\"Id\":2,\"Code\":\"ACD124\",\"Description\":\"La lavadora no enciente\",\"ScheduleDate\":\"2021-03-06T00:00:00\",\"IsEmergency\":false,\"CurrentStatus\":{\"Id\":2,\"Name\":\"Ejecutado\",\"CreationDateTime\":\"2021-02-22T21:13:03.437\",\"ModificationDateTime\":\"2021-02-22T21:13:03.437\",\"RegisterStatus\":1,\"RegisterStatusName\":\"Activo\"},\"Responsable\":null,\"StartDateTime\":\"2021-02-24T01:00:00\",\"EndDateTime\":\"2021-02-24T08:01:00\",\"Hours\":7},{\"Id\":12,\"Code\":\"ACD132\",\"Description\":\"Faltan 12 horas\",\"ScheduleDate\":\"2021-02-24T00:00:00\",\"IsEmergency\":false,\"CurrentStatus\":{\"Id\":2,\"Name\":\"Ejecutado\",\"CreationDateTime\":\"2021-02-22T21:13:03.437\",\"ModificationDateTime\":\"2021-02-22T21:13:03.437\",\"RegisterStatus\":1,\"RegisterStatusName\":\"Activo\"},\"Responsable\":null,\"StartDateTime\":\"2021-02-24T08:30:00\",\"EndDateTime\":\"2021-02-24T16:00:00\",\"Hours\":7},{\"Id\":15,\"Code\":\"ACD139\",\"Description\":\"Para ver si salen en nocturna\",\"ScheduleDate\":\"2021-02-25T00:00:00\",\"IsEmergency\":true,\"CurrentStatus\":{\"Id\":2,\"Name\":\"Ejecutado\",\"CreationDateTime\":\"2021-02-22T21:13:03.437\",\"ModificationDateTime\":\"2021-02-22T21:13:03.437\",\"RegisterStatus\":1,\"RegisterStatusName\":\"Activo\"},\"Responsable\":null,\"StartDateTime\":\"2021-02-25T01:00:00\",\"EndDateTime\":\"2021-02-25T05:00:00\",\"Hours\":4},{\"Id\":13,\"Code\":\"ACD136\",\"Description\":\"Para alcanzar las 48 horas\",\"ScheduleDate\":\"2021-02-25T00:00:00\",\"IsEmergency\":false,\"CurrentStatus\":{\"Id\":2,\"Name\":\"Ejecutado\",\"CreationDateTime\":\"2021-02-22T21:13:03.437\",\"ModificationDateTime\":\"2021-02-22T21:13:03.437\",\"RegisterStatus\":1,\"RegisterStatusName\":\"Activo\"},\"Responsable\":null,\"StartDateTime\":\"2021-02-25T01:01:00\",\"EndDateTime\":\"2021-02-25T06:00:00\",\"Hours\":4},{\"Id\":8,\"Code\":\"ACD129\",\"Description\":\"HOPE No lanza trigger de flujo\",\"ScheduleDate\":\"2021-02-24T00:00:00\",\"IsEmergency\":true,\"CurrentStatus\":{\"Id\":2,\"Name\":\"Ejecutado\",\"CreationDateTime\":\"2021-02-22T21:13:03.437\",\"ModificationDateTime\":\"2021-02-22T21:13:03.437\",\"RegisterStatus\":1,\"RegisterStatusName\":\"Activo\"},\"Responsable\":null,\"StartDateTime\":\"2021-02-25T01:01:00\",\"EndDateTime\":\"2021-02-25T06:20:00\",\"Hours\":5},{\"Id\":7,\"Code\":\"ACD128\",\"Description\":\"Xbox no enciende\",\"ScheduleDate\":\"2021-03-01T00:00:00\",\"IsEmergency\":false,\"CurrentStatus\":{\"Id\":2,\"Name\":\"Ejecutado\",\"CreationDateTime\":\"2021-02-22T21:13:03.437\",\"ModificationDateTime\":\"2021-02-22T21:13:03.437\",\"RegisterStatus\":1,\"RegisterStatusName\":\"Activo\"},\"Responsable\":null,\"StartDateTime\":\"2021-02-25T08:00:00\",\"EndDateTime\":\"2021-02-25T10:00:00\",\"Hours\":2},{\"Id\":14,\"Code\":\"ACD136\",\"Description\":\"se dañó el TV\",\"ScheduleDate\":\"2021-02-25T00:00:00\",\"IsEmergency\":false,\"CurrentStatus\":{\"Id\":2,\"Name\":\"Ejecutado\",\"CreationDateTime\":\"2021-02-22T21:13:03.437\",\"ModificationDateTime\":\"2021-02-22T21:13:03.437\",\"RegisterStatus\":1,\"RegisterStatusName\":\"Activo\"},\"Responsable\":null,\"StartDateTime\":\"2021-02-25T09:00:00\",\"EndDateTime\":\"2021-02-25T13:03:00\",\"Hours\":4}]";
            List<ServiceDataForReport> servicesData = JsonConvert.DeserializeObject<List<ServiceDataForReport>>(data);

            if(servicesData != null && servicesData.Count > 0)
            {
                servicesData[0].Responsable = new Technician();
                servicesData[0].Responsable.Identification = "1140836470";
                servicesData[0].Responsable.Id = 1;
                servicesData[0].Responsable.FullName = "Luis Felipe Cuello Ayala";

                DateTime[] dates = controller.WeekDays(2021, 8);
                dates[dates.Length - 1] = new DateTime(dates[dates.Length - 1].Year, dates[dates.Length - 1].Month, dates[dates.Length - 1].Day, 23, 59, 0);
                List<WeeklyHoursTechnicianReport> report = controller.OrganizeReportInformation(servicesData, 2021, 8, dates[0], dates[1]) as List<WeeklyHoursTechnicianReport>;

                if(report != null)
                {
                    Assert.AreEqual("26;22;0;6;1;0", string.Format("{0};{1};{2};{3};{4};{5}", 
                                                     report[0].NormalHours, 
                                                     report[0].NightHours,
                                                     report[0].SundayHours,
                                                     report[0].ExtraNormalHours,
                                                     report[0].ExtraNightHours,
                                                     report[0].ExtraSundayHours));
                }
                else
                {
                    Assert.Fail("No report informatio obtained from dummy data");
                }
            }
            else
            {
                Assert.Fail("There is no services information obtained from dummy JSON");
            }
        }
    }
}
