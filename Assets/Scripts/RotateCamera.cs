using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float rotationSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up , Time.deltaTime * rotationSpeed* horizontalInput ); // this way the first parameter will be the axis and the second parameter will be the rotaiton angel (axis angle rotation)
    }
}
