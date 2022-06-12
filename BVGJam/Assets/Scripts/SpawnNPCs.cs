using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNPCs : MonoBehaviour {

    public GameObject npcPrefab;



    void Start() {
        GameObject casey = Object.Instantiate<GameObject>(npcPrefab, new Vector3(5, 0, 0), Quaternion.identity);
        casey.GetComponent<NPC_Behaviour>().npcName = "Casey";
        //garbanzo.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Garbanzo/overworld.jpg");

        GameObject mark = Object.Instantiate<GameObject>(npcPrefab, new Vector3(-5, 0, 0), Quaternion.identity);
        mark.GetComponent<NPC_Behaviour>().npcName = "Mark";
        //rafael.GetComponent<SpriteRenderer>().sprite = rafae
        
        //Resources.Load<Sprite>("Sprites/Rafael/overworld.jpg");
    
        //activeSpeakerImage = (Image)Resources.Load("Sprites/Player/dialogIcon_" + _playerMood + ".jpg");
    }

    // Update is called once per frame
    void Update() {
        
    }
}
