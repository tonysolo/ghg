﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmLight1.Model
{
   
    //need static utility class to store patients providers and images to azure
    //providers are clinical providers - loaders include clerical to register patients
    //provider app will list all their current patients next appointments and options to check for updates by other providers
   public class provider : person
    {
        provider() { }
        provider(string json) { }
        //string name { get; set; }
        //string gender { get; set; }
        //public string qnnneee { get; set; }
        string reg_authority { get; set; }
        string reg_number { get; set; }
        string specialty { get; set; }
        string qualification { get; set; }
        string[] commonicds { get; set; }  //top40
        string[] commonvisits { get; set; }  //top40
        string[] commonprescripions { get; set; } //top40
        List<patient> recentpatients {get; set;}
        string tojson() { return ""; }

        byte[] toUTF8data() { return null; } //put this in shared library

        bool getpatient(string qnexxx) //? combine with getpatient_sa_id by checking if starts with valid qnnee
        {
            patient p = new patient();
            //check patient is in same county as provider
            //and check if patient exists
            //patient p = getpatient
            if (p == null) return false;
            recentpatients.Add(new patient());
            return true;
        }

        bool getpatient_sa_id(string sa_id)//looks up sa id number cross reference to qnneexxxx to load patient 
        { return true; }

        bool registerpatient()
        {  // opens patreg dialog
           //must use saID to register that as well

            return true;
        }

        void trimrecentpatients() //removes the oldest accessed 
        { }

        void updatepatient(int ndx) {}     //checks and updates changes by provides 


    }
}
