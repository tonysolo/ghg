using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmLight1.Model
{
    //need to make the class public access
    //need static utility class to store patients providers and images to azure
    class provider
    {
        provider() { }
        provider(string json) { }
        string name { get; set; }
        string gender { get; set; }
        string specialty { get; set; }
        string qualification { get; set; }
        string[] commonicds { get; set; }  //top40
        string[] commonvisits { get; set; }  //to40
        string[] commonprescripions { get; set; }
        patient[] recentpatients { get; set; }
        string tojson() { return ""; }
        byte[] toUTF8data() { return null; } //put this in shared library
    }
}
