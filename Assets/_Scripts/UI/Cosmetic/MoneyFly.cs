using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using Random = UnityEngine.Random;

public class MoneyFly : MonoBehaviour
{
    [SerializeField] private Transform toFlyPoint;
    [SerializeField] private Transform spawnPointDefault;
    [SerializeField] private Image[] moneyPool;
    
    [Space]
    
    [SerializeField] private float animationTime;
    [SerializeField] private float firstAnim = 0.5f;
    [SerializeField] private float secondAnim = 0.5f;
    [SerializeField] private float imagesAlpha = 0.25f;
    
    [Space]
    
    [SerializeField] private int moneyCountDivider;
    [SerializeField] private float moneySpawnSpreadPower;

    [Space]
    
    [SerializeField] private float startImageScale;
    [SerializeField] private float endImageScale;

    [Space]
    
    [SerializeField] private bool donateMoneyUse;
    
    private int imageCanUse;
    private int imageUses;

    private void Start()
    {
        var zero = new Color(1,1,1,imagesAlpha);
        
        imageCanUse = moneyPool.Length;

        var playerMoney = PlayerMoneyXpService.instance;

        if (!donateMoneyUse)
            playerMoney.OnMoneyChange += difference =>
            {
                if(difference <= 0)
                    return;

                difference /= moneyCountDivider;
                
                PlayMoneyFly(difference);
            };
        else
            playerMoney.OnDonateMoneyChange += difference =>
            {
                if(difference <= 0)
                    return;

                difference /= moneyCountDivider;
                
                PlayMoneyFly(difference);
            };
        
        void PlayMoneyFly(int count, Transform customSpawnPoint = null)
        {
            if(imageUses >= imageCanUse)
                return;

            for (int i = imageUses; i < imageCanUse; i++)
            {
                if(count <= 0)
                    return;

                var currentImage = moneyPool[i];
                var currentImageT = currentImage.transform;
                
                InitImage();
                void InitImage()
                {
                    currentImageT.gameObject.SetActive(true);

                    if (customSpawnPoint == null)
                        currentImageT.position = spawnPointDefault.position;
                    else
                        currentImageT.position = currentImageT.position;

                    currentImageT.localScale = new Vector3(startImageScale, startImageScale, startImageScale);
                    
                    currentImage.color = zero;
                    imageUses++;
                    count--;
                }

                MoveImage();
                void MoveImage()
                {
                    ColorMoveAndTweenStop();
                    void ColorMoveAndTweenStop()
                    {
                        currentImage.DOColor(Color.white, animationTime * firstAnim).OnComplete(() =>
                        {
                            var colorTweenCore = currentImage.DOColor(zero, animationTime * secondAnim)
                                .SetEase(Ease.OutCubic);
                            
                            colorTweenCore.OnComplete(() =>
                            {
                                imageUses--;
                                currentImageT.gameObject.SetActive(false);
                            }); 
                        }).SetEase(Ease.OutExpo);
                    }

                    PositionMove();
                    void PositionMove()
                    {
                        currentImageT.DOLocalMove
                        (currentImageT.localPosition + new Vector3(GetSpread(), GetSpread(), 0),
                            animationTime * firstAnim).SetEase(Ease.OutExpo)
                            .OnComplete(() =>
                            {
                                currentImageT.DOLocalMove
                                    (toFlyPoint.localPosition, animationTime * secondAnim).SetEase(Ease.OutCubic); 
                            });
                    }

                    ScaleMove();
                    void ScaleMove()
                    {
                        currentImageT.DOScale(endImageScale, animationTime);
                    }
                }
            }

            float GetSpread()
            {
                return Random.Range(-moneySpawnSpreadPower, moneySpawnSpreadPower);
            }
        }
    }
    
}
