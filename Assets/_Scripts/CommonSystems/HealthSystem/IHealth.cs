using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public float Health
    {
        get;
    }
    public float MaxHealth
    {
        get;
    }

    public void TakeDamage(float damageValue);
    public void Heal(float healValue);
    public void SetMaxHealth(float maxHealth);

    public void Died();
}
