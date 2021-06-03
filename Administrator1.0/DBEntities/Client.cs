using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrator1._0.DBEntities
{
   public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string FatherName { get; set; }
        public string Sex { get; set; }
        public DateTime Birthday { get; set; }
        public int PassportSerias { get; set; }
        public int PassportNumber { get; set; }

        public List<Order> Orders { get; set; }
        public Tour Tour { get; set; }
        
    }
}
