using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DungeonRoom : MonoBehaviour
{
    [BoxGroup("Read Only")] [ReadOnly] public bool RoomCleared = false;

    [BoxGroup("Settings")] public RoomType roomType = RoomType.Combat;
    [BoxGroup("Settings")] public Transform entranceL, entranceN, entranceW, entranceS;
    [BoxGroup("Settings")] [ShowIf("roomType", RoomType.Lobby)] public Transform playerSpawnPoint;
    [BoxGroup("Settings")] [ShowIf("roomType", RoomType.Combat)] public List<Transform> enemySpawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator CheckIfRoomIsCleared()
    {
        while(RoomCleared == false)
        {



            yield return new WaitForSeconds(5);
        }
    }
    public enum RoomType
    {
        Lobby,
        Combat,
        Rest,
        Event,
        Boss,
        Treasure,
    }
}