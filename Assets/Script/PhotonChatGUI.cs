using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PhotonChatGUI : MonoBehaviour
{
    public Text CurrentChannelText;
    public InputField InputFieldChat;
    public RectTransform ChatOutputPanel;
    public RectTransform SendButton;

    public void OnEnterSend()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            //this.chatTest.SendChatMessage(this.InputFieldChat.text);
            this.InputFieldChat.text = "";
        }
    }

    public void OnClickSend()
    {
        if (this.InputFieldChat != null)
        {
            //this.chatTest.SendChatMessage(this.InputFieldChat.text);
            this.InputFieldChat.text = "";
            OnEndEdit();
        }
    }

    public void OnValueChange()
    {
        //GameManager.SetPlayerInputEnabled(false);
    }

    public void OnEndEdit()
    {
        //GameManager.SetPlayerInputEnabled(true);
    }

    public void Update()
    {
        if (InputFieldChat.isFocused)
        {
            //GameManager.SetPlayerInputEnabled(false);
        }

        if (Input.GetKey(KeyCode.Return))
        {
            if (!InputFieldChat.isFocused)
            {
                InputFieldChat.Select();
            }
        }
    }
}
