using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.COMMON.Tools;
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
        public ActionResult RegisterNow(AppUser appUser,AppUserProfile profile)
        {
            appUser.ActivationCode = Guid.NewGuid();
            appUser.Password = DantexCrypt.Crypt(appUser.Password); // sifreyi kriptoladık

            //if(_apRep.Any(x=>x.UserName == appUser.UserName) || _apRep.Any(x=>x.Email == appUser.Email))
            //{
            //    ViewBag.ZatenVar = "Kullanıcı zaten kayıtlı";
            //    return View();
            //}

            if(_apRep.Any(x=>x.UserName == appUser.UserName))
            {
                ViewBag.ZatenVar = "Kullanıcı ismi daha önce alınmıs";
                return View();
            }
            else if(_apRep.Any(x=>x.Email == appUser.Email))
            {
                ViewBag.ZatenVar = "Email zaten kayıtlı";
                return View();
            }

            //Kullanıcı basarılı bir şekilde register işlemini tamamladıysa önce ona mail gönder...

            string gonderilecekMail = "Tebrikler...Hesabınız olusturulmustur...Hesabınızı aktive etmek icin https://localhost:44388/Register/Activation/"+appUser.ActivationCode+" linkine tıklayabilirsiniz";

            MailService.Send(appUser.Email, body: gonderilecekMail, subject: "Hesap aktivasyon!!");
            _apRep.Add(appUser); //öncelikle bunu eklemelisiniz...Cünkü AppUser'in ID'si ilk basta olusmalı...Cünkü bizim kurdugumuz birebir ilişkide AppUser zorunlu alan Profile ise opsiyonel alandır...Dolayısıyla ilk basta AppUser'in ID'si SaveChanges ile olusmalı ki sonra Profile'i rahatca eklenebilsin...

            if(!string.IsNullOrEmpty(profile.FirstName.Trim()) || !string.IsNullOrEmpty(profile.LastName.Trim()))
            {
                profile.ID = appUser.ID;
                _proRep.Add(profile);
            }
            return View("RegisterOk");
        }

        public ActionResult Activation(Guid id)
        {
            AppUser aktifEdilecek = _apRep.FirstOrDefault(x => x.ActivationCode == id);
            if(aktifEdilecek != null)
            {
                aktifEdilecek.Active = true;
                _apRep.Update(aktifEdilecek);
                TempData["HesapAktifMi"] = "Hesabınız aktif hale getirildi";
                return RedirectToAction("Login", "Home");
            }

            TempData["HesapAktifMi"] = "Hesabınız bulunamadı";
            return RedirectToAction("Login", "Home");

        }

        public ActionResult RegisterOk()
        {
            return View();
        }
    }
}