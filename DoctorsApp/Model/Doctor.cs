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
        /// <summary>
        /// A doctor's specialty (eg.  Internal Medicine, Radiology)
        /// </summary>
        public string Specialty { get; set; }


        /// <summary>
        /// Name of the primary hospital or clinic that employs the Doctor
        /// </summary>
        public string Hospital { get; set; }


        /// <summary>
        /// True -> The doctor is accepting new patients
        /// False -> The doctor is not accepting new patients
        /// </summary>
        public bool IsAcceptingNewPatients { get; set; }

        /// <summary>
        /// Id of the medical group/network that the doctor is a part of.
        /// </summary>
        public int MedicalGroupId { get; set; }

        /// <summary>
        /// True -> The doctor accepts medicaid
        /// False -> The doctor does not accept medicaid
        /// </summary>
        public bool DoesAcceptMedicaid { get; set; }

        /// <summary>
        /// Average Review Score
        /// </summary>
        public double AverageReviewScore { get; set; }
        
        /// <summary>
        /// Doctor's address
        /// All Doctors must have addresses
        /// </summary>
        public Address Address { get; set; }
    }
}