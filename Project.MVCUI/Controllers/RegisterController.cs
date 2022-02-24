using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class RegisterController : Controller
    {

        AppUserRepository _apRep;
        ProfileRepository _proRep;

        public RegisterController()
        {
            _apRep = new AppUserRepository();
            _proRep = new ProfileRepository();
        }
      
        public ActionResult RegisterNow()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterNow(AppUser appUser,AppUserProfile userProfile)
        {
            return View();
        }
    }
}