using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmLight1.Model
{//need to make public access
    class imageprocessor
    {
        string qnnee { get; set; }
        UInt16 offset { get; set; }
        ushort length { get; set; }
        char imagesize { get; set; }
        //tostring() return .... qnnee | image range | offset | length 
        // or image range | page offset | byte length // because qnnee is the same as the patient region
        byte[] image { get; set; }
        byte[] compress(byte[] b) { return b; } //use dotnet compress gzip
        byte[] decompress(byte[] b) { return b; } //use dotnet gzip
    }
}
