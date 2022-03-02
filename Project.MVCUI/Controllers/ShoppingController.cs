using PagedList;
using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.COMMON.Tools;
using Project.ENTITIES.Models;
using Project.MVCUI.CustomTools;
using Project.MVCUI.VMClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class ShoppingController : Controller
    {

        OrderRepository _oRep;
        ProductRepository _pRep;
        CategoryRepository _cRep;
        OrderDetailRepository _odRep;

        public ShoppingController()
        {
            _pRep = new ProductRepository();
            _odRep = new OrderDetailRepository();
            _oRep = new OrderRepository();
            _cRep = new CategoryRepository();
        }


        // GET: Shopping
        public ActionResult ShoppingList(int? page,int? categoryID) //nullable int vermemizin sebebi aslında buradaki int'in kacıncı sayfada oldugumuzu temsil edecek olmasıdır...Ancak birisi direkt alısveriş sayfasına ulastıgında hangi sayfada oldugu verisi olamayacagından dolayı bu şekilde de (yani sayfa numarası gönderilmeden de) bu Action'in calısabilmesini istiyoruz...
        {

            //string a = "Mehmet";

            //string b = a ?? "Cagri"; // a null ise b'ye Cagri degerini at...Ama a'nın degeri null degilse b'ye a'yı at...

            //page??1

            //page?? ifadesi page null ise demektir...

            PaginationVM pavm = new PaginationVM
            {
                PagedProducts = categoryID == null ? _pRep.GetActives().ToPagedList(page ?? 1, 9) : _pRep.Where(x => x.CategoryID == categoryID).ToPagedList(page ?? 1, 9),
                Categories = _cRep.GetActives()
            };

            if (categoryID != null) TempData["catID"] = categoryID;

            return View(pavm);
        }

        public ActionResult AddToCart(int id)
        {
            Cart c = Session["scart"] == null ? new Cart() : Session["scart"] as Cart;
            Product eklenecekUrun = _pRep.Find(id);

            CartItem ci = new CartItem
            {
                ID = eklenecekUrun.ID,
                Name = eklenecekUrun.ProductName,
                Price = eklenecekUrun.UnitPrice,
                ImagePath = eklenecekUrun.ImagePath
            };

            c.SepeteEkle(ci);
            Session["scart"] = c;
            return RedirectToAction("ShoppingList");

        }

        public ActionResult CartPage()
        {
            if(Session["scart"] != null)
            {
                CartPageVM cpvm = new CartPageVM();
                Cart c = Session["scart"] as Cart;
                cpvm.Cart = c;
                return View(cpvm);
            }

            TempData["bos"] = "Sepetinizde ürün bulunmamaktadır";
            return RedirectToAction("ShoppingList");
        }


        public ActionResult DeleteFromCart(int id)
        {
            if(Session["scart"] != null)
            {
                Cart c = Session["scart"] as Cart;
                c.SepettenSil(id);
                if(c.Sepetim.Count == 0)
                {
                    Session.Remove("scart");
                    TempData["sepetBos"] = "Sepetinizde ürün bulunmamaktadır";
                    return RedirectToAction("ShoppingList");
                }
                return RedirectToAction("CartPage");
            }
            return RedirectToAction("ShoppingList");

        }

        public ActionResult SiparisiOnayla()
        {
            AppUser mevcutKullanici;
            if (Session["member"] != null)
            {
                mevcutKullanici = Session["member"] as AppUser;
            }
            else TempData["anonim"] = "Kullanıcı üye degil";
            return View();
        }


        //https://localhost:44305/api/Payment/ReceivePayment


        //WebAPIRestService.WebApiClient kütüphanesini indirmeyi unutmayın cünkü API ile bu kütüphane sayesinde BackEnd'in haberlesmesini saglayacagız...
        [HttpPost]
        public ActionResult SiparisiOnayla(OrderVM ovm)
        {
            bool result;
            Cart sepet = Session["scart"] as Cart;

            if (Session["member"] != null)
            {
                AppUser kullanici = Session["member"] as AppUser;
                ovm.Order.Email = kullanici.Email;
                ovm.Order.UserName = kullanici.UserName;
                ovm.Order.AppUserID = kullanici.ID;
            }
            else ovm.Order.UserName = TempData["anonim"].ToString();

            ovm.Order.TotalPrice = ovm.PaymentDTO.ShoppingPrice = sepet.TotalPrice;

            #region APISection

            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44305/api/");

                Task<HttpResponseMessage> postTask = client.PostAsJsonAsync("Payment/ReceivePayment", ovm.PaymentDTO);

                HttpResponseMessage sonuc;

                try
                {
                    sonuc = postTask.Result;
                }
                catch (Exception ex)
                {

                    TempData["baglantiRed"] = "Banka baglantıyı reddetti";
                    return RedirectToAction("ShoppingList");
                }

                if (sonuc.IsSuccessStatusCode) result = true;
                else result = false;
                if (result)
                {
                    _oRep.Add(ovm.Order); //OrderRepository bu noktada Order'i eklerken  onun ID'sini olusturuyor...


                    foreach (CartItem item in sepet.Sepetim)
                    {
                        OrderDetail od = new OrderDetail();
                        od.OrderID = ovm.Order.ID;
                        od.ProductID = item.ID;
                        od.TotalPrice = item.SubTotal;
                        od.Quantity = item.Amount;
                        _odRep.Add(od);

                        //Stoktan da düsürelim
                        Product stokDus = _pRep.Find(item.ID);
                        stokDus.UnitsInStock -= item.Amount;
                        _pRep.Update(stokDus);
                    }

                    TempData["odeme"] = "Siparişiniz bize ulasmıstır.. Tesekkür ederiz...";
                    MailService.Send(ovm.Order.Email, body: $"Siparişiniz basarıyla alındı..{ovm.Order.TotalPrice}");
                    Session.Remove("scart");

                    return RedirectToAction("ShoppingList");
                }

                else
                {
                    TempData["sorun"] = "Odeme ile ilgili bir sorun olustu...Lütfen bankanız ile iletişime geciniz";
                    return RedirectToAction("ShoppingList");
                }

                

            }



            #endregion



        }



    }
}