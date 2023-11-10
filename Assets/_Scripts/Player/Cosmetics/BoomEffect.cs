using System;
using UnityEngine;

public class BoomEffect : MonoBehaviour
{
    [SerializeField] private Transform boomT;
    [SerializeField] private MeshRenderer boomMesh;
    [SerializeField] private float startAlpha = -0.5f;
    [SerializeField] private float liveTime;
    [SerializeField] private float destroyTime = 5;
    
    [Space]
    
    [SerializeField] private float boomAddSpeed = 15;
    [SerializeField] private float boomClearSpeed = 15;
    private static readonly int Alpha = Shader.PropertyToID("_alpha");

    private void Start()
    {
        boomMesh.sharedMaterial.SetFloat(Alpha,startAlpha);
        Destroy(gameObject,destroyTime);
    }

    private void Update()
    {
        if (liveTime <= 0)
        {
            if(boomT != null)
                Destroy(boomT.gameObject);
            
            return;
        }

        liveTime -= Time.deltaTime;
        
        ClearBoom();
        void ClearBoom()
        {
            var resultAlpha = boomMesh.sharedMaterial.GetFloat(Alpha);
            resultAlpha = Mathf.Lerp(resultAlpha, -1, Time.deltaTime * boomClearSpeed);
            
            boomMesh.sharedMaterial.SetFloat(Alpha,resultAlpha);
        }

        AddBoom();
        void AddBoom()
        {
            boomT.localScale =
                Vector3.MoveTowards(boomT.localScale, boomT.localScale * 2, Time.deltaTime * boomAddSpeed);
        }
    }
}
