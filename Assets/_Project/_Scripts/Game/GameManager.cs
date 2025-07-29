using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    // Singleton Pattern
    public static GameManager Instance { get; private set; }


    [SerializeField] 
    private SO_GameParameters soGameParameters;
    [SerializeField, Tooltip("Used when SO is null")] 
    private GameParameters _debugGameParameters;

    private GameParameters Parameters
    {
        get { return soGameParameters ? soGameParameters.gameParameters : _debugGameParameters; }
    }



    [Header("References")]
    [SerializeField] private AsteroidSpawner _asteroidSpawner;
    [SerializeField] private ShipController _shipPrefab;


    [SerializeField] private int _playerLives;
    public int PlayerLives
    {
        get { return _playerLives; }
        set
        {
            _playerLives = value;
            OnPlayerLivesChange?.Invoke(_playerLives);
        }
    }


    [SerializeField] private GameState _gameState;
    public GameState GameState
    {
        get { return _gameState; }
        set {
            _gameState = value;
            OnGameStateChange?.Invoke(_gameState);
        }
    }


    public int Score { get; private set; }

    public Action<int> OnScore;
    public Action<int> OnPlayerLivesChange;

    public Action<GameState> OnGameStateChange;

    public Action OnGameStart;
    public Action OnGameOver;


    private HashSet<Asteroid> _activeAsteroids = new HashSet<Asteroid>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        GameState = GameState.Intro;
    }


    public void AddScore(int points)
    {
        Score += points;
        OnScore?.Invoke(Score);
    }

    [ContextMenu("StartGame")]
    public void StartGame()
    {
        PlayerLives = Parameters.playerLives;

        _asteroidSpawner.Spawn();
        SpawnPlayerShip();
        Debug.Log("Game Start");
    }

    [ContextMenu("SpawnPlayer")]
    void SpawnPlayerShip()
    {
        Vector3 spanwLocation = Vector3.zero;
        ShipController ship =  Instantiate(_shipPrefab, spanwLocation, Quaternion.identity);

        ship.OnDestroyed += PlayerHit;
    }


    [ContextMenu("PlayerHit")]
    void PlayerHit()
    {
        PlayerLives--;
        if (PlayerLives > 0)
        {
            StopAllCoroutines();    // To prevent multiple player ships
            StartCoroutine(WaitRespawn(1));
        }
        else
        {
            Debug.Log("GameOver");
            OnGameOver?.Invoke();
        }
    }

    public void AsteroidCreated(Asteroid asteroid)
    {
        _activeAsteroids.Add(asteroid);
    }

    public void AsteroidDestroied(Asteroid asteroid)
    {
        _activeAsteroids.Remove(asteroid);
        CheckGameEnd();
    }

    void CheckGameEnd()
    {
        if(_activeAsteroids.Count == 0)
        {
            Debug.Log("Game Won");
        }
    }

    IEnumerator WaitRespawn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SpawnPlayerShip();
    }
}

public enum GameState
{
    None,
    Intro,
    Gameplay,
    GameOver,
    GameWon
}
