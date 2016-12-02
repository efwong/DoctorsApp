using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoctorsApp.Repository;
using DoctorsApp.Model;
using Moq;
using DoctorsApp.Service;
using System.Collections.Generic;

namespace DoctorsAppUnitTest
{
    [TestClass]
    public class DoctorServiceTest
    {
        private DoctorService doctorService;
        private Mock<IRepository<Doctor>> mock;

        [TestInitialize]
        public void Initialize()
        {
            mock = new Mock<IRepository<Doctor>>();
            doctorService = new DoctorService(mock.Object);
        }

        [TestMethod]
        public void GetSimilarDoctors_Calls_Repository_Get()
        {

            Doctor doctor = new Doctor(1234, "Dan Dee", "Internal Medicine", "Kaiser", true, 5134, false, 6.0, new Address("123 Main St", "San Francisco", "CA", 93413));

            mock.Setup(x => x.Get()).Returns(new List<Doctor>() { doctor });
            var doctors = doctorService.GetSimilarDoctors(It.IsAny<Doctor>());

            mock.Verify(x => x.Get(), Times.Once());

        }
    }
}
