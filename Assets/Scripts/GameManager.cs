using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}
    public GameObject player;
    public float weaponChangeStunDuration = 0.5f;
    public float weaponChangeBackgroundColorDuration = 1f;
    public float gameOverDelay = 1.5f;
    public float nextLevelLoadDelay = 2f;
    public GameObject enemy;
    public GameState state {get; private set;}
    public WeaponState weaponState = WeaponState.RangedWeapon;
    public MeleeWeapon meleeWeapon;
    public RangedWeapon rangedWeapon;
    public DoorsController doors;
    public string nextSceneName;
    public bool doorsOpen {get; private set;} = false;
    public static event System.Action<GameState> OnGameStateChanged;
    public static event System.Action<WeaponState> OnWeaponStateChanged;
    public AudioClip distortion1;
    public AudioClip distortion2;

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
        SetState(GameState.EnemySpawning);
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
        OnWeaponStateChanged?.Invoke(newState);
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
            StartCoroutine(DistortSound());
            StartCoroutine(DistortCamera());
            player.SendMessage("Stun", weaponChangeStunDuration);

            yield return new WaitForSeconds(10);
        }
    }

    IEnumerator DistortSound() {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = distortion1;
        audioSource.Play();

        while (audioSource.isPlaying){
            yield return null;
        }

        audioSource.clip = distortion2;
        audioSource.Play();
    }

    IEnumerator DistortCamera() {
        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        float colorChangeRate = 0.5f / (weaponChangeBackgroundColorDuration / 2f);
        colorChangeRate *= 0.01f;  // Period of 1 change

        for (float r = 0f; r <= 0.5f; r += colorChangeRate) {
            camera.backgroundColor = new Color(r, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }

        for (float r = 0.5f; r >= 0f; r -= colorChangeRate) {
            camera.backgroundColor = new Color(r, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void OpenDoors() {
        doors.Open();
    }

    public IEnumerator GameOver() {
        yield return new WaitForSeconds(gameOverDelay);
        SceneManager.LoadScene("GameOver");
    }

    public IEnumerator LoadNextLevel() {
        yield return new WaitForSeconds(nextLevelLoadDelay);
        SceneManager.LoadScene(nextSceneName);
    }

    void OnDestroy()
    {
        OnGameStateChanged = null;
        OnWeaponStateChanged = null;
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
