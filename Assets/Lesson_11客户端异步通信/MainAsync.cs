using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class MainAsync : MonoBehaviour
{
    private void Start() {
        //保证回场景也不会多
        if (NetAsyncMgr.Instance == null) {
            GameObject go = new GameObject("NetAsync");
            go.AddComponent<NetAsyncMgr>();
        }
        NetAsyncMgr.Instance.Connect("127.0.0.1", 8080);
    }
}
