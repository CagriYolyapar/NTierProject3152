using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DTO.DTOClasses
{
    public class OrderDTO:BaseEntityDTO
    {
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public DateTime OrderDate { get; set; }

    }
}
