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

        void GenerateFirstRoad()
        {
            var levelCell = 
                levelMap[levelScale / 2, levelScale / 2] = (Random.Range(0, 3),Random.Range(1, 5));
            var generationPoint = new GameObject().transform;
            generationPoints.Add(generationPoint);

            switch (levelCell)
            {
               case (0,_):
               {
                   SetRotationY(generationPoint,0);
                   
                   break;
               }
               
               case (1,_):
               {
                   SetRotationY(generationPoint,90);
                   break;
               }

               case (2,_):
               {
                   SetRotationY(generationPoint,0);
                   break;
               }
            }
            void SetRotationY(Transform target, int defaultRotation)
            {
                target.eulerAngles =  
                    new Vector3(0, defaultRotation + levelCell.Item2 * 90,0);
            }
            
            SpawnLevelPart(generationPoint,levelCell.Item1);

            GenerationPointWork
                (GetLevelCell(generationPoint),generationPoint,true);
        }
        
        GenerateFirstRoad();
        
        void GenerateRoad()
        {
            if(currentSteps >= maxSteps)
                return;
            currentSteps++;
            
            for (int i = 0; i < generationPoints.Count; i++)
            {
                var generationPoint = generationPoints[i];
                
                
            }

            bool IsLevelCellEmpty(Transform generationPoint)
            {
                return GetLevelCell(generationPoint).Item1 != 0;
            }
            
        }

        void GenerationPointWork((int,int) levelPoint,Transform generatedPoint, bool clear = false)
        {
            const int rotationMultiplier = 90;
            const int movementDistance = 15;
            var generatePointOldRotation = generatedPoint.rotation;
            var generatePointOldPosition = generatedPoint.position;
            
            void GoForward(Transform target)
            {
                target.position += target.forward * movementDistance;
            }

            void SetRotationY(Transform target, int defaultRotation)
            {
                target.eulerAngles =  new Vector3(0, defaultRotation + levelPoint.Item2 * rotationMultiplier,0);
            }

            Transform CreatePoint()
            {
                var newPoint = new GameObject().transform;
                newPoint.position = generatePointOldPosition;
                newPoint.rotation = generatePointOldRotation;

                return newPoint;
            }
            
            switch (levelPoint)
            {
                case (0, _):
                {
                    SetRotationY(generatedPoint,0);
                    GoForward(generatedPoint); 
                    
                    if (clear)
                    {
                        var point2 = CreatePoint();
                        SetRotationY(point2,180);
                        GoForward(point2);
                        
                        generationPoints.Add(point2);
                    }
                    
                    break;
                }

                case (1, _):
                {
                    var point2 = CreatePoint();

                    SetRotationY(generatedPoint,90);
                    SetRotationY(point2,-90);
                    
                    GoForward(generatedPoint);
                    GoForward(point2);
                    
                    generationPoints.Add(point2);
                    
                    if (clear)
                    {
                        var point3 = CreatePoint();
                        SetRotationY(point3,180);
                        GoForward(point3);
                        generationPoints.Add(point3);                        
                    }

                    break;
                }
                
                case (2, _):
                {
                    var point2 = CreatePoint();
                    var point3 = CreatePoint();
                    
                    SetRotationY(generatedPoint,0);
                    SetRotationY(point2,-90);
                    SetRotationY(point3,90);
                    
                    GoForward(generatedPoint);
                    GoForward(point2);
                    GoForward(point3);
                    
                    generationPoints.Add(point2);
                    generationPoints.Add(point3);
                    
                    if (clear)
                    {
                        var point4 = CreatePoint();
                        SetRotationY(point4,180);
                        GoForward(point4);
                        generationPoints.Add(point4);
                    }
                    
                    break;
                }
                
                case (3, _):
                {
                    SetRotationY(generatedPoint,-90);
                    GoForward(generatedPoint); 
                    
                    if (clear)
                    {
                        var point2 = CreatePoint();
                        SetRotationY(point2,180);
                        GoForward(point2);
                        
                        generationPoints.Add(point2);
                    }
                    
                    break;
                }
            }
        }

        void SpawnLevelPart(Transform generatePoint,int id,bool isGreen = false)
        {
            var cityPart = roads.GetCityPart(id, isGreen);

            Instantiate(cityPart, generatePoint.position,generatePoint.rotation);
        }

        (int, int) GetLevelCell(Transform generationPoint)
        {
            return levelMap[(int)(generationPoint.position.x / 15), (int)(generationPoint.position.z / 15)];
        }
    }

    [Serializable]
    public class RoadsPrefs
    {
        public GameObject[] forwardRoads;
        public GameObject[] threeTurnsRoads;
        public GameObject[] fourTurnsRoads;
        public GameObject[] turnRoads;

        [Space] public GameObject[] pavements;
        public GameObject[] pavementsBuildings;

        [Space] public GameObject[] borders;

        public GameObject GetCityPart(int id, bool isGreen = false)
        {
            switch (id)
            {
                case 0:
                    return !isGreen ? forwardRoads[0] : forwardRoads[1];
                
                case 1:
                    return !isGreen ? threeTurnsRoads[0] : threeTurnsRoads[1];

                case 2:
                {
                    var rand = Random.Range(0, 2);
                    
                    return !isGreen ? fourTurnsRoads[rand] : fourTurnsRoads[rand+2];
                }
                
                case 3:
                    return !isGreen ? turnRoads[0] : turnRoads[1];
            }

            if (id > pavements.Length + pavementsBuildings.Length + 3)
                return borders[id];
            
            if (id > pavements.Length + 3)
                return pavementsBuildings[id];
            
            return pavements[id];
        }
    }
    
}
