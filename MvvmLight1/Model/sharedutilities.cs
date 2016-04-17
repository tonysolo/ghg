using System;

//using System.Linq;
//using System.Text;
//using System.Xml.Linq;
//using System.IO;
//using System.Net;
//using System.Web;

namespace MvvmLight1.Model
{
    internal enum Gender
    {
        Female,
        Male
    }

    internal enum Agegroup
    {
        Neonate,
        FirstYear,
        TwoToSixYears,
        SevenToTwelve,
        TeenAge,
        TwentyToForty,
        FortyToSixty,
        SixtyAndOlder
    }

    internal enum Facilty
    {
        ConsultingRoom,
        Clinic,
        Level1Hosp,
        Level2Hosp,
        Level3Hosp
    }

    internal enum Stay
    {
        Outpatient,
        Inpatient
    }

    internal enum Funder
    {
        Public,
        Private
    }

    internal enum Treater
    {
        Generalist,
        ObstetsGynae,
        Paediatrics,
        PaediatricSurgery,
        Anaesthetics,
        Surgery,
        Ent,
        Orthopaedics,
        Opthalmology,
        Emergency,
        Plastics,
        Dermatology,
        Neurology,
        Neurosurgery,
        Pathology,
        Forensics
    }

    internal enum Providers
    {
        Coder,
        ParaMedic,
        Nurse,
        Therapist,
        GeneralPractitioner,
        Specialist
    }

    internal enum Countries
    {
        SouthAfrica,
        UnitedKingdom
    }


    /// <summary>
    ///     collection of static methods for gis reqions and filenames
    /// </summary>
    internal static class Qnnee
    {
        /// <summary>
        /// </summary>
        /// <param name="coords"></param>
        /// <returns> decimal string to 40 arc min distict name</returns>
        private static string CoordsToQnnee(string coords)
        {
            char[] ca = {','};
            string[] sarr = coords.Split(ca);
            double lat = Convert.ToDouble(sarr[0]);
            double lon = Convert.ToDouble(sarr[1]);
            bool neglat = (lat < 0);
            bool neglon = (lon < 0);
            int q = 0;
            q = (neglat && neglon) ? 3 : (neglat) ? 2 : 1;
            var ilat = (int) Math.Round(lat/180*256);
            var ilon = (int) Math.Round(lon/180*256);
            return string.Format("{0:X1}{1:X2}{2:X2}", q, ilat, ilon);
        }

        /// <summary>
        ///     low resolution csv string point from 8 bit integers 0 - ff = 180 degrees
        /// </summary>
        /// <param name="q"></param>
        /// <param name="n"></param>
        /// <param name="e"></param>
        /// <returns>csv string latid,longit</returns>
        private static string Point(Byte q, Byte n, Byte e)
        {
            float fn = n/128*90;
            float fe = e/256*180;
            if (q == 1) fe *= -1;
            if (q == 2) fn *= -1;
            if (q != 3) return String.Format("{0:0,0},{1:0,0}", fn, fe);
            fe *= -1;
            fn *= -1;
            return String.Format("{0:0,0},{1:0,0}", fn, fe);
        }

        /// <summary>
        ///     high resolution csv string point from 12 bit integers 0 - fff = 180 degrees
        /// </summary>
        /// <param name="q"></param>
        /// <param name="n"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private static string Point(ushort q, ushort n, ushort e)
        {
            float fn = n/2048*90;
            float fe = e/4096*180;
            if (q == 1) fe *= -1;
            if (q == 2) fn *= -1;
            if (q != 3) return $"{fn:0,0},{fe:0,0}";
            fe *= -1;
            fn *= -1;
            return $"{fn:0,0},{fe:0,0}";
        }

        /// <summary>
        /// </summary>
        /// <param name="qne"></param>
        /// <returns>four points (in a csv string) that are the corners of the region</returns>
        private static string Boundary(string qne) //qnnneee or qnnee
        {
            var coords = new string[4];
            if (qne.Length == 7)
            {
                coords[0] = qne;
                coords[1] = ""; //incy
                coords[2] = ""; //incxy
                coords[3] = ""; //incx
            }

            ushort q = 0, n = 0, e = 0, q0 = 0, e0 = 0, n0 = 0;

            if (qne.Length == 5)
            {
                q0 = q = Convert.ToUInt16(qne[0]);
                n0 = n = Convert.ToUInt16(qne.Substring(1, 2), 16);
                e0 = e = Convert.ToUInt16(qne.Substring(3, 2), 16);

                if (n < 127) //cant calc to the poles
                {
                    if (e < 255) e = (ushort) (e + 1);
                    else q = (q > 1) ? (ushort) (5 - q) : (ushort) (1 - q); //toggle quadrant east west
                    n++;
                }
            }
            char[] s = {','};
            string[] coord1 = Point(q0, n0, e0).Split(s);
            string[] coord2 = Point(q, n, e).Split(s);

            return $"\"{coord1[0]},{coord1[1]}\"{coord2[0]},{coord1[0]}\"{coord2[1]}{coord1[1]}\"{coord1[1]}{coord2[1]}";
        }

        private static int SecsToMidnight(string qe)
        {
            byte q = Convert.ToByte(qe[0]);
            int e = Convert.ToByte(qe.Substring(1, 1), 16)*45*60;
            DateTime dt = DateTime.UtcNow;
            int secs = dt.Hour*60*60 + dt.Minute*60 + dt.Second;
            secs = (q == 1 || q == 3) ? secs + e : secs - e;
            //const int y = 0; // = longit 0 SecsToMidnight              
            return 86400 - secs%86400;
        }

        private static string ToCsv(string[] sarr)
        {
            foreach (var s in sarr) return s.Replace(',', ';');
            return string.Join(",", sarr);
        }
    }
}