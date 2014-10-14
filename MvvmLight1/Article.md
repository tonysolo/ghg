# A practical e-Health implementation #

There is worldwide interest in e-health, encouraged by the World Health Organisation. Many counties have already implemented it or are in the process of doing so. 

 A greatest promise of e-health is the potential for population health management (PHM). For example to solve maternal and child mortality in developing countries. (A simple e-health record could be designed to include actionable data to ensure that mothers receive antenatal care and babies are fed and vaccinated.)

I have designed a cloud based e-health system that implements health information exchanges (HIEs) using a geographic information system (GIS) design. These HIEs store the e-health data for all patients belonging to a GIS region. The system has many advantages including allocation of unique ID's for patients, having geographic and economic data to combine with healthcare data, a full e-health patient controlled record, and actionable data to prevent disasters. 

Cloud computing and cloud storage continually improve and become cheaper. Similarly mobile devices with Internet access have become affordable for widespread use. Internet access provides communication and GIS and makes it possible to use these technologies to improve healthcare and raise living standards in these countries. Cloud storage and Internet access are incredibly low cost.

I have two related Cloud applications - e-health and e-epidemiology. The e-epidemiology application provides a simple way to record and process epidemiology on a large scale without any specific patient details.

The e-health application deals with individual patients. It stores their preferences, history, medications etc. It handles prescriptions, referrals and SMS appointment reminders to patients. It flags missed appointments or special risks such as disabilities, can refer patients for telemedicine or community worker help using SMS or email.

The e-epidemiolgy application provides epidemiolgy statistics such as road accidents for a region and date range. Any disease, trauma, health facility type, provider type, geographic region for a date range will be available. The application can poll opinions from contributing providers to assist healthcare planning. 

 More information is available [here](http://tonysolo@github.com).

Tony Manicom






Both applications store patients data in geographic regions. This allows the healthcare data to include information such as population and economic data for the region.

Geographic Regions are named in a special way to provide unique cloud storage container names. 

The container names consist of 5 hexadecimal characters: 'QNNEE' where:

'Q' is the Quadrant 'NE','NW','SE', or 'SW' : '0','1','2', or '3'.
'NN' is the latitude '00' to '7f' : equiv to 0 to 90 degrees latitude.
'EE' is the longitude '00' to 'ff' : equiv to 0 to 180 degrees latitude.

Each uniquely named 'QNNEE' container then contains the same contents:
Three Page Blobs named as follows:
'L' for Loaders (contains healthcare providers' details) for the region.
'P' for Patients (patients e-health records) for the region.
'E' for Epidemiology for the region.
Page blobs are similar to mechanical Disc drives - optimised for random read write and offer large data storage capacity of 1 terabyte.
The container will also contain smaller items (block blobs) indexed for the patients storing variable size data, for example to record images and scans.

Registration

Each country will be set up in advance with all its regions named and allocated. For example South Africa consists of approximately 200 'QNNEE' regions. Providers will choose the region where they work, register themselves, register patients and view or update patients' records.

Patients will maintain control of their data, including whether of not to participate and which region to join. Patients should choose a place such as their region of birth or their home region when they arrived in the country.

Providers will need to register as a provider in the region where they currently practice. They will need to re-register if they relocate (See epidemiology).

How it works


Health Information Exchange (HIE)

 Each region will function as a health information exchange. When patients register they will be allocated a unique 'next in sequence ID' for that region and their e-health record will always be located in that region. However patients will not be restricted and may seek treatment in any region by any provider.






