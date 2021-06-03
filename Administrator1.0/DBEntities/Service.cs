using System;
using System.Collections.Generic;

namespace Administrator1._0.DBEntities
{
    public class Service
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string ServiceStaus { get; set; }
        public DateTime Date { get; set; }
        public string Duration { get; set; }
        public decimal Price { get; set; }

        public List<Client> Clients { get; set; }
    }
}