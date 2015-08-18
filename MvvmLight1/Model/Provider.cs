using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmLight1.Model
{
    class provider
    {
        string name { get; set; }
        string specialty { get; set; }
        string qualification { get; set; }
        string[] commonicds { get; set; }  //top40
        string[] commonvisits { get; set; }  //to40
        string[] commonprescripions { get; set; }
        patient[] recentpatients { get; set; }
    }
}
