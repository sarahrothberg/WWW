using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using MazeGeneratorLib;
public class GenerateMaze : MonoBehaviour {

    //public enum MazeGeneratorType { LongWalls, ShortWalls } // short wall is mean: wall with localScale x == localScale.z, long wall means: wall with 3*localScale x <= localScale.z
    //public MazeGeneratorType GeneratorType = MazeGeneratorType.ShortWalls;
    public enum MazeGeneratorMode { WithPrefabs, WihtoutPrefabs}
  
    public MazeGeneratorMode MazeMode = MazeGeneratorMode.WithPrefabs;

    public GameObject wallPrefab;
    public int MazeWidth = 10, MazeHeight = 10;
    public float TunnelWidth = 1;
    public float WallWidth = 1;        
    public bool CreateRooms = false;
    public int RoomCount = 5;
    public int RoomWidth = 2;
    public int RoomHeight = 2;

    public bool IsCreateGround = true;
    public GameObject groundPrefab, ceilingPrefab;

    
    public Vector3 startPos = new Vector3(0, 0, 0);
    public bool IsCreateMazeOnStart = true, IsCreateCeiling = false;
    public bool UseSeed = false;
    public int MazeSeed = 1000;

    public bool UseStartEndWall = false;
    public GameObject StartWallPrefab, EndWallPrefab;   
      
    private GameObject endWallobj, startWallobj;
    private GameObject groundObg, ceilingObj;
    private List<GameObject> mazeWalls;
    private List<Vector3[]> roomCells;
    private List<Vector3> wallPosArray, startWallPos, endWallPos;
    
    private MazeSkeleton maze;
    void Start () {
        roomCells = new List<Vector3[]>();        
        wallPosArray = new List<Vector3>();
        endWallPos = new List<Vector3>();
        startWallPos = new List<Vector3>();
        mazeWalls = new List<GameObject>();

        maze = new MazeSkeleton(new Vector3(0,0,0),MazeWidth,MazeHeight,TunnelWidth,WallWidth,1,UseSeed,MazeSeed,IsCreateMazeOnStart,CreateRooms,RoomCount,RoomHeight,RoomHeight);
        maze.SetRoomCreating(CreateRooms);
        maze.SetRoomParams(RoomCount, RoomWidth, RoomHeight);
        maze.SetMazeDepth(1);
        if(IsCreateMazeOnStart) DrawMaze();
        else
        {
            maze.GenerateNewMaze(MazeWidth, MazeHeight);
            DrawMaze();
        }

    }
    private bool IsPosInArray(Vector3 pos, List<Vector3> array)
    {
        bool toRet = false;
        for (int i = 0; i < array.Count; i++)
        {
            if (pos == array[i])
            {
                toRet = true;
                break;
            }

        }
        return toRet;
    }
    private void DrawMaze()
    {     
        
        
        wallPosArray = maze.GetWallArray();
        
        endWallPos = maze.GetEndWallArray();
        startWallPos = maze.GetStartWallArray();
        
        
        if (MazeMode == MazeGeneratorMode.WithPrefabs)
        {
            for (int j = 0; j < wallPosArray.Count; j++)
            {
                
                if (UseStartEndWall)
                {

                    if (IsPosInArray(wallPosArray[j], endWallPos))
                    {

                        GameObject obj = (GameObject)Instantiate(EndWallPrefab, wallPosArray[j], Quaternion.identity);
                        obj.transform.parent = transform;
                        mazeWalls.Add(obj);
                    }
                    else if (IsPosInArray(wallPosArray[j], startWallPos))
                    {

                        GameObject obj = (GameObject)Instantiate(StartWallPrefab, wallPosArray[j], Quaternion.identity);
                        obj.transform.parent = transform;
                        mazeWalls.Add(obj);
                    }
                    else
                    {
                        GameObject obj = (GameObject)Instantiate(wallPrefab, wallPosArray[j], Quaternion.identity);
                        obj.transform.parent = transform;
                        mazeWalls.Add(obj);
                    }
                }
                else
                {
                    GameObject obj = (GameObject)Instantiate(wallPrefab, wallPosArray[j], Quaternion.identity);
                    obj.transform.parent = transform;
                    mazeWalls.Add(obj);
                }
            }
            
        }
        if (IsCreateGround)
        {
            groundObg = (GameObject)Instantiate(groundPrefab, maze.GetPosionForGround(), Quaternion.identity);
            groundObg.transform.localScale = new Vector3(maze.GetSizeForGround().x, 1, maze.GetSizeForGround().y);
            groundObg.transform.parent = transform;
            mazeWalls.Add(groundObg);

        }

        if (IsCreateCeiling)
        {
            ceilingObj = (GameObject)Instantiate(ceilingPrefab, maze.GetPosionForCeiling(), Quaternion.identity);
            ceilingObj.transform.localScale = new Vector3(maze.GetSizeForGround().x, 1, maze.GetSizeForGround().y);
            ceilingObj.transform.parent = transform;
            mazeWalls.Add(ceilingObj);
        }
    }
    private void UnDrawMaze()
    {
        for(int i=0;i< mazeWalls.Count;i++)
        {
            Destroy(mazeWalls[i]);
        }
        mazeWalls.Clear();
    }
    public void DeleteMaze()
    {
        UnDrawMaze();
        maze.DeleteMaze();
    }
    public void SetCreateGround(bool create)
    {
        IsCreateGround = create;
    }
    public void SetCreateCeiling(bool create)
    {
        IsCreateCeiling = create;
    }
    public void SetWallType(GameObject newWall)
    {
        wallPrefab = newWall;
    }  
    
    public void SetGroundPrefab(GameObject newGround)
    {
        groundPrefab = newGround;
    }
    public void SetCeilingPrefab(GameObject newGround)
    {
        ceilingPrefab = newGround;
    }
    public void SetStartWallPrefab(GameObject newGround)
    {
        StartWallPrefab = newGround;
    }
    public void SetStopWallPrefab(GameObject newGround)
    {
        EndWallPrefab = newGround;
    }
    public Vector2 GetMazeSizeInCells()
    {
        return new Vector2(MazeWidth, MazeHeight);
    }
   
    public void ClearAndGenerateNewMaze(int newMazeWidth, int newMazeHeight)
    {
        UnDrawMaze();
        maze.DeleteMaze();
        MazeWidth = newMazeWidth;
        MazeHeight = newMazeHeight;
        Invoke("GenMaze", 0.5f);
    }        
    private void GenMaze()
    {
        if(maze.IsGenerated())
        {
            UnDrawMaze();
            maze.DeleteMaze();
        }
        maze.SetRoomCreating(CreateRooms);
        maze.SetRoomParams(RoomCount, RoomWidth, RoomHeight);
        maze.SetMazeDepth(1);
        maze.SetUseSeed(UseSeed);
        maze.SetSeedForRandom(MazeSeed);
        maze.SetStartPosition(startPos);
        maze.GenerateNewMaze(MazeWidth, MazeHeight);
        DrawMaze();
    }

    public void GeneratemazeWithSeed(int newMazeWidth, int newMazeHeight, Vector3 position, int seed)
    {
        UseSeed = true;        
        MazeSeed = seed;
        startPos = position;
        MazeWidth = newMazeWidth;
        MazeHeight = newMazeHeight;  
        
        Invoke("GenMaze", 0.5f);

    }
    public void GenerateNewMaze(int newMazeWidth, int newMazeHeight)
    {
        MazeWidth = newMazeWidth;
        MazeHeight = newMazeHeight;
        GenMaze();
    }

    // Returns size for Ground in Vector2
    public Vector2 GetSizeForGround()
    {
        return maze.GetSizeForGround();
    }
    // Set width for tunnel
    public void SetMazeTunnelWidth(float w)
    {
        maze.SetMazeTunnelWidth(w);
    }
    // Set width for wall
    public void SetMazeWallWidth(float w)
    {
        maze.SetMazeWallWidth(w);
    }
    // Set startPosition for maze(Center of the maze)
    public void SetStartPosition(Vector3 pos)
    {
        maze.SetStartPosition(pos);
    }
    // If true, room will be created
    public void SetRoomCreating(bool isCreateRoom)
    {
        maze.SetRoomCreating(isCreateRoom);
    }
    // Set room params: count, width and height in cell size
    public void SetRoomParams(int count, int w, int h)
    {
        maze.SetRoomParams(count, w, h);
    }
    // Sets maze depth
    public void SetMazeDepth(float depth)
    {
        maze.SetMazeDepth(depth);
    }
    // Returns positions for ground
    public Vector3 GetPosionForGround()
    {
        return maze.GetPosionForGround();
    }
    // Returns Positions for Ceiling
    public Vector3 GetPosionForCeiling()
    {
        return maze.GetPosionForCeiling();
    }
    // Returns MazeSize in cell size
    public Vector2 GetMazeSize()
    {
        return maze.GetMazeSize();
    }
    
    public bool IsGenerated()
    {
        return maze.IsGenerated();
    }    
    // Retruns List of rooms, it is containing Array of Vector3 center positions of cells in romm
    public List<Vector3[]> GetRooms()
    {
        return maze.GetRooms();
    }    
    // Returns all Impasses
    public List<Vector3> GetImpasses()
    {
        return maze.GetImpasses();
    }
    // Returns all CrossRoads
    public List<Vector3> GetCrossRoads()
    {
        return maze.GetCrossRoads();
    }
    // Returns array of all Cell Centers
    public Vector3[,] GetGridCenters()
    {
        return maze.GetGridCenters();
    }
    // Returns List of all Cell Centers
    public List<Vector3> GetCellCenters()
    {
        return maze.GetCellCenters();
    }
    // Get positions of start cell
    public Vector3 GetCenterPositionOfStartCell()
    {
        return maze.GetCenterPositionOfStartCell();
    }
    // Get positions of end cell
    public Vector3 GetCenterPositionOfEndCell()
    {
        return maze.GetCenterPositionOfEndCell();
    }
    // Function for set maze Seed
    public void SetSeedForRandom(int s)
    {
        maze.SetSeedForRandom(s);
    }
    // This functions told to maze to use seed
    public void SetUseSeed(bool useSeed)
    {
        maze.SetUseSeed(useSeed);
    }
    // Returns true, if wall is in position
    public bool IsThereWall(Vector3 position)
    {
        return maze.IsThereWall(position);
    }
    // Returns array of Vector3 of cell centers from start panel to end panel
    public Vector3[] FindWayToEnd()
    {
        return maze.FindWayToEnd();        
    }
    // Returns List of all wall Positions
    public List<Vector3> GetWallArray()
    {
        return wallPosArray;
    }
    // Returns List of all start wall Positions
    public List<Vector3> GetStartWallArray()
    {
        return startWallPos;
    }
    // Returns List of all end wall Positions
    public List<Vector3> GetEndWallArray()
    {
        return endWallPos;
    }



}
