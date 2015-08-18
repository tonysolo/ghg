using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmLight1.Model
{
    class patient
    {
        string name { get; set; }
        string age { get; set; }
        string gender { get; set; }
        List<visit> visits { get; set; }
        List<patient> dependants { get; set; }
    }
}
