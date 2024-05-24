using System;
using UnityEngine;

public class InstaKill : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IHealth health))
            health.TakeDamage(health.MaxHealth+1);
    }
}
