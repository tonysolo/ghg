# Global Health Grid (GHG)#
#Processing #

The  purpose of this application is to store Healthcare information for Epidemiology and E-Health. It uses *Cloud Storage and GIS Maps* for storing and retrieving health data using a *Grid system* that defines regions of approximately 80 X 80 Km at the equator.

GHG has two applications: *Epidemiology* (e-epidemiology) and *Patient clinical records* (e-health).

### e-Epidemiology and e-Health ###
 
Both applications share features so they have been combined in a single program for recording patient clinical records and epidemiology statistics.

E-Health is clinical information. It contains details for treatment, prescriptions and referrals. It belongs to the patient and is stored privately in the patient's private e-health record.

E-Epidemiology is information about the disease: the ICD10 diagnostic codes and other data - it uses no person identifiable information and it is kept in shared storage and used for epidemiology statistics and surveillance.

   
### GHG - Azure Cloud Blob Storage Containers###

Microsoft Azure blob storage is used to store data. There is a separate **storage container for each GIS region** that in turn **contains three page blobs**. Each region is set up the same way: a unique container name containing with three page blobs **named 'P','L' and 'E'**

Name|Purpose|Data Structure
:--:|:------------------------------|:------------------------|
**P** | Patient e-Health|64 Kilobytes (128 pages) for each patient
**L** | Loaders (providers)| 1 Kilobyte (2 pages) for each provider
**E** | Epidemiology|All patients for the region (variable length allocated for each day using index table)
**I[0..11]**|Images|Images from 16Kb to 32Mb ranges in separate blobs

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
There is much confusion about e-health, dissatisfaction and failures in some countries including the UK. This implementation is a simple clinical record of the patients alerts and chronic medication, risks and preferences and short clinical notes for each visit. Unlike other implementations, this is a complete health information registry for patients to share their information with any healthcare provider they consult. It is designed for population health management using actionable data, for individual patients information and for recording epidemiology in near real time. The data is stored using Microsoft Azure Page blobs and includes storage of text as well as images such as scans, handwritten notes, xrays, ECGs and future data sources such as wearable devices.

### Actionable Data / Population Health Management##
The design supports actionable data. The e-health record will flag risky behaviour such as a missed appointment or vaccination. An Azure worker role periodically checks all the records in a region and acts by sending an SMS to the patient or a healthcare visitor to follow up the problem.

Actionable data could be a solution to many serious health problems like maternal and child mortality, HIV and TB medication compliance. It will be valuable for organising telemedicine and for implementing Population Health Management. This would be done automatically and at low cost.

### Scheduled Processing ## 
Azure storage queues will be used for scheduling tasks such as sending appointment reminders to patients on a specific day. For long delays the task could be flagged in the patients e-Health record as actionable data.

## Epidemiology ## 
Epidemiology is possibly the most important feature of the application. Before cloud storage, computing and GIS it would have been impossible to collect, process and map complete geographic healthcare data in real time.
This application gives healthcare providers a simple way to upload a short record of the conditions they treat every day. A cloud process continually sorts and files the information according to region and time.
The table shows how it works. Data is spread across 8 storage queues and setting the visibility to hide the data until the owner region reaches midnight (solar time). At that time the data for the region becomes visible for processing. This arrangement extends the processing time to 6 hours a day for each region. 

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


#Azure Page Blob Storage #
###Blobs 'P' and L' Patients and Providers (Loaders)
Patient and Provider data are fixed length records and stored at fixed offsets in their respective page blobs. Azure provides a useful 'GetPageRanges(offset,length)' method to retrieve to occupied page ranges at specific offsets to guide retrieval and editing details. This process is handled by the client software. 

Epidemiology data is queued and then stored into the Epidemiology blob by an Azure Worker Role daily scheduled task.

###Blobs 'I0' to 'I11' .. (12 image ranges) 

Six kilobyte Ranges: 16,32,64,128,256 and 512KB 
Six megabyte Ranges: 1,2,4,8,16 and 32MB

The storage of image data includes an intermediate step to set a 'file allocation table' to track how long to keep images (in weeks). Images, scans, ECG trace...etc will occupy most of the data storage space in e-health records. Items such as handwritten 'to-do lists' will require less than one week storage, whereas other information might need to be stored for years. The volume of storage is managed to ensure that space is always available for new images.

The file allocation table (FAT) provides a single byte to store the number of weeks to store the image (maximum 255 = 256 weeks, 0 == deleted at week end). Each entry (position) in the FAT corresponds to the image data offset and its value is decremented weekly (using an Azure Worker Role weekly scheduled task). The image data is deleted and space recycled after its FAT value passes zero.

Images will be PDF files converted to byte arrays and stored in the lowest fitting page blob.

The image index will be stored with the patients data and easily retrieved for viewing.
##Summary
Organising countries into 'qnnee' regions is a simple process requiring a few minutes to complete.

Each 'qnnee' region serves as a container and is automatically when the program runs with page blobs for patients(P) ,providers/loaders(L), epidemiology(E) and twelve images blobs(I0 to I11).

Azure charges 0.5 cents (US) per gigabyte for data used. This application manages data efficiently making it practical to implement epidemiology and ehealth for any country at low cost.

The ehealth design is similar to a standard paper record medical practice but with the advantage of digital storage of patient records, images, digital prescriptions, telemedicine and population health management and appointment reminders.

The application is presently implemented for Windows WPF.

 


