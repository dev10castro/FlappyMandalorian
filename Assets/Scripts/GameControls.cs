using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para reiniciar la escena

public class GameControls : MonoBehaviour
{
    public Renderer background;
    public float velocity = 1.1f;
    public float acceleration = 0.01f;
    public GameObject colFloor;
    public List<GameObject> colFloors = new List<GameObject>();
    public GameObject stone, stone2, enemy, boss;
    public List<GameObject> obstacles = new List<GameObject>();
    public float minimumDistanceBetweenEnemies = 3f;
    public float enemySpawnTimer;
    private bool _bossSpawned = false;
    private float bossSpawnDelay = 3f; 
    private float bossTimer = 0f;
    private int _enemyCount = 0;
    
    //marcador
    private int score = 0;
    public TMPro.TextMeshProUGUI scoreText; // Referencia al UI Text


    // UI Elements
    public GameObject menuUI;
    public GameObject gameOverUI;
    public GameObject youWinUI;

    private bool isGameRunning = false;
    private bool gameOver = false; // Variable para saber si el juego terminó

    void Start()
    {
        for (int i = 0; i <= 20; i++)
        {
            colFloors.Add(Instantiate(colFloor, new Vector2(-12 + i, -1), Quaternion.identity));
        }
        ShowMenu();
    }

    void Update()
    {
        if (!isGameRunning && Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Tecla S presionada: iniciando el juego.");
            StartGame();
        }
        if (!isGameRunning && Input.GetKeyDown(KeyCode.S)) // Ahora el juego comienza con "S"
        {
            StartGame();
        }

        if (gameOver && Input.GetKeyDown(KeyCode.R)) 
        {
            RestartGame(); // Reinicia el juego cuando se presiona "R"
        }

        if (!isGameRunning) return;

        velocity += acceleration * Time.deltaTime;

        foreach (var floor in colFloors)
        {
            if (floor != null)
            {
                Vector2 currentPosition = floor.transform.position;
                currentPosition.x -= 1.55f * Time.deltaTime;

                if (currentPosition.x < -12)
                {
                    currentPosition.x = 8;
                }

                floor.transform.position = currentPosition;
            }
        }

        for (int i = obstacles.Count - 1; i >= 0; i--)
        {
            if (obstacles[i] == null)
            {
                obstacles.RemoveAt(i);
                continue;
            }

            Vector2 position = obstacles[i].transform.position;
            position.x -= 1.55f * Time.deltaTime;

            if (position.x < -12)
            {
                Destroy(obstacles[i]);
                obstacles.RemoveAt(i);
            }
            else
            {
                obstacles[i].transform.position = position;
            }
        }

        if (_enemyCount < 29)
        {
            enemySpawnTimer += Time.deltaTime;
            if (enemySpawnTimer >= 3f)
            {
                SpawnEnemy();
                enemySpawnTimer = 0f;
            }
        }

        if (_enemyCount == 29 && !_bossSpawned)
        {
            bossTimer += Time.deltaTime; // ⏳ Comienza a contar el tiempo
            if (bossTimer >= bossSpawnDelay) 
            {
                SpawnBoss();
            }
        }

        if (background != null && background.material != null)
        {
            background.material.mainTextureOffset += new Vector2(velocity, 0) * Time.deltaTime;
        }
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = null;
        Vector2 spawnPosition = Vector2.zero;
        int rand = Random.Range(0, 3);

        if (rand == 0)
        {
            newEnemy = stone;
            spawnPosition = new Vector2(12f, -2.74f);
        }
        else if (rand == 1)
        {
            newEnemy = Instantiate(enemy, new Vector2(12f, -1.65f), Quaternion.identity);

            // Si el enemigo es DarkMouth, le asignamos su posición y vida correctamente
            if (newEnemy.CompareTag("EnemyDarkMouth"))
            {
                Enemy enemyScript = newEnemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.health = 3; // Fija la vida a 3
                    Debug.Log("DarkMouth generado con vida: " + enemyScript.health);
                }
            }
            spawnPosition = new Vector2(12f, -1.65f); // DarkMouth aparecerá en la misma posición que los demás enemigos
        }
        else
        {
            newEnemy = stone2;
            spawnPosition = new Vector2(12f, 1.5f);
        }

        bool canSpawn = true;
        foreach (var obstacle in obstacles)
        {
            if (Vector2.Distance(obstacle.transform.position, spawnPosition) < minimumDistanceBetweenEnemies)
            {
                canSpawn = false;
                break;
            }
        }

        if (canSpawn)
        {
            GameObject spawnedEnemy = Instantiate(newEnemy, spawnPosition, Quaternion.identity);
            obstacles.Add(spawnedEnemy);
            _enemyCount++;

            if (rand == 2)
            {
                spawnedEnemy.AddComponent<Stone2Movement>();
            }
        }
    }



    void SpawnBoss()
    {
        if (_bossSpawned) return; // Evita múltiples invocaciones
    
        _bossSpawned = true; 
        GameObject spawnedBoss = Instantiate(boss, new Vector2(12f, -0.2f), Quaternion.identity);
        obstacles.Add(spawnedBoss);

        // Llamamos a un script en el Boss para detectar su eliminación
        spawnedBoss.AddComponent<BossHealth>();
    }

    public void ShowMenu()
    {
        menuUI.SetActive(true);
        gameOverUI.SetActive(false);
        Time.timeScale = 0;
        isGameRunning = false;
    }

    public void StartGame()
    {
        menuUI.SetActive(false);
        gameOverUI.SetActive(false);
        youWinUI.SetActive(false);
        Time.timeScale = 1; // Se asegura de que el tiempo vuelve a la normalidad
        isGameRunning = true;
        gameOver = false; // Reiniciamos la variable
    }
    
    public void AddScore(int points)
    {
        score += points;
        if (scoreText != null)
        {
            scoreText.text = "Score: " + "0"+score;
        }
    }


    public void GameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0;
        isGameRunning = false;
        gameOver = true; // Indicamos que el juego ha terminado
    }

    public void YouWin()
    {
        youWinUI.SetActive(true);
        Time.timeScale = 0;
        isGameRunning = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Asegurar que el juego no esté pausado
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reinicia la escena actual
    }
}
