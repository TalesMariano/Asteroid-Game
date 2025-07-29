using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugShotcuts : MonoBehaviour
{

    [Tooltip("StartGame" )] public KeyCode startGame = KeyCode.F1;


#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(startGame))
        {
            GameManager.Instance.StartGame();
        }
    }

#endif
}
