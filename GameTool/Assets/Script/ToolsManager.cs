using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using game;
using UnityEngine.UI;

public enum ANIM_TYPE : short            //가독성을 높이기 위한 타입
{
    CHAR,
    NPC,
    MON,
    MAX
}

public class ToolsManager : MonoSingleton<ToolsManager>
{
    public GameObject objbuttonPrefab;
    public GameObject animbuttonPrefab;

    private List<AnimButton> animbuttonList = new List<AnimButton>();
    private List<ObjectButton> objbuttonList = new List<ObjectButton>();
    private List<AnimInfo>[] animationList = new List<AnimInfo>[3];             //리스트 배열 플레이어 엔피씨 몬스터의 애니메이션 정보를 모두 넣기위해 사용

    GameObject _objCurPlayer = null;
    Animator _animCurPlayer = null;
    Text _textFrame = null;
    Slider _sliderFrame = null;
    int _nCurAnimIndex = 0;

    public void Initial()
    {
        objbuttonPrefab = Resources.Load("ObjListButton") as GameObject;
        animbuttonPrefab = Resources.Load("AnimListButton") as GameObject;

        _textFrame = GameObject.Find("CurFrame").GetComponent<Text>();
        _sliderFrame = GameObject.Find("AnimFrame").GetComponent<Slider>();

        LoadObjectList();

        LoadAnimationList();
    }

    #region [AnimatonList]

    public void InstantiateAnimButton(ANIM_TYPE _TYPE)
    {
        DeleteAnimList();

        foreach (AnimInfo info in animationList[(byte)_TYPE])
        {
            GameObject obj = Instantiate(animbuttonPrefab, transform.position, Quaternion.identity);
            obj.transform.SetParent(GameObject.Find("AnimContent").transform);

            AnimButton button = obj.GetComponent<AnimButton>();
            button.SetInfo(info.GetAnimName(), info.GetFileName());
            animbuttonList.Add(button);
        }
    }

    private void LoadAnimationList()
    {
        //리스트를 배열로 넣어줄때는 따로따로 선언해주세요
        for (int i = 0; i < (byte)ANIM_TYPE.MAX; i++)
        {
            animationList[i] = new List<AnimInfo>();
        }

        LoadCharAnimationList();
        LoadNpcAnimationList();
        LoadMonsterAnimationList();
    }

    private void Update()
    {
        if(_objCurPlayer != null)
        {
            AnimatorStateInfo info = _animCurPlayer.GetCurrentAnimatorStateInfo(_nCurAnimIndex);
            int nFrame = (int)_animCurPlayer.runtimeAnimatorController.animationClips[_nCurAnimIndex].frameRate;
            _sliderFrame.value = (int)(info.normalizedTime * nFrame) % nFrame;
            _textFrame.text = _sliderFrame.value.ToString();
        }
    }

    //캐릭터 애니메이션 리스트파일 로드
    private void LoadCharAnimationList()
    {
        //File Load
        string _path = Application.dataPath + " /StreamAssets" + "/" + "CharAnimation.txt";
        string[] textArr = File.ReadAllLines(_path);

        foreach (string _text in textArr)
        {
            string[] _textarr = _text.Split(',');

            AnimInfo anim = new AnimInfo();
            anim.SetInfo(_textarr[0], _textarr[1]);

            animationList[(byte)ANIM_TYPE.CHAR].Add(anim);
        }
    }

    //Npc 애니메이션 리스트파일 로드
    private void LoadNpcAnimationList()
    {
        //File Load
        string _path = Application.dataPath + " /StreamAssets" + "/" + "NpcAnimation.txt";
        string[] textArr = File.ReadAllLines(_path);

        foreach (string _text in textArr)
        {
            string[] _textarr = _text.Split(',');

            AnimInfo anim = new AnimInfo();
            anim.SetInfo(_textarr[0], _textarr[1]);

            animationList[(byte)ANIM_TYPE.NPC].Add(anim);
        }
    }

    //Monster 애니메이션 리스트파일 로드
    private void LoadMonsterAnimationList()
    {
        //File Load
        string _path = Application.dataPath + " /StreamAssets" + "/" + "MonsterAnimation.txt";
        string[] textArr = File.ReadAllLines(_path);

        foreach (string _text in textArr)
        {
            string[] _textarr = _text.Split(',');

            AnimInfo anim = new AnimInfo();
            anim.SetInfo(_textarr[0], _textarr[1]);

            animationList[(byte)ANIM_TYPE.MON].Add(anim);
        }
    }

    private void DeleteAnimList()
    {
        foreach (AnimButton animbutton in animbuttonList)
        {
            Destroy(animbutton.transform.gameObject);
        }

        animbuttonList.Clear();
    }

    #endregion

    #region [ObjectList]

    //캐릭터 오브젝트 리스트파일 로드
    private void LoadObjectList()
    {
        //File Load
        string _path = Application.dataPath + " /StreamAssets" + "/" + "CharList.txt";
        string[] textArr = File.ReadAllLines(_path);

        foreach (string _text in textArr)
        {
            string[] _textarr = _text.Split(',');

            GameObject _button = Instantiate(objbuttonPrefab, transform.position, Quaternion.identity);
            _button.transform.SetParent(GameObject.Find("ObjContent").transform);

            ObjectButton button = _button.GetComponent<ObjectButton>();
            button.SetInfo(_textarr[0], _textarr[1], byte.Parse(_textarr[2]));
            objbuttonList.Add(button);
        }


    }

    public void CreatePlayer(string szFileName)
    {
        if (_objCurPlayer != null)
            Destroy(_objCurPlayer);

        GameObject objCurPlayer = Resources.Load(szFileName) as GameObject;

        if (objCurPlayer != null)
        {
            _objCurPlayer = Instantiate(objCurPlayer, new Vector3(0, 0, 0), Quaternion.identity);
            _animCurPlayer = _objCurPlayer.GetComponent<Animator>();

            int nFrame = (int)_animCurPlayer.runtimeAnimatorController.animationClips[_nCurAnimIndex].frameRate;
            _sliderFrame.maxValue = nFrame;
        }

    }

    public void OnclickDeleteAll()
    {
        foreach (ObjectButton objbutton in objbuttonList)
        {
            Destroy(objbutton.transform.gameObject);
        }

        DeleteAnimList();

        objbuttonList.Clear();
    }

    public void OnClickReset()
    {
        if(objbuttonList.Count == 0)
        {
            LoadObjectList();
        }
        else
        {
            Debug.Log("이미 리스트를 불러왔습니다.");
        }
    }

    #endregion
}
