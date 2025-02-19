using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    private float spawnRange = 9.0f;
    public int enemeyCount;
    public int waveNumber = 1; // to keep track of the waves
    private PlayerController playerControllerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnEnemyWave(waveNumber);
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
        for (int i = 0; i < enemiesToSpawn; i++) // that it spawnes enemies at the same time
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);

        }
    }
}
