

using Newtonsoft.Json;

namespace MvvmLight1.Model
{
    public class Ghgaccoun
    {
        public Ghgaccount(string account, string container, string[] regions)
        {
            this.Account = account;
            this.Container = container;
            this.Regions = regions;
        }

        public Ghgaccount(string json)
        {
        }


        public string Account { get; } //could be longer than country name for available azure storage
        public string Container { get; } //short two character eg za ls sz gb for Container names
        public string[] Regions { get; } //list of qnnee Regions
    }                                                        // public string toString() { return JsonConvert.SerializeObject(this); }

}



