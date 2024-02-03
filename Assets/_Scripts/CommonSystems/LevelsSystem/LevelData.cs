using UnityEngine;

[CreateAssetMenu(fileName = "New Level Data", menuName = "Level Data", order = 51)]
public class LevelData : ScriptableObject
{
    [SerializeField] private int name;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int secrets;
    [SerializeField] private float completeTime;
    [SerializeField] private int completeScore;
    
    public int Name => name;
    public GameObject Prefab => prefab;
    public int Secrets => secrets;
    public float CompleteTime => completeTime;
    public int CompleteScore => completeScore;
}
