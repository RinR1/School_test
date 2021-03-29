using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton : MonoBehaviour
{

    public string szDispName;
    public string szFileName;
    public int nAnimFileIndex = 0;

    public void SetInfo(string szName, string szFile, int nIndex)
    {
        szDispName = szFileName;
        szFileName = szFile;
        nAnimFileIndex = nIndex;
    }

    public void OnClickMyButton()
    {
        
    }
}
