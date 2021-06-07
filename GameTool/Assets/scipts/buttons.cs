using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttons : MonoBehaviour
{

    public string szDispName;
    public string szFileName;
    public byte bytype = 0;
    public int nIndex;

    public void SetInfo(string szName,string szFile,byte byValue, int nIdx)
    {
        szDispName = szName;
        szFileName = szFile;
        bytype = byValue;
        nIndex = nIdx;

        transform.Find("Text").GetComponent<Text>().text = szDispName;
    }

    public void OnClickButton()
    {
        reasd.Instance.CreatePlayer(szFileName, nIndex);
        reasd.Instance.UpdateUIformAnimList(bytype);
    }
}
