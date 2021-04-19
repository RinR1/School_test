using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimInfo
{
    private string animName;
    private string fileName;

    public void SetInfo(string _name, string _file)
    {
        animName = _name;
        fileName = _file;
    }

    public string GetAnimName()
    {
        return animName;
    }

    public string GetFileName()
    {
        return fileName;
    }
}
