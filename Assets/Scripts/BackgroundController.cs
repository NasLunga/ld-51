using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BackgroundController : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    void GameManagerOnGameStateChanged (GameState gameState) {
        if (gameState == GameState.Battle) {
            gameObject.GetComponent<Animator>().SetBool("is_lit", true);
        }
    }
}
