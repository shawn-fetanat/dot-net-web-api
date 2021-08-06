using System;
using System.Collections.Generic;
using System.Text;

namespace Sabio.Models.Domain
{
    public class Addresses: BaseAddresses
    {       
        public int SuiteNumber { get; set; }

        public string PostalCode { get; set; }

        public bool IsActive { get; set; }

        public double Lat { get; set; }

        public double Long { get; set; }
    }
}
