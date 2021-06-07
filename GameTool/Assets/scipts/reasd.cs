using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public enum ANIM_TYPE : byte
{
    CHAR = 0,
    NPC,
    MON,
    MAX,
}

public class reasd : MonoSingleton<reasd>
{


    public GameObject _objContent;
    //액션 슬라이더 뷰포트 컨텐츠 이름을 변화 21.5.17
    public GameObject _actionContent;

    public GameObject _objButton;
    public GameObject _objAnimContent;
    public GameObject _objAnimButton;
    public GameObject _objActionDataButton;
    public GameObject _effectContent;
    public GameObject _objEffectDataButton;

    public List<ObjectInfo> _lstObject;
    public List<AnimationInfo>[] _arAnim;
    public List<CActionData> _lstActionData;
    public List<CEffectData> _lstEffectData;

    GameObject _objCurPlayer = null;
    Animator _animCurPlayer = null;
    Text _textFrame;
    Text _textPlayPauseButton;
    Text _textMaxFrame; //오른쪽 UI Info
    Text _textAnimTime; //오른쪽 UI Info
    Slider _sliderFrame;

    int nCurObjIndex = 0; // 현재 선택된 오브젝트 인덱스
    int _nCurAnimIndex = 0; // 현재 선택된 애니 인덱스 // 
    int _nCurActionindex = -1; // 현재 선택된 액션 인덱스
    int _nCurEffectindex = -1; // 현재 선택된 액션 인덱스
    string _szLastAnimFile = "";
    bool _bAnimPause = false;

    bool _bAnimLoop = true;

    // Start is called before the first frame update
    public void Initial()
    {
        //UI찾아서매칭
        _objContent = GameObject.Find("Obj_contents");
        _actionContent = GameObject.Find("Content_Action");
        _objAnimContent = GameObject.Find("Anim_contents");
        _objButton = Resources.Load("Button") as GameObject;
        _objAnimButton = Resources.Load("AnimButton") as GameObject;
        _objActionDataButton = Resources.Load("Button_ActionData") as GameObject;
        _effectContent = GameObject.Find("Content_Effect");
        _objEffectDataButton = Resources.Load("Button_EffectData") as GameObject;

        _textFrame = GameObject.Find("Text_CurFrame").GetComponent<Text>();
        _textPlayPauseButton = GameObject.Find("Text_Button_Play").GetComponent<Text>();
        _sliderFrame = GameObject.Find("Slider_AnimFrame").GetComponent<Slider>();
        _textMaxFrame = GameObject.Find("Text_MaxFrame").GetComponent<Text>();
        _textAnimTime = GameObject.Find("Text_AnimationTime").GetComponent<Text>();


        _lstObject = new List<ObjectInfo>();
        _arAnim = new List<AnimationInfo>[3];
        _lstActionData = new List<CActionData>();
        _lstEffectData = new List<CEffectData>();

        for (int i = 0; i < (short)ANIM_TYPE.MAX; i++)
        {
            _arAnim[i] = new List<AnimationInfo>();
        }
        //오브젝트리스트읽어온다.
        OpenObjectList();
        UpdateUIformObjectList();

        //애니메이션리스트읽어온다.
        OpenCharAnimList();
        OpenNPCAnimList();
        OpenMonAnimList();

        // UpdateUIformAnimList((short)ANIM_TYPE.CHAR);
        //UpdateUIformAnimList((short)ANIM_TYPE.NPC);
        // UpdateUIformAnimList((short)ANIM_TYPE.MON);
    }

    public void Update()
    {
        if (_objCurPlayer != null)
        {
            if (!_bAnimPause)
            {
                _animCurPlayer.speed = 1.0f;

                AnimatorStateInfo info = _animCurPlayer.GetCurrentAnimatorStateInfo(0);
                int nMaxFrame = (int)_animCurPlayer.runtimeAnimatorController.animationClips[_nCurAnimIndex].frameRate;
                _sliderFrame.value = (int)(info.normalizedTime * nMaxFrame) % nMaxFrame;
                _textFrame.text = _sliderFrame.value.ToString();

                if (_bAnimLoop == true) 
                {
                    if (info.IsName(_szLastAnimFile) == false)
                    {
                        _animCurPlayer.Play(_szLastAnimFile, -1, 0.0f);
                    }
                }
            }

            else
            {
                _animCurPlayer.speed = 0.0f;

                int nCurFrame = (int)_sliderFrame.value;
                _textFrame.text = nCurFrame.ToString();
                int nMaxFrame = (int)_animCurPlayer.runtimeAnimatorController.animationClips[_nCurAnimIndex].frameRate;

                _animCurPlayer.Play(_szLastAnimFile, -1, _sliderFrame.value / nMaxFrame);
            }
        }
    }
    //오브젝트 리스트에서 객체 선택되면 오브젝트(Player) 로딩

    public void CreatePlayer(string szFileName, int nindex)
    {
        if (_objCurPlayer != null)
        {
            Destroy(_objCurPlayer);
            _animCurPlayer = null;
            _szLastAnimFile = "";
        }

        GameObject objPlayer = Resources.Load(szFileName) as GameObject;
        if (objPlayer != null)
        {
            nCurObjIndex = nindex;

            _objCurPlayer = Instantiate(objPlayer, new Vector3(0, 0, 0), Quaternion.identity);
            _animCurPlayer = _objCurPlayer.GetComponent<Animator>();

            int nFrame = (int)_animCurPlayer.runtimeAnimatorController.animationClips[_nCurAnimIndex].frameRate;
            _sliderFrame.maxValue = nFrame;
        }
    }

    //애니메이션 리스트에서 선택이 변경되면 해당하는 애니메이션을 실행
    public void PlayAnimation(string szAnimName)
    {
        if (_animCurPlayer != null)
        {
            _szLastAnimFile = szAnimName;

            _animCurPlayer.Play(_szLastAnimFile, -1, 0f);

            DisplayAnimationinfo();
            for (int i = 0; i < _animCurPlayer.runtimeAnimatorController.animationClips.Length; i++) 
            {
                //name == szAnim으로 정리해도 가능
                if (_animCurPlayer.runtimeAnimatorController.animationClips[i].name.CompareTo(szAnimName) == 0) 
                {
                    _nCurAnimIndex = i;

                    _lstActionData.Clear();
                    _lstEffectData.Clear();
                    RemoveAllActionListContent();
                    RemoveAllEffectListContent();

                    LoadAction();
                    LoadEffect();

                    UpdateUIformActionDataList();
                    UpdateUIformEffectDataList(); 
                    
                    break;
                }
            }
        }
    }

    //버튼 종류에 따른 기능들
    public void PlayPause()
    {
        if (_animCurPlayer != null)
        {
            //이거 바뀔듯 10주차 영상 9분쯤
            if (!_bAnimPause)
            {
                _bAnimPause = true;
                _textPlayPauseButton.text = "Play";
            }
            else
            {
                _bAnimPause = false;
                _textPlayPauseButton.text = "Pause";
            }
        }
    }

    public void UpdateUIformAnimList(short nAnimType)
    {
        RemoveAllAnimListContent();

        foreach (AnimationInfo info in _arAnim[nAnimType])
        {
            var inst = Instantiate(_objAnimButton, new Vector3(0, 0, 0), Quaternion.identity);
            inst.transform.SetParent(_objAnimContent.transform);

            AnimButton btnScript = inst.GetComponent("AnimButton") as AnimButton;
            btnScript.SetInfo(info.szDispName, info.szFileName);
        }
    }

    //캐릭터애니메이션읽기
    public void OpenCharAnimList()
    {
        string szPath = Application.dataPath + "/StramAssets" + "/" + "CharAnimation.txt";
        string[] arFileDesc = File.ReadAllLines(szPath);

        foreach (string szLine in arFileDesc)
        {
            string[] szParts = szLine.Split(',');

            AnimationInfo anim = new AnimationInfo();
            anim.szDispName = szParts[0];
            anim.szFileName = szParts[1];

            _arAnim[(short)ANIM_TYPE.CHAR].Add(anim);
        }
    }

    //NPC애니메이션읽기
    public void OpenNPCAnimList()
    {
        string szPath = Application.dataPath + "/StramAssets" + "/" + "NpcAnimation.txt";
        string[] arFileDesc = File.ReadAllLines(szPath);

        foreach (string szLine in arFileDesc)
        {
            string[] szParts = szLine.Split(',');

            AnimationInfo anim = new AnimationInfo();
            anim.szDispName = szParts[0];
            anim.szFileName = szParts[1];

            _arAnim[(short)ANIM_TYPE.NPC].Add(anim);
        }
    }

    //몬스터애니메이션읽기
    public void OpenMonAnimList()
    {
        string szPath = Application.dataPath + "/StramAssets" + "/" + "MonsterAnimation.txt";
        string[] arFileDesc = File.ReadAllLines(szPath);

        foreach (string szLine in arFileDesc)
        {
            string[] szParts = szLine.Split(',');

            AnimationInfo anim = new AnimationInfo();
            anim.szDispName = szParts[0];
            anim.szFileName = szParts[1];

            _arAnim[(short)ANIM_TYPE.MON].Add(anim);
        }
    }

    //오브젝트 리스트의 내용을 UI 스크롤뷰에 반영
    public void UpdateUIformObjectList()
    {
        int nidx = 0;
        foreach (ObjectInfo info in _lstObject)
        {
            var inst = Instantiate(_objButton, new Vector3(0, 0, 0), Quaternion.identity);
            inst.transform.SetParent(_objContent.transform);

            buttons btnScript = inst.GetComponent<buttons>();
            btnScript.SetInfo(info.szDispName, info.szFileName, info.byType, nidx++);
        }
    }

    // 액션 데이터 리스트의 내용을 UI Scroll View에 반영
    public void UpdateUIformActionDataList()
    {
        int nidx = 0;
        foreach (CActionData info in _lstActionData)
        {
            var inst = Instantiate(_objActionDataButton, new Vector3(0, 0, 0), Quaternion.identity);
            inst.transform.SetParent(_actionContent.transform);

            ActionButton btnScript = inst.GetComponent("ActionButton") as ActionButton;

            string szButtonDisp = string.Format("F:{0} Y:{1} D:{2}", info.nAttackFrame, info.byAttackType, info.fAttackDist);
            btnScript.SetInfo(szButtonDisp, nidx++);
        }
    }

    public void UpdateUIformEffectDataList()
    {
        int nidx = 0;
        foreach (CEffectData info in _lstEffectData)
        {
            var inst = Instantiate(_objEffectDataButton, new Vector3(0, 0, 0), Quaternion.identity);
            inst.transform.SetParent(_effectContent.transform);

            EffectDataButton btnScript = inst.GetComponent("EffectButton") as EffectDataButton;

            string szButtonDisp = string.Format("F:{0} T:{1}}", info.nEffectFrame, info.szFilename);
            btnScript.SetInfo(szButtonDisp, nidx++);
        }
    }

    public void OnclickRemoveAll()
    {
        Transform[] objChild = _objContent.GetComponentsInChildren<Transform>();
        foreach (Transform trans in objChild)
        {
            if (trans != _objContent.transform)
                Destroy(trans.gameObject);
        }
    }

    public void OnclickReload()
    {
        _lstObject.RemoveAll(e => true);
        OpenObjectList();
        UpdateUIformObjectList();
    }

    public void OpenObjectList()
    {
        string szPath = Application.dataPath + "/StramAssets" + "/" + "CharList.txt";
        string[] arFileDesc = File.ReadAllLines(szPath);

        foreach (string szLine in arFileDesc)
        {
            string[] szParts = szLine.Split(',');

            ObjectInfo obj = new ObjectInfo();
            obj.szDispName = szParts[0];
            obj.szFileName = szParts[1];
            obj.byType = byte.Parse(szParts[2]);

            _lstObject.Add(obj);
        }
    }

    public void RemoveAllAnimListContent()
    {
        Transform[] objChild = _objAnimContent.GetComponentsInChildren<Transform>();
        foreach (Transform trans in objChild)
        {
            if (trans != _objAnimContent.transform)
                Destroy(trans.gameObject);
        }
    }

    public void RemoveAllActionListContent()
    {
        Transform[] objChild = _actionContent.GetComponentsInChildren<Transform>();
        foreach (Transform trans in objChild)
        {
            if (trans != _actionContent.transform)
                Destroy(trans.gameObject);
        }
    }

    public void RemoveAllEffectListContent()
    {
        Transform[] objChild = _effectContent.GetComponentsInChildren<Transform>();

        foreach (Transform trans in objChild)
        {
            if (trans != _effectContent.transform)
                Destroy(trans.gameObject);
        }
    }

    //우측 UI에 애니메이션의 정보를 받아온다
    public void DisplayAnimationinfo()
    {
        int nMaxFrame = (int)_animCurPlayer.runtimeAnimatorController.animationClips[_nCurAnimIndex].frameRate;

        _textMaxFrame.text = nMaxFrame.ToString() + " Frame";
        _textAnimTime.text = (nMaxFrame / 30.0f).ToString() + " Sec";
    }

    public void AddActionData(CActionData xActionData)
    {
        _lstActionData.Add(xActionData);
    }


    public void AddEffectData(CEffectData xEffectData)
    {
        _lstEffectData.Add(xEffectData);
    }

    public void RemoveAction()
    {
        if (_nCurActionindex != -1)
        {
            _lstActionData.RemoveAt(_nCurActionindex);

            RemoveAllActionListContent();
            UpdateUIformActionDataList();
        }

        _nCurActionindex = -1;
    }
    public void RemoveEffect()
    {
        if (_nCurEffectindex != -1)
        {
            _lstEffectData.RemoveAt(_nCurEffectindex);

            RemoveAllEffectListContent();
            UpdateUIformEffectDataList();
        }

        _nCurEffectindex = -1;
    }

    public void SetactionIndex(int nindex)
    {
        _nCurActionindex = nindex;
    }
    public void SetEffectIndex(int nindex)
    {
        _nCurEffectindex = nindex;
    }

    public void SaveAction()
    {
        string szFileName = string.Format(@"{0}/{1}_{2}.act", Application.streamingAssetsPath,
            _lstObject[nCurObjIndex].szFileName, _animCurPlayer.runtimeAnimatorController.animationClips[_nCurAnimIndex].name);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(szFileName);

        int nCount = _lstActionData.Count;
        bf.Serialize(fs, _lstActionData);
        fs.Close();
    }


    public void SaveEffect()
    {
        string szFileName = string.Format(@"{0}/{1}_{2}.eff", Application.streamingAssetsPath,
            _lstObject[nCurObjIndex].szFileName, _animCurPlayer.runtimeAnimatorController.animationClips[_nCurAnimIndex].name);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(szFileName);

        int nCount = _lstEffectData.Count;
        bf.Serialize(fs, _lstEffectData);
        fs.Close();
    }

    public void LoadAction()
    {
        string szFileName = string.Format(@"{0}/{1}_{2}.act", Application.streamingAssetsPath,
              _lstObject[nCurObjIndex].szFileName, _animCurPlayer.runtimeAnimatorController.animationClips[_nCurAnimIndex].name);

        if (File.Exists(szFileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(szFileName, FileMode.Open);

            if (fs != null)
            {
                if (fs.Length > 0)
                {
                    _lstActionData = (List<CActionData>)bf.Deserialize(fs);
                }
                fs.Close();
            }
        }
    }

    public void LoadEffect()
    {
        string szFileName = string.Format(@"{0}/{1}_{2}.eff", Application.streamingAssetsPath,
              _lstObject[nCurObjIndex].szFileName, _animCurPlayer.runtimeAnimatorController.animationClips[_nCurAnimIndex].name);

        if (File.Exists(szFileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(szFileName, FileMode.Open);

            if (fs != null)
            {
                if (fs.Length > 0)
                {
                    _lstEffectData = (List<CEffectData>)bf.Deserialize(fs);
                }
                fs.Close();
            }
        }
    }

    public void SetAnimLoop(bool bLoop) 
    {
        _bAnimLoop = bLoop;
    }
    public void ObjectResetPosition() 
    {
        if (_objCurPlayer != null) 
        {
            _objCurPlayer.transform.position = Vector3.zero;
        }
    }

}


