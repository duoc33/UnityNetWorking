using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>
public class MainUdp : MonoBehaviour
{
    public Button btn_send;
    private void Start() {
        if (UdpNetMgr.Instance == null)
        {
            GameObject go = new GameObject("UdpNet");
            go.AddComponent<UdpNetMgr>();
        }
        UdpNetMgr.Instance.StartClient("127.0.0.1",8080);

        btn_send.onClick.AddListener(()=>{
            PlayerMsg msg = new PlayerMsg();
            msg.playerID = 0;
            msg.playerData = new PlayerData();
            msg.playerData.name = "CC's Client";
            msg.playerData.atk = 80;
            msg.playerData.lev = 100;
            UdpNetMgr.Instance.Send(msg);
        });
    }
}
