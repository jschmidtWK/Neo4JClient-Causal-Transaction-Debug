using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Globalization;
using Library.Data;

namespace Library.Neo4J
{
    /// <summary>
    /// The Driver of a Vehicle, attached to one or more Accounts for management, one of the three main Neo4J objects.
    /// </summary>
    public class Driver : DataObjectImpl
    {
        public Driver()
        {

        }

        public String Firstname { get; set; }

        public String Lastname { get; set; }

        public string CompanyIdentifier { get; set; }
    }
}
