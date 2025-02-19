using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossMinions : MonoBehaviour
{
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private int minionCount = 5;
    void Start()
    {
        StartCoroutine(MinionsCoolDown());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void GenerateMinions()
    {
        for(int i = 0; i<minionCount; i++)
        {
            Instantiate(minionPrefab, transform.position + GenerateRandomVector3(), minionPrefab.transform.rotation);
        }
    }
    IEnumerator MinionsCoolDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            GenerateMinions();
        }
    }
    Vector3 GenerateRandomVector3() {
        return new Vector3(Random.Range(0f, 9f), 0, Random.Range(0f, 9f));
    }
    
}
