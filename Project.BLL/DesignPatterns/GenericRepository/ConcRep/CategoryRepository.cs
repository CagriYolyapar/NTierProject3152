using Project.BLL.DesignPatterns.GenericRepository.BaseRep;
using Project.DTO.DTOClasses;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DesignPatterns.GenericRepository.ConcRep
{
    public class CategoryRepository:BaseRepository<Category>
    {
        public IQueryable<CategoryDTO> SelectForAdmin(Expression<Func<Category,CategoryDTO>> exp)
        {
            return _db.Set<Category>().Select(exp);
        }
    }
}
