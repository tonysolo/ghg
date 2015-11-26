using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace MvvmLight1.Model
{

    ////need static utility class to store patients providers and images to azure
    ////providers are clinical providers - loaders include clerical to register patients
    ////Provider app will list all their current patients next appointments and options to check for updates by other providers
    ////providers get 4 megabytes storage so they can store a local copy of their recent patients and other important data

    ////data layout:

    //// 0 name,address,cell,email,qnnneee,regauth,regnum,specialty,qualif
    //// 1  commICDs ,,,
    //// 2  commVisits ,,,
    //// 3  commVisits presciptions ,,,
    //// 4  recentPatients qnneexxxx plus data ,,,


    //public class Provider : Person
    //{
    //    Provider() { }
    //    Provider(string csv) { }  //?use json
    //    //string name { get; set; }
    //    //string Gender { get; set; }
    //    //public string qnnneee { get; set; }
    //    string RegAuthority { get; set; }
    //    string RegNumber { get; set; }
    //    string Specialty { get; set; }
    //    string Qualification { get; set; }
    //    string[] Commonicds { get; set; }  //top40
    //    string[] Commonvisits { get; set; }  //top40
    //    string[] Commonprescripions { get; set; } //top40
    //    private readonly List<Patient> _recentpatients = new List<Patient>();//was observablecollection
    //    string Tocsv() { return ""; }  //?tojson

    //    byte[] ToUtf8Data() { return null; } //put this in shared library

    //    //gets patient from azure storage and saves it locally to recentpatient list

    //    bool Getpatient(string qnexxx)
    //    {
    //        var p = new Patient
    //        {
    //            name = "Tony Manicom",
    //            Dateofbirth = new DateTime(1948, 7, 8),
    //            Gender = 'M'
    //        };
    //        _recentpatients.Add(p);
    //        //var p1 = _recentpatients[0];
    //        //_recentpatients.
    //        //check Patient is in same county as Provider
    //        //and check if Patient exists
    //        //Patient p = Getpatient
    //        //_recentpatients.Add(new Patient());
    //        return true;
    //    }

    //    //looks up Id number cross reference to azure qnneexxxx to load Patient
    //    private bool getpatient_id(string countryid)
    //    {
    //        return true;
    //    }  //or gets qnneexxx lookup if passes country check


    //    // opens patreg dialog
    //    // includes country id for access as well
    //    bool Registerpatient()
    //    {
    //        return true;
    //    }

    //    //removes the oldest accessed 
    //    void Trimrecentpatients()
    //    {
            
    //    }


    //    //checks and updates changes by provide
    //    void Updatepatient(int ndx)
    //    {
            
    //    }
    //}
}
