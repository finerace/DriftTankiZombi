using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shop Data", menuName = "ShopData", order = 51)]
public class ShopData : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private int nameLanguageId;
    [SerializeField] private GameObject tank;
    
    [Space]
    
    [SerializeField] private int price;
    [SerializeField] private int priceDonate;

    [Space] 
    
    [SerializeField] private int[] engineUpdatePrices;
    [SerializeField] private float[] engineParam1PerLevel;
    [SerializeField] private float[] engineParam2PerLevel;

    [Space] 
    
    [SerializeField] private int[] gunsUpdatePrices;
    [SerializeField] private float[] gunsParam1PerLevel;
    [SerializeField] private float[] gunsParam2PerLevel;
    
    [Space] 

    [SerializeField] private int[] fuelUpdatePrices;
    [SerializeField] private float[] fuelParam1PerLevel;
    
    [Space] 
    
    [SerializeField] [Range(0,1)] private float manageChar;
    [SerializeField] [Range(0,1)] private float gunChar;
    [SerializeField] [Range(0,1)] private float speedChar;
    [SerializeField] [Range(0,1)] private float fuelChar;

    public int ID => id;

    public int NameLanguageId => nameLanguageId;

    public GameObject Tank => tank;

    public int Price => price;

    public int PriceDonate => priceDonate;

    public int[] EngineUpdatePrices => engineUpdatePrices;

    public float[] EngineParam1PerLevel => engineParam1PerLevel;

    public float[] EngineParam2PerLevel => engineParam2PerLevel;

    public int[] GunsUpdatePrices => gunsUpdatePrices;

    public float[] GunsParam1PerLevel => gunsParam1PerLevel;

    public float[] GunsParam2PerLevel => gunsParam2PerLevel;

    public int[] FuelUpdatePrices => fuelUpdatePrices;

    public float[] FuelParam1PerLevel => fuelParam1PerLevel;

    public float ManageChar => manageChar;

    public float GunChar => gunChar;

    public float SpeedChar => speedChar;

    public float FuelChar => fuelChar;
}   
