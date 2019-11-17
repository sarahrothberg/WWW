using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GenerateMaze))]
[CanEditMultipleObjects]
public class VisualizatorComponentEditor : Editor
{

    GenerateMaze subject;

    SerializedProperty MazeMode, wall, Mwidth, Mheight, TunnelWidth, WallWidth;
    SerializedProperty CreateRooms, RoomCount, RoomWidth, RoomHeight;
    SerializedProperty CreateGround, createCeiling, CreateMazeOnStart, ground, ceiling;
    SerializedProperty UseSeed, MazeSeed;
    SerializedProperty startPos, UseStartEndWall, StartWallPrefab, EndWallPrefab;
    
    void OnEnable()
    {
        subject = target as GenerateMaze;
       
        MazeMode = serializedObject.FindProperty("MazeMode");
        wall = serializedObject.FindProperty("wallPrefab");
        Mwidth = serializedObject.FindProperty("MazeWidth");
        Mheight = serializedObject.FindProperty("MazeHeight");
        TunnelWidth = serializedObject.FindProperty("TunnelWidth");
        WallWidth = serializedObject.FindProperty("WallWidth");
        wall = serializedObject.FindProperty("wallPrefab");        

        CreateRooms = serializedObject.FindProperty("CreateRooms");
        RoomCount = serializedObject.FindProperty("RoomCount");
        RoomWidth = serializedObject.FindProperty("RoomWidth");
        RoomHeight = serializedObject.FindProperty("RoomHeight");        

        CreateGround = serializedObject.FindProperty("IsCreateGround");
        createCeiling = serializedObject.FindProperty("IsCreateCeiling");
        CreateMazeOnStart = serializedObject.FindProperty("IsCreateMazeOnStart");
        ground = serializedObject.FindProperty("groundPrefab");
        ceiling = serializedObject.FindProperty("ceilingPrefab");

        UseSeed = serializedObject.FindProperty("UseSeed");
        MazeSeed = serializedObject.FindProperty("MazeSeed");

        startPos = serializedObject.FindProperty("startPos");
        UseStartEndWall = serializedObject.FindProperty("UseStartEndWall");
        StartWallPrefab = serializedObject.FindProperty("StartWallPrefab");
        EndWallPrefab = serializedObject.FindProperty("EndWallPrefab");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Vector3Field("Maze position", subject.startPos);
        
        EditorGUILayout.PropertyField(Mwidth);
        EditorGUILayout.PropertyField(Mheight);
        
        EditorGUILayout.PropertyField(TunnelWidth);
        EditorGUILayout.PropertyField(WallWidth);
        EditorGUILayout.PropertyField(MazeMode);
        if (subject.MazeMode == GenerateMaze.MazeGeneratorMode.WithPrefabs)
        {
            EditorGUILayout.PropertyField(wall);           

        }
       
        EditorGUILayout.PropertyField(CreateRooms);
        if (subject.CreateRooms)
        {
            EditorGUILayout.PropertyField(RoomCount);
            EditorGUILayout.PropertyField(RoomWidth);
            EditorGUILayout.PropertyField(RoomHeight);

        }
        EditorGUILayout.PropertyField(CreateMazeOnStart);
        EditorGUILayout.PropertyField(CreateGround);
        if (subject.IsCreateGround)
        {
            EditorGUILayout.PropertyField(ground);
        }
        EditorGUILayout.PropertyField(createCeiling);
        if (subject.IsCreateCeiling)
        {
            EditorGUILayout.PropertyField(ceiling);
        }
        EditorGUILayout.PropertyField(UseSeed);
        EditorGUILayout.PropertyField(MazeSeed);

        EditorGUILayout.PropertyField(UseStartEndWall);
        if (subject.UseStartEndWall)
        {
            EditorGUILayout.PropertyField(StartWallPrefab);
            EditorGUILayout.PropertyField(EndWallPrefab);
        }
        
        
        serializedObject.ApplyModifiedProperties();
    }

}
