using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoctorsApp.Model;

namespace DoctorsAppUnitTest
{
    /// <summary>
    /// Check that all properties are set by the doctor constructor
    /// </summary>
    [TestClass]
    public class DoctorModelUnitTest
    {
        [TestMethod]
        public void Doctor_Constructor_SetsAllProperties()
        {
            var address = new Address("123 Main St", "San Francisco", "CA", 93413);
            var doctor = new Doctor(1231, "Aaron Test", "Internal Medicine", "Kaiser", true, 5134, false, 6.0, address);

            Assert.AreEqual(doctor.Id, 1231);
            Assert.AreEqual(doctor.Name, "Aaron Test");
            Assert.AreEqual(doctor.Specialty, "Internal Medicine");
            Assert.AreEqual(doctor.Hospital, "Kaiser");
            Assert.AreEqual(doctor.IsAcceptingNewPatients, true);
            Assert.AreEqual(doctor.MedicalGroupId, 5134);
            Assert.AreEqual(doctor.DoesAcceptMedicaid, false);
            Assert.AreEqual(doctor.AverageReviewScore, 6.0);
            Assert.AreEqual(doctor.Address, address);
        }
    }
}
