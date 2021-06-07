using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class GameInit : MonoBehaviour
{
    public Text _textAttackFrame;
    public Dropdown _dropdownAttackType;
    public Text _textAttackDist;
    public Text _textAttackOffset;
    public Dropdown _dropdownDamageType;
    public Text _textDamageEffect;
    public Text _textEffectFrame;
    public Text _textEffectFile;

    // Start is called before the first frame update
    void Start()
    {
        reasd.Instance.Initial();   
  
    }

    //버튼 기능들을 호출하여 버튼을 눌렀을 때 실행
    public void OnClickButtonPlayPause()
    {
        reasd.Instance.PlayPause();
    }

    public void OnClickAddAction()
    {
        CActionData xActionData = new CActionData();

        xActionData.nAttackFrame = int.Parse(_textAttackFrame.text);
        xActionData.byAttackType = (byte)_dropdownAttackType.value;
        xActionData.fAttackDist = float.Parse(_textAttackDist.text);
        xActionData.fAttackOffset = float.Parse(_textAttackOffset.text);
        xActionData.byDamageType = (byte)_dropdownDamageType.value;
        xActionData.szDamageEffect = _textDamageEffect.text;

        reasd.Instance.AddActionData(xActionData);
        reasd.Instance.UpdateUIformActionDataList();
        //UI에 액션 데이터 추가

    }

    public void onclickSave() 
    {
        reasd.Instance.SaveAction();
    }

    public void OnClickDeleteAction()
    {
        reasd.Instance.RemoveAction();
    }

    /*public void onClickButtonActionSave() 
    {
        //string szFileName = string.Format(@"{0}/@{1}_{2}.act", Application.streamingAssetsPath + @"/test.act", );
           

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(szFileName, FileMode.Create, FileAccess.Write);
        bf.Serialize(fs, data);
        fs.Close();
    }*/

    public void OnValueChangeAnimLoop(bool bCheck) 
    {
        reasd.Instance.SetAnimLoop(bCheck);
    }

    public void onClickButtonResetPosition()
    {
        reasd.Instance.ObjectResetPosition();
    }

    public void OnClickButtonAddEffect() 
    {
        CEffectData xEffectData = new CEffectData();
        xEffectData.nEffectFrame = int.Parse(_textEffectFrame.text);
        xEffectData.szFilename = _textEffectFile.text;

        reasd.Instance.AddEffectData(xEffectData);
        reasd.Instance.UpdateUIformEffectDataList();
    }

    public void OnClickButtonDeleteEffect()
    {
        reasd.Instance.RemoveEffect();
    }

    public void OnClickButtonEffectSave() 
    {
        reasd.Instance.SaveEffect();
    }
}

[System.Serializable]
public class CActionData
{
    public int nAttackFrame;
    public byte byAttackType;
    public float fAttackDist;
    public float fAttackOffset;
    public byte byDamageType;
    public string szDamageEffect;
}


[System.Serializable]
public class CEffectData
{
    public int nEffectFrame;
    public string szFilename;
}
