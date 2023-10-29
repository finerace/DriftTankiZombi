using System;
using UnityEngine;

public static class AuxiliaryFunc
{
    
    public static float LengthXZ(this Vector3 b)
    {
        float returnFloat = Mathf.Abs(b.x)+ Mathf.Abs(b.z);
        return returnFloat;
    }

    public static float LengthY(this Vector3 b)
    {
        float returnFloat = Mathf.Abs(b.y);
        return returnFloat;
    }

    public static bool IsLayerInMask(this LayerMask mask, int layer)
    {
        int maskValue = mask.value;
        int layerValue = 1 << layer;

        if (maskValue < layerValue)
            return false;

        if (maskValue == layerValue)
            return maskValue == layerValue;

        int dynamicMaskValue = maskValue;

        for (int i = 30; i >= 0; i--)
        {
            int localMaskNum = 1 << i;

            if (localMaskNum > maskValue)
                continue;

            if (dynamicMaskValue == layerValue)
                return true;

            if (((layerValue * 2) - 1) >= dynamicMaskValue && layerValue <= dynamicMaskValue)
                return true;

            if ((dynamicMaskValue - localMaskNum) < 0)
                continue;

            
            dynamicMaskValue -= localMaskNum;

            if (((layerValue * 2) - 1) >= dynamicMaskValue && layerValue <= dynamicMaskValue)
                return true;
        }

        return false;

    }

    public static float PointDirection_TargetLocalPosDOT(Vector3 targetLocalPos, Vector3 pointDirection)
    {
        return Vector3.Dot(pointDirection.normalized, targetLocalPos.normalized);
    }
    
    public static float ClampToTwoRemainingCharacters(this float target)
    {
        return (int)(target * 100f) / 100f;
    }
    
    public static string ConvertNumCharacters(int charactersCount)
    {
        var resultString = charactersCount.ToString();

        if (resultString.Length == 1)
        {
            resultString = "0" + resultString;
        }

        return resultString;
    }

    public static TimeSpan ConvertSecondsToTimeSpan(int seconds)
    {
        return new TimeSpan(0, 0, seconds);
    }
    
    public static void SetChildsActive(this Transform objectT, bool active)
    {
        for (int i = 0; i < objectT.childCount; i++)
        {
            objectT.GetChild(i).gameObject.SetActive(active);
        }
    }

    public static Vector3 ClampMagnitude(this Vector3 vector, float max)
    {
        if (max == 0)
            return vector;
        
        var magnitudeExcess = max / vector.magnitude;

        if (magnitudeExcess < 1)
            return vector * magnitudeExcess;

        return vector;
    }

    public static RaycastHit? Raycast(this Transform origin,Vector3 rayDirection,float distance = Mathf.Infinity,int layerMask = -1)
    {
        RaycastHit raycastHitOut;

        if (layerMask >= 0)
        {
            if (Physics.Raycast(origin.position, rayDirection, out raycastHitOut, distance, layerMask))
                return raycastHitOut;
        }
        else 
            if (Physics.Raycast(origin.position, rayDirection, out raycastHitOut, distance))
                return raycastHitOut;
            

        return null;
    }
}