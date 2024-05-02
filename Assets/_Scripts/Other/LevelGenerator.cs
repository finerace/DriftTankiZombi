using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private RoadsPrefs roads;
    [SerializeField] private int levelScale = 18;
    
    private (int,int)[,] levelMap;
    
    private void GenerateLevel()
    {
        levelMap = new(int,int)[levelScale, levelScale];
        
        var maxSteps = 128;
        var currentSteps = 0;
        
        if (levelScale < 4)
            throw new Exception("Level is too small!");

        var generationPoints = new List<Transform>(64);
        levelMap[levelScale / 2, levelScale / 2] = (Random.Range(0, 3),Random.Range(1, 5));

        void AddGenerationPoints((int,int) levelPoint, bool clear = false)
        {
            const int rM = 90;
            
            switch (levelPoint)
            {
                case (0, _):
                {
                    var point1 = new GameObject().transform;
                    
                    point1.Rotate(0,levelPoint.Item1 * rM,0);
                    
                    generationPoints.Add(point1);

                    if (clear)
                    {
                        var point2 = new GameObject().transform;
                        point2.Rotate(0,180 + levelPoint.Item1 * rM,0);
                        generationPoints.Add(point2);

                    }
                    
                    break;
                }

                case (1, _):
                {
                    var point1 = new GameObject().transform;
                    var point2 = new GameObject().transform;

                    point1.Rotate(0, 90 + levelPoint.Item1 * rM,0);
                    point2.Rotate(0,-90 + levelPoint.Item1 * rM,0);

                    generationPoints.Add(point1);
                    generationPoints.Add(point2);
                    
                    if (clear)
                    {
                        var point3 = new GameObject().transform;
                        point3.Rotate(0, 180 + levelPoint.Item1 * 90, 0);
                        generationPoints.Add(point3);                        
                    }

                    break;
                }
                
                case (2, _):
                {
                    
                    
                    break;
                }

            }
        }

        void GenerateRoad()
        {
            if(currentSteps >= maxSteps)
                return;
            currentSteps++;
            
            for (int i = 0; i < generationPoints.Count; i++)
            {
                var generationPoint = generationPoints[i];
                    
            }
            
        }
    }

    [Serializable]
    public class RoadsPrefs
    {
        public GameObject[] forwardRoads;
        public GameObject[] threeTurnsRoads;
        public GameObject[] fourTurnsRoads;
        public GameObject[] turnRoads;

        [Space] 
        
        public GameObject[] pavements;
        public GameObject[] pavementsBuildings;

        [Space] 
        
        public GameObject[] borders;
    }
    
}
