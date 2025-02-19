using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float speed = 1.0f;
    private GameObject player;
    private Rigidbody enemyRb;
    private float limitY = -4.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>(); // to assign the game object of the enemy
        player = GameObject.Find("Player"); // to find the player in the heirachy
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);// we are first fiding the vector between the enemy and the player then we are normalizing it that the great distance doesn't create a great force
        if (transform.position.y < limitY)
        {
            Destroy(gameObject); // to destroy the objects that fall of the platform
        }
    }

}
