using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentorController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < GetComponentsInChildren<Transform>().Length - 1; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectMentorAvator(int num)
    {
        transform.GetChild(num).gameObject.SetActive(true);
    }
}
