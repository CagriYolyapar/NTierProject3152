using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI.Areas.Admin.Data.AdminVMClasses
{
    public class ProductVM //PaginationVM ile neredeyse aynı görevi yapıyor gibi gözükebilir...Aslında cok benzer görevleri yapmaktadır ancak PaginationVM sadece alışveris tarafında kullanılacak ve sayfalandırmayı yapacak bir VM iken ProductVM sadece Admin tarafında kullanılması icin tasarlanmıs bir VM class'tır...
    {
        public List<Product> Products { get; set; }
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }


    }
}