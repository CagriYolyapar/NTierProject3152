using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI.ConsumeDTO
{
    public class PaymentDTO
    {
        //Sanal Pos Entegrasyonu

        //Normalde bu tarz sınıflar calıstıgınız ilgili yerden aldıgınız dökümantasyonların kılavuzlugu sayesinde olusturulur...
        public int ID { get; set; }
        public string CardUserName { get; set; }
        public string SecurityNumber { get; set; }
        public string CardNumber { get; set; }
        public int CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }
        public int ShoppingPrice { get; set; }
    }
}