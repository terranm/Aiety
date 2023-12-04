using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomizingController : MonoBehaviour
{
    //public const int NUMBER_OF_AVATAR = 11;
    //const int INITIAL_NUMBER = 6;
    //float divisionRatio;

    //int currentIndex = INITIAL_NUMBER;
    //int previousValue = 0;

    GameObject currentGameObject;
    GameObject previousGameObject;
    
    private int CurrentTapNum = 0;

    CharctorMeshAndMaterialController charCtrl;

    //GameManager gm;

    //InputField nickNameText;
    TMPro.TMP_InputField nickNameText;

    //public Image leftBlockImage;
    //public Image rightBlockImage;


    //public AvatarState state;

    [Serializable]
    public class AvatarSetInfo
    {
        public string hair;
        public string face;
        public string top;
        public string bottom;
        public string shoes;
    }
    
    private void Awake()
    {
        GameEvents.Instance.OnRequestAvatarCustomize += OpenCustomizingView;
        GameEvents.Instance.OnRequestAvatarModify += SetAvatarStateModify;
    }

    private void OnDestroy()
    {
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.OnRequestAvatarCustomize -= OpenCustomizingView;
            GameEvents.Instance.OnRequestAvatarModify -= SetAvatarStateModify;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        //divisionRatio = 1.0f / (NUMBER_OF_AVATAR - 1);
        
        
        charCtrl = transform.Find("Customizable Avatar Set/CustomizingAvatar").GetComponent<CharctorMeshAndMaterialController>();
        
        //SetCurrentGameObject(0);
        this.gameObject.SetActive(ReactCommunicator.Instance.needToBeCustomize);
        if (ReactCommunicator.Instance.needToBeCustomize)
        {
            ReactCommunicator.Instance.needToBeCustomize = false;
            transform.Find("Customizable Avatar Set/CustomizingAvatar").gameObject.SetActive(true); 
            charCtrl.CharacterSetting(ProcessManager.Instance.state);
        }

        ReactCommunicator.Instance.LoadingSetActive(false);
    }

    private void SetAvatarStateModify(string msg)
    {
        AvatarSetInfo data = JsonUtility.FromJson<AvatarSetInfo>(msg);
        ProcessManager.Instance.state.hairCode = data.hair;
        ProcessManager.Instance.state.hairColorCode = ProcessManager.Instance.HairColorCodeSetter(data.hair);
        ProcessManager.Instance.state.faceCode = data.face;
        ProcessManager.Instance.state.bottomCode = data.bottom;
        ProcessManager.Instance.state.topCode = data.top;
        ProcessManager.Instance.state.shoesCode = data.shoes;
        charCtrl.CharacterSetting(ProcessManager.Instance.state, true);
    }
    
    private void OpenCustomizingView(bool open)
    {
        this.gameObject.SetActive(open);
        if (open)
        {
            charCtrl.CharacterSetting(ProcessManager.Instance.state);
#if !UNITY_EDITOR
            WebGLInput.captureAllKeyboardInput = false;
#endif
        }
        else
        {
            string data = JsonUtility.ToJson(ProcessManager.Instance.state);
            ProcessManager.Instance.player.GetComponent<PhotonView>().RPC("SetNewCustom", RpcTarget.AllBuffered, data,
                ProcessManager.Instance.player.GetComponent<PhotonView>().InstantiationId);
#if !UNITY_EDITOR
            WebGLInput.captureAllKeyboardInput = true;
#endif 
        }
    }
    

    /// <summary>
    /// 
    /// </summary>
    public void GetUserNickName(string nickName)
    {
        nickNameText.text = nickName;
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnChangeNickNameLimitByte()
    {
        int bytecount = System.Text.Encoding.Default.GetByteCount(nickNameText.text);
        if (bytecount < 20) return;
        byte[] byteTEMP = System.Text.Encoding.Default.GetBytes(nickNameText.text);

        string text = System.Text.Encoding.Default.GetString(byteTEMP, 0, 20);
        nickNameText.text = text;
    }



    /// <summary>
    /// 
    /// </summary>
    public void OnClickEvent()
    {
        OnScrollViewEvent(() => {
            currentGameObject = EventSystem.current.currentSelectedGameObject;
        });

        //gm.state = charCtrl.state;
    }

    /// <summary>
    /// 
    /// </summary>
    void SetPreviousIndex()
    {
        previousGameObject = currentGameObject;
    }
    void SetCurrentGameObject(int parts)
    {
        string code = "";
        switch (parts)
        {
            case 0:
                code = charCtrl.state.hairCode;
                break;
            case 1:
                code = charCtrl.state.faceCode;
                break;
            case 2:
                code = charCtrl.state.topCode;
                break;
            case 3:
                code = charCtrl.state.bottomCode;
                break;
            case 4:
                code = charCtrl.state.shoesCode;
                break;
        }
        OnScrollViewEvent(() => {
            currentGameObject = GameObject.Find("Scroll View/Viewport/Content/" + code);
        });
    }

    /// <summary>
    /// 
    /// </summary>
    void OnScrollViewEvent(Action action, bool update = true)
    {
        SetPreviousIndex();
        action?.Invoke();
        FocusOnContent(update);
    }

    /// <summary>
    /// 
    /// </summary>
    void FocusOnContent(bool update)
    {
        //t = 0.0f;
        if(currentGameObject!= null)
            OnSelectAvatarParts(currentGameObject.name);

        //bLerp = update;
    }


    /// <summary>
    /// 
    /// </summary>
    void OnSelectAvatarParts(string targetName)
    {
        string colorCode = ProcessManager.Instance.HairColorCodeSetter(targetName);

        charCtrl.CharactorPartsChange(targetName, colorCode);
    }


    /// <summary>
    /// 
    /// </summary>
    //float t = 0.0f;
    //void SetScrollbarValue()
    //{
    //    if (bLerp)
    //    {
    //        t += Time.deltaTime;

    //        currentValue = scrollRect.verticalScrollbar.value;
    //        //scrollRect.verticalScrollbar.value = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * 5.0f);

    //        if (1.0f < t)
    //        {
    //            bLerp = false;
    //            targetValue = 0.0f;
    //            currentValue = 0.0f;
    //        }
    //    }
    //}


    /// <summary>
    /// 
    /// </summary>
    void OnOffBlockImage()
    {
        //if (scrollRect.verticalScrollbar.value < 0.05f)
        //{
        //    SetActive(leftBlockImage, true);
        //}
        //else if (leftBlockImage.enabled)
        //{
        //    SetActive(leftBlockImage, false);
        //}

        //if (0.95f < scrollRect.verticalScrollbar.value)
        //{
        //    SetActive(rightBlockImage, true);
        //}
        //else if (rightBlockImage.enabled)
        //{
        //    SetActive(rightBlockImage, false);
        //}
    }

    /// <summary>
    /// 
    /// </summary>
    void SetActive(Image target, bool enabled)
    {
        target.enabled = enabled;
    }


    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        //SetScrollbarValue();
    }
}
