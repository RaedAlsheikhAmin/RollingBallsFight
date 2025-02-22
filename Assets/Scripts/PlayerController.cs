using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//this line is important to test the Exit button while we are in the editor mode.
#if UNITY_EDITOR
using UnityEditor;
#endif


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
    [SerializeField] private Button startGameButton;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI userNameLabel;
    [SerializeField] private TMP_InputField userNameTextArea; // to get the input from the text area
    [SerializeField] private TextMeshProUGUI userNameShowText;
    [SerializeField] private TextMeshProUGUI userNameErrorMessage;
    [SerializeField] private Button exitGameButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isGameActive = false;   
        playerRb = GetComponent<Rigidbody>();//to get the rigid body component
        focalPoint = GameObject.Find("Focal Point"); // we can use this, because it is in the same heirachy and scene    
        restartButton.onClick.RemoveAllListeners(); // to get rid of all the listeners first
        restartButton.onClick.AddListener(RestartGame); // to restart the game
        spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive)
        {
            float verticalInput = Input.GetAxis("Vertical");
            playerRb.AddForce(focalPoint.transform.forward * speed * verticalInput);// take the forward of the focal point will make sure that the player will move according to the camer position
            powerupIndicator.transform.position = transform.position + offsetIndicator; // to make the indicator on the player
            if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
            {
                playerRb.AddForce(focalPoint.transform.up * jumpStrength, ForceMode.Impulse);
                isOnGround = false;
            }
            if (transform.position.y < limitY)
            {
                gameOverText.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(true);
                isGameActive = false;// because the player lost
            }
            waveNumberText.text = "Wave: " + spawnManagerScript.waveNumber;
        }
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
    public void StartGame()
    {
        if (!string.IsNullOrWhiteSpace(userNameTextArea.text)) // Check if it's empty or spaces
        {
            isGameActive = true; // Game starts
            startGameButton.gameObject.SetActive(false);
            titleText.gameObject.SetActive(false);
            userNameLabel.gameObject.SetActive(false);
            string userName = userNameTextArea.text.Trim(); // Remove extra spaces
            userNameShowText.text = userName;
            userNameTextArea.gameObject.SetActive(false);
            userNameErrorMessage.gameObject.SetActive(false);
            exitGameButton.gameObject.SetActive(false);
        }
        else
        {
            userNameErrorMessage.gameObject.SetActive(true);
            
        }
    }
    public void Exit()
    {
        //this is called conditonal compiling which uses # for the conditions, which will not be build during standalone player build.
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit(); // original code to quit Unity player
        #endif
    }
}

