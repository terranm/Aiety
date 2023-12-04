using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounseilingRoomController : MonoBehaviour
{
    private void Awake()
    {
        GameEvents.Instance.OnRequestSetCounseilingRoom += SetCounseilingRoom;
        GameEvents.Instance.OnRequestExitCounseilingRoom += ExitCounseilingRoom;
    }

    private void OnDestroy()
    {
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.OnRequestSetCounseilingRoom -= SetCounseilingRoom;
            GameEvents.Instance.OnRequestExitCounseilingRoom -= ExitCounseilingRoom;
        }
    }

    GameObject RoomObj;
    //GameObject MentorObj;
    //GameObject SeatObj;

    GameObject MainCanvasObj;

    Transform seatPos;
    Transform cameraPos;

    // Start is called before the first frame update
    void Start()
    {
        MainCanvasObj = GameObject.Find("Main UI Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetCounseilingRoom(ReactCommunicator.CounseilingRoomData data)
    {
        Debug.Log("SetCounseilingRoom");
        RoomObj = transform.GetChild(int.Parse(data.room)-1).gameObject;
        RoomObj.GetComponentInChildren<MentorController>().SelectMentorAvator(int.Parse(data.mentor.Split("TYPE")[1]) - 1);
        seatPos = RoomObj.transform.Find("Seats").GetChild(CheckSeat());
        cameraPos = RoomObj.transform.Find("CameraPos");

        //MainCanvasObj.SetActive(false);

        //GameEvents.Instance.RequestTeleport(seatPos.position, seatPos.rotation.eulerAngles);
        GameEvents.Instance.RequestSetCounseilingRoomAndPlayer(seatPos.position, seatPos.rotation.eulerAngles, cameraPos.position, cameraPos.rotation.eulerAngles);
    }

    private int CheckSeat()
    {
        int i = 0;
        foreach (CounseilingRoomSeatsController seat in RoomObj.transform.Find("Seats").GetComponentsInChildren<CounseilingRoomSeatsController>())
        {
            if (seat.EmptySeat)
            {
                return i;
            }
            Debug.Log("CheckSeat : " + i++);
        }
        

        return 0;
    }

    private void ExitCounseilingRoom()
    {
        //MainCanvasObj.SetActive(true);

        //RoomObj.transform.Find("Camera").gameObject.SetActive(false);


        GameEvents.Instance.RequestExitCounseilingRoomAndPlayer();
    }

}
