using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    public string szDispName;
    public int nListIndex;


    public void SetInfo(string szName, int nIndex)
    {
        szDispName = szName;
        nListIndex = nIndex;


        transform.Find("Text").GetComponent<Text>().text = szDispName;
    }

    public void OnClickActionDataButton()
    {
        reasd.Instance.SetactionIndex(nListIndex);
    }
}
