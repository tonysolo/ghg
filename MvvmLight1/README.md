# Global Health Grid (GHG)#

### e-Health Records ##
e-Health is sought by all countries as a way to improve healthcare. Unfortunately there have been costly failures and criticism about ease of use and clinical benefits. 

The implementation described here is a simple concept based on using Cloud storage with fixed length binary data records (Stored in Microsoft Azure Page Blobs). The method is technically simple and clinically complete.

###Scope #
This is a global scale geographic information system (GIS) that records *E-Health and Epidemiology and using Cloud Storage and Mapping to store and retrieve healthcare data using a GIS Grid of 42 arc minutes or 80 X 80 Km at the equator*.

E-Health is clinical information. It contains details of treatment, prescriptions and referrals. E-health data belongs to the patient and is stored confidentially in the patient's private e-health record. It is stored in a cloud repository that allows patients to access their medical record wherever needed. 

E-Epidemiology is information about the diseases occurring in the population: it contains diagnostic codes and other data without recording any person identifiable information. It is used for healthcare statistics, surveillance and planning.

##Data Storage   
### Azure Cloud Blob Storage ###

Microsoft Azure BLOB storage is used to store all the data using a separate **storage container for each region** which in turn contains eleven BLOBs: ** 'P','L','E', I-0 ... to I-7** as shown in the following table:

BLOBs|Purpose|Data Structure
:--:|:------------------------------|:------------------------|
**P** | Patient e-Health|256 Kilobytes (512 pages) for each patient
**L** | Loaders (providers)| 4 Kilobytes (8 pages) for each provider
**E** | Epidemiology|All patients for the region (page offset recorded each day in an index table)
**I-0... I-7**|Images| Ranges from 8Kb to 1Mb


### Azure Container Names ('qnnee') ### 

Container names consist of 5 hexadecimal characters defining a GIS region "qnnee" (quadrant, latitude and longitude).

    nn = degrees latitude / 180 * 256;
    ee = degrees Longitude /180 * 256;
    q == 0 for NE
    q == 1 for NW
    q == 2 for SE
    q == 3 for SW

The "qnnee" values range from "00000" to "37FFF" and represent regions separated by 42 arc minutes.

### Patient and Provider ID's ##

Unique ID's for patients and providers are equivalent to their data offset in the 'P' blob or 'L' blobs.

Patient's e-Health data aligns with 256Kb boundaries so the data location in the 'P' blob will be: 

    Start Location = ID<<18   (ID X 256K)

Similarly provider data aligns with 4Kb boundaries in the 'L' blob  at 

    Start Location = ID<<12    (ID X 4K)

## Processing ## 

The purpose of the software is to simplify collecting healthcare data and then getting the most value from processing.
 
### Actionable Data / Population Health Management##
The design supports actionable data. For example e-health will record risky behaviour such as missed appointment and vaccination, or diseases such as drug resistant TB. A scheduled cloud process will periodically check all the records in a region and will act by sending an SMS to the patient or email a healthcare visitor to follow up the problem.

Actionable data will help control serious health problems like maternal and child mortality, HIV, Drug Resistant TB, road accidents and violence patterns. It will be used for managing telemedicine, health visitors and appointment reminders. This will be done automatically and at low cost.

### Scheduled Processing ## 
Azure storage queues allow deferred scheduling of tasks such as appointment reminders on a specific weekday. For long delays, tasks will be recorded in the patient's e-Health record as actionable data.

## Epidemiology ## 
Epidemiology is very important. Before cloud computing and GIS it would have been impossible to collect, process and map complete geographic epidemiology data in real time.

This application gives healthcare providers a simple way to record the conditions they treat every day without person identifiable data. A cloud process sorts and updates epidemiology overnight. 

(This process could be extended to any scale at very low cost)

The table shows how epidemiology processing on a global scale could work using distributed storage and processing.

Storage container names '**q**nn**e**e' provide a mechanism for allocating regions to one of 32 time zones (0..31 in the table) which allows data to be distributed across 8 storage queues (0..7) for separate concurrent processing. 

*(...e.g. Queue '0' stores data for timezones '0','8','16' and '24' for sheduled processing lasting six hours for each zone...)*

Epidemiology data is distributed across eight storage queues (0..7) according to regions' *'45 minute time zones'* (0..31) as shown in the table below. By setting the queue visibility to hide the data until the owner region reaches midnight, each region data has exclusive visibility for 6 hours of processing (from midnight to 6 am). 

### Processing using Azure Queue Storage and Epidemiology data##

|queue|
|--|--|--|--|--|
|**0**|0|8|16|24|
|**1**|1|9|17|25|
|**2**|2|10|18|26|
|**3**|3|11|19|27|
|**4**|4|12|20|28|
|**5**|5|13|21|29|
|**6**|6|14|22|30|
|**7**|7|15|23|31|


##Azure Page Blob Storage #

###Blobs 'P' and L' - (Patients and Providers)
Patients and Providers data stored as fixed length records at fixed offsets in their respective page blobs.

###Blob 'E (Epidemiology):
Epidemiology data is queued and then stored into the epidemiology blob by an Azure Worker Role using a daily scheduled task. 

###Blobs 'I-0' to 'I-7' .. (8 image ranges) 

#####(8KB, 16KB, 32KB, 64KB, 128KB, 256KB, 512KB, 1MB)

The idea is to have fixed length records for efficient storage and retrieval and to manage the duration of storage from 1 month to 20 years.

The storage of image data includes an intermediate step to set a 'time-to-live table' used to control how long to keep images (in months). Images, scans, ECG traces...etc will occupy most of the data storage space in e-health records. Items such as handwritten 'to-do lists' or photographs might need less than one month storage, whereas other information might need to be stored for years. The volume of storage is managed to ensure that space is always available for new images.

Images will be PDF files converted to byte arrays and stored in the lowest fitting page blob.

The image index will be stored with the patients data and efficiently retrieved for viewing.
##Other Details
The system could be used for any country or region. The initial set-up requires countries to be added as a collection of 'qnnee' regions. This is a simple once-off task.

When the program runs page blobs for patients(P), providers/loaders(L), epidemiology(E) and the eight images blobs(I-0 to I-7) are automatically added as required.

Usage is similar to a standard medical practice paper records, but with the advantages of automation for storing and sharing images, digital prescriptions, telemedicine, population health management and appointment reminders.

Azure charges 0.5 cents (US) per gigabyte per month for data used. This application manages data efficiently making it practical to implement epidemiology and e-health for any country at very low cost.

Epidemiology will be particularly valuable for global scale disease surveillance. Similarly population health management has potential to recognize high risk patients and prevent bad outcomes.

