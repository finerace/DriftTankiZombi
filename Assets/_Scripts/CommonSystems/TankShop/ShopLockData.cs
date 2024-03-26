using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopLockData : MonoBehaviour
{

    [SerializeField] private TMP_Text moneyPrice;
    [SerializeField] private TMP_Text donateMoneyPrice;
    
    [SerializeField] private GameObject donationMoneyPrice;

    public TMP_Text MoneyPrice => moneyPrice;

    public TMP_Text DonateMoneyPrice => donateMoneyPrice;

    public GameObject DonationMoneyPrice => donationMoneyPrice;
}
