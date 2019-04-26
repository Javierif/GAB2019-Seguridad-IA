using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAB2019_ia_cybersecurity.Models.Speaker
{
    public class VerifyResult
    {
        public string Result { get; set; } // [Accept | Reject]

        public string Confidence { get; set; } // [Low | Normal | High]

        public string Phrase { get; set; }
    }
}
