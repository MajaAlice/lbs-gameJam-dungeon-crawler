using Unity.Mathematics;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    // Dungeon Settings
    public int DungeonSize; // How Many Squares Will Be Covered
    // Dungeon Exit Data
    public float DungeonEndChance;
    public bool EndExists;

    public class DungeonRoom
    {
        public bool IsInUse;
        public Vector3 RoomPosition;
        public bool4 RoomConnections;
    }
    public void GenerateDungeon()
    {
    }
}
