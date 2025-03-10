﻿using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

using YG.Utils.Pay;

namespace YG
{
    public partial class YandexGame
    {
        public static Action GetPaymentsEvent;
        public static Action<string> PurchaseSuccessEvent;
        public static Action<string> PurchaseFailedEvent;
        public static event Action<Sprite> PriceCurrencySpriteEvent;
        public static Purchase[] purchases = new Purchase[0];
        public static string langPayments = "ru";
        public static Sprite priceCurrencySprite;
        private static bool isInitPayments;

        // Initialization

        [DllImport("__Internal")]
        private static extern string InitPayments_js();

        [InitYG]
        public static void InitPayments()
        {
#if !UNITY_EDITOR
            Debug.Log("Init Payments inGame");
            Instance.PaymentsEntries(InitPayments_js());
#else
            Instance.PaymentsEntries("");
#endif
            isInitPayments = true;

            if (purchases?.Length > 0)
            {
                var url = purchases[0].priceCurrencyImage;
                Instance.StartCoroutine(DownloadPriceCurrencySprite(url));
            }
        }

        [StartYG]
        public static void PayDoEvent()
        {
            if (purchases.Length > 0)
                GetPaymentsEvent?.Invoke();
        }


        // Sending messages

        [DllImport("__Internal")]
        private static extern void GetPayments_js();

        public static void GetPayments()
        {
            Message("Get Payments");
#if !UNITY_EDITOR
            GetPayments_js();
#else
            Instance.PaymentsEntries("");
#endif
        }

        public void _GetPayments() => GetPayments();


        public static Purchase PurchaseByID(string ID)
        {
            for (int i = 0; i < purchases.Length; i++)
            {
                if (purchases[i].id == ID)
                {
                    return purchases[i];
                }
            }

            return null;
        }


        [DllImport("__Internal")]
        private static extern void ConsumePurchase_js(string id);

        public static void ConsumePurchaseByID(string id)
        {
#if !UNITY_EDITOR
            ConsumePurchase_js(id);
#endif
        }

        [DllImport("__Internal")]
        private static extern void ConsumePurchase_js();

        public static void ConsumePurchases()
        {
#if !UNITY_EDITOR
            ConsumePurchase_js();
#endif
        }


        [DllImport("__Internal")]
        private static extern void BuyPayments_js(string id);

        public static void BuyPayments(string id)
        {
#if !UNITY_EDITOR
            BuyPayments_js(id);
#else
            Message($"Buy Payment. ID: {id}");
            Instance.OnPurchaseSuccess(id);
#endif
        }

        public void _BuyPayments(string id) => BuyPayments(id);



        // Receiving Data

        public void PaymentsEntries(string data)
        {
#if !UNITY_EDITOR
            if (data == "none")
                return;

            JsonPayments paymentsData = JsonUtility.FromJson<JsonPayments>(data);
            purchases = new Purchase[paymentsData.id.Length];

            for (int i = 0; i < purchases.Length; i++)
            {
                purchases[i] = new Purchase();
                purchases[i].id = paymentsData.id[i];
                purchases[i].title = paymentsData.title[i];
                purchases[i].description = paymentsData.description[i];
                purchases[i].imageURI = paymentsData.imageURI[i];
                purchases[i].price = paymentsData.price[i];
                purchases[i].priceValue = paymentsData.priceValue[i];
                purchases[i].priceCurrencyCode = paymentsData.priceCurrencyCode[i];
                purchases[i].priceCurrencyImage = paymentsData.priceCurrencyImage[i];
                purchases[i].consumed = paymentsData.consumed[i];
            }
            langPayments = paymentsData.language;
#else
            purchases = Instance.infoYG.purshasesSimulation;
            langPayments = "ru";
#endif
            if (isInitPayments)
                GetPaymentsEvent?.Invoke();
        }


        public void OnPurchaseSuccess(string id)
        {
            PurchaseByID(id).consumed = true;
            PurchaseSuccess?.Invoke();
            PurchaseSuccessEvent?.Invoke(id);
        }

        public void OnPurchaseFailed(string id)
        {
            PurchaseFailed?.Invoke();
            PurchaseFailedEvent?.Invoke(id);
        }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ResetStaticPayments()
        {
            purchases = new Purchase[0];
            GetPaymentsEvent = null;
            PurchaseSuccessEvent = null;
            PurchaseFailedEvent = null;
        }

        private static System.Collections.IEnumerator DownloadPriceCurrencySprite(string url)
        {
#if UNITY_2020_1_OR_NEWER
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.ConnectionError &&
                    webRequest.result != UnityWebRequest.Result.DataProcessingError)
                {
                    DownloadHandlerTexture handlerTexture = webRequest.downloadHandler as DownloadHandlerTexture;
                    if (handlerTexture.isDone)
                    {
                        var spriteSize = new Rect(0, 0, handlerTexture.texture.width, handlerTexture.texture.height);
                        priceCurrencySprite = Sprite.Create(handlerTexture.texture, spriteSize, Vector2.zero);
                        PriceCurrencySpriteEvent?.Invoke(priceCurrencySprite);
                    }
                }
            }
#endif
        }
    }
}
