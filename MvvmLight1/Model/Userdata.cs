using System.Collections.Generic;

namespace MvvmLight1.Model
{
    public static class Userdata
    {

        static bool _invalid;
        static Dictionary<string, string> coords;
        // string cloudid;
        // public userdata() { invalid = false; }

        static public void Loaddata(string locationPin)
        {
            //bool invalid = false;
            //cloudid = location_pin;
            //validate and read prefs and GIS from cloud        
        }

        static void AddCoords(string district, string qnnneee)
        {
            coords.Add(district, qnnneee);
            _invalid = true;

        }

        // void Save()
        // {
        //     if (invalid == true)
        //    {
        //use cloudid to locate storage
        //locate offset
        //overwrite coords location
        //    }
        //}
    }
}