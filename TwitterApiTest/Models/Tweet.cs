using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterApiTest.Models
{
    public class Tweet
    {
        public long id { get; set; }
        public string keyword { get; set; }
        public string name { get; set; }
        public string text { get; set; }
        public DateTimeOffset date { get; set; }

        public override string ToString()
        {
            return $"{this.date.ToString("yyyy/MM/dd HH:mm:ss")}\n{this.name}\n{this.text}";
        }
    }
}
