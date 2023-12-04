using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CenterImgSetter : MonoBehaviour
{
    public GameObject eventContent;
    public GameObject seminarImg;
    private float timer;
    private bool nowMoving;
    
    [Serializable]
    public class eventIMGsData
    {
        public List<string> eventIMGs;
    }

    

    private void Start()
    {
        timer = 0f;
        nowMoving = false;

        GameEvents.Instance.OnRequestSeminarImgRedraw += RedrawSeminarImg;
        
        if (ReactCommunicator.Instance.CurrentEventIMGs.Count != 0)
        {
            foreach (var eventImG in ReactCommunicator.Instance.CurrentEventIMGs)
            {
                GameObject temp = new GameObject();
                var rawImg = Instantiate(temp, Vector3.zero, quaternion.identity);
                rawImg.AddComponent<Image>();
                rawImg.name = "eventImg";
                rawImg.transform.SetParent(eventContent.transform);
                RectTransform rectTran = rawImg.GetComponent<RectTransform>();
                rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1462);
                rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 920);
                rectTran.localPosition = Vector3.zero;
                rectTran.localRotation = Quaternion.Euler(0,0,0);
                rectTran.localScale = new Vector3(1f, 1f, 1f);
                StartCoroutine(MakeUrlImageSet(eventImG, rawImg.GetComponent<Image>()));
                Destroy(temp);
            }
        }

        if (!string.IsNullOrEmpty(ReactCommunicator.Instance.seminarUrl))
        {
            StartCoroutine(MakeUrlImageSet(ReactCommunicator.Instance.seminarUrl,
                seminarImg.GetComponent<Image>()));
        }
    }

    private void OnDestroy()
    {
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.OnRequestSeminarImgRedraw -= RedrawSeminarImg;
        }
    }

    private void RedrawSeminarImg()
    {
        if (!string.IsNullOrEmpty(ReactCommunicator.Instance.seminarUrl))
        {
            StartCoroutine(MakeUrlImageSet(ReactCommunicator.Instance.seminarUrl,
                seminarImg.GetComponent<Image>()));
        }
    }

    private IEnumerator MakeUrlImageSet(string url, Image img)
    {
            
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        www.SetRequestHeader("Origin","https://test-www.aiety.co.kr/");
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            DownloadHandlerTexture temp = (DownloadHandlerTexture)www.downloadHandler;
            img.sprite = Sprite.Create(temp.texture, new Rect(0, 0, temp.texture.width, temp.texture.height),
                new Vector2(0.5f, 0.5f));
            Debug.Log("이벤트 이미지 추가");
        }
    }

    private void Update()
    {
        if (ReactCommunicator.Instance.CurrentEventIMGs.Count > 1 && !nowMoving)
        {
            if (timer > 5f)
            {
                if (math.abs(eventContent.transform.localPosition.x +
                             1462 * (ReactCommunicator.Instance.CurrentEventIMGs.Count - 1)) < 10f)
                {
                    //처음으로
                    nowMoving=true;
                    StartCoroutine(SlideContentTo(0));
                }
                else
                {
                    //다음으로
                    nowMoving=true;
                    StartCoroutine(SlideContentTo(eventContent.transform.position.x+1462f));
                }
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    private IEnumerator SlideContentTo(float xpos)
    {
        float diff = (xpos!=0)?eventContent.transform.localPosition.x - xpos: -eventContent.transform.localPosition.x;
        int count = 0;
        while (count < 10)
        {
            eventContent.transform.localPosition = new Vector3(eventContent.transform.localPosition.x + diff / 10f,
                eventContent.transform.localPosition.y, eventContent.transform.localPosition.z);
            yield return new WaitForFixedUpdate();
            count++;
        }

        timer = 0;
        nowMoving = false;
        yield return null;
    }
}
