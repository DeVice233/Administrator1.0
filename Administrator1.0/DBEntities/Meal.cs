using System.Collections.Generic;

namespace Administrator1._0.DBEntities
{
    public class Meal
    {
        public int Id { get; set; }
        public string MealName { get; set; }
        public decimal Price { get; set; }

        public List<Tour> Tours { get; set; }
    }
}