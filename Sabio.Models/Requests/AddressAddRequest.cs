﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Sabio.Models.Requests
{
    public class AddressAddRequest
    {
        public string LineOne { get; set; }

        public int SuiteNumber { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public bool IsActive { get; set; }

        public double Lat { get; set; }

        public double Long { get; set; }
    }
}
