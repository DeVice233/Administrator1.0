using System.Collections.Generic;

namespace Administrator1._0.DBEntities
{
    public class HotelRoom
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public int BedNumber { get; set; }

        public RoomType RoomType { get; set; }
        public RoomStatus RoomStatus { get; set; }
        public List<Tour> Tours { get; set; }
    }
}