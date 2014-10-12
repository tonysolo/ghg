A practical e-Health implementation

Cloud computing and cloud storage continually improve and become cheaper. Similarly mobile devices with Internet access have become affordable to use in poor countries. It should be possible to use these technologies to help improve healthcare and raise living standards in these countries.

In this article I describe two related Cloud applications - e-health and e-epidemiology. More information is available at my code repository at tonysolo@github.com.

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






