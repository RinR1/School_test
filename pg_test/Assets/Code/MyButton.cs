using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyButton : MonoBehaviour
{

    public string szDispName;
    public string szFileName;
    public byte byType = 0;

    public void SetInfo(string szName, string szFile, byte byValue)
    {
        szDispName = szName;
        szFileName = szFile;
        byType = byValue;

        transform.Find("Text").GetComponent<Text>().text = szDispName;
    }

    public void OnClickMyButton()
    {
        Debug.Log("Button Click => " + szDispName);
    }
}
