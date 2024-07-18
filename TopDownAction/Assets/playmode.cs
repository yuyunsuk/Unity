using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playmode : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveDistanceZ = moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");
        transform.Translate(0, -moveDistanceZ, 0);
        float rotateAngle = rotateSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        transform.Rotate(0, 0, rotateAngle);
    }
}
