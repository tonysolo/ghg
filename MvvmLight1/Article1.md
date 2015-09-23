# A Practical e-Health Solution #

This application is presently under development. It is an e-health application based on **GIS** (a geographic information system).

----------

There is worldwide interest in e-health, encouraged by the World Health Organisation. Some countries have already implemented it, others are in the process of doing so. 

A particular case for e-health is to ensure basic healthcare in poor countries: that mothers receive antenatal care and safe childbirth, children are vaccinated, and HIV and TB are treated. 

A low cost and large scale e-health system is possible using cloud storage and processing. This could improve healthcare and living standards in needy countries and benefit millions of people.

The system I plan uses Microsoft Azure cloud storage. It includes registration for health providers and patients, patients' e-health, image storage and large scale epidemiology.

Data is indexed according to geographic region which will allow epidemiology it to be combined with similarly indexed population and economic data from the 'Socioeconomic Data and Applications Centre data' (SEDAC).

## Registration of Providers and Patients ##

![](http://i.imgur.com/JMM64rB.jpg)

Patients register in their region of birth or their first region in the country. Patients will be able to control and limit access to their e-health data if they choose to. Similarly healthcare providers register in the region where they work.

## How it works ##

###Health Information Exchanges

 Each region works as a separate health information exchange (**HIE**). When patients register they are allocated a unique 'next in sequence ID' for their region and their e-health record will always be located in that region. However patients are not restricted to their region and may seek treatment in any other region and by any provider.

The GIS based HIE has many advantages as a result of linking geographic, economic and healthcare data with population health management and actionable data. 

###e-Health, population health management, and e-Epidemiology.

**e-Health** deals with individual patients. It stores their preferences, risks, medical histories and medications. It manage storage of images such as scanned documents, x-rays and and handwritten notes. It handles prescriptions and referrals and provides** population health management** features including automatically sending SMS appointment reminders to patients, flagging missed appointments and special risks such as disabilities and high risk pregnancies, alerting community workers using SMS or email or referring patients to telemedicine providers.

**e-Epidemiology** provides constantly updated statistics on a large scale without recording any specific patient details. Epidemiology data will be available for any disease, health facility type, provider type, geographic region, or date range. 

Finally, the application will poll opinions from providers and provide them with e-health statistics to help achieve continuous improvement.

"Plan, Do, Check, Adjust".

----------

Tony Manicom


