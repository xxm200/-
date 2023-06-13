using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    // Start is called before the first frame update
    Quaternion init;
    void Start()
    {
        init = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = init;
    }
}
