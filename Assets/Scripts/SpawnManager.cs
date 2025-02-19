using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public GameObject powerupPrefab;
    private float spawnRange = 9.0f;
    public int enemeyCount;
    public int waveNumber; // to keep track of the waves
    public int bossToSpawn;
    private PlayerController playerControllerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossToSpawn = 0;
        Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation); // to generate a power up at the begging of the game
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>(); // to get the script of
    }

    // Update is called once per frame
    void Update()
    {
        enemeyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;// this will return an array of objects, and it uses a scripts as a type
        if(enemeyCount == 0 && playerControllerScript.isGameActive) // no enemies left in the battle
        {
            waveNumber++; // increase the enemies
            SpawnEnemyWave(waveNumber); //spawn the enemies based on the wave number
            Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation); // to generate power up for each wave.
        }
    }
    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }
    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        if (enemiesToSpawn % 5 == 0 && waveNumber!=0) // condition to spawn a special enemy
        {
            bossToSpawn++;
            for (int i = 0; i < enemiesToSpawn-bossToSpawn; i++) // that it spawnes enemies at the same time
            {
                int randomEnemyIndex = Random.Range(0, 2);
                Instantiate(enemyPrefab[randomEnemyIndex], GenerateSpawnPosition(), enemyPrefab[randomEnemyIndex].transform.rotation);

            }
            SpawnBossEnemy(bossToSpawn);
        }
        else
        {
            for (int i = 0; i < enemiesToSpawn; i++) // that it spawnes enemies at the same time
            {
                int randomEnemyIndex = Random.Range(0, 2);
                Instantiate(enemyPrefab[randomEnemyIndex], GenerateSpawnPosition(), enemyPrefab[randomEnemyIndex].transform.rotation);

            }
        }
    }
    private void SpawnBossEnemy(int bossToSpawn)
    {
        for(int i = 0;i < bossToSpawn; i++)
        {
            Instantiate(enemyPrefab[2], GenerateSpawnPosition(), enemyPrefab[2].transform.rotation);
        }
        
    }
}
