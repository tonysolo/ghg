

using Newtonsoft.Json;

namespace MvvmLight1.Model
{
    public class ghgaccount
    {
        string _account;
        string _container;
        string[] _regions;

        public ghgaccount(string account, string container, string[] regions)
        {
            _account = account;
            _container = container;
            _regions = regions;
        }

        public ghgaccount(string json)
        {
        }


        public string account { get { return _account; } } //could be longer than country name for available azure storage
        public string container { get { return _container; } } //short two character eg za ls sz gb for container names
        public string[] regions { get { return _regions; } } //list of qnnee regions
    }                                                        // public string toString() { return JsonConvert.SerializeObject(this); }

}



