using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.DTO.DTOClasses;
using Project.DTO.ExternalDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Project.API.Controllers
{
    public class CategoryController : ApiController
    {
        CategoryRepository _cRep;
        public CategoryController()
        {
            _cRep = new CategoryRepository();
        }

        [HttpGet]
        public List<ExternalCategoryDTO> ListCategories()
        {
          return _cRep.Select(x => new ExternalCategoryDTO
            {
              CategoryName  = x.CategoryName,
              Description = x.Description
            }).ToList();
        }

        [HttpGet]
        public List<CategoryDTO> ListCategoriesForAdmin()
        {
            //List<BaseEntityDTO> dtoList =  _cRep.SelectByDTO(x => new CategoryDTO
            //{
            //    CategoryName = x.CategoryName,
            //    ID = x.ID,
            //    Description = x.Description
            //}).ToList();

            //List<CategoryDTO> cDtoList = new List<CategoryDTO>();
            //foreach (CategoryDTO item in dtoList)
            //{
            //    cDtoList.Add(item);
            //}
            //return cDtoList;

            return _cRep.SelectForAdmin(x => new CategoryDTO
            {
                CategoryName = x.CategoryName,
                Description = x.Description,
                ID = x.ID
            }).ToList();


        }

        
    }
}
