using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimButton : MonoBehaviour
{
    public string szDispName;
    public string szFileName;


    public void SetInfo(string szName, string szFile)
    {
        szDispName = szName;
        szFileName = szFile;
   

        transform.Find("Text").GetComponent<Text>().text = szDispName;
    }

    public void OnClickButton()
    {
        reasd.Instance.PlayAnimation(szFileName);
    }
}
