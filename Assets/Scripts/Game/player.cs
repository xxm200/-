using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class player : MonoBehaviour
{
    // Start is called before the first frame update
    public VideoCreat videoCreat;
    [SerializeField] CinemachineDollyCart cinemachine;


    void Start()
    {
        EventManager.AddListener<RedArmyStateEvent>(PlayMove);
    }

    // Update is called once per frame
    void Update()
    {
        



    }


    void PlayMove(RedArmyStateEvent redArmyStateEvent)
    {
        cinemachine.m_Speed = 1;
    }

    private void ShowVedio(string videoname)
    {
        cinemachine.m_Speed = 0;
        UserMode.Instance.VedioName = videoname;
        videoCreat.CreatPlayVideo();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShowVedio(collision.name+"");
        Debug.Log(collision.name);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<RedArmyStateEvent>(PlayMove);
    }

}
