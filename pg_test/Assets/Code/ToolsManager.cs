using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ToolsManager : MonoBehaviour
{
    public GameObject _objContent;
    public GameObject _objMybutton;
    public List<ObjectInfo> _listObject;

    // Start is called before the first frame update
    void Start()
    {
        _listObject = new List<ObjectInfo>();

        OpenObjectList();
        UpdateUIformObjectList();
    }

    public void OpenObjectList()
    {

        string szPath = Application.dataPath + "/StreamAssets" + "/" + "CharList.txt";
        string[] arFileDesc = File.ReadAllLines(szPath);

        foreach (string szLine in arFileDesc)
        {
            string[] szPart = szLine.Split(',');

            ObjectInfo obj = new ObjectInfo();
            obj.szDispName = szPart[0];
            obj.szFileName = szPart[1];
            obj.byType = byte.Parse(szPart[2]);

            _listObject.Add(obj);
        }
    }

    public void UpdateUIformObjectList()
    {
        foreach(ObjectInfo info in _listObject)
        {
            var inst = Instantiate(_objMybutton, new Vector3(0, 0, 0), Quaternion.identity);
            inst.transform.SetParent(_objContent.transform);

            MyButton btnScript = inst.GetComponent("MyButton") as MyButton;
            btnScript.SetInfo(info.szDispName, info.szFileName, info.byType);
        }
    }

    public void OnClickRemoveAll()
    {
        Transform[] objChilds = _objContent.GetComponentsInChildren<Transform>();


        foreach (Transform trans in objChilds)
        {
            if(trans != _objContent.transform)
               Destroy(trans.gameObject);
        }
    }

    public void OnClickReset()
    {
        _listObject.RemoveAll(e => true);
        OpenObjectList();
        UpdateUIformObjectList();
    }
}
