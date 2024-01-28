using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
struct TrainData
{
    [SerializeField]
    public Vector2 startingPos;
    [SerializeField]
    public Vector2 endingPos;
    [SerializeField]
    public Color trainColor;
    [SerializeField]
    public string trainName;

}
[System.Serializable]
struct PathStruct
{
    public List<Vector2> pathPositions;

}



public class GridManager : MonoBehaviour
{
    [SerializeField]
    private int gridSizeX;

    [SerializeField]
    private int gridSizeY;

    [SerializeField]
    private GameObject gridObject;
    [SerializeField]
    GameObject trainObject;

    [SerializeField]
    List<TrainData> trainDataList = new List<TrainData>();

    
    List<PathStruct> pathList = new List<PathStruct>();

    int turnCounter = 0;

    Dictionary<Vector2,GameObject> gridPositions = new Dictionary<Vector2, GameObject>();

    
    public static GridManager Instance { get; private set; }


    public UnityEvent UpdateInstructionText;
    public UnityEvent SpawnStartButton;

    [SerializeField]
    GameObject GameOverObject;
    
    List<GameObject> trains = new List<GameObject>();

    [SerializeField]
    Sprite startingStationSprite;
    [SerializeField]
    Sprite endingStationSprite;

    [SerializeField]
    List<Vector2> excludePositions = new List<Vector2>();

    

    List<Vector2> trainPositions = new List<Vector2>();

    bool SpawnedTrains;
    [SerializeField]
    private float gameOverScreenTimer;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {

        

        MakeGrid();
        SetStartingAndEndingPoints();
        UpdateInstructionText.Invoke();

    }

    public string GetTurnTrainName()
    {
        if(turnCounter < trainDataList.Count)
        {
            return trainDataList[turnCounter].trainName + " Turn";
        }
        return null;
    }

    public Color GetTurnTrainColor()
    {
        if (turnCounter < trainDataList.Count)
        {
            return trainDataList[turnCounter].trainColor;
        }
        return Color.white;
    }

    void MakeGrid()
    {
        SetCameraPosition();
        for (int i = 0; i < gridSizeX; i++)
        {
            for(int j = 0;j<gridSizeY;j++)
            {
                
                Vector2 spawnPos = new Vector2(i, j);
                if(!excludePositions.Contains(spawnPos))
                {
                    GameObject gridObjRef = Instantiate(gridObject, spawnPos, Quaternion.identity);
                    gridPositions.Add(spawnPos, gridObjRef);
                }
                
            }
        }
        
    }

    void SetCameraPosition()
    {
        Camera.main.transform.position = new Vector3(gridSizeX/2, gridSizeY/2,Camera.main.transform.position.z);
       
        
    }

    void SetStartingAndEndingPoints()
    {
        foreach(var item in trainDataList)
        {
            GridCell gridCellRefStart = GetGridObjectAtPosition(item.startingPos).GetComponent<GridCell>();
            gridCellRefStart.SetSprite(startingStationSprite);
            gridCellRefStart.SetCellColor(item.trainColor);
            gridCellRefStart.AddPathReference(trainDataList.IndexOf(item));
            GridCell gridCellRefEnd = GetGridObjectAtPosition(item.endingPos).GetComponent<GridCell>();
            gridCellRefEnd.SetSprite(endingStationSprite);
            gridCellRefEnd.SetCellColor(item.trainColor);
            gridCellRefEnd.AddPathReference(trainDataList.IndexOf(item));
            PathStruct tempPathStruct;
            tempPathStruct.pathPositions = new List<Vector2>
            {
                item.startingPos
            };
            pathList.Add(tempPathStruct);


        }
    }

    GameObject GetGridObjectAtPosition(Vector2 position)
    {
        gridPositions.TryGetValue(position, out GameObject gridObj);
        return gridObj;
    }
    

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 correspondingGridPos = new Vector2(Mathf.Round(mousePos.x),Mathf.Round(mousePos.y));    
            if(turnCounter<trainDataList.Count)
            {
                AddPointToPath(turnCounter, correspondingGridPos);
            }
            
            
            
        }
        
    }


    public void SpawnTrains()
    {
        foreach(var item in trainDataList)
        {
            GameObject trainRef = Instantiate(trainObject, item.startingPos, Quaternion.identity);
            Train trainScript = trainRef.GetComponent<Train>();
            trainScript.SetTrainIndex(trainDataList.FindIndex(data=>data.trainName == item.trainName));
            trainScript.SetColor(item.trainColor);
            trainScript.SetFinalPosition(item.endingPos);
            AddToTrainPositions(trainRef.transform.position);
            trains.Add(trainRef);
            trainScript.StartCoroutine(trainScript.MoveOnPath());
            
            
        }
    }
    void AddPointToPath(int pathIndex,Vector2 gridPos)
    {
        
        if(!gridPositions.TryGetValue(gridPos, out GameObject gridObj)) { return; }
        GridCell gridObjCellRef = gridObj.GetComponent<GridCell>();
        if(gridObjCellRef.GetPathReferences().Contains(pathIndex))
        {
            return;
        }

        Vector2 lastAddedPos = pathList[pathIndex].pathPositions[pathList[pathIndex].pathPositions.Count - 1];
        if(!CheckForNeighbouringPoints(lastAddedPos,gridPos))
        {
            return;
        }

        pathList[pathIndex].pathPositions.Add(gridPos);
        gridObjCellRef.AddPathReference(pathIndex);
        gridObjCellRef.SetCellColor(trainDataList[pathIndex].trainColor);
        ChangeTurn(gridPos,pathIndex);
    }

    void ChangeTurn(Vector2 lastAddedPosition,int pathIndex)
    {
        if(CheckForNeighbouringPoints(trainDataList[pathIndex].endingPos,lastAddedPosition ))
        {
            pathList[pathIndex].pathPositions.Add(trainDataList[pathIndex].endingPos);
            turnCounter++;
          
            UpdateInstructionText.Invoke();
            if(turnCounter>=trainDataList.Count)
            {
                SpawnStartButton.Invoke();
            }
        }
    }

    bool CheckForNeighbouringPoints(Vector2 point,Vector2 pointToCheck)
    {
        List<Vector2> adjacentPoints = new List<Vector2>();
        adjacentPoints.Add(new Vector2(point.x + 1, point.y)); 
        adjacentPoints.Add(new Vector2(point.x - 1, point.y)); 
        adjacentPoints.Add(new Vector2(point.x, point.y + 1)); 
        adjacentPoints.Add(new Vector2(point.x, point.y - 1)); 
        if(adjacentPoints.Contains(pointToCheck))
        {
            return true;
        }

        return false;
    }

    public Vector2 GetPositionFromPath(int index,int positionIndex)
    {
        
       
        
        return pathList[index].pathPositions[positionIndex];
    }
    
    void AddToTrainPositions(Vector2 positionToAdd)
    {
        trainPositions.Add(positionToAdd);
    }

    public void UpdateTrainPositions(int index,Vector2 modifiedPosition)
    {
        trainPositions[index] = modifiedPosition;
    }

    public void CheckForCollision()
    {
        bool hasCollided = trainPositions.Count != trainPositions.Distinct().Count();
        if(hasCollided)
        {
            foreach(var item in trains)
            {
                
                item.SetActive(false);
                StartCoroutine(GameOverEnumerator());
            }
        }
    }
    IEnumerator GameOverEnumerator()
    {
        bool gameOver = true;
        while(gameOver)
        {

            yield return new WaitForSeconds(gameOverScreenTimer);
            GameOverObject.SetActive(true);
        }
    }

    public void CheckForEndLevel()
    {
        int i = 0;
        int levelCompleteCheck = 0;

        if (trainPositions.Count != trainDataList.Count) { return; }
        foreach (var item in trainDataList)
        {
            

            if(item.endingPos == trainPositions[i] )
            {
                levelCompleteCheck++;
            }
            i++;
        }
        if(levelCompleteCheck == i)
        {
            LevelManager.Instance.MarkLevelComplete();
            SceneManager.LoadScene(0);
        }
    }
}

    