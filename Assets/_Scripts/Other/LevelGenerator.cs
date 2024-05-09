using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private RoadsPrefs roads;
    [SerializeField] private int levelScale = 18;

    [Space] 
    
    [SerializeField] private int newPointGenerateAllowChance = 30;
    [SerializeField] private int pointRotateChance = 40;

    [Space] 
    
    [SerializeField] private int newPointGenerateChance = 30;

    [SerializeField] private int startPointsGenerateChance = 75;
    
    private (int,int)[,] levelMap;

#if UNITY_EDITOR
    
    [ContextMenu("Generate Level")]
    private void GenerateLevel()
    {
        ClearChilds();
        void ClearChilds()
        {
            transform.DeleteChilds();
        }
        
        levelMap = new(int,int)[levelScale, levelScale];
        
        var maxSteps = levelScale * 8;
        var currentSteps = 0;
        const int rotationMultiplier = 90;
        const int cellScale = 15;
        
        var generationPoints = new List<Transform>(1024);
        
        if (levelScale < 4)
            throw new Exception("Level is too small!");

        (int, int) GetLevelCell(Transform point)
        {
            var x = Mathf.RoundToInt(point.position.x / cellScale);
            var z = Mathf.RoundToInt(point.position.z / cellScale);

            if ((x < 0 || x >= levelScale) || (z < 0 || z >= levelScale))
                return (-9, -9);
            
            return levelMap[z, x];
        }
        (int, int) GetLevelCellNext(Vector3 pointPos,Vector3 direction)
        {
            var x = Mathf.RoundToInt((pointPos.x + direction.x * cellScale) / cellScale);
            var z = Mathf.RoundToInt((pointPos.z + direction.z * cellScale) / cellScale);

            if ((x < 0 || x >= levelScale) || (z < 0 || z >= levelScale))
                return (-9, -9);
            
            return levelMap[z, x];
        }
        void SetLevelCell(Transform generationPoint,(int,int) value)
        {
            levelMap[Mathf.RoundToInt(generationPoint.position.z / cellScale), 
                    Mathf.RoundToInt(generationPoint.position.x / cellScale)]
                = value;
        }
        Vector2 GetCellEnvironmentCof(Vector3 cellPos,Transform parent = null)
        {
            var finalCof = new Vector2();
            var checkBool = false;

            var forward = Vector3.forward;
            var right = Vector3.right;

            if (parent != null)
            {
                forward = parent.forward;
                right = parent.right;
            }
            
            if (GetLevelCellNext(cellPos, forward).Item1 != 0)
            {
                checkBool = true;
                finalCof.y++;
            }

            if (GetLevelCellNext(cellPos, -forward).Item1 != 0)
            {
                checkBool = true;
                finalCof.y--;
            }

            if (!checkBool)
                finalCof.y = -9;
            checkBool = false;
            
            if (GetLevelCellNext(cellPos, -right).Item1 != 0)
            {
                checkBool = true;
                finalCof.x--;
            }

            if (GetLevelCellNext(cellPos, right).Item1 != 0)
            {
                checkBool = true;
                finalCof.x++;
            }
            
            if (!checkBool)
                finalCof.x = -9;
            
            return finalCof;
        }

        //GenerateRoads();
        void GenerateRoads()
        {
            Transform CreatePoint(Transform parent,int rotationId)
            {
                var newGenerationPoint = new GameObject().transform;

                newGenerationPoint.position = parent.position;
                newGenerationPoint.eulerAngles = parent.eulerAngles + new Vector3(0,rotationId * rotationMultiplier,0);
                generationPoints.Add(newGenerationPoint);
                
                return newGenerationPoint;
            }

            bool RandChance(int chance)
            {
                var rand = Random.Range(0, 100);

                return chance >= rand;
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
            bool IsPointLifeAllow(Transform parent, int rotationId,bool isForNewPoints = false)
            {
                var cof = GetCellEnvironmentCof(parent.position,parent);
                var result = cof.x < -2 && cof.x != 0;
                
                if (isForNewPoints)
                {
                    cof = GetCellEnvironmentCof(parent.position + parent.forward * cellScale,parent);
                    var cof2 = GetCellEnvironmentCof(parent.position - parent.forward * cellScale,parent);

                    result = ((cof.x < -2 || cof.x != rotationId) && cof.x != 0) &&
                             ((cof2.x < -2 || cof2.x != rotationId) && cof2.x != 0);

                    result = cof.x != 0;
                }
                
                return result;
            }
           
            //Test();
            //return;
            void Test()
            {
                levelMap[17, 15] = (-1, 0);
                levelMap[15, 17] = (-1, 0);

                var point = 
                    new GameObject().transform;
                point.position = new Vector3(16 * cellScale, 0, 16 * cellScale);
            
                point.Rotate(new Vector3(0,0,0));
            
                var data = GetCellEnvironmentCof(point.position,point);
                //print(data.x);
                print(IsPointLifeAllow(point,-1,true));
                DestroyImmediate(point.gameObject);
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
                    if (IsPointLifeAllow(parent,-1,true) && RandChance(newPointGenerateChance))
                        CreatePoint(parent, -1);
                    
                    if (IsPointLifeAllow(parent,1,true) && RandChance(newPointGenerateChance))
                        CreatePoint(parent, 1);
                }
                else
                {
                    if (RandChance(startPointsGenerateChance))
                        CreatePoint(parent, -1);
                    
                    if (RandChance(startPointsGenerateChance))
                        CreatePoint(parent, 1);
                    
                    if (RandChance(startPointsGenerateChance))
                        CreatePoint(parent, 2);
                }
            }
            
            PointsMoving();
            void PointsMoving()
            {
                while (currentSteps <= maxSteps)
                {
                    currentSteps++;
                    for (int i = 0; i < generationPoints.Count; i++)
                    {
                        var point = generationPoints[i];

                        void PointDelete()
                        {
                            generationPoints.Remove(point);
                            DestroyImmediate(point.gameObject);
                        }
                        
                        GoForward(point);

                        var currentLevelCell = GetLevelCell(point);
                        if (currentLevelCell.Item1 == 0)
                        {
                            SetLevelCell(point,(-1,currentLevelCell.Item2));
                            
                            if (!IsPointLifeAllow(point,0))
                            {
                                PointDelete();
                                continue;
                            }
                        }
                        else
                        {
                            PointDelete();
                            continue;
                        }

                        if (RandChance(pointRotateChance))
                            SetRotation(point,Random.Range(-1,2),point);
                        else if(RandChance(newPointGenerateAllowChance))
                            RotateParentPointAndGenerateNewPoints(point);
                    }
                }
                
                //print(d + " d");
                //print(currentSteps);
            }
            
        }

        SetRoads();
        void SetRoads()
        {
            for (int i = 0; i < levelScale; i++)
            {
                for (int j = 0; j < levelScale; j++)
                {
                    if(levelMap[i,j] == (0,0))
                        continue;
                        
                    var cellEnvironmentVector = GetCellEnvironmentCof(new Vector3(i*cellScale,0,j*cellScale));
                    var cellEnvironment = (cellEnvironmentVector.x, cellEnvironmentVector.y);

                    print(cellEnvironment);
                    
                    var result = (0, 0);
                    
                    switch (cellEnvironment)
                    {
                        case (0,0):
                        {
                            result = (3, 0);
                            break;
                        }
                        
                        case (1,0):
                        {
                            result = (2, 1);
                            break;
                        }

                        case (-1,0):
                        {
                            result = (2, -1);
                            break;
                        }

                        case (0,1):
                        {
                            result = (2, 2);
                            break;
                        }

                        case (0,-1):
                        {
                            result = (2, 0);
                            break;
                        }

                        case (1,-1):
                        {
                            result = (4, 0);
                            break;
                        }

                        case (1,1):
                        {
                            result = (4, 1);
                            break;
                        }
                        
                        case (-1,1):
                        {
                            result = (4, 2);
                            break;
                        }
                        
                        case (-1,-1):
                        {
                            result = (4, -1);
                            break;
                        }
                        
                        case (-9,0):
                        {
                            result = (1, 0);
                            break;
                        }
                        
                        case (0,-9):
                        {
                            result = (1, 1);
                            break;
                        }
                        
                        case (1,-9):
                        {
                            result = (1, 0);
                            break;
                        }
                        
                        case (-1,-9):
                        {
                            result = (1, 0);
                            break;
                        }
                        
                        case (-9,1):
                        {
                            result = (1, 1);
                            break;
                        }
                        
                        case (-9,-1):
                        {
                            result = (1, 1);
                            break;
                        }
                        
                        default:
                        {
                            result = (3, 0);
                            break;
                        }
                    }

                    if(result.Item2 > -2)
                        result.Item2 = -result.Item2;

                    if (result.Item2 == 0)
                        result.Item2 = 2;
                    
                    levelMap[i, j] = result;
                }   
            }
        }

        FinalSpawn();
        void FinalSpawn()
        {
            var points = generationPoints.ToArray();
            
            for (int i = 0; i < points.Length; i++)
            {
                var item = points[i];
                if(item != null)
                    DestroyImmediate(generationPoints[i].gameObject);
            }

            for (int i = 0; i < levelScale; i++)
            {
                for (int j = 0; j < levelScale; j++)
                {
                    var cellData = levelMap[j, i];
                    
                    if (cellData.Item1 != 0)
                        Instantiate(roads.GetCityPart
                            (cellData.Item1-1),new Vector3
                                (j*cellScale - levelScale/2*cellScale,0,i*cellScale- levelScale/2*cellScale)
                            ,Quaternion.Euler(new Vector3(0,rotationMultiplier*cellData.Item2,0)))
                            .transform.parent = transform;
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
