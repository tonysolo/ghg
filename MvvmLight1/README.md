# Global Health Grid (GHG)#

### e-Health Records ##
e-Health is sought by all countries as the means to contain costs and improve access to healthcare. Unfortunately there have been failures in some countries and resistance to change without clear clinical benefits. This implementation is a simple clinical record of the patients alerts and chronic medication, risks and preferences and short clinical notes for each visit. Unlike other implementations it can store handwritten notes and scans as and other information for patients to share with any healthcare provider they consult. It is provides population health management using actionable data. The data is stored using Microsoft Azure Page blobs and includes storage of text as well as images such as scans, handwritten notes, xrays, ECGs and future data sources such as wearable devices.

###Scope #

This is a global scale application to store Healthcare information for Epidemiology and E-Health. It uses *Cloud Storage and GIS Maps* for storing and retrieving health data using a *Grid system* that defines regions of 42 arc minutes - 80 X 80 Km at the equator.

GHG has two purposes: *e-Epidemiology* and  and *e-health*.

E-Health is about clinical management. It contains details for treatment, prescriptions and referrals. E-health data belongs to the patient and is stored confidentially in the patient's private e-health record. E-health records are stored in a repository that allows patients to access their medical record wherever needed. 

E-Epidemiology is information about the diseases occurring in the population: it contains diagnostic codes and other data without any person identifiable information and used for healthcare statistics, surveillance and planning.

##Data Storage   
### Azure Cloud Blob Storage ###

Microsoft Azure blob storage is used to store all the data according to geographic regions (GIS). There is a separate **storage container for each GIS region** which in turn contains eleven binary large objects (BLOB's)** - 'P','L','E', I0 ... to I7** as shown in the following table:

BLOB|Purpose|Data Structure
:--:|:------------------------------|:------------------------|
**P** | Patient e-Health|64 Kilobytes (128 pages) for each patient
**L** | Loaders (providers)| 1 Kilobyte (2 pages) for each provider
**E** | Epidemiology|All patients for the region (variable length allocated for each day using index table)
**I0 .. I7**|Images| Ranges from 32Kb to 4Mb  in 8 separate blobs


### Azure Container Names ('qnnee') ### 

Container names are strings consisting of 5 hexadecimal characters defining a GIS region "qnnee" (quadrant, latitude and longitude).

    nn = degrees latitude / 180 * 256;
    ee = degrees Longitude /180 * 256;
    q == 0 for NE
    q == 1 for NW
    q == 2 for SE
    q == 3 for SW

The "qnnee" values range from "00000" to "37fff" and represent regions separated by 42 arc minutes.

### Patient and Provider ID's ##

Unique ID's for patients and providers are equivalent to their data offset in the 'P' blob or 'L' blobs.

Patient's e-Health data aligns with 64Kb boundaries so the data location in the 'P' blob will be: 

    Start Location = ID<<16

Similarly provider data aligns with 4Kb boundaries in the 'L' blob  at 

    Start Location = ID<<12

## Processing ## 

The purpose of the software is to maximise the value of the data. To provide ways to simplify collecting and then fully using the data.
 
### Actionable Data / Population Health Management##
The design supports actionable data. The e-health record will flag risky behaviour such as a missed appointment or vaccination. An Azure worker role periodically checks all the records in a region and could, for example, act by sending an SMS to the patient or a healthcare visitor to follow up the problem.

Similarly actionable data could be a solution to many serious health problems like maternal and child mortality, HIV and TB medication compliance. It will also be valuable for organising telemedicine, health visitors and appointment reminders. This would be done automatically and at low cost.

### Scheduled Processing ## 
Azure storage queues will be used for scheduling tasks such as appointment reminders on a specific day. For long delays the task could be flagged in the patient's e-Health record as actionable data.

## Epidemiology ## 
Epidemiology is possibly the most important property of the application. Before cloud computing and GIS it would have been impossible to collect, process and map geographic healthcare data in real time.
This application gives healthcare providers a simple way to record the conditions they treat every day without person identifiable data. A cloud process continually sorts and files epidemiology for regions and the country and updates the information overnight.

The table shows how epidemiology works. Data is spread across 8 storage queues and setting the visibility to hide the data until the owner region reaches midnight. At that time the data for the region becomes visible for processing. This arrangement extends the processing time to 6 hours a day for each region. 

### Processing using Azure Queue Storage and Epidemiology data##

|queue |
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
###Blobs 'P' and L' Patients and Providers (Loaders)
Patient, Provider and Image data are fixed length records and stored at fixed offsets in their respective page blobs. Azure processing provides a useful 'GetPageRanges(offset,length)' method to retrieve the occupied page ranges from specific offsets to guide efficient retrieval and editing. 

###Blob 'E (Epidemiology):
Epidemiology data is queued and then stored into the 
epidemiology blob by an Azure Worker Role using a daily scheduled task. 

###Blobs 'I0' to 'I11' .. (12 image ranges) 

Six Kilobyte Ranges: 16,32,64,128,256 and 512KB 
Six Megabyte Ranges: 1,2,4,8,16 and 32MB
The idea is to have fixed length records for efficient storage and retrieval and to manage the duration of storage from 1 month to 20 years.

The storage of image data includes an intermediate step to set a 'file allocation table' to track how long to keep images (in weeks). Images, scans, ECG trace...etc will occupy most of the data storage space in e-health records. Items such as handwritten 'to-do lists' will require less than one week storage, whereas other information might need to be stored for years. The volume of storage is managed to ensure that space is always available for new images.

The file allocation table (FAT) provides a single byte to store the number of weeks to store the image (maximum 255 months or 20 years, 0 = deleted at month end). Each entry (position) in the FAT corresponds to the image data offset and its value is decremented weekly (using an Azure Worker Role weekly scheduled task). The image data is deleted and space recycled after its FAT value passes zero.

Images will be PDF files converted to byte arrays and stored in the lowest fitting page blob.

The image index will be stored with the patients data and efficiently retrieved for viewing.
##Other Details
Organising countries into 'qnnee' regions is a once-off task that requires a few minutes to set up each country.

When the program runs page blobs for patients(P), providers/loaders(L), epidemiology(E) and the eight images blobs(I0 to I7) are automatically added as required.

Azure charges 0.5 cents (US) per gigabyte only for data used. This application manages data efficiently making it practical to implement epidemiology and ehealth for any country at very low cost.

The process is similar to a standard medical practice paper records with the advantages of automation for storing images, digital prescriptions, telemedicine, population health management and appointment reminders.

The application is presently implemented for Windows WPF with plans for other platforms in future. It would be very practical for tablet devices.

 


