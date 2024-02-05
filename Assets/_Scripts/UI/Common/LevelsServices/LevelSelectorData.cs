using TMPro;
using UnityEngine;

public class LevelSelectorData : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    
    [Space]
    
    [SerializeField] private Transform star1;
    [SerializeField] private Transform star2;
    [SerializeField] private Transform star3;

    [Space] 
    
    [SerializeField] private TMP_Text levelHighScore;

    public LevelData LevelData => levelData;

    public Transform Star1 => star1;

    public Transform Star2 => star2;

    public Transform Star3 => star3;

    public TMP_Text LevelHighScore => levelHighScore;
}
