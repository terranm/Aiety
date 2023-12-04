using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System;

public class ReactCommunicator : Singleton<ReactCommunicator>
{
    
    [DllImport("__Internal")]
    private static extern void okayToLeave();
    
    
    [DllImport("__Internal")]
    private static extern void openLoadingModal();
    
    
    [DllImport("__Internal")]
    private static extern void closeLoadingModal();
    
    [DllImport("__Internal")]
    private static extern void pingAck();
    
    [DllImport("__Internal")]
    private static extern void onUnityLoaded();
    
    [DllImport("__Internal")]
    private static extern void onDisconnectServer();
    
    [DllImport("__Internal")]
    private static extern void fullScreen(string msg);
    
    [DllImport("__Internal")]
    private static extern void requestUIChange(string type);
    
    [DllImport("__Internal")]
    private static extern void isSeminarEnterable();

    [Serializable]
    public class AvatarData
    {
        public string hair;
        public string face;
        public string top;
        public string bottom;
        public string shoes;
    }
    
    [Serializable]
    public class InitialData
    {
        public string eventIMGs; // ”[ “1stUrl“ , ”2ndUrl”, …, “NthUrl“]”
        public string avatar; // AvatarData class 의 string 형
        public string nickName; // 닉네임
        public string isSetAvatar; // 초기 세팅 여부 Y or N, N의 경우 커스터마이징 창 출력 필요
        public string seminarImgUrl;
        public string loc;
    }

    [Serializable]
    public class CounseilingRoomData
    {
        public string room;//방번호
        public string mentor;// A || B || C
        //public string seat;// 1 || 2
        public string open;// "false" - 입장, "true" - 퇴장
    }
    
    [Serializable]
    public class CustomizeViewSetData
    {
        public string open;// "false" - 입장, "true" - 퇴장
    }
    
    [Serializable]
    public class FullScreenData
    {
        public string type; //”Seminar” || ”Event” || ”Board”
    }
    
    [Serializable]
    public class UITypeData
    {
        public string type; //”Stand” || ”Sit”
    }
    
    [Serializable]
    public class EventIMGsData
    {
        public List<string> list;
    }
    

    public bool needToBeCustomize;
    public List<string> CurrentEventIMGs;
    public string seminarUrl;
    public bool goToSeminar;

    public void Init()
    {
        
        Debug.Log("React Communicator init");
    }

    private void Start()
    {
        Debug.Log("React Communicator start");
        StartCoroutine(OnUnityLoadedLateSender());
        CurrentEventIMGs = new List<string>();
    }

    private IEnumerator OnUnityLoadedLateSender()
    {
        yield return new WaitForSeconds(3f);
#if !UNITY_EDITOR
        onUnityLoaded();
#endif
        yield return null;
    }
    
    private void Awake()
    {
        //GameEvents.Instance.OnRequestLoadingSetActive += LoadingSetActive;
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadingSetActive(bool active)
    {
        Debug.Log("RequestLoadingSetActive called" + active);
#if UNITY_WEBGL && !UNITY_EDITOR
        if (active)
        {
            Debug.Log("Loading opne");
            openLoadingModal();
        }
        else
        {
            Debug.Log("Loading close");
            closeLoadingModal();
        }
#endif
    }

    public void intializingUnity(string msg)
    {
        InitialData data = JsonUtility.FromJson<InitialData>(msg);
        needToBeCustomize = data.isSetAvatar.Equals("N");
        AvatarData temp = JsonUtility.FromJson<AvatarData>(data.avatar);
        EventIMGsData templist = JsonUtility.FromJson<EventIMGsData>(data.eventIMGs);
        CurrentEventIMGs = templist.list;
        ProcessManager.Instance.state.faceCode = temp.face;
        ProcessManager.Instance.state.hairCode = temp.hair;
        ProcessManager.Instance.state.hairColorCode = ProcessManager.Instance.HairColorCodeSetter(ProcessManager.Instance.state.hairCode);
        ProcessManager.Instance.state.topCode = temp.top;
        ProcessManager.Instance.state.bottomCode = temp.bottom;
        ProcessManager.Instance.state.shoesCode = temp.shoes;
        ProcessManager.Instance.state.nickName = data.nickName;
        seminarUrl = data.seminarImgUrl;
        goToSeminar = data.loc.Equals("seminar");
        ServerManager.Instance.Initialize();
    }
    
    public void SetAvatar(string msg)
    {
        Debug.Log("Got modifying parts sign : " + msg);
        GameEvents.Instance.RequestAvatarModify(msg);
    }

    public void offKeyFocus()
    {
        Debug.Log("키 포커스 오프 신호 수신");
        WebGLInput.captureAllKeyboardInput = false;
        GameEvents.Instance.RequestSetActivePlayerInputSys(false);
    }

    public void onKeyFocus()
    {
        Debug.Log("키 포커스 온 신호 수신");
        WebGLInput.captureAllKeyboardInput = true;
        GameEvents.Instance.RequestSetActivePlayerInputSys(true);
    }
    
    public void sendHello()
    {
        Debug.Log("안녕 동작 신호 수신");
        GameEvents.Instance.RequestPlayerAction("Hi");
    }

    public void sendClap()
    {
        Debug.Log("박수 동작 신호 수신");
        GameEvents.Instance.RequestPlayerAction("Clap");
    }

    public void sendNod()
    {
        Debug.Log("동의 동작 신호 수신");
        GameEvents.Instance.RequestPlayerAction("Agree");
    }

    public void sendItsme()
    {
        Debug.Log("저요 동작 신호 수신");
        GameEvents.Instance.RequestPlayerAction("itsme");
    }

    public void sendRunOn()
    {
        Debug.Log("달리기 신호 수신");
        GameEvents.Instance.RequestSetActiveSprint(true);
    }
    
    public void sendRunOff()
    {
        Debug.Log("걷기 신호 수신");
        GameEvents.Instance.RequestSetActiveSprint(false);
    }

    public void seminarLeave()
    {
        Debug.Log("세미나 퇴장 신호 수신");
        LoadingSetActive(true);
        GameEvents.Instance.RequestTeleport(ProcessManager.Instance.OutPoint.position, ProcessManager.Instance.OutPoint.rotation.eulerAngles);
    }
    
    public void leaveCenter()
    {
        Debug.Log("나가기 수신");
#if UNITY_WEBGL && !UNITY_EDITOR
        WhenUserLeavesRoom();
#endif
    }
    
    public void ping()
    {
        Debug.Log("핑 수신");
#if UNITY_WEBGL && !UNITY_EDITOR
        pingAck();
#endif
    }
    
    public void WhenUserLeavesRoom()
    {
        Debug.Log("나가기 완료 송신");
        okayToLeave();
        Application.Quit();
    }

    public void SendFullScreen(string target)
    {
        FullScreenData data = new FullScreenData();
        data.type = target;
        Debug.Log("full screen request type : " + target);
#if !UNITY_EDITOR
        fullScreen(JsonUtility.ToJson(data));
#endif
    }
    
    public void avatarSetting(string msg)
    {
        CustomizeViewSetData data = JsonUtility.FromJson<CustomizeViewSetData>(msg);
        GameEvents.Instance.RequestAvatarCustomize(data.open.Equals("true"));
    }

    public void SendDisconnectSign()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        onDisconnectServer();
#endif
    }
    
    public void SendRequestUIChange(bool isSit)
    {
        Debug.Log((isSit)?"앉은상태 UI요청" : "일어난 상태UI 요청");
#if UNITY_WEBGL && !UNITY_EDITOR
        UITypeData data = new UITypeData();
        data.type = (isSit)?"Sit" : "Stand";
        requestUIChange(JsonUtility.ToJson(data));
#endif
    }
    
    public void SendIsSeminarEnteravle()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        isSeminarEnterable();
#endif
    }

    public void seminarEnterable()
    {
        Debug.Log("세미나 입장 신호 수신");
        LoadingSetActive(true);
        GameEvents.Instance.RequestTeleport(new Vector3(-27f, 3f, -215f), new Vector3(0f, 0f, 0f),true);
    }

    public void counseiling(string msg)
    {
        Debug.Log("상담실 입장 신호 수신");
        CounseilingRoomData data = JsonUtility.FromJson<CounseilingRoomData>(msg);
        LoadingSetActive(true);
        if (data.open == "true")
        {
            GameEvents.Instance.RequestSetCounseilingRoom(data);
        }
        else if (data.open == "false")
        {
            GameEvents.Instance.RequestExitCounseilingRoom();
        }
    }
    
    
#if UNITY_EDITOR
    InitialData test = null;
    
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Z))
        {
            if (test == null)
            {
                test = new InitialData();
                AvatarData tempState = new AvatarData();
                tempState.hair ="HAIR_AK_001";
                tempState.face = "FACE_AA_000";
                tempState.top = "TOP_AA_001";
                tempState.bottom = "BOTTOM_AA_001";
                tempState.shoes = "SHOES_AA_001";
                test.avatar = JsonUtility.ToJson(tempState);
                test.nickName = "테스터";
                //"https://aiety-tnmeta.s3.ap-northeast-2.amazonaws.com/test/plain/F7uVHtJ6SE-7lR7SeshPjw-2023-10-27_15-31-40.png",
                //"https://aiety-tnmeta.s3.ap-northeast-2.amazonaws.com/test/plain/oW1MmAInSL-tTJgCJzi_hA-2023-10-27_15-32-19.png"
                EventIMGsData templist = new EventIMGsData();
                templist.list = new List<string>();
                templist.list.Add("https://aiety-tnmeta.s3.ap-northeast-2.amazonaws.com/test/plain/F7uVHtJ6SE-7lR7SeshPjw-2023-10-27_15-31-40.png");
                templist.list.Add("https://aiety-tnmeta.s3.ap-northeast-2.amazonaws.com/test/plain/oW1MmAInSL-tTJgCJzi_hA-2023-10-27_15-32-19.png");
                test.eventIMGs = JsonUtility.ToJson(templist);
                test.isSetAvatar = "Y";
                test.seminarImgUrl =
                    "https://aiety-tnmeta.s3.ap-northeast-2.amazonaws.com/test/plain/oW1MmAInSL-tTJgCJzi_hA-2023-10-27_15-32-19.png";
                test.loc = "waitingRoom";
                intializingUnity(JsonUtility.ToJson(test));
            }
        }
        
        if (Input.GetKey(KeyCode.X))
        {
            if (test == null)
            {
                test = new InitialData();
                AvatarData tempState = new AvatarData();
                tempState.hair ="HAIR_AA_001";
                tempState.face = "FACE_AA_000";
                tempState.top = "TOP_AA_001";
                tempState.bottom = "BOTTOM_AA_001";
                tempState.shoes = "SHOES_AA_001";
                test.avatar = JsonUtility.ToJson(tempState);
                test.nickName = "테스터";
                //"https://aiety-tnmeta.s3.ap-northeast-2.amazonaws.com/test/plain/F7uVHtJ6SE-7lR7SeshPjw-2023-10-27_15-31-40.png",
                //"https://aiety-tnmeta.s3.ap-northeast-2.amazonaws.com/test/plain/oW1MmAInSL-tTJgCJzi_hA-2023-10-27_15-32-19.png"
                EventIMGsData templist = new EventIMGsData();
                templist.list = new List<string>();
                templist.list.Add("https://aiety-tnmeta.s3.ap-northeast-2.amazonaws.com/test/plain/F7uVHtJ6SE-7lR7SeshPjw-2023-10-27_15-31-40.png");
                templist.list.Add("https://aiety-tnmeta.s3.ap-northeast-2.amazonaws.com/test/plain/oW1MmAInSL-tTJgCJzi_hA-2023-10-27_15-32-19.png");
                test.eventIMGs = JsonUtility.ToJson(templist);
                test.isSetAvatar = "Y";
                test.seminarImgUrl =
                    "https://aiety-tnmeta.s3.ap-northeast-2.amazonaws.com/test/plain/oW1MmAInSL-tTJgCJzi_hA-2023-10-27_15-32-19.png";
                test.loc = "seminar";
                intializingUnity(JsonUtility.ToJson(test));
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            CounseilingRoomData tempData = new CounseilingRoomData();
            tempData.room = "1";
            tempData.mentor = "TYPE1";
            //tempData.seat = "1";
            tempData.open = "true";
            counseiling(JsonUtility.ToJson(tempData));
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            CounseilingRoomData tempData = new CounseilingRoomData();
            tempData.room = "1";
            tempData.mentor = "TYPE1";
            //tempData.seat = "1";
            tempData.open = "false";
            counseiling(JsonUtility.ToJson(tempData));
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            seminarLeave();
        }

        if (Input.GetKey(KeyCode.C))
        {
            CustomizeViewSetData tempData = new CustomizeViewSetData();
            tempData.open = "true";
            avatarSetting(JsonUtility.ToJson(tempData));
        }
        
        if (Input.GetKey(KeyCode.V))
        {
            CustomizeViewSetData tempData = new CustomizeViewSetData();
            tempData.open = "false";
            avatarSetting(JsonUtility.ToJson(tempData));
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            sendHello();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            sendRunOn();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            sendRunOff();
        }


        if (Input.GetKeyDown(KeyCode.H))
        {
            sendClap();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            sendNod();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            sendItsme();
        }
#endif

    }
    
#endif
}
