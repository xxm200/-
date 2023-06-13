using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Types;

public class UserMode : Singleton<UserMode>, IMActer
{
    public BundToValue<int> currentLevel = new BundToValue<int>(0);
    public BundToValue<int> score = new BundToValue<int>(0);

    public string VedioName;
    public string VedioPath = "Video/";
    public int rightCnt;
    public int errorCnt;


    public void Init()
    {

    }


}
