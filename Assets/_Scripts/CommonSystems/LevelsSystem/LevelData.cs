using UnityEngine;

[CreateAssetMenu(fileName = "New Level Data", menuName = "Level Data", order = 51)]
public class LevelData : ScriptableObject
{
    [SerializeField] private int name;
    [SerializeField] private int id;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int secrets;
    [SerializeField] private float completeTime;
    
    public int Name => name;
    public int ID => id;
    public GameObject Prefab => prefab;
    public int Secrets => secrets;
    public float CompleteTime => completeTime;
}
