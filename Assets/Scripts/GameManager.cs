using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}
    public GameObject player;
    public GameObject playerPrefab;
    public GameObject particlePrefab;
    public GameObject enemy;
    public GameState state = GameState.RangedWeapon;
    public List<Weapon> weapons;

    void Awake()
    {
        if (instance != null) {
            GameObject.Destroy(this);
        } else {
            instance = this;
        }

        weapons = new List<Weapon>();
        Weapon sword = new MeleeWeapon(10, 0.5f, 0.3f, 2);
        weapons.Add(sword);

        Weapon gun = new RangeWeapon(5, 0.5f, 0.3f, 4f, particlePrefab);
        weapons.Add(gun);
    }

    void Start()
    {
        StartCoroutine(ChangeWeapons());

        float enemySpawnDuration = enemy.GetComponent<EnemyController>().spawnDuration;
        player.SendMessage("Stun", enemySpawnDuration);
    }

    public void SetState(GameState newState)
    {
        state = newState;
    }

    

    IEnumerator ChangeWeapons()
    {
        while (true) {
            if (state == GameState.MeleeWeapon) {
                player.SendMessage("SetWeapon", weapons[1]);
                SetState(GameState.RangedWeapon);
            } else {
                player.SendMessage("SetWeapon", weapons[0]);
                SetState(GameState.MeleeWeapon);
            }

            yield return new WaitForSeconds(10);
        }
    }
}


public enum GameState {
    MeleeWeapon,
    RangedWeapon
}
