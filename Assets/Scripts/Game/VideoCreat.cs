using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoCreat : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject gameObject1;
    public void CreatPlayVideo()
    {
        Instantiate(gameObject1, this.transform.position, Quaternion.identity);
    }
}
