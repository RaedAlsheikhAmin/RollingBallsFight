using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public GameObject powerupPrefab;
    public GameObject nuclearPrefab;
    private float spawnRange = 9.0f;
    public int enemeyCount;
    public int waveNumber; // to keep track of the waves
    public int bossToSpawn;
    private float timeTogenerateNuclear = 0;
    private float timeForNextNuclear = 30.0f;
    private bool canSpawn = true; // Controls whether enemies can spawn
    private PlayerController playerControllerScript;
    [SerializeField] private AudioClip enemyThunderAudio;
    private bool breakBetweenWaves;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossToSpawn = 0;
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>(); // to get the script of
    }

    // Update is called once per frame
    void Update()
    {
        timeTogenerateNuclear += Time.deltaTime;
        enemeyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;// this will return an array of objects, and it uses a scripts as a type

        if (playerControllerScript.isNuclearActive)
        {
            StartCoroutine(WaitNuclearActive()); // Pause spawning when nuclear is active
        }
        if (enemeyCount == 0 && playerControllerScript.isGameActive && canSpawn && !breakBetweenWaves) // no enemies left in the battle
        {
            breakBetweenWaves = true;
            StartCoroutine(BreakBetweenWaves());
            
        }
        else if (enemeyCount > 6 && playerControllerScript.isGameActive && timeTogenerateNuclear > timeForNextNuclear && canSpawn)
        {
            timeTogenerateNuclear = 0;
            timeForNextNuclear += 10;
            Instantiate(nuclearPrefab, GenerateSpawnPosition(), nuclearPrefab.transform.rotation); // to generate a nuclear powerup
            
        }
    }
    IEnumerator BreakBetweenWaves()//this will spawn the enemeies after few break time
    {
        yield return new WaitForSeconds(Random.Range(1,4));
        breakBetweenWaves=false;
        waveNumber++; // increase the enemies
        SpawnEnemyWave(waveNumber); //spawn the enemies based on the wave number
        Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation); // to generate power up for each wave.
        playerControllerScript.playerAudio.PlayOneShot(enemyThunderAudio, 0.5f); // to get the thunder sound
    }
    IEnumerator WaitNuclearActive()
    {
        canSpawn = false;
        yield return new WaitForSeconds(3);
        canSpawn = true;
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
