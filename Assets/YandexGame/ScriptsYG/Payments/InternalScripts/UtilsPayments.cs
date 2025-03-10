﻿using System;

namespace YG.Utils.Pay
{
    [Serializable]
    public class Purchase
    {
        public string id;
        public string title;
        public string description;
        public string imageURI;
        public string price;
        public string priceValue;
        public string priceCurrencyCode;
        public string priceCurrencyImage;
        public bool consumed;
    }

    public class JsonPayments
    {
        public string[] id;
        public string[] title;
        public string[] description;
        public string[] imageURI;
        public string[] price;
        public string[] priceValue;
        public string[] priceCurrencyCode;
        public string[] priceCurrencyImage;
        public bool[] consumed;
        public string language;
    }
}