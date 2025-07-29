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
    private ShipController _currentShip;


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
    public Action OnGameReset;


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

    private void Update()
    {
        if(GameState == GameState.Intro && Input.anyKeyDown)
        {
            StartGame();
        }

        // Quit Game
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }


    public void AddScore(int points)
    {
        Score += points;
        OnScore?.Invoke(Score);
    }

    [ContextMenu("StartGame")]
    public void StartGame()
    {
        ResetScene();

        GameState = GameState.Gameplay;

        PlayerLives = Parameters.playerLives;

        _asteroidSpawner.Spawn();
        SpawnPlayerShip();
        Debug.Log("Game Start");
    }

    [ContextMenu("SpawnPlayer")]
    void SpawnPlayerShip()
    {
        Vector3 spanwLocation = Vector3.zero;
        _currentShip =  Instantiate(_shipPrefab, spanwLocation, Quaternion.identity);

        _currentShip.OnDestroyed += PlayerHit;
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
            GameOver();
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
            //GameWon();
            PlayerLives++;
            _asteroidSpawner.Spawn();
        }
    }

    void GameWon()
    {
        Debug.Log("Game Won");
        GameState = GameState.GameWon;

        StartCoroutine(WaitGameOver(3));
    }

    void GameOver()
    {
        Debug.Log("GameOver");
        GameState = GameState.GameOver;
        OnGameOver?.Invoke();

        StartCoroutine(WaitGameOver(3));
    }

    void GoToIntro()
    {
        GameState = GameState.Intro;
    }

    [ContextMenu("ResetScene")]
    void ResetScene()
    {
        OnGameReset?.Invoke();

        _activeAsteroids.Clear();

        if(_currentShip) Destroy(_currentShip.gameObject);

        Score = 0;
    }

    IEnumerator WaitRespawn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SpawnPlayerShip();
    }

    IEnumerator WaitGameOver(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GoToIntro();
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
