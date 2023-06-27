using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>
public class Lesson11 : MonoBehaviour
{
    public Button btnSend;
    public InputField input;
    private void Start() {
        btnSend.onClick.AddListener(()=>{
            if(input.text!="")
                NetAsyncMgr.Instance.Send(input.text);
            input.text="";
        });
    }
}
