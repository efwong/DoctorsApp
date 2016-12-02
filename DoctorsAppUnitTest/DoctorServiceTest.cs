using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoctorsApp.Repository;
using DoctorsApp.Model;
using Moq;
using DoctorsApp.Service;
using System.Linq;
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
        public void GetSimilarDoctors_CallsGet_Once()
        {

            mock.Setup(x => x.Get()).Returns(Enumerable.Empty<Doctor>());
            var doctors = doctorService.GetSimilarDoctors(It.IsAny<Doctor>());

            mock.Verify(x => x.Get(), Times.Once(), "GetSimilarDoctors should only call Repository.Get once.");
        }

        /// <summary>
        /// If Doctor parameter is null, it should return an empty list.
        /// </summary>
        [TestMethod]
        public void GetSimilarDoctors_NullInput_ReturnsEmptyList()
        {
            mock.Setup(x => x.Get()).Returns(Enumerable.Empty<Doctor>());
            IEnumerable<Doctor> doctors = doctorService.GetSimilarDoctors(null);

            Assert.IsNotNull(doctors);
            Assert.AreEqual(doctors.Count(), 0);
        }

        /// <summary>
        /// If the repository returns a null list of doctors, GetSimilarDoctors should return an empty list.
        /// </summary>
        public void GetSimilarDoctors_RepositoryDoctorsIsNull_ReturnsEmptyList()
        {
            mock.Setup(x => x.Get()).Returns(null as IEnumerable<Doctor>);
            IEnumerable<Doctor> doctors = doctorService.GetSimilarDoctors(GetDefaultDoctor());

            Assert.IsNotNull(doctors);
            Assert.AreEqual(doctors.Count(), 0);
        }

        /// <summary>
        /// Given a doctor, return a list of doctors in descending order based on the average review score
        /// </summary>
        [TestMethod]
        public void GetSimilarDoctors_ReturnsListOfDoctorsInDescendingOrderBasedOnAverageReview()
        {
            var doctor = GetDefaultDoctor();
            var expectedDoctorList = GenerateDoctorsWithDifferentAverageReviewScores(true);
            mock.Setup(x => x.Get()).Returns(GenerateDoctorsWithDifferentAverageReviewScores());
            var doctors = doctorService.GetSimilarDoctors(doctor).ToList();
            
            Assert.IsTrue(doctors != null && doctors.Count() > 0);
            Assert.AreEqual(expectedDoctorList.Count(), doctors.Count());

            bool result = false;
            for (int i = 0; i < expectedDoctorList.Count(); i++)
            {
                bool currentResult = (expectedDoctorList[i].Id == doctors[i].Id);
                if (i == 0)
                {
                    result = currentResult;
                }
                else
                {
                    result &= currentResult;
                }
            }

            Assert.IsTrue(result, "GetSimilarDoctors returned doctors in incorrect order");
        }

        /// <summary>
        /// If Doctors have everything similar except Specialty, return the doctor with with a matching specialty
        /// </summary>
        [TestMethod]
        public void GetSimilarDoctors_InputDifferentSpecialty_ReturnsDoctorsWithMatchingSpecialty()
        {
            var doctor = GetDefaultDoctor();
            mock.Setup(x => x.Get()).Returns(GenerateDoctorsWithDifferentSpecialty());
            IEnumerable<Doctor> doctors = doctorService.GetSimilarDoctors(doctor);

            Assert.IsTrue(doctors != null && doctors.Count() > 0);
            bool hasNonMatchingDoctors = doctors.Any(x => x.Specialty != doctor.Specialty);
            Assert.IsFalse(hasNonMatchingDoctors, "GetSimilarDoctors returned a doctor with a non-matching specialty.");
        }

        /// <summary>
        /// If Doctors have everything similar except the MedicalGroupId, return the doctor with a matching MedicalGroupId
        /// </summary>
        [TestMethod]
        public void GetSimilarDoctors_InputDifferentMedicalGroupId_ReturnsDoctorsWithMedicalGroupId()
        {
            var doctor = GetDefaultDoctor();
            mock.Setup(x => x.Get()).Returns(GenerateDoctorsWithDifferentMedicalGroupId());
            IEnumerable<Doctor> doctors = doctorService.GetSimilarDoctors(doctor);

            Assert.IsTrue(doctors != null && doctors.Count() > 0);
            bool hasNonMatchingDoctors = doctors.Any(x => x.MedicalGroupId != doctor.MedicalGroupId);
            Assert.IsFalse(hasNonMatchingDoctors, "GetSimilarDoctors returned a doctor with a non-matching medical group id.");
        }

        /// <summary>
        /// If Doctors have everything similar except the AverageReviewScore, return the doctors with a score >= than the given doctor's score
        /// </summary>
        [TestMethod]
        public void GetSimilarDoctors_InputDifferentAverageReviewScore_ReturnsDoctorsWithGreaterThanOrEqualScore()
        {
            var doctor = GetDefaultDoctor();
            mock.Setup(x => x.Get()).Returns(GenerateDoctorsWithDifferentAverageReviewScores());
            IEnumerable<Doctor> doctors = doctorService.GetSimilarDoctors(doctor);

            Assert.IsTrue(doctors != null && doctors.Count() > 0);
            bool hasNonMatchingDoctors = doctors.Any(x => x.AverageReviewScore < doctor.AverageReviewScore); // test for average score below that of the given doctor
            Assert.IsFalse(hasNonMatchingDoctors, "GetSimilarDoctors returned a doctor with an average review score lower than the input doctor.");
        }

        /// <summary>
        /// Given a doctor that accepts medicaid, only return other doctors that accept medicaid
        /// </summary>
        [TestMethod]
        public void GetSimilarDoctors_InputDoctorAcceptsMedicaid_ReturnsDoctorsThatAcceptMedicaid()
        {
            var doctor = new Doctor(1231, "Aaron Aaron", "Internal Medicine", "Kaiser", true, 5134, true, 6.0, new Address("123 Main St", "San Francisco", "CA", 93413));
            mock.Setup(x => x.Get()).Returns(GenerateDoctorsWithDifferentMedicaidStatus());
            IEnumerable<Doctor> doctors = doctorService.GetSimilarDoctors(doctor);

            Assert.IsTrue(doctors != null && doctors.Count() > 0);
            bool hasNonMatchingDoctors = doctors.Any(x => x.DoesAcceptMedicaid == false); // check for any doctors that do not accept medicaid
            Assert.IsFalse(hasNonMatchingDoctors, "GetSimilarDoctors returned a doctor with that does not accept medicaid when filtering only for doctors that accept medicaid.");
        }


        /// <summary>
        /// Given a doctor that does not rejects medicaid, can return any doctor that accepts or denies medicaid
        /// </summary>
        [TestMethod]
        public void GetSimilarDoctors_InputDoctorRejectsMedicaid_ReturnsDoctorsThatAcceptOrRejectMedicaid()
        {
            var doctor = GetDefaultDoctor(); // a doctor that rejects medicaid
            var originalDoctorList = GenerateDoctorsWithDifferentMedicaidStatus();
            mock.Setup(x => x.Get()).Returns(originalDoctorList);
            IEnumerable<Doctor> doctors = doctorService.GetSimilarDoctors(doctor);

            Assert.IsTrue(doctors != null && doctors.Count() > 0);
            CollectionAssert.AreEqual(originalDoctorList.ToList(), doctors.ToList(), "Encountered mismatched doctors when filtering for medicaid");
        }

        /// <summary>
        /// Given a doctor that has no address, return an empty list.
        /// Because a similar doctor is a doctor who is located in the same city, it would be impossible to find a matching doctor without an address.
        /// </summary>
        [TestMethod]
        public void GetSimilarDoctors_InputDoctorWithNoAddress_ReturnsEmptyList()
        {
            var doctor = new Doctor(1231, "Aaron Aaron", "Internal Medicine", "Kaiser", true, 5134, false, 6.0, null);
            var originalDoctorList = GenerateDoctorsWithDifferentSpecialty();
            mock.Setup(x => x.Get()).Returns(originalDoctorList);
            IEnumerable<Doctor> doctors = doctorService.GetSimilarDoctors(doctor);

            Assert.AreEqual(doctors.Count(), 0, "An input doctor without an address should return an empty list of doctors.");
        }

        /// <summary>
        /// Given a doctor, return a list of doctors in the same city
        /// </summary>
        [TestMethod]
        public void GetSimilarDoctors_InputDoctorDifferentCity_ReturnsDoctorsWithMatchingCity()
        {
            var doctor = GetDefaultDoctor();
            var originalDoctorList = GenerateDoctorsWithDifferentCities();
            mock.Setup(x => x.Get()).Returns(originalDoctorList);
            IEnumerable<Doctor> doctors = doctorService.GetSimilarDoctors(doctor);

            Assert.IsTrue(doctors != null && doctors.Count() > 0);
            bool hasNonMatchingDoctors = doctors.Any(x => x.Address == null || x.Address.City != doctor.Address.City); // check for null addresses or different cities
            Assert.IsFalse(hasNonMatchingDoctors, "GetSimilarDoctors returned a doctor with a mismatched city");
        }

        #region Doctor Generators

        /// <summary>
        /// Returns a default doctor object
        /// </summary>
        private Doctor GetDefaultDoctor()
        {
            return new Doctor(1231, "Aaron Aaron", "Internal Medicine", "Kaiser", true, 5134, false, 6.0, new Address("123 Main St", "San Francisco", "CA", 93413));
        }

        /// <summary>
        /// Returns a list of doctors with different specialties
        /// </summary>
        private List<Doctor> GenerateDoctorsWithDifferentSpecialty()
        {
            return new List<Doctor>(){
                new Doctor(1234, "Dan Dee", "Internal Medicine", "Kaiser", true, 5134, false, 6.0, new Address("123 Main St", "San Francisco", "CA", 93413)),
                new Doctor(1235, "Adam Adam", "Cardiology", "Kaiser", true, 5134, false, 6.0, new Address("123 Main St", "San Francisco", "CA", 93413))
            };
        }

        /// <summary>
        /// Returns a list of doctors with different medical group id
        /// </summary>
        private List<Doctor> GenerateDoctorsWithDifferentMedicalGroupId()
        {
            return new List<Doctor>(){
                new Doctor(1234, "Dan Dee", "Internal Medicine", "Kaiser", true, 5134, false, 6.0, new Address("123 Main St", "San Francisco", "CA", 93413)),
                new Doctor(1235, "Adam Adam", "Internal Medicine", "Kaiser", true, 6235, false, 6.0, new Address("123 Main St", "San Francisco", "CA", 93413))
            };
        }

        /// <summary>
        /// Returns a list of doctors with different average review scores
        /// <getResult>
        ///     True => return expected doctors list after filtering
        ///     False => return mocked doctors list before filtering
        /// </getResult>
        /// </summary>
        private List<Doctor> GenerateDoctorsWithDifferentAverageReviewScores(bool getResult=false)
        {
            if (!getResult)
            {
                return new List<Doctor>(){
                    new Doctor(1234, "Dan Dee", "Internal Medicine", "Kaiser", true, 5134, false, 6.0, new Address("123 Main St", "San Francisco", "CA", 93413)),
                    new Doctor(1235, "Jesse Jesse", "Internal Medicine", "Kaiser", true, 5134, false, 5.5, new Address("123 Main St", "San Francisco", "CA", 93413)),
                    new Doctor(1236, "Clara Clara", "Internal Medicine", "Kaiser", true, 5134, false, 8.0, new Address("123 Main St", "San Francisco", "CA", 93413)),
                    new Doctor(1237, "Adam Adam", "Internal Medicine", "Kaiser", true, 5134, false, 6.01, new Address("123 Main St", "San Francisco", "CA", 93413)),
                    new Doctor(1238, "Ben Ben", "Internal Medicine", "Kaiser", true, 5134, false, 9.0, new Address("123 Main St", "San Francisco", "CA", 93413))
                };
            }else{
                return new List<Doctor>(){                    
                    new Doctor(1238, "Ben Ben", "Internal Medicine", "Kaiser", true, 5134, false, 9.0, new Address("123 Main St", "San Francisco", "CA", 93413)),
                    new Doctor(1236, "Clara Clara", "Internal Medicine", "Kaiser", true, 5134, false, 8.0, new Address("123 Main St", "San Francisco", "CA", 93413)),
                    new Doctor(1237, "Adam Adam", "Internal Medicine", "Kaiser", true, 5134, false, 6.01, new Address("123 Main St", "San Francisco", "CA", 93413)),
                    new Doctor(1234, "Dan Dee", "Internal Medicine", "Kaiser", true, 5134, false, 6.0, new Address("123 Main St", "San Francisco", "CA", 93413))
                };
            }
        }

        /// <summary>
        /// Returns a list of doctors with different Medicaid Acceptance status
        /// </summary>
        private List<Doctor> GenerateDoctorsWithDifferentMedicaidStatus()
        {
            return new List<Doctor>(){
                new Doctor(1234, "Dan Dee", "Internal Medicine", "Kaiser", true, 5134, false, 6.0, new Address("123 Main St", "San Francisco", "CA", 93413)),
                new Doctor(1235, "Adam Adam", "Internal Medicine", "Kaiser", true, 5134, true, 6.0, new Address("123 Main St", "San Francisco", "CA", 93413))
            };
        }
        /// <summary>
        /// Returns a list of doctors with different Addresses
        /// </summary>
        private List<Doctor> GenerateDoctorsWithDifferentCities()
        {
            return new List<Doctor>(){
                new Doctor(1234, "Dan Dee", "Internal Medicine", "Kaiser", true, 5134, false, 6.0, new Address("123 Main St", "San Francisco", "CA", 93413)),
                new Doctor(1235, "Adam Adam", "Internal Medicine", "Kaiser", true, 5134, false, 6.0, null),
                new Doctor(1236, "Ben Ben", "Internal Medicine", "Kaiser", true, 5134, false, 6.0, new Address("441 Clay St", "Oakland", "CA", 93413)),
                new Doctor(1237, "Jesse Jesse", "Internal Medicine", "Kaiser", true, 5134, false, 6.0, new Address("333 Test St", "Berkeley", "CA", 93413))
            };
        }
        #endregion
    }
}
