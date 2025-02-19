using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 2.0f;
    private Rigidbody playerRb;
    private GameObject focalPoint; // to get a reference for the focal point
    public bool hasPowerup = false; // to check when it collides 
    private float powerupStrength = 15.0f;
    public GameObject powerupIndicator; // to get a reference
    private Vector3 offsetIndicator = new Vector3(0,-0.5f,0); // to move the indicator down a little
    private float jumpStrength = 5.0f;
    private float limitY = -4.0f;
    public bool isOnGround = true;
    private SpawnManager spawnManagerScript;
    [SerializeField] private TextMeshProUGUI gameOverText; // to show the game over
    [SerializeField] private Button restartButton;
    [SerializeField] public bool isGameActive;
    [SerializeField] private TextMeshProUGUI waveNumberText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isGameActive = true; // when the player starts the game
        playerRb = GetComponent<Rigidbody>();//to get the rigid body component
        focalPoint = GameObject.Find("Focal Point"); // we can use this, because it is in the same heirachy and scene    
        restartButton.onClick.AddListener(RestartGame); // to restart the game
        spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * verticalInput );// take the forward of the focal point will make sure that the player will move according to the camer position
        powerupIndicator.transform.position = transform.position + offsetIndicator; // to make the indicator on the player
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(focalPoint.transform.up * jumpStrength, ForceMode.Impulse);
            isOnGround = false;
        }
        if(transform.position.y < limitY)
        {
            gameOverText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            isGameActive = false;// because the player lost
        }
        waveNumberText.text = "Wave: " + spawnManagerScript.waveNumber;
    }
    private void OnTriggerEnter(Collider other) // we use the trigger when we want to understand something 
    {
        if (other.gameObject.CompareTag("Powerup"))  
        {
            hasPowerup = true; // trun on the Powerup boolean
            powerupIndicator.SetActive(true); // to show the powerup Indicator
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCountDownRoutine());// to start the thread, useful to get the time out of update method
        }
       
    }
    private void OnCollisionEnter(Collision collision) // we use the collision when we want to change physics
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();//to get the rigid body of the enemy that we collided with
            Vector3 awayFromThePlayer= (collision.gameObject.transform.position - transform.position);
            enemyRb.AddForce(awayFromThePlayer * powerupStrength , ForceMode.Impulse); // to apply the force imediatly
            Debug.Log("Collides with: "+ collision.gameObject.name + " power up is set to " + hasPowerup);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }
    IEnumerator PowerUpCountDownRoutine()//interface to create a thread
    {
        yield return new WaitForSeconds(7); // it will create a thread that waits for 7 seconds
        hasPowerup = false;
        powerupIndicator.SetActive(false);// to turn off the power up
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
         
}
