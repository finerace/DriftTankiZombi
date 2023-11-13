using UnityEngine;

public class OnAwakeObjectChildsDisabler : MonoBehaviour
{
    private void Awake()
    {
        transform.SetChildsActive(false);
    }
}
