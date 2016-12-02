# DoctorsApp
by Edwin Wong
for Practice Fusion Coding Challenge
Find similar doctors in a prioritized order

# Similar Doctors
I'll assume similar doctors are doctors that have the same specialty, medical group, located in the same city, and have an average review greater or equal than the given doctor.

Also, if the selected doctor accepts medicaid, then all similar doctors must also accept medicaid; otherwise if the selected doctor does not accept medicaid, then all similar
doctors can accept or reject medicaid.  Assuming the user submitted a search based on a doctor with medicaid, the user might also require medicaid; therefore the list of doctors
returned should also accept medicaid.


# Returned Doctors Ordering
When the list of doctors are returned, the ordering will be base don the average review in descending order (from highest to lowest) 