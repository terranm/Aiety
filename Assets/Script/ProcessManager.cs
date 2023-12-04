using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using StarterAssets;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ProcessManager : Singleton<ProcessManager>
{
    public GameObject player;
    public AvatarState state = new AvatarState();
    private RaycastHit hit;
    private Ray ray;
    private GameObject hitObject;

    [SerializeField]
    private Transform startPoint;
    
    [SerializeField]
    private Transform seminarPoint;

    [SerializeField]
    private Transform outPoint;

    //public Transform StartPoint { get { return startPoint; } }
    public Transform OutPoint { get { return outPoint; } }


    private void Awake()
    {
        
        GameEvents.Instance.OnRequestPlayerAction += OnPlayAnimation;
        GameEvents.Instance.OnRequestSetActiveSprint += ConvertSprint;
        GameEvents.Instance.OnRequestTeleport += Teleport;
        GameEvents.Instance.OnRequestSetCounseilingRoomAndPlayer += SetCounseilingRoomAndPlayer;
        GameEvents.Instance.OnRequestExitCounseilingRoomAndPlayer += ExitCounseilingRoomAndPlayer;
        //GameEvents.Instance.OnRequestAnimatorChange += ConvertAnimationSitStand;
        GameEvents.Instance.OnRequestStandUp += StandUPAction;
        GameEvents.Instance.OnRequestSetActivePlayerInputSys += SetActivePlayerInputSys;
    }

    private void OnDestroy()
    {
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.OnRequestPlayerAction -= OnPlayAnimation;
            GameEvents.Instance.OnRequestSetActiveSprint -= ConvertSprint;
            GameEvents.Instance.OnRequestTeleport -= Teleport;
            GameEvents.Instance.OnRequestSetCounseilingRoomAndPlayer -= SetCounseilingRoomAndPlayer;
            GameEvents.Instance.OnRequestExitCounseilingRoomAndPlayer -= ExitCounseilingRoomAndPlayer;
            //GameEvents.Instance.OnRequestAnimatorChange -= ConvertAnimationSitStand;
            GameEvents.Instance.OnRequestStandUp -= StandUPAction;
            GameEvents.Instance.OnRequestSetActivePlayerInputSys -= SetActivePlayerInputSys;
        }
            
    }
    
    private void Start()
    {
        DontDestroyOnLoad(this);
        //SetRandomAvatarState();
        GameEvents.Instance.Init();
        //ReactCommunicator.Instance.Init();
        ServerManager.Instance.Init();
        ReactCommunicator.Instance.Init();
        //SetInitialAvatarState();
        SetRandomAvatarState();
    }
    
    private void SetActivePlayerInputSys(bool active)
    {
        if (player != null)
            player.GetComponent<StarterAssetsInputs>().enabled = active;
    }
    
    private void ConvertSprint(bool isSprint)
    {
        if (player != null)
            player.GetComponent<StarterAssetsInputs>().sprint = isSprint;
    }

    public void OnPlayAnimation(string animationName)
    {
        if (player != null)
            player.GetComponent<Animator>().SetTrigger(animationName);
    }

    public void SetInitialAvatarState()
    {
        state.hairCode = "HAIR_AA_001";
        state.hairColorCode = HairColorCodeSetter(state.hairCode);//"#463C33";
        state.bottomCode = "BOTTOM_AA_001";
        state.bottomColorCode = "#000000";
        state.topCode = "TOP_AA_001";
        state.topColorCode = "#000000";
        state.shoesCode = "SHOES_AA_001";
        state.shoesColorCode = "#000000";
        state.faceCode = "FACE_AA_001";
        state.faceColorCode = "#000000";
        state.skinCode = "SKIN_AA_001";
        state.skinColorCode = "#F0C8B4";
        state.nickName = "";
    }


    public string HairColorCodeSetter(string targetName)
    {
        string[] code = targetName.Split('_');
        string colorCode = "#000000";
        if (code[0].ToUpper() == "HAIR")
        {
            string[] ColorCode = {
                "#434343", // 0
                "#A43D3D",
                "#FFAE3D",
                "#0F6C3F",
                "#2B5B7B",
                "#D1A4E9",
                "#653625",
                "#B3CFCF", // 7

                "#FAE7D6", // 8
                "#F7DAC6",
                "#F5D5C2",
                "#F1D1B3",
                "#F0C8B4",
                "#EEC2AC",
                "#E7B49B",
                "#DBA58A",
                "#A17766",
                "#845C4E",
                "#614C39",
                "#443521",
                "#D13319",
                "#298A3A",
                "#4DB199",
                "#23759E", // 23
                "#4F2B18",
                "#463C33", // 25
                "#5E4834",
                "#81715E",
                "#675446",
                "#80593A", // 29
                "#603D2B"
            };
            switch (code[1])
            {
                case "AA":
                    switch (code[2])
                    {
                        case "001":
                            colorCode = ColorCode[25];
                            break;
                        case "002":
                            colorCode = ColorCode[29];
                            break;
                        case "003":
                            colorCode = ColorCode[24];
                            break;
                        case "004":
                            colorCode = ColorCode[27];
                            break;
                    }
                    break;
                case "AB":
                    colorCode = ColorCode[24];
                    break;
                case "AC":
                    colorCode = ColorCode[25];
                    break;
                case "AD":
                    colorCode = ColorCode[26];
                    break;
                case "AE":
                    colorCode = ColorCode[27];
                    break;
                case "AF":
                    colorCode = ColorCode[28];
                    break;
                case "AG":
                    colorCode = ColorCode[29];
                    break;
                case "AK":
                    colorCode = ColorCode[30];
                    break;
            }
        }

        return colorCode;
    }

    public void SetRandomAvatarState()
    {

        string[] ColorCode =
        {
        "#434343", // 0
        "#A43D3D",
        "#FFAE3D",
        "#0F6C3F",
        "#2B5B7B",
        "#D1A4E9",
        "#653625",
        "#B3CFCF", // 7

        "#FAE7D6", // 8
        "#F7DAC6",
        "#F5D5C2",
        "#F1D1B3",
        "#F0C8B4",
        "#EEC2AC",
        "#E7B49B",
        "#DBA58A",
        "#A17766",
        "#845C4E",
        "#614C39",
        "#443521",
        "#D13319",
        "#298A3A",
        "#4DB199",
        "#23759E", // 23
        "#4F2B18",
        "#463C33", // 25
        "#5E4834",
        "#81715E",
        "#675446",
        "#80593A", // 29

        "#603D2B",

    };

        string[] MeshCode =
        {
        "AA", //0
        "AB",
        "AC",
        "AD",
        "AE",
        "AF",
        "AG", //6
        "AH",
        "AI",
        "AJ"
    };
        /// ??? ??? ???? ???? ???
        string faceMat = Random.Range(1, 14).ToString("D3");
        string hairMesh = MeshCode[Random.Range(0, 7)];
        string bottomMesh = MeshCode[Random.Range(0, 4)];
        string bottomMat = bottomMesh == "AC" ? Random.Range(1, 7).ToString("D3") : Random.Range(1, 4).ToString("D3");
        string shoesMesh = MeshCode[Random.Range(0, 3)];
        string shoesMat = Random.Range(1, 5).ToString("D3");
        string topMesh = MeshCode[Random.Range(0, 5)];
        string topMat = (topMesh == "AA" || topMesh == "AB")
            ? Random.Range(1, 5).ToString("D3")
            : Random.Range(1, 8).ToString("D3");
        string hairColor = ColorCode[Random.Range(0, 8)];
        string skinColor = ColorCode[12]; //Random.Range(8, 24)];

        /// 35?? ? ?? ?? ?? ??
        string hairMat = hairMesh != "AA" ? "001" : Random.Range(1, 4).ToString("D3");
        //switch (hairMesh)
        //{
        //    case "AA":
        //        switch (hairMat)
        //        {
        //            case "001":
        //                hairColor = ColorCode[25];
        //                break;
        //            case "002":
        //                hairColor = ColorCode[29];
        //                break;
        //            case "003":
        //                hairColor = ColorCode[24];
        //                break;
        //            case "004":
        //                hairColor = ColorCode[27];
        //                break;
        //        }

        //        break;
        //    case "AB":
        //        hairColor = ColorCode[24];
        //        break;
        //    case "AC":
        //        hairColor = ColorCode[25];
        //        break;
        //    case "AD":
        //        hairColor = ColorCode[26];
        //        break;
        //    case "AE":
        //        hairColor = ColorCode[27];
        //        break;
        //    case "AF":
        //        hairColor = ColorCode[28];
        //        break;
        //    case "AG":
        //        hairColor = ColorCode[29];
        //        break;
        //    case "AK":
        //        hairColor = ColorCode[30];
        //        break;
        //}
        /////

        state.hairCode = "HAIR_" + hairMesh + "_" + hairMat;
        state.hairColorCode = HairColorCodeSetter(state.hairCode);
        state.bottomCode = "BOTTOM_" + bottomMesh + "_" + bottomMat;
        state.bottomColorCode = "#000000";
        state.topCode = "TOP_" + topMesh + "_" + topMat;
        state.topColorCode = "#000000";
        state.shoesCode = "SHOES_" + shoesMesh + "_" + shoesMat;
        state.shoesColorCode = "#000000";
        state.faceCode = "FACE_AA_" + faceMat;
        state.faceColorCode = "#000000";
        state.skinCode = "SKIN_AA_001";
        state.skinColorCode = skinColor;
        state.nickName = "";

/*
        state.hairCode = "HAIR_AD_001";
        state.hairColorCode = "#653625";
        state.bottomCode = "BOTTOM_AE_002";
        state.bottomColorCode = "#000000";
        state.topCode = "TOP_AG_002";
        state.topColorCode = "#000000";
        state.shoesCode = "SHOES_AD_002";
        state.shoesColorCode = "#000000";
        state.faceCode = "FACE_AA_001" ;
        state.faceColorCode = "#000000";
        state.skinCode = "SKIN_AA_001";
        state.skinColorCode = "#F0C8B4";
        state.nickName = "";
*/
        //SetRandomizeNickName();
    }

    public void Initialize(string avatarName)
    {
        string jsondata = JsonUtility.ToJson(state);

        Vector3 whereTo = (ReactCommunicator.Instance.goToSeminar) ? seminarPoint.position : startPoint.position;
        player = PhotonNetwork.Instantiate("Avatars/" + avatarName, whereTo, Quaternion.identity, 0,
            new object[] { jsondata });
        player.tag = "Player";
        ServerManager.Instance.isConnecting = true;
        player.GetComponent<PlayerInput>().enabled = true;
        player.GetComponent<CharctorMeshAndMaterialController>().CharacterSetting(state);

        GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow =
            player.transform.Find("PlayerCameraRoot");
        GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().LookAt =
            player.transform.Find("PlayerCameraRoot");

        GameObject.Find("Main UI Canvas").GetComponent<UICanvasControllerInput>().starterAssetsInputs =
            player.GetComponent<StarterAssetsInputs>();

    }

    private void Teleport(Vector3 pos, Vector3 rot,bool isSeminarEnter=false)
    {
        Debug.Log("teleport called : " + pos + rot);

        player.GetComponent<StarterAssetsInputs>().enabled = false;
        player.GetComponent<PlayerInput>().enabled = false;
        player.GetComponent<ThirdPersonController>().enabled = false;
        StartCoroutine(MakeSurePlayerMoved(pos, rot,isSeminarEnter));
    }

    private void SetCounseilingRoomAndPlayer(Vector3 charctorPos, Vector3 charctorRot, Vector3 cameraPos, Vector3 cameraRot)
    {
        Debug.Log("SetCounseilingRoomAndPlayer called : char " + charctorPos + charctorRot + " cam " + cameraPos + cameraRot);
        player.GetComponent<StarterAssetsInputs>().enabled = false;
        player.GetComponent<PlayerInput>().enabled = false;
        player.GetComponent<ThirdPersonController>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;


        player.transform.position = charctorPos;
        player.transform.rotation = Quaternion.Euler(charctorRot);

        Transform tr = player.transform.Find("PlayerCameraRoot");
        tr.position = cameraPos;
        tr.rotation = Quaternion.Euler(cameraRot);

        player.GetComponent<StarterAssetsInputs>().enabled = true;
        player.GetComponent<PlayerInput>().enabled = true;
        player.GetComponent<ThirdPersonController>().enabled = true;
        player.GetComponent<ThirdPersonController>().isInCounseilingRoom = true;

        GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = 0.001f;

        ConvertAnimationSitStand(true);
        ReactCommunicator.Instance.LoadingSetActive(false);
    }

    private void ExitCounseilingRoomAndPlayer()
    {
        Debug.Log("ExitCounseilingRoomAndPlayer called");
        player.GetComponent<StarterAssetsInputs>().enabled = false;
        player.GetComponent<PlayerInput>().enabled = false;
        player.GetComponent<ThirdPersonController>().enabled = false;


        player.transform.position = outPoint.position;
        player.transform.rotation = outPoint.rotation;

        Transform tr = player.transform.Find("PlayerCameraRoot");
        tr.position = new Vector3(player.transform.position.x, 1.72f, player.transform.position.z);
        tr.rotation = Quaternion.Euler(0,180,0);

        player.GetComponent<StarterAssetsInputs>().enabled = true;
        player.GetComponent<PlayerInput>().enabled = true;
        player.GetComponent<ThirdPersonController>().enabled = true;
        player.GetComponent<ThirdPersonController>().isInCounseilingRoom = false;
        player.GetComponent<CharacterController>().enabled = true;

        

        GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = 11f;

        ConvertAnimationSitStand(false);
        Teleport(outPoint.position, outPoint.rotation.eulerAngles);
    }

    private void ConvertAnimationSitStand(bool isSit)//string AnimatorName)
    {
        player.GetComponent<Animator>().SetBool("Sit", isSit);
        //player.GetComponent<Animator>().runtimeAnimatorController = Resources.Load(AnimatorName) as RuntimeAnimatorController;

        //foreach (AnimatorControllerParameter param in player.GetComponent<Animator>().parameters)
        //{
        //    player.GetComponent<PhotonAnimatorView>().SetParameterSynchronized(param.name, (PhotonAnimatorView.ParameterType)Enum.Parse(typeof(PhotonAnimatorView.ParameterType), param.type.ToString()), PhotonAnimatorView.SynchronizeType.Continuous);
        //}
    }

    private void StandUPAction()
    {
        //ConvertAnimationSitStand(false);
        player.GetComponent<CharacterController>().enabled = true;
        ReactCommunicator.Instance.SendRequestUIChange(false);
    }

    private IEnumerator MakeSurePlayerMoved(Vector3 pos, Vector3 rot, bool isSeminarEnter=false)
    {
        bool flag = true;
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");


        StarterAssetsInputs assetsInputs = player.GetComponent<StarterAssetsInputs>();
        Vector3 prevPos;
        while (flag)
        {
            prevPos = cam.transform.position;
            Debug.Log("loop called");
            player.transform.position = pos;
            player.transform.rotation = Quaternion.Euler(rot);
            yield return new WaitForSeconds(0.5f);

            if (math.abs(player.transform.position.x - pos.x) + math.abs(player.transform.position.z - pos.z) < 5
                && Vector3.Distance(prevPos, cam.transform.position) < 1f)
                flag = false;
        }

        player.GetComponent<StarterAssetsInputs>().enabled = true;
        player.GetComponent<ThirdPersonController>().enabled = true;
        player.GetComponent<PlayerInput>().enabled = true;

        yield return new WaitForSeconds(0.5f);
        float diff = GetCalculatableValue(cam.transform.localRotation.eulerAngles.y) - rot.y;
        Debug.Log("angle diff : " + diff.ToString());
        while (Mathf.Abs(diff) > 10f)
        {
            yield return new WaitForFixedUpdate();
            assetsInputs.LookInput(new Vector2(-diff / 40f, 0));
            diff = GetCalculatableValue(cam.transform.localRotation.eulerAngles.y) -
                   rot.y; //GetCalculatableValue(player.transform.localRotation.eulerAngles.y);

            Debug.Log("angle diff : " + diff.ToString() + " vector force : " + (-diff / 10f).ToString());
        }

        assetsInputs.LookInput(new Vector2(0, 0));
        
        ReactCommunicator.Instance.LoadingSetActive(false);
        
        yield return null;
    }

    private float GetCalculatableValue(float deg)
    {
        return (deg < 0) ? deg + 360 : deg;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("CounselingRoom"))
        {
            if (Input.GetMouseButtonUp(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out hit);
                if (hit.collider == null) return;

                hitObject = hit.collider.gameObject;

                if (hitObject.layer.Equals(LayerMask.NameToLayer("Seminar")))
                {
                    ReactCommunicator.Instance.SendFullScreen("Seminar");
                }
                else if (hitObject.layer.Equals(LayerMask.NameToLayer("Event")))
                {
                    ReactCommunicator.Instance.SendFullScreen("Event");
                }
                else if (hitObject.layer.Equals(LayerMask.NameToLayer("Board")))
                {
                    ReactCommunicator.Instance.SendFullScreen("Board");
                }
                else if (hitObject.layer.Equals(LayerMask.NameToLayer("SeminarDoor")))
                {
                    ReactCommunicator.Instance.LoadingSetActive(true);
                    ReactCommunicator.Instance.SendIsSeminarEnteravle();
                    GameEvents.Instance.RequestTeleport(new Vector3(-27f, 3f, -215f), new Vector3(0f, 0f, 0f),true);

// #if UNITY_EDITOR
//                     ReactCommunicator.Instance.LoadingSetActive(true);
//                     GameEvents.Instance.RequestTeleport(new Vector3(-27f, 3f, -215f), new Vector3(0f, 0f, 0f),true);
// #elif !UNITY_EDITOR
//                      LoadingSetActive(true);
//                     GameEvents.Instance.RequestTeleport(new Vector3(-27f, 3f, -215f), new Vector3(0f, 0f, 0f),true);
//
// #endif
                }
                else if (hitObject.layer.Equals(LayerMask.NameToLayer("Chair")))
                {
                    if (player.GetComponent<ThirdPersonController>().isSit) return;
                    player.GetComponent<StarterAssetsInputs>().enabled = false;
                    player.GetComponent<PlayerInput>().enabled = false;
                    player.GetComponent<ThirdPersonController>().enabled = false;


                    player.transform.position = new Vector3(hitObject.transform.position.x, hitObject.transform.position.y - 1f, hitObject.transform.position.z);
                    player.transform.rotation = Quaternion.Euler(0, hitObject.transform.rotation.eulerAngles.y - 180, 0);

                    player.GetComponent<StarterAssetsInputs>().enabled = true;
                    player.GetComponent<PlayerInput>().enabled = true;
                    player.GetComponent<ThirdPersonController>().enabled = true;

                    //ConvertAnimationSitStand(true);
                    player.GetComponent<ThirdPersonController>().isSit = true;
                    player.GetComponent<CharacterController>().enabled = false;
                    ReactCommunicator.Instance.SendRequestUIChange(true);
                }
            }
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.L)) // bottom
        {
            ServerManager.Instance.AdjustNewestCustom();
        }
#endif
    }

}
