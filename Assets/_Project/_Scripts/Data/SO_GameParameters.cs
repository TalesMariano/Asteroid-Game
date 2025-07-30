using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_GameParameters : ScriptableObject
{
    public GameParameters gameParameters;
}
[System.Serializable]
public class GameParameters
{
    public int playerLives = 3;
}
