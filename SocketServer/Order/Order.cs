using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace SocketServer
{
    class Order
    {
        public Order()
        {
            this.Day = DateTime.Now.Day;
            this.Month = DateTime.Now.Month;
            this.Year = DateTime.Now.Year;
            this.Hour = DateTime.Now.Hour;
            this.Minute = DateTime.Now.Minute;
        }




        [JsonProperty(PropertyName = "day")]
        public int Day { get; set; }

        [JsonProperty(PropertyName = "month")]
        public int Month { get; set; }

        [JsonProperty(PropertyName = "year")]
        public int Year { get; set; }


        [JsonProperty(PropertyName = "hour")]
        public int Hour { get; set; }

        [JsonProperty(PropertyName = "minute")]
        public int Minute { get; set; }

  



    }
}
