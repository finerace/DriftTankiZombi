using System;
using UnityEngine;

public class ZombieAnimation : MonoBehaviour
{

    [SerializeField] private ZombieAI zombieAI;
    [SerializeField] private Rigidbody zombieRb;
    [SerializeField] private Animator zombieMeshAnimator;
    [SerializeField] private float toFlyModOnPower = 15;
    
    private static readonly int IsAnnoyed = Animator.StringToHash("IsAnnoyed");
    private static readonly int IsFly = Animator.StringToHash("IsFly");
    private static readonly int IsDie = Animator.StringToHash("IsDie");

    private bool isAnnoyed;
    private bool isDie;
    
    private void Start()
    {
        zombieAI.onAnnoyedChange += () =>
        {
            isAnnoyed = zombieAI.IsAnnoyed;
        };
        
        zombieAI.OnDie += () =>
        {
            isDie = zombieAI.IsDie;
        };
    }

    private void Update()
    {
        if(!zombieMeshAnimator.gameObject.activeSelf)
            return;
            
        var isFly = zombieRb.velocity.magnitude >= toFlyModOnPower;

        if(zombieMeshAnimator.GetBool(IsAnnoyed) != isAnnoyed)
            zombieMeshAnimator.SetBool(IsAnnoyed,isAnnoyed);
        
        if(zombieMeshAnimator.GetBool(IsFly) != isFly)
            zombieMeshAnimator.SetBool(IsFly,isFly);

        if(zombieMeshAnimator.GetBool(IsDie) != isDie)
            zombieMeshAnimator.SetBool(IsDie,isDie);
    }
}
