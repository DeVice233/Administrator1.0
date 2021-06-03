using System;
using System.Collections.Generic;

namespace Administrator1._0.DBEntities
{
    public class Tour
    {
        public int Id { get; set; }
        public DateTime ArriveDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public decimal FullDays { get; set; }
        public Nullable<decimal> Price { get; set; }

        public List<Client> Clients { get; set; }
        public HotelRoom HotelRoom { get; set; }
    }
}