using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private RoadsPrefs roads;
    [SerializeField] private int levelScale = 18;

    private (int,int)[,] levelMap;

#if UNITY_EDITOR
    
    [ContextMenu("Generate Level")]
    private void GenerateLevel()
    {
        levelMap = new(int,int)[levelScale, levelScale];
        
        var maxSteps = levelScale * 8;
        var currentSteps = 0;
        const int rotationMultiplier = 90;
        const int cellScale = 15;
        
        var generationPoints = new List<Transform>(1024);
        
        if (levelScale < 4)
            throw new Exception("Level is too small!");

        (int, int) GetLevelCell(Transform generationPoint)
        {
            var x = (int)(generationPoint.position.x / cellScale);
            var z = (int)(generationPoint.position.z / cellScale);

            if (x < 0 || x >= levelScale || z < 0 || z >= levelScale)
                return (-9, -9);
            
            return levelMap[x, z];
        }

        void SetLevelCell(Transform generationPoint,(int,int) value)
        {
            levelMap[(int)(generationPoint.position.x / cellScale), (int)(generationPoint.position.z / cellScale)]
                = value;
        }
        
        GenerateRoads();
        void GenerateRoads()
        {
            Transform CreatePoint(Transform parent,int rotationId)
            {
                var newGenerationPoint = new GameObject().transform;

                newGenerationPoint.position = parent.position;
                newGenerationPoint.eulerAngles = parent.eulerAngles + new Vector3(0,rotationId * rotationMultiplier,0);

                return newGenerationPoint;
            }

            void GoForward(Transform point)
            {
                point.position += point.forward * cellScale;
            }
            void SetRotation(Transform point,int rotationId,Transform parent = null)
            {
                if(parent == null)
                    point.eulerAngles = new Vector3(0, rotationId * rotationMultiplier, 0);
                else
                    point.eulerAngles = parent.eulerAngles + new Vector3(0, rotationId * rotationMultiplier, 0);
            }
            
            bool RandChance(int chance)
            {
                var rand = Random.Range(0, 100);

                return chance >= rand;
            }
            
            levelMap[levelScale / 2, levelScale / 2] = (-1, 0);

            var firstPoint = CreateFirstPoint();
            Transform CreateFirstPoint()
            {
                var firstPoint = new GameObject().transform;
                firstPoint.position = new Vector3(levelScale / 2 * 15, 0, levelScale / 2 * 15);
                firstPoint.eulerAngles = new Vector3(0, Random.Range(0, 4) * rotationMultiplier, 0);
                
                generationPoints.Add(firstPoint);
                return firstPoint;
            }
            
            RotateParentPointAndGenerateNewPoints(firstPoint,true);
            void RotateParentPointAndGenerateNewPoints(Transform parent,bool isFirst = false)
            {
                if (!isFirst)
                {
                    if (RandChance(30))
                        CreatePoint(parent, -1);
                    
                    if (RandChance(30))
                        CreatePoint(parent, 1);
                }
                else
                {
                    if (RandChance(75))
                        CreatePoint(parent, -1);
                    
                    if (RandChance(75))
                        CreatePoint(parent, 1);
                    
                    if (RandChance(75))
                        CreatePoint(parent, 2);
                }
            }
            
            PointsMoving();
            
            void PointsMoving()
            {
                var d = 0;
                while (currentSteps <= maxSteps)
                {
                    currentSteps++;
                    
                    for (int i = 0; i < generationPoints.Count; i++)
                    {
                        d++;
                        var point = generationPoints[i];
                        if(point == null)
                            return;
                        
                        GoForward(point);

                        if(RandChance(40))
                            RotateParentPointAndGenerateNewPoints(point);
                        else if (RandChance(50))
                            SetRotation(point,Random.Range(-1,2),point);

                        var currentLevelCell = GetLevelCell(point);
                        
                        print(currentLevelCell.Item1);
                        if (currentLevelCell.Item1 == 0)
                        {
                            SetLevelCell(point,(-1,currentLevelCell.Item2));
                        }
                        
                        
                    }
                }
                
                print(d + " d");
                //print(currentSteps);
            }
            
        }

        End();
        void End()
        {
            var points = generationPoints.ToArray();
            
            for (int i = 0; i < points.Length; i++)
            {
                var item = points[i];
                if(item != null)
                    DestroyImmediate(generationPoints[i]);
                
            }

            for (int i = 0; i < levelScale; i++)
            {
                for (int j = 0; j < levelScale; j++)
                {
                    if (levelMap[i, j].Item1 != 0)
                        Instantiate(roads.GetCityPart
                            (2),new Vector3
                                (i*cellScale - levelScale/2*cellScale,0,j*cellScale- levelScale/2*cellScale)
                            ,Quaternion.identity);
                }
            }
            
        }
    }
#endif
    
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
                return borders[id-borders.Length];
            
            if (id > pavements.Length + 3)
                return pavementsBuildings[id-pavementsBuildings.Length];
            
            return pavements[id-pavements.Length];
        }
    }
    
}
