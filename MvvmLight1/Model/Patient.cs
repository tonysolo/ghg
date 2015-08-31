using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmLight1.Model
{
    class patient
    {//need to make public access
        string name { get; set; }
        string age { get; set; }
        string gender { get; set; }
        string cellphone { get; set; }
        string email { get; set; }
        string address { get; set; }
        string qnnneee { get; set; }
        string[] alerts { get; set; }
        patient nextofkin { get; set; }
        List<visit> visits { get; set; }
        List<patient> dependants { get; set; }
    }
}
