using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwticher : MonoBehaviour
{
    // Start is called before the first frame update

    int index = 0;
    [SerializeField] List<GameObject> players = new List<GameObject>();
    PlayerInputManager manager;

    void Start()
    {
      manager = GetComponent<PlayerInputManager>();   
        index = 0;
        manager.playerPrefab = players[index];
    }

    public void SwitchNextSpawnCharacter(PlayerInput input)
    {
        index += 1;
        manager.playerPrefab = players[index]; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
