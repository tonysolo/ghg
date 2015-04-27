# Global Health Grid (GHG)#

### e-Health Records ##
e-Health is sought by all countries as the way to contain costs and improve access to healthcare. Unfortunately there have been failures and criticism about ease of use and clinical benefits. 

This implementation is a new approach based on Cloud storage using a method that deals with clinical data mostly as fixed length records. The method has both technical and clinical advantages for recording patients alerts, chronic medication, risks and preferences and clinical notes for each visit. Unlike other e-health systems it stores handwritten notes and scans as well as digital data.

###Scope #
This is a global scale application to store Healthcare information for Epidemiology and E-Health. It uses *Cloud Storage and GIS Maps* for storing and retrieving health data using a *Grid system* that defines regions of 42 arc minutes. This is equivalent to 80 X 80 Km at the equator.

GHG has two purposes: *e-Epidemiology* and  and *e-Health*.

E-Health is information about clinical management. It contains details of treatment, prescriptions and referrals. E-health data belongs to the patient and is stored confidentially in the patient's private e-health record. E-health is stored in a cloud repository that allows patients to access their medical record wherever needed. 

E-Epidemiology is information about the diseases occurring in the population: it contains diagnostic codes and other data without recording any person identifiable information. It is used for healthcare statistics, surveillance and planning.

##Data Storage   
### Azure Cloud Blob Storage ###

Microsoft Azure BLOB (Binary Large Object) cloud storage is used to store all the data according to geographic regions (GIS). There is a separate **storage container for each region** which in turn contains eleven BLOBs: ** 'P','L','E', I-0 ... to I-7** as shown in the following table:

BLOBs|Purpose|Data Structure
:--:|:------------------------------|:------------------------|
**P** | Patient e-Health|64 Kilobytes (128 pages) for each patient
**L** | Loaders (providers)| 4 Kilobytes (8 pages) for each provider
**E** | Epidemiology|All patients for the region (page offset recorded each day in an index table)
**I-0... I-7**|Images| Ranges from 32Kb to 4Mb  in 8 separate blobs


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

Patient's e-Health data aligns with 64Kb boundaries so the data location in the 'P' blob will be: 

    Start Location = ID<<16

Similarly provider data aligns with 4Kb boundaries in the 'L' blob  at 

    Start Location = ID<<12

## Processing ## 

The purpose of the software is to maximise the value of the data. To provide ways to simplify collecting the data and then fully using the data.
 
### Actionable Data / Population Health Management##
The design supports actionable data. For example e-health will flag risky behaviour such as missed appointment or vaccination, or diseases such as drug resistant TB. An scheduled cloud process will periodically check all the records in a region and will act by sending an SMS to the patient or email a healthcare visitor to follow up the problem.

Actionable data will help control serious health problems like maternal and child mortality, HIV, road accidents and violence patterns. It will be used for managing telemedicine, health visitors and appointment reminders. This will be done automatically and at low cost.

### Scheduled Processing ## 
Azure storage queues allow scheduling of tasks such as appointment reminders on a specific day. For long delays the task could be flagged in the patient's e-Health record as actionable data.

## Epidemiology ## 
Epidemiology is possibly the most important application. Before cloud computing and GIS it would have been impossible to collect, process and map geographic healthcare data in real time.

This application gives healthcare providers a simple way to record the conditions they treat every day without person identifiable data. A cloud process could sort and record epidemiology provide updated information overnight. 

(This process could be applied on a global scale at very low cost)

The table shows how epidemiology processing works.

Storage container names 'qnnee' provide a mechanism for allocating regions to one of 32 time zones (0..31 in the table) which allows data to be distributed across 8 storage queues (0..7) for separate processing. 

Epidemiology data is loaded into one of eight storage queues (0..7) according to '45 minute time zones' (0..31) as shown in the table below. By setting the queue visibility to hide the data until the owner region reaches midnight, each region data has exclusive visibility for 6 hours of processing (from midnight to 6 am). 

### Processing using Azure Queue Storage and Epidemiology data##

|queue|
|--|--|--|--|--|
|**0**|0|8|16|24|
|**1**|1|9|17|25|
|**2**|2|10|18|26|
|**3**|3|11|19|27|
|**4**|4|12|20|27|
|**5**|5|13|21|29|
|**6**|6|14|22|30|
|**7**|7|15|23|31|


##Azure Page Blob Storage #

###Blobs 'P' and L' - (Patients and Providers)
Patients, Providers and Images are stored as fixed length records at fixed offsets in their respective page blobs.

###Blob 'E (Epidemiology):
Epidemiology data is queued and then stored into the epidemiology blob by an Azure Worker Role using a daily scheduled task. 

###Blobs 'I-0' to 'I-7' .. (8 image ranges) 

#####(32KB, 64KB, 128KB, 256KB, 512KB, 1MB, 2MB, 4MB)

The idea is to have fixed length records for efficient storage and retrieval and to manage the duration of storage from 1 month to 20 years.

The storage of image data includes an intermediate step to set a 'time-to-live table' used to control how long to keep images (in months). Images, scans, ECG traces...etc will occupy most of the data storage space in e-health records. Items such as handwritten 'to-do lists' or photographs might need less than one month storage, whereas other information might need to be stored for years. The volume of storage is managed to ensure that space is always available for new images.

Images will be PDF files converted to byte arrays and stored in the lowest fitting page blob.

The image index will be stored with the patients data and efficiently retrieved for viewing.
##Other Details
The system could be used for any country or region. The initial set-up requires countries to be represented as a collection of 'qnnee' regions. This is a simple once-off task.

When the program runs page blobs for patients(P), providers/loaders(L), epidemiology(E) and the eight images blobs(I0 to I7) are automatically added as required.

Usage is similar to a standard medical practice paper records, but with the advantages of automation for storing and sharing images, digital prescriptions, telemedicine, population health management and appointment reminders.

Azure charges 0.5 cents (US) per gigabyte per month for data used. This application manages data efficiently making it practical to implement epidemiology and e-health for any country at very low cost.

Epidemiology data is particularly interesting. This application simplifies recording and uses minimal storage. It would cost (globally) less than $5 a month to store a year's worth of data!
