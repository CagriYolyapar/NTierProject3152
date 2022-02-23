using Project.BLL.DesignPatterns.GenericRepository.BaseRep;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DesignPatterns.GenericRepository.ConcRep
{
    public class OrderDetailRepository:BaseRepository<OrderDetail>
    {
        public override void Update(OrderDetail item)
        {
            OrderDetail toBeUpdated  = FirstOrDefault(x=> x.ProductID == item.ProductID && x.OrderID == item.OrderID);
            item.Status = ENTITIES.Enums.DataStatus.Updated;
            item.ModifiedDate = DateTime.Now;
            _db.Entry(toBeUpdated).CurrentValues.SetValues(item);
            Save();
        }

        public override void UpdateRange(List<OrderDetail> list)
        {
            foreach (OrderDetail item in list)
            {
                Update(item);
            }
        }
    }
}
