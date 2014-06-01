using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmLight1.Model
{
    enum gender { female, male }
    enum agegroup { Neonate, FirstYear, TwoToSixYears, SevenToTwelve, TeenAge, TwentyToForty, FortyToSixty, SixtyAndOlder }
    enum facilty {ConsultingRoom, Clinic, Level1Hosp, Level2Hosp, Level3Hosp }
    enum stay { Outpatient, Inpatient }
    enum funder { Public, Private }
    enum treater { Generalist, ObstetsGynae, Paediatrics, PaediatricSurgery, Anaesthetics, Surgery, ENT, Orthopaedics, Opthalmology, Emergency, Plastics, Dermatology, Neurology, Neurosurgery, Pathology, Forensics }
    enum qualification { ParaMedic, Nurse, Therapist, GeneralPractitioner, Specialist }

    /// <summary>
    /// collection of static methods for gis reqions and filenames
    /// </summary>
    static class qnnee
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="coords"></param>
        /// <returns> decimal string to 2.5 arc min distict name </returns>
        static string coordsToQNNNEEE(string coords)
        {
            char[] ca = { ',' };
            string[] sarr = coords.Split(ca);
            double lat = Convert.ToDouble(sarr[0]);
            double lon = Convert.ToDouble(sarr[1]);
            bool neglat = (lat < 0);
            bool neglon = (lon < 0);
            int q = (neglat && neglon) ? 3 : (neglat) ? 2 : (neglon) ? 1 : 0;
            int ilat = (int)Math.Round(lat / 180 * 4096);
            int ilon = (int)Math.Round(lon / 180 * 4096);
            return string.Format("{0:X1}{1:X3}{2:X3}", q, ilat, ilon);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="coords"></param>
        /// <returns> decimal string to 40 arc min distict name</returns>
        static string coordsToQNNEE(string coords)
        {
            char[] ca = { ',' };
            string[] sarr = coords.Split(ca);
            double lat = Convert.ToDouble(sarr[0]);
            double lon = Convert.ToDouble(sarr[1]);
            bool neglat = (lat < 0);
            bool neglon = (lon < 0);
            int q = (neglat && neglon) ? 3 : (neglat) ? 2 : 1;
            int ilat = (int)Math.Round(lat / 180 * 256);
            int ilon = (int)Math.Round(lon / 180 * 256);
            return string.Format("{0:X1}{1:X2}{2:X2}", q, ilat, ilon);
        }

        /// <summary>
        /// low resolution csv string point from 8 bit integers 0 - ff = 180 degrees
        /// </summary>
        /// <param name="q"></param>
        /// <param name="n"></param>
        /// <param name="e"></param>
        /// <returns>csv string latid,longit</returns>
        static string point(Byte q, Byte n, Byte e)
        {
            float fn = n / 128 * 90;
            float fe = e / 256 * 180;
            if (q == 1) fe *= -1;
            if (q == 2) fn *= -1;
            if (q == 3) { fe *= -1; fn *= -1; }
            return String.Format("{0:0,0},{1:0,0}", fn, fe);
        }

        /// <summary>
        /// high resolution csv string point from 12 bit integers 0 - fff = 180 degrees
        /// </summary>
        /// <param name="q"></param>
        /// <param name="n"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        static string point(UInt16 q, UInt16 n, UInt16 e)
        {
            float fn = n / 2048 * 90;
            float fe = e / 4096 * 180;
            if (q == 1) fe *= -1;
            if (q == 2) fn *= -1;
            if (q == 3) { fe *= -1; fn *= -1; }
            return String.Format("{0:0,0},{1:0,0}", fn, fe);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="qnnee"></param>
        /// <returns>lower left point of district or region</returns>
        static string point(string qnnee)
        {
            UInt16 q, n, e;

            if (qnnee.Length == 5)
            {
                q = Convert.ToUInt16(qnnee[0]);
                n = Convert.ToUInt16(qnnee.Substring(1, 2), 16);
                e = Convert.ToUInt16(qnnee.Substring(3, 2), 16);
                float fn = n / 128 * 90;
                float fe = n / 256 * 180;
                if (q == 1) fe *= -1;
                if (q == 2) fn *= -1;
                if (q == 3) { fe *= -1; fn *= -1; }
                return String.Format("{0},{1}", fn, fe);
            }
            if (qnnee.Length == 7)
            {
                q = Convert.ToUInt16(qnnee[0]);
                n = Convert.ToUInt16(qnnee.Substring(1, 3), 16);
                e = Convert.ToUInt16(qnnee.Substring(4, 3), 16);
                float fn = n / 2048 * 90;
                float fe = n / 4096 * 180;
                if (q == 1) fe *= -1;
                if (q == 2) fn *= -1;
                if (q == 3) { fe *= -1; fn *= -1; }
                return String.Format("{0},{1}", fn, fe);
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qne"></param>
        /// <returns>four points (in a csv string) that are the corners of the region</returns>

        static string Boundary(string qne)
        {
            UInt16 q = 0, n = 0, e = 0, q0 = 0, e0 = 0, n0 = 0;
            if (qne.Length == 5)
            {
                q0 = q = Convert.ToUInt16(qne[0]);
                n0 = n = Convert.ToUInt16(qne.Substring(1, 2), 16);
                e0 = e = Convert.ToUInt16(qne.Substring(3, 2), 16);

                if (n < 127)
                {
                    if (e < 255) e = (UInt16)(e + 1); //toggle east west
                    else if (q > 1) q = (UInt16)(5 - q);
                    else q = (UInt16)(1 - q);
                    n++;
                }
            }
            char[] s = {','};
            string[] coord1 = point(q0, n0, e0).Split(s);
            string[] coord2 = point(q, n, e).Split(s);

            return String.Format("\"{0},{1}\"{2},{3}\"{4}{5}\"{6}{7}",
             coord1[0], coord1[1], coord2[0], coord1[0], coord2[1], coord1[1], coord1[1], coord2[1]);
        }


    }
}
