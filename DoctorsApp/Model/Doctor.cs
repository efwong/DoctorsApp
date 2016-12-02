using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorsApp.Model
{
    public class Doctor
    {
        public Doctor(int id, string name, string specialty, string hospital, bool isAcceptingNewPatients, int medicalGroupId, bool doesAcceptMedicaid, double averageReviewScore, Address address)
        {
            Id = id;
            Name = name;
            Specialty = specialty;
            Hospital = hospital;
            IsAcceptingNewPatients = isAcceptingNewPatients;
            MedicalGroupId = medicalGroupId;
            DoesAcceptMedicaid = doesAcceptMedicaid;
            AverageReviewScore = averageReviewScore;
            Address = address;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialty { get; set; }
        public string Hospital { get; set; }
        public bool IsAcceptingNewPatients { get; set; }
        public int MedicalGroupId { get; set; }
        public bool DoesAcceptMedicaid { get; set; }
        public double AverageReviewScore { get; set; }
        public Address Address { get; set; }
    }
}