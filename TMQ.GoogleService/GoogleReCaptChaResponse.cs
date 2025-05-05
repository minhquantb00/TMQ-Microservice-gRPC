using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.GoogleService
{
    public class GoogleReCaptChaResponse
    {
        public bool success { get; set; }
        public DateTime challenge_ts { get; set; }
        public decimal score { get; set; }
        public string hostname { get; set; }
        public string action { get; set; }

    }
}
