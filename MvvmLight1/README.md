# Global Health Grid - Storage and Processing #

The  purpose of this application is to store Healthcare data for Epidemiology and EHealth. It uses *Microsoft Azure Cloud Storage and Microsoft Bing Maps* for storing and retrieving health data using a *GIS grid that represents regions of approximately 80 X 80 Km at the equator*.

GHG has two important applications: *Epidemiology* (e-epidemiology) and *Patient clinical records* (e-health).

### e-Epidemiology and e-Health ###
 
For both applications healthcare providers use the system to record healthcare data.

E-Health is clinical information. It contains details for treatment, prescriptions and referrals. It belongs to the patient and is stored directly in the patient's private e-health record.

E-Epidemiology contains information about the disease: the ICD10 diagnostic codes and other data and it excludes person identifiable information. It is sorted and filed in shared storage and used for epidemiology statistics and surveillance.

   
### GHG - Azure Cloud Blob Storage Containers###

Microsoft Azure blob storage uses containers which then contain the blobs. For this application there a separate **container** for each GIS region in turn contains three page blobs. Each region is set up the same way: a unique container name containing with three page blobs named 'P','L' and 'E'

Name|Purpose|Data Structure
:--:|:------------------------------|:------------------------|
P | Patient e-Health|64 Kilobytes (128 pages) for each patient
L | Loaders (providers)| 1 Kilobyte (2 pages) for each provider
E | Epidemiology|All patients for the region (variable length allocated for each day using index table)


### Container Names ('qnnee') ### 

Container names are strings consisting of 5 hexadecimal characters, "qnnee"

    nn = degrees latitude / 180 * 256;
    ee = degrees Longitude /180 * 256;
    q == 0 for NE
    q == 1 for NW
    q == 2 for SE
    q == 3 for SW

The "qnnee" values range from "00000" to "37fff" and represent regions separated by 42 arc minutes.

### Patient and Provider ID's ##

Unique ID's consist of the region and the page offset in the 'P' blob or 'L' blob for patients or providers.

For locating data, patient e-Health data will align with 64Kb boundaries so the location in the 'P' blob will be: 


    Start Location = ID<<16



The purpose of the software is to maximise the value of the data. To provide ways to simplify collecting and using the data.

## e-Health Processing ## 
 
### e-Health Records ##
There is much confusion about e-health, dissatisfaction and failures in some countries including the UK. This implementation is a simple clinical record of the patients alerts and chronic medication, risks and preferences and short clinical notes for each visit. It doesn't directly set aside space for huge image data or continuing monitoring in operating theatres. Unlike other implementations, it is a complete health information registry for patients to share with any healthcare provider. It is designed for population health management using actionable data for individual patients and for recording and mapping epidemiology in near real time.

### Actionable Data ##
This Cloud Storage design is well suited for 'actionable data'. The patient e-health record could, for example use a location flag events for risky behaviour such as missed appointments or vaccination. An Azure worker role could periodically check all the records in a region and act by sending an SMS to the patient of healthcare visitor to follow up.

Actionable data could be a solution to many serious health problems like maternal and child mortality, HIV and TB. It could be valuable for organising telemedicine and for implementing Population Health Management. Furthermore this could be done at low cost.

### Scheduled Processing ## 
Azure storage queues could be used for scheduling tasks such as sending appointment reminders to patients on a specific day. For long delays the task could be flagged in the patients e-Health record.

## Epidemiology ## 
Epidemiology is possibly the most important feature of this software. Before cloud storage and computing and GIS was impossible to collect, process and map complete geographic healthcare data in real time.
This application gives healthcare providers a simple way to upload a short record of the conditions they treat every day. A cloud process continually sorts and files the information according to region and time.
The table shows how it works. Data is queued across 8 storage queues and setting the visibility to hide the data until the owner region reaches midnight. At that time the data for the region becomes visible for processing.  

### Processing using Azure Queue Storage and Epidemiology data##

|queue |
|--|--|--|--|--|
|0|0|8|16|24|
|1|1|9|17|25|
|2|2|10|18|26|
|3|3|11|19|27|
|4|4|12|20|27|
|5|5|13|21|29|
|6|6|14|22|30|
|7|7|15|23|31|

## Images ##

There are mobile device scanner applications that could be used for recording hand-written notes and copying documents. These and other images will be compressed and stored as block blobs in the region container they will be automatically named and referenced in the patient's data.