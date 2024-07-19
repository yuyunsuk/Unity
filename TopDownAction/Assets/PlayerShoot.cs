using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    public GameObject bulletPrefab;
    float delayTimer = 0f;
    public float shootDelayTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        delayTimer += Time.deltaTime;
        if (Input.GetButtonDown("Jump") && delayTimer > shootDelayTime)
        {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
            delayTimer = 0f;
        }
    }
}
