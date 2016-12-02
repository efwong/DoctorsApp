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
        private readonly IRepository<Doctor> _doctorRepository;

        public DoctorService(IRepository<Doctor> doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public IEnumerable<Doctor> GetSimilarDoctors(Doctor doctor)
        {
            var doctors = _doctorRepository.Get();
            return doctors;
        }
    }
}