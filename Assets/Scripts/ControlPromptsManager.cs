using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPromptsManager : MonoBehaviour
{
    public GameObject spacePrompt;
    public GameObject mousePrompt;

    void Awake() {
        GameManager.OnWeaponStateChanged += GameManagerOnWeaponStateChanged;
    }

    void GameManagerOnWeaponStateChanged(WeaponState weaponState) {
        bool isMelee = weaponState == WeaponState.MeleeWeapon;
        bool isRanged = !isMelee;
        spacePrompt.SetActive(isMelee);
        mousePrompt.SetActive(isRanged);
    }
}
