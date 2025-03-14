using System.Collections.Generic;
using UnityEngine;

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
    private int _enemyCount = 0;

    void Start()
    {
        for (int i = 0; i <= 20; i++)
        {
            colFloors.Add(Instantiate(colFloor, new Vector2(-12 + i, -1), Quaternion.identity));
        }
    }

    void Update()
    {
        velocity += acceleration * Time.deltaTime;

        // 🔹 Movimiento del suelo
        for (int i = 0; i < colFloors.Count; i++)
        {
            if (colFloors[i] != null)
            {
                Vector2 currentPosition = colFloors[i].transform.position;
                currentPosition.x -= 1.55f * Time.deltaTime;

                if (currentPosition.x < -12)
                {
                    currentPosition.x = 8;
                }

                colFloors[i].transform.position = currentPosition;
            }
        }

        // 🔹 Movimiento de obstáculos y eliminación segura de objetos destruidos
        for (int i = obstacles.Count - 1; i >= 0; i--) // 🔹 Iteramos al revés para evitar errores de índice
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

        // 🔹 Spawner de enemigos (máximo 19 enemigos)
        if (_enemyCount < 29)
        {
            enemySpawnTimer += Time.deltaTime;
            if (enemySpawnTimer >= 3f)
            {
                SpawnEnemy();
                enemySpawnTimer = 0f;
            }
        }

        // 🔹 Spawnear el jefe SOLO cuando no haya enemigos en pantalla y después del enemigo 19
        if (_enemyCount == 29 && !_bossSpawned)
        {
            SpawnBoss();
        }

        // 🔹 Movimiento del fondo (evita errores si `background` es `null`)
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
            newEnemy = enemy;
            spawnPosition = new Vector2(12f, -1.65f);
        }
        else
        {
            newEnemy = stone2;
            spawnPosition = new Vector2(12f, -0.5f);
        }

        // Verificar si hay suficiente distancia entre el nuevo enemigo y los existentes
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
            Debug.Log("Enemigo generado: " + newEnemy.name + " | Total enemigos: " + _enemyCount);
        }
    }

    void SpawnBoss()
    {
        GameObject spawnedBoss = Instantiate(boss, new Vector2(12f, -0.2f), Quaternion.identity);
        obstacles.Add(spawnedBoss);
        _bossSpawned = true;
        Debug.Log("Boss generado después de 19 enemigos y cuando no quedan enemigos en pantalla.");
    }
}
