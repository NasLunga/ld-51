using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}
    public GameObject player;
    public GameObject playerPrefab;
    public GameObject particlePrefab;
    public float gameOverDelay = 1.5f;
    public GameObject enemy;
    public GameState state;
    public WeaponState weaponState = WeaponState.RangedWeapon;
    public MeleeWeapon meleeWeapon;
    public RangedWeapon rangedWeapon;

    public static event System.Action<GameState> OnGameStateChanged;

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
        state = GameState.EnemySpawning;
        StartCoroutine(ChangeWeapons());

        enemy.SendMessage("InitiateSpawn");
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
                player.SendMessage("SetWeapon", rangedWeapon);
                SetWeaponState(WeaponState.RangedWeapon);
            } else {
                player.SendMessage("SetWeapon", meleeWeapon);
                SetWeaponState(WeaponState.MeleeWeapon);
            }

            yield return new WaitForSeconds(10);
        }
    }

    public IEnumerator GameOver() {
        yield return new WaitForSeconds(gameOverDelay);
        SceneManager.LoadScene("GameOver");
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
