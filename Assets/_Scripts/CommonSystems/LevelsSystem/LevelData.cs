using UnityEngine;

[CreateAssetMenu(fileName = "New Level Data", menuName = "Level Data", order = 51)]
public class LevelData : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private int name;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int secrets;
    [SerializeField] private float completeTime;
    [SerializeField] private int completeScore;

    [Space] 
    
    [SerializeField] private int oneStarScore;
    [SerializeField] private int twoStarScore;
    [SerializeField] private int threeStarScore;
    
    public int Id => id;
    public int Name => name;
    public GameObject Prefab => prefab;
    public int Secrets => secrets;
    public float CompleteTime => completeTime;
    public int CompleteScore => completeScore;

    public int OneStarScore => oneStarScore;
    public int TwoStarScore => twoStarScore;
    public int ThreeStarScore => threeStarScore;
}
