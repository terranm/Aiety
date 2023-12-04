using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : Singleton<GameEvents>
{
   public void Init()
   {
      Debug.Log("Game Event init");
   }
   
   public event Action<string> OnRequestAvatarModify;
   public void RequestAvatarModify(string msg)
   {
      if (OnRequestAvatarModify != null)
      {
         OnRequestAvatarModify(msg);
      }
   }
   
   public event Action<bool> OnRequestAvatarCustomize;
   public void RequestAvatarCustomize(bool open)
   {
      if (OnRequestAvatarCustomize != null)
      {
         OnRequestAvatarCustomize(open);
      }
   }
   
   public event Action<string> OnRequestPlayerAction;
   public void RequestPlayerAction(string action)
   {
      if (OnRequestPlayerAction != null)
      {
         OnRequestPlayerAction(action);
      }
   }
   
   public event Action<bool> OnRequestSetActiveSprint;
   public void RequestSetActiveSprint(bool active)
   {
      if (OnRequestSetActiveSprint != null)
      {
         OnRequestSetActiveSprint(active);
      }
   }


    public event Action<ReactCommunicator.CounseilingRoomData> OnRequestSetCounseilingRoom;
    public void RequestSetCounseilingRoom(ReactCommunicator.CounseilingRoomData data)
    {
        if (OnRequestSetCounseilingRoom != null)
        {
            OnRequestSetCounseilingRoom(data);
        }
    }

    public event Action OnRequestExitCounseilingRoom;
    public void RequestExitCounseilingRoom()
    {
        if (OnRequestExitCounseilingRoom != null)
        {
            OnRequestExitCounseilingRoom();
        }
    }


    public event Action<Vector3, Vector3, bool> OnRequestTeleport;
    public void RequestTeleport(Vector3 pos, Vector3 rot, bool isSeminar = false)
    {
        if (OnRequestTeleport != null)
        {
            OnRequestTeleport(pos,rot, isSeminar);
        }
    }


    public event Action<Vector3, Vector3, Vector3, Vector3> OnRequestSetCounseilingRoomAndPlayer;
    public void RequestSetCounseilingRoomAndPlayer(Vector3 charctorPos, Vector3 charctorRot, Vector3 cameraPos, Vector3 cameraRot)
    {
        if (OnRequestSetCounseilingRoomAndPlayer != null)
        {
            OnRequestSetCounseilingRoomAndPlayer(charctorPos, charctorRot, cameraPos, cameraRot);
        }
    }

    public event Action OnRequestExitCounseilingRoomAndPlayer;
    public void RequestExitCounseilingRoomAndPlayer()
    {
        if (OnRequestExitCounseilingRoomAndPlayer != null)
        {
            OnRequestExitCounseilingRoomAndPlayer();
        }
    }

    //public event Action<string> OnRequestAnimatorChange;
    //public void RequestAnimatorChange(string AnimatorName)
    //{
    //    if (OnRequestAnimatorChange != null)
    //    {
    //        OnRequestAnimatorChange(AnimatorName);
    //    }
    //}

    public event Action OnRequestStandUp;
    public void RequestStandUp()
    {
        if (OnRequestStandUp != null)
        {
            OnRequestStandUp();
        }
    }

    public event Action<bool> OnRequestSetActivePlayerInputSys;
    public void RequestSetActivePlayerInputSys(bool active)
    {
        if (OnRequestSetActivePlayerInputSys != null)
        {
            OnRequestSetActivePlayerInputSys(active);
        }
    }
    
    
    public event Action OnRequestSeminarImgRedraw;
    public void RequestSeminarImgRedraw()
    {
        if (OnRequestSeminarImgRedraw != null)
        {
            OnRequestSeminarImgRedraw();
        }
    }
}