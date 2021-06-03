using System;
using System.Collections.Generic;

namespace Administrator1._0.DBEntities
{
    public class Tour
    {
        public int Id { get; set; }
        public string TourStatus { get; set; }
        public DateTime ArriveDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public Nullable<DateTime> FactArriveDate { get; set; }
        public Nullable<DateTime> FactLeaveDate { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Payment { get; set; }
        public Nullable<decimal> Debt { get; set; }
        public string Customer { get; set; }

        public Meal Meal { get; set; }
        public Treatment Treatment { get; set; }
        public List<Client> Clients { get; set; }
        public HotelRoom HotelRoom { get; set; }
    }
}