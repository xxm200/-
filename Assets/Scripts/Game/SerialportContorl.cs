using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
public class SerialportContorl : MonoBehaviour
{
    [SerializeField] float arx;
    [SerializeField] float lerp;
    float currentSpeed;
    float targetSpeed;

    float blueToothCurrentInput;
    CinemachineDollyCart cinemachineDollyCart;
    private void Start() {
        cinemachineDollyCart = GetComponent<CinemachineDollyCart>();
        EventManager.AddListener<SeriportReciveEvent>(SerialRecive);
    }
    private void Update() {
        currentSpeed = Mathf.Lerp(currentSpeed,targetSpeed,lerp*Time.deltaTime);
        cinemachineDollyCart.m_Speed = currentSpeed;
    }
    private void SerialRecive(SeriportReciveEvent evt)
    {
        var data = evt.data;
        byte[] fourBytes = new byte[4];
        Array.Copy(data, fourBytes,4);
        int value = (fourBytes[0] << 24) | (fourBytes[1] << 16) | (fourBytes[2] << 8) | fourBytes[3];
        targetSpeed = value/arx;
        // Debug.Log(targetSpeed+"  "+value+"   "+BitConverter.ToString(fourBytes) );
        // int value = string.IsNullOrEmpty(data.Substring(18,2))? 0:int.Parse(data.Substring(18,2),System.Globalization.NumberStyles.HexNumber);
        // blueToothCurrentInput = value;
        // targetSpeed = blueToothCurrentInput/arx;
    }

    private void OnDestroy() {
         EventManager.RemoveListener<SeriportReciveEvent>(SerialRecive);
    }
}
