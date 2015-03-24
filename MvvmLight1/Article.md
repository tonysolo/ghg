# A practical e-Health implementation #

----------

I am submitting this application for consideration for Grand Challenge in Healthcare. It exploits a novel idea of using low resolution GIS to store and manage complete healthcare information in all countries.

----------


There is worldwide interest in e-health, encouraged by the World Health Organisation. Some countries have already implemented it, and most are in the process of doing so. 

 A greatest promise of e-health is that it will improve population health in all countries. However, we only hear about e-health happening in first world countries and at cost that is out of reach for poor countries. A particular case for e-health is the potential to solve maternal and child mortality in developing countries by simply managing healthcare to ensure that mothers receive antenatal care, safe childbirth and understanding for feeding and looking after their babies. A simple low cost e-health record is possible with direct use of cloud storage. The system I have designed uses Microsoft Azure Page Blob directly that costs 5 US cents per gigabyte per month, a small fraction of the cost of equivalent database storage. The system includes storage and processing of health provider records, patients' e-health and large scale epidemiology.

The system stores data corresponding to geographic regions. This allows the healthcare data to combine reference information such as population and economic data from sources such a SEDAC that are similarly indexed using GIS.

In my application geographic regions are named in a special way to provide unique Azure cloud storage container names essential for isolating the data storage per region. 

The container names consist of 5 hexadecimal characters: 'QNNEE' where:

- 'Q' is the Earth's quadrant 'Northeast','Northwest','Southeast', or 'Southwest' : '0','1','2', or '3'.
- 'NN' is the latitude '00' to '7f' : representing 0 to 90 degrees latitude.
- 'EE' is the longitude '00' to 'ff' : representing 0 to 180 degrees longitude.

Each uniquely named 'QNNEE' container then contains the same contents:
A list of Page Blobs named as follows:
- 'L' - Loaders - healthcare providers and other data loaders for the region.
- 'P' - Patients - the e-health records for the region.
- 'E' - Epidemiology for the region.
- 'I' - Images for images such as scans, x-rays and handwriting. 

Page blobs are similar to mechanical Disc drives - optimised for random read write and offer large data storage capacity of 1 terabyte.


## Registration ##

Each country will be set up in advance with all its regions named and allocated. For example South Africa consists of approximately 200 'QNNEE' regions. Providers will choose the region where they work, then use GIS map enabled application to register themselves, then register patients and view or update patients' records and record epidemiology

Patients will have control of their data, including whether of not to participate and which region to join. Patients should choose a place to register such as their region of birth or their home region when they arrived in the country.

Providers will need to register as a provider in the region where they currently practice. They will need to re-register if they relocate.

## How it works ##


Health Information Exchange (HIE)

 Each region will function as a separate health information exchange. When patients register they will be allocated a unique 'next in sequence ID' for that region. Their e-health record will always be located in that region. However patients will not be restricted and may seek treatment in any region by any provider.

The system has many advantages including allocation of unique ID's for patients and combining geographic, economic and healthcare data for a full e-health patient controlled record with actionable data. 

Cloud computing and cloud storage continually improve and become cheaper. Similarly mobile devices with Internet access increasingly become affordable and widespread. Internet access provides communication and GIS and makes it possible to map regions, people and their environment. Taken together these things could dramatically improve healthcare and living standards in needy countries and improve the lives of millions of people.

e-Health and e-Epidemiology.

The e-health application deals with individual patients. It stores their preferences, medical history, medications etc. It handles prescriptions, referrals and SMS appointment reminders to patients. It flags missed appointments or special risks such as disabilities, can refer patients for telemedicine or community worker help using SMS or email.

The e-health application is able to store and retrieve images such as scanned documents, x-rays and and handwritten notes. These are stored in separate page blobs and managed (generally they are set to expire after a time to free storage space)

The e-epidemiology application provides a simple way to record and process epidemiology on a large scale without any recording any specific patient details. It provides healthcare statistics for regions and date ranges. Any disease, trauma, health facility type, provider type, geographic region for a date range will be available. 

Epidemiology data could be stored in a global account in larger 'biomes'(qne) rather than a specific country 'qnnee' regions. This will allow it to be a global resource for infectious diseases.

Finally, the application will poll and record opinions from contributing providers to assist healthcare planning. 


----------

Tony Manicom









