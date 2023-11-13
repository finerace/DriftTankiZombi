using UnityEngine;

public class LevelOptimization : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger)
            return;
        
        other.gameObject.transform.SetChildsActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.isTrigger)
            return;

        other.gameObject.transform.SetChildsActive(false);
    }
}
