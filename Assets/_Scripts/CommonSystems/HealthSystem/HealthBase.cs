using System;
using UnityEngine;

public abstract class HealthBase : MonoBehaviour,IHealth
{
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;
    
    public float Health => health;
    public float MaxHealth => maxHealth;

    public event Action OnDie;
    public event Action<float> OnTakeDamage;

    protected bool isDie;

    public void TakeDamage(float damageValue)
    {
        health -= damageValue;
        
        OnTakeDamage?.Invoke(damageValue);
        
        if (!(health <= 0)) 
            return;

        if(isDie)
            return;
        
        isDie = true;
        
        Died();
        OnDie?.Invoke();
    }

    public void Heal(float healValue)
    {
        health += healValue;
        
        if (health > maxHealth)
            health = maxHealth;
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    public abstract void Died();
}
