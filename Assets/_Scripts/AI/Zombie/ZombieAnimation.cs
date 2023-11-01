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

    private void Start()
    {
        zombieAI.onAnnoyed += () => {zombieMeshAnimator.SetBool(IsAnnoyed,true);};
        zombieAI.OnDie += () => {zombieMeshAnimator.SetBool(IsDie,true);};
    }

    private void Update()
    {
        var isFly = zombieRb.velocity.magnitude >= toFlyModOnPower;
        zombieMeshAnimator.SetBool(IsFly,isFly);
    }
}
