# A practical e-Health implementation #

----------
This application uses a novel idea of using low resolution GIS to store and manage healthcare information for e-health in any country.

----------


There is worldwide interest in e-health, encouraged by the World Health Organisation. Some countries have already implemented it, and most are in the process of doing so. 

 A particular case for e-health is to ensure basic healthcare in poor countries, that mothers receive antenatal care and safe childbirth, children are vaccinated, and HIV and TB are treated. 

A simple low cost e-health system is possible with direct use of cloud storage.


The system I plan uses Microsoft Azure Page Blob storage.


The system includes processing of health provider registration, patients' e-health and large scale epidemiology and image storage.

Healthcare data is stored according to geographic region which allows it to be combined with population and economic data from the 'Socioeconomic Data and Applications Centre data' (SEDAC) which is similarly GIS indexed.

The application names regions for Azure cloud storage using 5 characters ('QNNEE') where:
  'Q' is the Earth's quadrant 'North-east,'North-west','South-east or 'South-west' : '0','1','2', or '3'.
- 'NN' is the latitude  '00' to '7f' : representing latitude.
- 'EE' is the longitude '00' to 'ff' : representing longitude.

'QNNEE' is used as the storage container name for the region, which then contains 11 page blobs:
- 'L' - Loaders - healthcare providers and other data loaders for the region.
- 'P' - Patients - the e-health records for the region.
- 'E' - Epidemiology for the region.
- 'I0' to I7 - Images for images such as scans, x-rays and handwriting in 8 size ranges from 32KB to 4MB. 

## Registration ##

Each country will be set up in advance with all its regions named and allocated. For example Sierra Leone consists of these 22 'QNNEE' regions:

![](http://i.imgur.com/Z1kjDjj.jpg)

 The application provides mapping to locate regions for registering providers and patients and to view and update patients' records and record epidemiology.

![](http://i.imgur.com/JMM64rB.jpg)
(Note: Regions measure 42 arc minutes (80 X 80 Km at the equator) and overlap with with adjacent country borders)

Patients have control of their data and can limit access in any way they want. Patients register in their region of birth or their first region when they arrived in the country.


## How it works ##


Health Information Exchanges

 Each region works as a separate health information exchange. When patients register they are allocated a unique 'next in sequence ID' for that region and their e-health record will always be located in that region. However patients will not be restricted and may seek treatment in any region by any provider.

The system has many advantages including allocation of unique ID's for patients and combining geographic, economic and healthcare data for a full e-health patient controlled record with actionable data. 

Cloud computing and cloud storage continually improve and become cheaper. Similarly mobile devices with Internet access have become affordable and widespread. Internet access provides communication and GIS and makes it possible to map regions, people and their environment. Taken together these things could dramatically improve healthcare and living standards in needy countries and improve the lives of millions of people.

e-Health and e-Epidemiology and Population Health Management.

The e-health application deals with individual patients. It stores their preferences, medical history, medications etc. It handles prescriptions, referrals and SMS appointment reminders to patients. It flags missed appointments or special risks such as disabilities and high risk pregnancies, can refer patients for telemedicine or community worker help using SMS or email.

The e-health application will store and retrieve images such as scanned documents, x-rays and and handwritten notes. These are stored in separate page blobs and managed (generally they are set to expire after a preset time to free storage space)

The e-epidemiology application provides a simple way to record and process epidemiology on a large scale without any recording any specific patient details. It provides healthcare statistics for regions and date ranges. Any disease, trauma, health facility type, provider type, geographic region for a date range will be available. 

Epidemiology data will be stored in a global account using larger regions of 11.25 degrees(qne). This will provide a global resource for diseases.

Part of the ehealth record will be set aside to flag risks and special attention for population health management (PHM). These will flag patients who need special follow-up such as high risk pregnancies, missed appointments, resistant TB, disabilities and old age. This PHM section of all patients' ehealth records will be processed weekly to trigger actions such as notifying health visitors, sms appointment reminders to patients or family. 

Finally, the application will poll opinions from providers and provide them with ehealth statistics to help achieve continuous improvement.

"Plan, Do, Check, Adjust".

----------

Tony Manicom









