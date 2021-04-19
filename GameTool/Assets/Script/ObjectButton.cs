using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectButton : MonoBehaviour
{
    private string charterName;
    private string fileName;
    private byte animIndex = 0;

    public void SetInfo(string _name, string _file, byte _index)
    {
        charterName = _name;
        fileName = _file;
        animIndex = _index;
        
        transform.GetChild(0).GetComponent<Text>().text = charterName;
    }

    public void OnClickbutton()
    {
        //ToolsManager toolsmanager = GameObject.Find("UI").GetComponent<ToolsManager>();

        Debug.Log(fileName + " / " + animIndex);

        ToolsManager.Instance.CreatePlayer(fileName);

        switch (animIndex)
        {
            case 0:
                ToolsManager.Instance.InstantiateAnimButton(ANIM_TYPE.CHAR);
                break;
            case 1:
                ToolsManager.Instance.InstantiateAnimButton(ANIM_TYPE.NPC);
                break;
            case 2:
                ToolsManager.Instance.InstantiateAnimButton(ANIM_TYPE.MON);
                break;
        }

    }
}
