using Project.ENTITIES.Models;
using Project.MVCUI.ConsumeDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI.VMClasses
{
    public class OrderVM
    {
        public Order Order { get; set; }
        public List<Order> Orders { get; set; }
        public PaymentDTO PaymentDTO { get; set; }
    }
}