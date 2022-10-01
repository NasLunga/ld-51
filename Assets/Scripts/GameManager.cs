using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}
    public GameObject playerPrefab;
    public GameObject particlePrefab;

    private GameObject player;

    void Awake()
    {
        if (instance != null) {
            GameObject.Destroy(this);
        } else {
            instance = this;
        }
    }

    void Start()
    {
        player = Instantiate(playerPrefab, new Vector3(-6f, -6f, 0f), Quaternion.identity);
        // Weapon sword = new MeleeWeapon(10, 0.5f, 2);
        // player.SendMessage("SetWeapon", sword);
        Weapon gun = new RangeWeapon(5, 0.5f, 4f, particlePrefab);
        player.SendMessage("SetWeapon", gun);
    }
}
