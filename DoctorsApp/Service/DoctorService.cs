/* DoctorService
 * Main business logic for Doctors 
 */
using DoctorsApp.Model;
using DoctorsApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorsApp.Service
{
    public class DoctorService
    {
        // The DoctorRepository can perform CRUD operations for doctor data.
        private readonly IRepository<Doctor> _doctorRepository;

        public DoctorService(IRepository<Doctor> doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        /// <summary>
        /// Get similar doctors based on the input doctor.
        /// 
        /// Similar is defined as having the same specialty, medical group, city, and an average review score that is greater or equal to the given doctor
        /// Will also be based on their ability to accept medicaid. If the given doctor accepts medicaid, similar doctors must accept it as well; otherwise,
        /// if the given doctor does not accept it, medicaid would not be a criteria for the filtering of similar doctors.
        /// 
        /// All results are ordered by the average review score in descending order(high to low).
        /// </summary>
        /// <param name="doctor"></param>
        /// <returns></returns>
        public IEnumerable<Doctor> GetSimilarDoctors(Doctor doctor)
        {
            var doctors = _doctorRepository.Get();

            // Returne empty IEnumerable if doctors is null
            if (doctors == null)
            {
                return Enumerable.Empty<Doctor>();
            }

            return doctors.Where(x =>
                x.Specialty == doctor.Specialty &&
                x.MedicalGroupId == doctor.MedicalGroupId &&
                ((doctor.Address != null && x.Address != null) && (x.Address.City == doctor.Address.City)) &&
                x.AverageReviewScore >= doctor.AverageReviewScore &&
                ((doctor.DoesAcceptMedicaid && x.DoesAcceptMedicaid) || doctor.DoesAcceptMedicaid == false)
                ).OrderByDescending(x => x.AverageReviewScore);
        }
    }
}