using System.Collections.Generic;

namespace Administrator1._0.DBEntities
{
    public class RoomType
    {
        public int Id { get; set; }
        public string RoomTypeName { get; set; }
        public decimal Price { get; set; }

        public List<HotelRoom> HotelRooms { get; set; }
    }
}