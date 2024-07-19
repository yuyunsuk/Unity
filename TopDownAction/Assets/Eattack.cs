using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eattack : MonoBehaviour
{
    public ParticleSystem explosion;
    public int scoreValue = 10;
    bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "bullet")
        {
            ScoreManager.score += scoreValue;
            Destroy(gameObject);
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
    }
}
