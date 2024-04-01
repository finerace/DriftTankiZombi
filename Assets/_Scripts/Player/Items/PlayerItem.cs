using System;
using UnityEngine;

public abstract class PlayerItem : MonoBehaviour
{
    private PlayerTank playerTank;

    private void Awake()
    {
        playerTank = FindObjectOfType<PlayerTank>();
    }

    protected void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != 3)
            return;
        
        ItemWork(playerTank);
        
        Destroy(gameObject);
    }

    protected abstract void ItemWork(PlayerTank playerTank);

}
