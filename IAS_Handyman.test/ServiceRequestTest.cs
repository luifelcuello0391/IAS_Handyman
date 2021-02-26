using IAS_Handyman_Main.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;

namespace IAS_Handyman.test
{
    [TestClass]
    public class ServiceRequestTest
    {
        [TestMethod]
        public void TestDetailsView()
        {
            var controller = new ServiceRequestsController();
            var result = controller.Details(2) as ViewResult;
            Assert.AreEqual("Details", result.ViewName);
        }

        [TestMethod]
        public void TestDeleteView()
        {
            var controller = new ServiceRequestsController();
            var result = controller.Delete(2) as ViewResult;
            Assert.AreEqual("Delete", result.ViewName);
        }
    }
}
