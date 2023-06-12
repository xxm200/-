using KoboldCom;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class com : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int SerialPort;
    Communicator communicator;
    public InputField inputData;
    public Text txtResult;
    int isStart = 0;
    int recivecnt = 0;
    byte[] recivebuf = new byte[12];
    int i = 0;
    bool isret;
    void Start()
    {
        // 创建通讯器
        communicator = new Communicator(new SerialPort(), null);
        openCom();
        isret = false;
    }
    public static void Reset()
    {
        
    }
    private void OnDestroy()
    {
        closeCom();
    }
    public void openCom()
    {
        if (communicator == null)
        {
            Debug.Log("communicator 未初始化");
            return;
        }
        
        SerialPortSetting sps = new SerialPortSetting();
        sps.Baudrate = 115200;  //波特率
        sps.Port = SerialPort; //串口号Com1

        if (communicator.Com.Open(sps))
        {

            Debug.Log("串口打开成功");
            // txtResult.text = "串口打开成功";
            
        }
        else
        {
        //    txtResult.text = "串口打开失败";
            Debug.Log("串口打开失败");
        }
    }

    public void closeCom()
    {
        if (communicator == null)
        {
            Debug.Log("communicator 未初始化");
            txtResult.text = "未初始化";
            return;
        }
        communicator.Com.Close();
    }

    public void btnSend()
    {
        string senddata = inputData.text;
        SendData(Encoding.UTF8.GetBytes(senddata));
    }

    // Update is called once per frame
    void Update()
    {
         if(Input.GetKeyDown(KeyCode.A))
        {
            if(!isret)
            {
                isret = true;
                SendData(Encoding.UTF8.GetBytes("RST"));
            }
           
        }
        //  Debug.Log(communicator.Com.IsOpen);
        if (communicator != null && communicator.Com.IsOpen && communicator.Com.BytesToRead > 0)
        {
            int count = communicator.Com.BytesToRead;
            byte[] buffer = new byte[count];
            communicator.Com.Read(buffer, 0, count);
            // OnRawDataReceived(buffer);
            OndataReceived(buffer);
        }
        
    }

    void OndataReceived(byte[] bytes)
    {
        foreach (var by in bytes)
        {
            if (isStart == 0)
            {
                if (by == 0x5A)
                    isStart = 1;
            }
            else if (isStart == 1)
            {
                if (by == 0xA5)
                {
                    isStart = 2;
                    recivecnt = 0;
                }
            }
            else
            {
                if (by == 0xCC)
                {
                    isStart = 0;
                    recivecnt = 0;
                    SeriportReciveEvent evt = new SeriportReciveEvent();
                    evt.data =    recivebuf;
                    EventManager.Broadcast(evt);
                    var data = BitConverter.ToString(recivebuf);
                    Debug.Log(data);
                }
                recivebuf[recivecnt] = by;
                recivecnt++;
            }
        }
    }

    private void OnRawDataReceived1(byte[] bytes)
    {
        foreach (var by in bytes)
        {
            if (isStart == 0)
            {
                if (by == 84)
                {
                    isStart = 1;
                }
            }
            else if (isStart == 1)
            {
                if (by == 44)
                {
                    isStart = 2;
                    recivecnt = 2;
                    recivebuf[0] = 84;
                    recivebuf[1] = 44;
                    // Debug.Log("一帧");
                }
                else
                {
                    isStart = 0;
                }
            }
            else if (isStart == 2)
            {
                recivebuf[recivecnt] = by;
                recivecnt++;
                if (recivecnt > 43)
                {
                    // recivebufs = recivebuf;
                    // ISevent = true;
                    BlueToothReciveEvent evt = new BlueToothReciveEvent();
                    // evt.data = recivebuf;
                    EventManager.Broadcast(evt);

                    // creatPoint.instance.InstaaCube(recivebuf);
                    isStart = 0;
                    // string recivestring = "";
                    // foreach (var buf in recivebuf)
                    // {
                    //     recivestring += buf + "-";
                    // }
                    // Debug.Log(recivestring);
                    float raystart = (evt.data[5] * 256 + evt.data[4]) / 100;
                    float rayend = (evt.data[43] * 256 + evt.data[42]) / 100;

                    // if (rayend < raystart)
                    // {
                    //     Debug.Log(i + "");
                    //     i = 0;
                    // }
                    // else
                    // {
                    //     i++;
                    // }

                }
            }
        }
    }

    private void OnRawDataReceived(byte[] bytes)
    {
        Debug.Log(bytes.Length);
        StringBuilder builder = new StringBuilder();
        foreach (byte b in bytes)
        {
            builder.Append(b.ToString("X2") + " ");
        }
        Debug.Log("收到串口数据:" + builder.ToString());

        txtResult.text = "收到串口数据:" + builder.ToString();

    }

    private void SendData(byte[] d)
    {
        if (communicator == null)
        {
            Debug.Log("communicator 未初始化");
            txtResult.text = "未初始化";
            return;
        }
        if (d == null || d.Length == 0)
        {
            Debug.Log("发送数据为空");
            txtResult.text = "发送数据为空";
            return;
        }
        try
        {
            communicator.Com.Write(d, 0, d.Length);
          //  txtResult.text = "发送成功";
        }
        catch (Exception e)
        {
            Debug.Log( e.ToString());
            txtResult.text = "发送失败：" + e.ToString();
        }
    }
}
