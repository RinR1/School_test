using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ToolsManager : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        string szPath = Application.dataPath + "/StreamAssets" + "/" + "CharList.txt";
        string[] arFileDesc = File.ReadAllLines(szPath);

       foreach(string szLine in arFileDesc)
        {
            string[] szPart = szLine.Split(',');
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
