using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MvvmLight1.Model
{
  
        public class ghgstorage
        {
            string _account;
            string _container;
            string[] _regions;

            public ghgstorage(string account, string container, string[] regions)
            {
                _account = account;
                _container = container;
                _regions = regions;
            }

            public ghgstorage(string json)
            {
                var v = (ghgstorage)JsonConvert.DeserializeObject(json);
                _account = v._account;
                _container = v._container;
                _regions = v._regions;
            }


            public string account { get { return _account; } } //could be longer than country name for available azure storage
            public string container { get { return _container; } } //short two character eg za ls sz gb for container names
            public string[] regions { get { return _regions; } } //list of qnnee regions
           // public string toString() { return JsonConvert.SerializeObject(this); }

        }

    }

