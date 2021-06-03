using System.Collections.Generic;

namespace Administrator1._0.DBEntities
{
    public class RoomStatus
    {
        public int Id { get; set; }
        public string StatusName { get; set; }

        public List<HotelRoom> HotelRooms { get; set; }
    }
}