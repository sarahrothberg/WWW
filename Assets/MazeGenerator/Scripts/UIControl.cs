using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    public GameObject MazeContainer;

    public GameObject wayIndicator, gridIndicator, impassIndicator, crossIndicator, roomIndicator;
    public GameObject startIndicator, stopIndicator;
    private GenerateMaze mazeScript;

    private List<GameObject> indicators;

    void Start()
    {
        indicators = new List<GameObject>();
        mazeScript = MazeContainer.GetComponent<GenerateMaze>();
    }
    public void DeleteIndicators()
    {
        for(int i=0;i<indicators.Count;i++)
        {
            Destroy(indicators[i]);
        }
        indicators.Clear();
    }
    public void DeleteMaze()
    {
        mazeScript.DeleteMaze();
    }
    public void CreateMaze()
    {
        mazeScript.GenerateNewMaze(20, 20);
    }
    public void ShowWay()
    {
        Vector3[] way = mazeScript.FindWayToEnd();
        for(int i=0;i<way.Length;i++)
        {
            GameObject ind = Instantiate(wayIndicator, way[i], Quaternion.identity) as GameObject;
            indicators.Add(ind);
        }
    }
    public void ShowCrossRoads()
    {
        List<Vector3> cross = mazeScript.GetCrossRoads();
        for (int i = 0; i < cross.Count; i++)
        {
            GameObject ind = Instantiate(crossIndicator, cross[i], Quaternion.identity) as GameObject;
            indicators.Add(ind);
        }
    }
    public void ShowImpasses()
    {
        List<Vector3> imp = mazeScript.GetImpasses();
        for (int i = 0; i < imp.Count; i++)
        {
            GameObject ind = Instantiate(impassIndicator, imp[i], Quaternion.identity) as GameObject;
            indicators.Add(ind);
        }
    }
    public void ShowRooms()
    {
        List<Vector3[]> rooms = mazeScript.GetRooms();
        for (int i = 0; i < rooms.Count; i++)
        {
            Vector3[] room = rooms[i];
            for (int j = 0; j < room.Length; j++)
            {
                GameObject ind = Instantiate(roomIndicator, room[j], Quaternion.identity) as GameObject;
                indicators.Add(ind);
            }
        }
    }
    
    public void ShowGrid()
    {
        Vector3[,] centers = mazeScript.GetGridCenters();

        for(int i=0;i< mazeScript.MazeWidth;i++)
        {
            for (int j = 0; j < mazeScript.MazeHeight; j++)
            {
                GameObject ind = Instantiate(gridIndicator, centers[i,j], Quaternion.identity) as GameObject;
                indicators.Add(ind);
            }
        }
    }
    
    public void ShowStartEnd()
    {
        Vector3 spos = mazeScript.GetCenterPositionOfStartCell();
        Vector3 epos = mazeScript.GetCenterPositionOfEndCell();
        GameObject ind = Instantiate(startIndicator, spos, Quaternion.identity) as GameObject;
        GameObject ind2 = Instantiate(stopIndicator, epos, Quaternion.identity) as GameObject;
        indicators.Add(ind);
        indicators.Add(ind2);
    }
}
