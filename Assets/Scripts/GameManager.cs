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
    public GameState state;
    public WeaponState weaponState = WeaponState.RangedWeapon;
    public List<Weapon> weapons;

    public static event System.Action<GameState> OnGameStateChanged;

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
        state = GameState.EnemySpawning;
        StartCoroutine(ChangeWeapons());

        float enemySpawnDuration = enemy.GetComponent<EnemyController>().spawnDuration;
        enemy.SendMessage("InitiateSpawn");
        player.SendMessage("Stun", enemySpawnDuration);
    }

    public void SetState(GameState newState) {
        state = newState;
        OnGameStateChanged?.Invoke(newState);
    }

    public void SetWeaponState(WeaponState newState)
    {
        weaponState = newState;
    }

    IEnumerator ChangeWeapons()
    {
        while (true) {
            if (weaponState == WeaponState.MeleeWeapon) {
                player.SendMessage("SetWeapon", weapons[1]);
                SetWeaponState(WeaponState.RangedWeapon);
            } else {
                player.SendMessage("SetWeapon", weapons[0]);
                SetWeaponState(WeaponState.MeleeWeapon);
            }

            yield return new WaitForSeconds(10);
        }
    }
}


public enum WeaponState {
    MeleeWeapon,
    RangedWeapon
}

public enum GameState {
    EnemySpawning,
    Battle,
    BattleEnded
}
