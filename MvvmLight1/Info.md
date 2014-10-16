# Global Health Grid - what it does: #

This is a geographic information system (GIS) for healthcare that provides three solutions: 
e-health
epidemiology
population health management solution.


 
 
The design uses digital GIS coordinates of 5 hexadecimal characters 'QNNEE':

where 'Q' is the quadrant where '0'is northeast,'1':northwest, '2':southeast and '3' is southwest.
'NN' is latitude ('00' to '7F') and 'EE' is longitude '00' to 'FF'

 are converted to 8 bit digital values and represented as a byte array with three dimensions: Quadrant, Latitude and Longitude. 

Byte[Q][N][E] - there are 4 quadrants(Q) with 128 latitude (N) and 256 longitude divisions (E). 



 
epidemiology and healthcare and plan providers and facilities. 

My system, for example, would divide South Africa into 200 GIS regions of 80 X 80 km each. This is still a large area but limits to number of people from millions to thousands and simplifies allocating unique patient IDs, planning and using IT more efficiently with distributed data.

Each GIS region would have a Cloud based health information exchange (HIE) for storing the patients' e-health records. The HIE would support the idea of population health management (PHM) and 'actionable data'.

 Actionable data means that patients' records will include ways to 'flag' situations for periodic automatic processing, for example to track missed antenatal visits of incomplete vaccinations history and notify patients or community workers to act and resolve the problem.

This will be an advanced system to streamline treatments and to collect epidemiology data. It will be achievable at low cost. It will replace static paper records with dynamic e-health records. It will provide statistics and enable telemedicine and community workers to improve healthcare and reduce costs. It will help manage and eliminate epidemics such as HIV and TB and maternal and child mortality,

*TM*

 