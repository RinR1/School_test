using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimButton : MonoBehaviour
{
    private string charterName;
    private string fileName;

    public void SetInfo(string _name, string _file)
    {
        charterName = _name;
        fileName = _file;

        transform.GetChild(0).GetComponent<Text>().text = charterName;
    }

    public void OnClickbutton()
    {
        Debug.Log(charterName + " / " + fileName);
    }
}
