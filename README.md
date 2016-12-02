# DoctorsApp
by Edwin Wong
for Practice Fusion Coding Challenge
Find similar doctors in a prioritized order

# Installation
To complete installation, please run either:

## With Visual Studio
* Tools > NuGet Package Manager > Package Manager Console
* In Console Enter: Update-Package

## With nuget.exe
* Run: nuget update Doctors.sln

# Similar Doctors
I'll assume "similar" doctors are defined as doctors who have the same specialty, medical group, located in the same city, have an average review greater or equal than the given doctor, and a matching medicaid status.

If the selected doctor accepts medicaid, then all similar doctors must also accept medicaid; otherwise if the selected doctor does not accept medicaid, then all similar doctors can accept or reject medicaid.  

Reasoning:
Assuming the user submitted a search based on a doctor with medicaid, the user might also require medicaid; therefore the list of doctors returned should all accept medicaid.  I would not want a user who requires medicaid to accidentally visit a doctor who doesn't accept medicaid.


# Returned Doctors Ordering
When the list of doctors is returned, the ordering will be based on the average review in descending order (from highest to lowest).


# Classes

## DoctorService
Contains the business logic for the Doctor searching algorithm.

GetSimilarDoctors: Will return a list of similar doctors when given a doctor as input.
* Input: Doctor
* Return: A list of doctors

## IRepository
Interface for a generic repository. I did not implement a concrete doctor repository because it is outside the scope of the project.

## Doctor
Doctor model
Contains:
* Id: Doctor's Id
* Name: Doctor's Name
* Specialty: Doctor's specialization (e.g. Internal Medicine)
* Hospital: Name of the Hospital
* IsAcceptingNewPatients: Whether or not the doctor is accepting new patients
* MedicalGroupId: The doctor's medical group id
* DoesAcceptMedicaid: Whether or not the doctor accepts medicaid
* AverageReviewScore: The doctor's average review score
* Address: The doctor's address

## Address
Model for an address: Street, City, State Zip

# Unit Tests

* AddressModelTest: Tests the Address model
* DoctorModelUnitTest: Tests the Doctor model
* DoctorServiceTest: Tests the DoctorService


## Notes
To improve on the searching functionality, we should implement a separate GetSimilarDoctorsByFilter method that searches based on an additional filter object.  Right now, the filtering is coupled directly to the Doctor object that is passed in, and I am assuming the definition of "similar".  Instead, we could allow users to define their search based on their own specified properties.  We then convert their search requirements into a filter object and pass it into the GetSimilarDoctorsByFilter.  This will allow for a more specific search.