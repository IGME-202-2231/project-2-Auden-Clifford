using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Menu,
    Gameplay,
    GameOver
}

public class GameManager : Singleton<GameManager>
{
    // in-game UI
    [SerializeField] private TextMesh playerRotation;
    [SerializeField] private TextMesh playerSpeed;
    [SerializeField] private TextMesh scoreDisplay;
    [SerializeField] private TextMesh roundDisplay;
    [SerializeField] private TextMesh EnemiesDisplay;

    // death screen UI
    [SerializeField] private TextMeshPro FinalTime;
    [SerializeField] private TextMeshPro FinalScore;
    [SerializeField] private TextMeshPro FinalRound;

    // UI containters
    [SerializeField] private CanvasRenderer menuPanel;
    [SerializeField] private CanvasRenderer helpPanel;
    [SerializeField] private CanvasRenderer gameUIPanel;
    [SerializeField] private CanvasRenderer gameOverPanel;

    // enemies and player prefabs
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyStandardPrefab;
    [SerializeField] private GameObject enemyShooterPrefab;
    [SerializeField] private GameObject enemyFastPrefab;
    
    private GameObject player;
    private List<GameObject> enemies = new List<GameObject>();

    // keep track of the game's current state
    public GameState currentState = GameState.Menu;

    // set initial pre-game values
    private int round = 1;
    private int score = 0;
    private float time = 0;

    private float markerRadius = 4;

    /// <summary>
    /// Gets or sets the game object that the player controls
    /// </summary>
    public GameObject Player { get { return player; } set { player = value; } }

    /// <summary>
    /// Gets a list of each enemy in the game
    /// </summary>
    public List<GameObject> Enemies { get { return enemies; } }

    /// <summary>
    /// Gets or sets the player's score this game
    /// </summary>
    public int Score { get { return score; } set { score = value; } }

    private void Start()
    {
        // set all panels except menu to hide at beginning
        menuPanel.gameObject.SetActive(true);
        helpPanel.gameObject.SetActive(false);
        gameUIPanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case GameState.Menu:
                break;

            case GameState.Gameplay:
                // In-Game UI

                // display rotation speed at different colors based on how low it is
                float rotation = player.GetComponent<PhysicsObject>().AngularVelocity;
                if (rotation > 300)
                {
                    playerRotation.color = Color.white;
                }
                else if(rotation > 100)
                {
                    playerRotation.color = Color.yellow;
                }
                else
                {
                    playerRotation.color = Color.red;
                }

                // display the values in the correct slots in the UI
                playerRotation.text = ((int)rotation / 6).ToString(); // devide by 6 to convert to rpm
                playerSpeed.text = ((int)player.GetComponent<PhysicsObject>().Velocity.magnitude).ToString();
                scoreDisplay.text = score.ToString();
                roundDisplay.text = round.ToString();
                EnemiesDisplay.text = enemies.Count.ToString();

                // start a new round when all enemies are defeated
                if(enemies.Count == 0)
                {
                    // give the player an extra speed boost for finishing the round
                    player.GetComponent<PhysicsObject>().SpeedUpSpin(round * Mathf.Sqrt(round) * 10);

                    round++;
                    SpawnEnemies(round);
                }

                // keep track of the time this game has lasted
                time += Time.deltaTime;

                // update the positions of the objects marking enemy locations
                DrawMarkers();
                break;

            case GameState.GameOver:
                //display the final results of the game
                FinalRound.text = "Round: " + round;
                FinalScore.text = "Score: " + score;
                FinalTime.text = "Time: " + (int)time / 60 + ":" + (int)time % 60;
                break;
        }
        
    }

    /// <summary>
    /// Opens the help panel
    /// </summary>
    public void OpenHelp()
    {
        helpPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// Closes the help panel
    /// </summary>
    public void CloseHelp()
    {
        helpPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// This function is called if the player quits back to the menu screen
    /// </summary>
    public void QuitToMenu()
    {
        currentState = GameState.Menu;
        menuPanel.gameObject.SetActive(true);
        gameUIPanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// This function is called whenever the player starts a new game
    /// </summary>
    public void StartGame()
    {
        menuPanel.gameObject.SetActive(false);
        gameUIPanel.gameObject.SetActive(true);
        gameOverPanel.gameObject.SetActive(false);

        // instantiate a new player for the scene
        player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        // reset game values
        score = 0;
        round = 1;
        time = 0;

        // spawn new enemies
        SpawnEnemies(round);

        currentState = GameState.Gameplay;
    }

    /// <summary>
    /// This function is called when the player is destroyed, signals the program to end the game
    /// </summary>
    public void GameOver()
    {
        currentState = GameState.GameOver;
        menuPanel.gameObject.SetActive(false);
        gameUIPanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(true);

        // clear any lefover enemies
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    /// <summary>
    /// Spawns a random number of shooter and standard enemies in a gaussian distribution around the player
    /// </summary>
    /// <param name="baseNumber">minimum number of enemies to spawn</param>
    private void SpawnEnemies(int baseNumber)
    {
        int numEnemies = Random.Range(baseNumber, baseNumber + 3);

        for(int i = 0; i < numEnemies; i++)
        {
            enemies.Add(Instantiate(
                enemyStandardPrefab,
                new Vector3(
                    Gaussian(player.transform.position.x, 20),
                    Gaussian(player.transform.position.y, 20),
                    0), Quaternion.identity));
        }

        // for every 5 normal enemies that spawn, 1 shooter enemy will spawn
        for (int i = 0; i < numEnemies / 5; i++) 
        {
            enemies.Add(Instantiate(
                enemyShooterPrefab,
                new Vector3(
                    Gaussian(player.transform.position.x, 20),
                    Gaussian(player.transform.position.y, 20),
                    0), Quaternion.identity));
        }

        // for every 10 normal enemies that spawn, 1 fast enemy will spawn
        for (int i = 0; i < numEnemies / 10; i++)
        {
            enemies.Add(Instantiate(
                enemyFastPrefab,
                new Vector3(
                    Gaussian(player.transform.position.x, 20),
                    Gaussian(player.transform.position.y, 20),
                    0), Quaternion.identity));
        }
    }

    /// <summary>
    /// Get a random number with gaussian distribution
    /// </summary>
    /// <param name="mean">the mean value</param>
    /// <param name="stdDev">the standard deviation from the mean</param>
    /// <returns>a random float with gaussian distribution</returns>
    private float Gaussian(float mean, float stdDev)
    {
        float val1 = Random.Range(0f, 1f);
        float val2 = Random.Range(0f, 1f);

        float gaussValue =
        Mathf.Sqrt(-2.0f * Mathf.Log(val1)) *
        Mathf.Sin(2.0f * Mathf.PI * val2);

        return mean + stdDev * gaussValue;
    }

    /// <summary>
    /// Draws markers around the player that indicate the direction of enemies
    /// </summary>
    private void DrawMarkers()
    {
        foreach(GameObject enemy in enemies)
        {
            if(enemy.GetComponent<EnemyController>().Marker != null)
            {
                enemy.GetComponent<EnemyController>().Marker.transform.position = player.transform.position + (enemy.transform.position - player.transform.position).normalized * markerRadius;
            }
        }
    }
}
